using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Summary description for VenueServiceFunctionality.
    /// </summary>
    public class DatabaseStorage
    {
        #region Private Properties
        /// <summary>
        /// The default sql connection string. Can be overridden in the web.config file
        /// </summary>
        private string SQL_CONNECTION_STRING;
        /// <summary>
        /// Holds the column ordinal for venue identifier in the database
        /// </summary>
        private const int vIDENTIFIER = 0;
        /// <summary>
        /// Holds the column ordinal for venue ip address in the database
        /// </summary>
        private const int   vIPADDRESS = 1;
        /// <summary>
        /// Holds the column ordinal for venue port in the database
        /// </summary>
        private const int   vPORT = 2;
        /// <summary>
        /// Holds the column ordinal for venue name in the database
        /// </summary>
        private const int   vNAME = 3;
        /// <summary>
        /// Holds the column ordinal for whether the venue is public in the database
        /// </summary>
        private const int   vISPUBLIC = 4;
        /// <summary>
        /// Holds the column ordinal for venue icon length in the database
        /// </summary>
        private const int   vICONLENGTH = 5;
        /// <summary>
        /// Holds the column ordinal for venue icon in the database
        /// </summary>
        private const int   vICON = 6;
        /// <summary>
        /// Holds the column ordinal for the participant identifier in the database
        /// </summary>
        private const int   pIDENTIFIER = 0;
        /// <summary>
        /// Holds the column ordinal for the participant name in the database
        /// </summary>
        private const int   pNAME= 1;
        /// <summary>
        /// Holds the column ordinal for the participant phone no in the database
        /// </summary>
        private const int   pPHONE= 2;
        /// <summary>
        /// Holds the column ordinal for the participant email in the database
        /// </summary>
        private const int   pEMAIL = 3;
        /// <summary>
        /// Holds the column ordinal for the participant icon length in the database
        /// </summary>
        private const int   pICONLENGTH = 4;
        /// <summary>
        /// Holds the column ordinal for the participant icon in the database
        /// </summary>
        private const int   pICON = 5;

        private const int   pNOTIFY = 6;

        private const int   pLASTACCESSED = 7;
        #endregion

        #region Constructors
        public DatabaseStorage(string connString)
        {
            SQL_CONNECTION_STRING = connString;
        }
        #endregion

        #region Public Methods
        public Venue[] GetVenues()
        {
            SqlConnection conn = new SqlConnection( SQL_CONNECTION_STRING);
            SqlCommand cmd = new SqlCommand( "GetVenues", conn );
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleResult);

            ArrayList venuesArray = new ArrayList();
            while ( r.Read() )
            {
                byte[] icon = null;
                if ( !r.IsDBNull(vICONLENGTH) )
                {
                    icon = new Byte[r.GetInt32(vICONLENGTH)];
                    r.GetBytes(vICON, 0,icon, 0, icon.Length);
                }

                Venue v = new Venue(    
                    r.GetString(vIDENTIFIER),
                    r.GetString(vIPADDRESS),
                    r.GetInt32(vPORT),
                    r.GetString(vNAME),
                    icon, null );

                venuesArray.Add( v);
            }
            r.Close();
            conn.Close();

            return (Venue[])venuesArray.ToArray(typeof( Venue));
        }

        public Participant[] GetParticipants()
        {
            SqlConnection conn = new SqlConnection( SQL_CONNECTION_STRING);
            SqlCommand cmd = new SqlCommand( "GetParticipants", conn );
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleResult);

            ArrayList participantsArray = new ArrayList();
            while ( r.Read() )
            {
                byte[] icon = null;
                if ( !r.IsDBNull(pICONLENGTH) )
                {
                    icon = new Byte[r.GetInt32(pICONLENGTH)];
                    r.GetBytes(pICON, 0,icon, 0, icon.Length);
                }

                Participant p = new Participant(
                    r.GetString(pIDENTIFIER),
                    r.GetString(pNAME),
                    r.IsDBNull(pEMAIL) ? null : r.GetString(pEMAIL),
                    icon );

                participantsArray.Add( p);
            }
            r.Close();
            conn.Close();

            return (Participant[])participantsArray.ToArray(typeof( Participant));
        }

        public string[] GetVenueSecurity( string venueIdentifier )
        {
            SqlConnection conn = new SqlConnection( SQL_CONNECTION_STRING);

            SqlCommand cmd = new SqlCommand( "GetVenueSecurity", conn );
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter identifier = cmd.Parameters.Add( "@identifier", SqlDbType.VarChar, 50);
            identifier.Direction = ParameterDirection.Input;
            identifier.Value = venueIdentifier;

            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            ArrayList arpatterns = new ArrayList();
            while(dr.Read())
            {
                arpatterns.Add(dr.GetString(0));
            }

            conn.Close();

            string[] patterns = new string[arpatterns.Count];

            for (int i = 0; i < arpatterns.Count; i++)
                patterns[i] = (string)arpatterns[i];

            return patterns;
        }
        #endregion
    }
}
