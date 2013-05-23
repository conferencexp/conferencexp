using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// General Database helper functions.  Performs all database transactions except BufferRecorder writes and BufferPlayer reads.
    /// </summary>
    /// <remarks>
    /// All database I/O is supposed to be transacted out of this helper.  This will provide some abstraction and ease for writing
    /// a future API to abstract away the filesystem for Archiver.  In time, such an API would make it possible to redirect Archiver
    /// to write directly to files, rather than to SQL Server.
    /// 
    /// Note that a complete set of methods has not been writen here.  The only methods in this file are primarily those that
    /// were needed during the writing of Archiver and Archiver Admin.  More methods are needed before implementing a proper API.
    /// </remarks>
    public class DBHelper
    {
        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.DBHelper);
        }
        #endregion

        #region CTor
        static DBHelper()
        {
            InitEventLog();
        }

        private DBHelper()   {}
        #endregion

        #region Create...
        /// <summary>
        /// Saves the conference details when we start recording.
        /// this is to allow the incoming rtp participants and streams to be associated with a particular conference instance
        /// </summary>
        /// <param name="groupID">the group that this conference is for</param>
        /// <param name="venueId">the venue that this conference is for</param>
        /// <param name="description">a friendly description of this conference</param>
        /// <param name="startTime">the time the conference starts</param>
        /// <returns>an identifier for the conference</returns>
        static public int CreateConference(string conferenceDescription, string venueIdentifier, DateTime startTime)
        {
            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);

                SqlCommand cmd = new SqlCommand( "CreateConference", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter description = cmd.Parameters.Add( "@description", SqlDbType.VarChar, 50);
                description.Direction = ParameterDirection.Input;
                description.Value = conferenceDescription;

                SqlParameter venue_identifier = cmd.Parameters.Add("@venue_identifier", SqlDbType.VarChar, 50);
                venue_identifier.Direction = ParameterDirection.Input;
                venue_identifier.Value = venueIdentifier;

                SqlParameter start_dttm = cmd.Parameters.Add("@start_dttm", SqlDbType.DateTime);
                start_dttm.Direction = ParameterDirection.Input;
                start_dttm.Value = startTime;

                SqlParameter conference_id = cmd.Parameters.Add("@conference_id", SqlDbType.Int);
                conference_id.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return (int) conference_id.Value;
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
        }

        /// <summary>
        /// Create a participant associated with the conference currently being recorded
        /// </summary>
        /// <param name="conferenceID">the conference</param>
        /// <param name="cname">canonical name of the participant</param>
        /// <param name="name">a firendly name of the participant</param>
        /// <param name="phone">contact info...</param>
        /// <param name="email"></param>
        /// <returns>the participant ID from the database</returns>
        static public int CreateParticipant( int conferenceID, string cname, string name)
        {
            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);

                SqlCommand cmd = new SqlCommand( "CreateParticipant", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter conference_id = cmd.Parameters.Add("@conference_id", SqlDbType.Int);
                conference_id.Direction = ParameterDirection.Input;
                conference_id.Value = conferenceID;

                SqlParameter cname_param = cmd.Parameters.Add("@cname", SqlDbType.VarChar, 255);
                cname_param.Direction = ParameterDirection.Input;
                cname_param.Value = cname;

                SqlParameter name_param = cmd.Parameters.Add("@name", SqlDbType.VarChar, 255);
                name_param.Direction = ParameterDirection.Input;
                name_param.Value = name;

                SqlParameter sqlParticipantID = cmd.Parameters.Add("@participant_id", SqlDbType.Int);
                sqlParticipantID.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                return (int)sqlParticipantID.Value;
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
        }

        /// <summary>
        /// create a new stream. used during recording to save a stream against a participant
        /// and record the raw stream to write the buffers to
        /// </summary>
        /// <returns>the stream id created so we can pass it to the buffers</returns>
        static public int CreateStream( int participantID, string name, string payloadType, Hashtable priExns )
        {
            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);

                SqlCommand cmd = new SqlCommand( "CreateStream", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParticipantID = cmd.Parameters.Add( "@participant_id", SqlDbType.Int);
                sqlParticipantID.Direction = ParameterDirection.Input;
                sqlParticipantID.Value = participantID;

                SqlParameter stream_name = cmd.Parameters.Add( "@name", SqlDbType.VarChar, 40);
                stream_name.Direction = ParameterDirection.Input;
                stream_name.Value = name;

                SqlParameter payload_type = cmd.Parameters.Add( "@payload_type", SqlDbType.VarChar, 40);
                payload_type.Direction = ParameterDirection.Input;
                payload_type.Value = payloadType;

                SqlParameter priExnsParam = cmd.Parameters.Add( "@priexns", SqlDbType.Image );
                priExnsParam.Direction = ParameterDirection.Input;

                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, priExns);
                priExnsParam.Value = ms.ToArray();

                SqlParameter streamID = cmd.Parameters.Add("@stream_id", SqlDbType.Int);
                streamID.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                return (int)streamID.Value;
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
        }
        #endregion

        #region Get... (Public)
        static public Conference[] GetConferences()
        {
            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);
                SqlCommand cmd = new SqlCommand("GetConferences", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleResult);

                ArrayList conList = new ArrayList(10);
                while( r.Read())
                {
                    Conference conf = new Conference(   
                        r.GetInt32(0),      // conference id
                        r.GetString(1),     // conference description
                        r.GetString(2),     // venue name
                        r.GetDateTime(3),   // start date time
                        r.IsDBNull(4) ? DateTime.MinValue : r.GetDateTime(4) ); // end date time
                    conList.Add(conf);
                }

                r.Close();
                conn.Close();

                return ( Conference[]) conList.ToArray( typeof(Conference));
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
        }

        static public Participant[] GetParticipants(int conferenceID)
        {
            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);
                SqlCommand cmd = new SqlCommand("GetParticipants", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter conference_id = cmd.Parameters.Add("@conference_id", SqlDbType.Int);
                conference_id.Direction = ParameterDirection.Input;
                conference_id.Value = conferenceID;

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleResult);

                ArrayList partList = new ArrayList(10);
                while( r.Read())
                {
                    Participant part = new Participant( 
                        r.GetInt32(0),  //session id
                        r.GetString(2), // cname
                        r.GetString(3));// name
                    partList.Add(part);
                }

                r.Close();
                conn.Close();

                return (Participant[]) partList.ToArray(typeof(Participant));
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
        }

        static public Stream[] GetStreams( int participantID )
        {
            try
            {
                SqlConnection conn = new SqlConnection(Constants.SQLConnectionString);
                SqlCommand cmd = new SqlCommand("GetStreams", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParticipantID = cmd.Parameters.Add("@participant_id", SqlDbType.Int);
                sqlParticipantID.Direction = ParameterDirection.Input;
                sqlParticipantID.Value = participantID;

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleResult);

                ArrayList streamList = new ArrayList(10);
                while( r.Read())
                {
                    Stream stream= new Stream(  
                        r.GetInt32(0),  //stream id
                        r.GetString(1), // name
                        r.GetString(2), // payload,
                        r.GetInt32(3),  // frames
                        r.IsDBNull(4) ? 0L : r.GetInt64(4), // seconds
                        r.GetInt32(5)); // bytes 
                    streamList.Add( stream);
                }

                r.Close();
                conn.Close();

                return ( Stream[]) streamList.ToArray( typeof(Stream));
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError,
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed);
                throw;
            }
        }
        #endregion

        #region Get... (Internal)
        /// <summary>
        /// Gets statistics on a stream for playback.
        /// </summary>
        static internal void GetStreamStatistics( int streamID, out long firstTick, out int maxFrameSize, out int maxFrameCount, out int maxBufferSize)
        {
            SqlConnection conn = new SqlConnection(Constants.SQLConnectionString);

            try 
            {
                SqlCommand cmd = new SqlCommand( "GetStreamStatistics ", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter strID = cmd.Parameters.Add( "@stream_id", SqlDbType.Int);
                strID.Direction = ParameterDirection.Input;
                strID.Value = streamID;

                SqlParameter interval = cmd.Parameters.Add( "@interval", SqlDbType.Int);
                interval.Direction = ParameterDirection.Input;
                interval.Value = Constants.PlaybackBufferInterval;

                SqlParameter startTick = cmd.Parameters.Add("@starting_tick", SqlDbType.BigInt);
                startTick.Direction = ParameterDirection.Output;

                SqlParameter max_frame_size = cmd.Parameters.Add("@max_frame_size", SqlDbType.Int);
                max_frame_size.Direction = ParameterDirection.Output;

                SqlParameter max_frame_count = cmd.Parameters.Add("@max_frame_count", SqlDbType.Int);
                max_frame_count.Direction = ParameterDirection.Output;

                SqlParameter max_buffer_size = cmd.Parameters.Add("@max_buffer_size", SqlDbType.Int);
                max_buffer_size.Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                // Occasionally these throw due to bad data from SQL.  It's not our job to handle here...
                firstTick = (long) startTick.Value;
                maxFrameSize = (int) max_frame_size.Value;
                maxFrameCount = (int) max_frame_count.Value;
                maxBufferSize = (int) max_buffer_size.Value;
            }
            catch (SqlException ex)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError,
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed);

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Gets the information about the stream & participant necessary to play back a single stream.
        /// </summary>
        static internal void GetStreamAndParticipantDetails( int streamID, out string name,  out string payload, out string cname, out Hashtable priExns)
        {
            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);

                SqlCommand cmd = new SqlCommand( "GetStreamAndParticipantDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter stream_id = cmd.Parameters.Add( "@stream_id", SqlDbType.Int);
                stream_id.Direction = ParameterDirection.Input;
                stream_id.Value = streamID;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                
                if( !dr.Read() )
                    throw new ArgumentException();

                name = dr.GetString(0);
                payload = dr.GetString(1);
                cname = dr.GetString(2);

                // Deserialize the private extensions
                MemoryStream ms = new MemoryStream((byte[])dr.GetValue(3));
                BinaryFormatter bf = new BinaryFormatter();
                priExns = (Hashtable)bf.Deserialize(ms);

                conn.Close();
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
        }

        /// <summary>
        /// Loads the next block of indexes into an array of indices.
        /// </summary>
        internal static bool LoadIndices(Index[] indices, long startingTick, int streamID, int maxBytes, out int indexCount)
        {
            SqlConnection conn = new SqlConnection(Constants.SQLConnectionString);
            SqlCommand cmd = new SqlCommand("LoadIndices", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter stream_id = cmd.Parameters.Add("@stream_id", SqlDbType.Int);
            stream_id.Direction = ParameterDirection.Input;
            stream_id.Value = streamID;

            SqlParameter starting_tick = cmd.Parameters.Add("@starting_tick", SqlDbType.BigInt);
            starting_tick.Direction = ParameterDirection.Input;
            starting_tick.Value = startingTick;

            SqlParameter ending_tick = cmd.Parameters.Add("@count", SqlDbType.BigInt);
            ending_tick.Direction = ParameterDirection.Input;
            ending_tick.Value = indices.Length;

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            int i = 0;

            int quitCase = 1; // keep up with _why_ we exit this loop...
            while (dr.Read())
            {
                indices[i].start = dr.GetInt32(0);
                indices[i].end = dr.GetInt32(1);
                indices[i].timestamp = dr.GetInt64(2);

                // Check to make sure we have enough to fill our buffer
                if( (indices[i].end - indices[0].start + 1) >= maxBytes )
                {
                    quitCase = 0;
                    break;
                }

                i++;

                // Make sure indices isn't full
                if( i >= indices.Length )
                {
                    quitCase = 2;
                    break;
                }
            }

            // Close our connections
            dr.Close();
            conn.Close();

            bool streamOutOfData = false;
            indexCount = i;

            // Now we do the appropriate follow-up based why we exited the loop
            switch( quitCase )
            {
                case 0: // enough data to fill buffer.  (note that 'i' is one less than it should be only in this case)
                    if( (indices[i].end - indices[0].start + 1) <= maxBytes )
                        indexCount = i + 1; // all the indices we parsed.
                    // else the last frame won't fit
                    Debug.Assert(indexCount > 0); // we can assert this because of the way maxBufferSize is calculated
                    break;
                case 1: // we're out of data.  Flag it.
                    streamOutOfData = true;
                    break;
                default: // we have enough indices to fill the array
                    break;
            }

            return streamOutOfData;
        }

        /// <summary>
        /// Loads a block of bytes for a given stream.
        /// </summary>
        internal static void LoadBuffer(int streamID, int start, int end, ref BufferChunk frame)
        {
            SqlConnection conn = new SqlConnection(Constants.SQLConnectionString);
            SqlCommand cmd = new SqlCommand( "ReadBuffer ", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlStreamID = cmd.Parameters.Add( "@stream_id", SqlDbType.Int);
            sqlStreamID.Direction = ParameterDirection.Input;
            sqlStreamID.Value = streamID;

            SqlParameter startParam = cmd.Parameters.Add("@start", SqlDbType.Int);
            startParam.Direction = ParameterDirection.Input;
            startParam.Value = start;

            int dataLength = end - start + 1;

            SqlParameter finish = cmd.Parameters.Add("@length", SqlDbType.BigInt);
            finish.Direction = ParameterDirection.Input;
            finish.Value = dataLength;

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);

            if ( dr.Read())
            {
                Debug.Assert( dataLength <= frame.Buffer.Length );
                dr.GetBytes(0, 0, frame.Buffer, 0, dataLength);
            }

            dr.Close();
            conn.Close();
        }
        #endregion

        #region Write data / Modify records
        /// <summary>
        /// Sets the end time against a conference. Marks the conference as finished
        /// </summary>
        /// <param name="conferenceID">the conference</param>
        /// <param name="endTime">and when it ended</param>
        static public void RecordConferenceEndTime( int conferenceID, DateTime endTime)
        {
            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);

                SqlCommand cmd = new SqlCommand( "RecordConferenceEndTime", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter conference_id = cmd.Parameters.Add( "@conference_id", SqlDbType.Int);
                conference_id.Direction = ParameterDirection.Input;
                conference_id.Value = conferenceID;

                SqlParameter end_time = cmd.Parameters.Add("@end_time", SqlDbType.DateTime);
                end_time.Direction = ParameterDirection.Input;
                end_time.Value = endTime;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return;
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
        }

        /// <summary>
        /// Finds the last frame sent in a conference and records that time as the ConferenceEndTime.
        /// </summary>
        /// <remarks>This is a database cleanup operation, intended for conferences with null end_time.</remarks>
        static public DateTime CreateConferenceEndTime (int conferenceID)
        {
            long lastFrameTicks;

            try
            {
                SqlConnection conn = new SqlConnection( Constants.SQLConnectionString);

                SqlCommand cmd = new SqlCommand( "GetConferenceLastFrameSentTime", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter conference_id = cmd.Parameters.Add( "@conference_id", SqlDbType.Int);
                conference_id.Direction = ParameterDirection.Input;
                conference_id.Value = conferenceID;

                conn.Open();
                lastFrameTicks = (long)cmd.ExecuteScalar();
                conn.Close();
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }

            DateTime lastFrameTime = new DateTime(lastFrameTicks);
            RecordConferenceEndTime(conferenceID, lastFrameTime);

            return lastFrameTime;
        }
        /// <summary>
        /// Changes the "name" column of a given conference.
        /// </summary>
        static public bool RenameConference( int conferenceID, string newName )
        {
            if( newName.IndexOfAny( new char[]{'\'', '\"'} ) >= 0 )
                throw new ArgumentException(Strings.ConferenceNameError);

            string command = "UPDATE Conference SET description = '" + newName + "' WHERE conference_id = " + 
                conferenceID;

            return (ExecuteNonQuery(command) == 1);
        }

        /// <summary>
        /// Commits data buffered during recording to disk.
        /// </summary>
        /// <param name="streamID">ID of the stream in the database to be written to.</param>
        /// <param name="indices">The indices of the new frames to be written.</param>
        /// <param name="indicesCount">The number of the frames in the indices array to be written.</param>
        /// <param name="buffer">The buffer where the data was buffered to.</param>
        static internal void SaveBufferAndIndices( int streamID, Index[] indices, int indicesCount, byte[] buffer )
        {
            #region Save Buffer (saves the raw bytes into the RawStream table in the DB)
            int offset;
            try
            {
                SqlConnection conn = new SqlConnection(Constants.SQLConnectionString);
                SqlCommand cmd = new SqlCommand( "AppendBuffer ", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlStreamID = cmd.Parameters.Add( "@stream_id", SqlDbType.Int);
                sqlStreamID.Direction = ParameterDirection.Input;
                sqlStreamID.Value = streamID;

                SqlParameter dataParam = cmd.Parameters.Add("@chunk", SqlDbType.Image);
                dataParam.Direction = ParameterDirection.Input;
                dataParam.Value = buffer;
                dataParam.Size = indices[indicesCount-1].end + 1;

                SqlParameter offsetParam = cmd.Parameters.Add( "@offset", SqlDbType.Int);
                offsetParam.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                offset = (int)offsetParam.Value;
            }
            catch( SqlException ex )
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError, 
                    ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed );
                throw;
            }
            #endregion

            #region Build the index inserts
            // lets do one big transaction to save on log writes etc.
            StringBuilder sb = new StringBuilder("BEGIN TRANSACTION" , 50 + 125*indicesCount);
            for ( int i = 0; i < indicesCount; i++)
            {
                sb.Append(" INSERT INTO Frame ( stream_id,  raw_start, raw_end, frame_time ) VALUES (");
                sb.Append( streamID);
                sb.Append(" ,");
                sb.Append( indices[i].start + offset);
                sb.Append( ",");
                sb.Append( indices[i].end + offset);
                sb.Append( ",");
                sb.Append ( indices[i].timestamp);
                sb.Append( ") ");
            }
            sb.Append("COMMIT TRANSACTION");
            string indexString = sb.ToString();
            #endregion

            #region Save the index inserts
            int affected = ExecuteNonQuery( indexString );

            if ( affected != indicesCount)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.FailedToSaveAllIndices, 
                    indicesCount, affected), EventLogEntryType.Error, ArchiveServiceEventLog.ID.IndiciesFailedToSave);
            }
            #endregion
        }
        #endregion

        #region Delete...
        public static void DeleteConferences(int[] conferenceIDs)
        {
            StringBuilder sb = new StringBuilder(500);
            if( conferenceIDs != null && conferenceIDs.Length > 0 )
            {
                sb.Append("DELETE FROM Conference WHERE ");
                sb.Append("conference_id = ");
                sb.Append(conferenceIDs[0]);

                for(int cnt = 1; cnt < conferenceIDs.Length; ++cnt)
                {
                    sb.Append(" OR conference_id = ");
                    sb.Append(conferenceIDs[cnt]);
                }
                sb.Append(" \n");

                ExecuteNonQuery(sb.ToString());
            }
        }

        public static void DeleteParticipants(int[] participantIDs)
        {
            StringBuilder sb = new StringBuilder(500);
            if( participantIDs != null && participantIDs.Length > 0 )
            {
                sb.Append("DELETE Participant FROM Participant WHERE ");
                sb.Append("participant_id = ");
                sb.Append(participantIDs[0]);

                for(int cnt = 1; cnt < participantIDs.Length; ++cnt)
                {
                    sb.Append(" OR participant_id = ");
                    sb.Append(participantIDs[cnt]);
                }
                sb.Append(" \n");

                ExecuteNonQuery(sb.ToString());
            }
        }

        public static void DeleteStreams(int[] streamIDs)
        {
            StringBuilder sb = new StringBuilder(500);
            if( streamIDs != null && streamIDs.Length > 0 )
            {
                sb.Append("DELETE FROM Stream WHERE ");
                sb.Append("stream_id = ");
                sb.Append(streamIDs[0]);

                for(int cnt = 1; cnt < streamIDs.Length; ++cnt)
                {
                    sb.Append(" OR stream_id = ");
                    sb.Append(streamIDs[cnt]);
                }
                sb.Append(" \n");

                ExecuteNonQuery(sb.ToString());
            }
        }
        #endregion

        #region Private Helper Methods
        private static int ExecuteNonQuery(string command)
        {
            SqlConnection conn = new SqlConnection( Constants.SQLConnectionString );
            SqlCommand cmd = new SqlCommand( command, conn );
            cmd.CommandTimeout = Constants.CommandTimeout;
            cmd.CommandType = CommandType.Text;

            int retVal = 0;

            try
            {
                conn.Open();
                retVal = cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            return retVal;
        }
        #endregion

    } // end of class
} // end of namespace
