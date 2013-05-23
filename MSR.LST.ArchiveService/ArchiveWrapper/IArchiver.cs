using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.Win32;

using MSR.LST;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Archiver presents the high-level interface to the archival system.
    /// The ArchiveServer is exposed through remoting, and can be hosted in a 
    /// windows service or standard .NET exe.
    /// </summary>
    public interface IArchiveServer
    {
        // Pri1: Add accessibility to Constants that are relevant to the client.

        #region Get Conferences / Participants / Streams
        Version GetVersion();

        Conference[] GetConferences();

        Participant[] GetParticipants(int conferenceID);

        Stream[] GetStreams(int sessionID);
        #endregion

        #region Record / Playback
        /// <summary>
        /// Records the conference.
        /// </summary>
        /// <param name="conferenceDescription">A description of the conference being recorded.</param>
        /// <param name="venueIdentifier">Venue name for the conference.</param>
        /// <param name="venue">Venue to record from.</param>
        /// <returns>The number of ongoing requests to record this conference.</returns>
        int Record( string conferenceDescription, string venueIdentifier, IPEndPoint venue );

        /// <summary>
        /// Plays back a conference, or at least a series of streams.
        /// </summary>
        /// <param name="venue">Venue to play back into (may be a unicast location).</param>
        /// <param name="streams">Database IDs of the streams to record.</param>
        /// <param name="playbackStopped">An Eventhandler to call when playback has 
        /// stopped due to reaching the end of all the streams.</param>
        void Play( IPEndPoint venue, int[] streams );

        /// <summary>
        /// Stops the ongoing playback to a particular venue.
        /// </summary>
        /// <param name="venue">Then venue to which playback is occuring which you would like stopped.</param>
        void StopPlaying(IPEndPoint venue);

        /// <summary>
        /// Decrements the number of "interested parties" in this recording, and stops the recording
        /// if that number has reached zero.
        /// </summary>
        /// <param name="venue">The venue in which recording is currently occurring.</param>
        /// <returns>The number of interested parties after this decrement.</returns>
        /// <remarks>Return of '0' indicates that recording was stopped.  Otherwise recording will continue.</remarks>
        int StopRecording(IPEndPoint venue);
        #endregion

        #region JumpTo / GetCurrentTime
        /// <summary>
        /// Jumps to a specific time in a recording.
        /// </summary>
        void JumpTo(IPEndPoint venue, long timeToJumpTo);

        /// <summary>
        /// Gets the current point in time, in ticks, of where playback is in the course of the recording.
        /// </summary>
        long GetCurrentTime(IPEndPoint venue);
        #endregion
    }

    /// <summary>
    /// A handy enum of the various states that a client could view the Archiver in.
    /// </summary>
    /// <remarks>
    /// Use as it pleases you.  Not important to use.
    /// </remarks>
    public enum ArchiverState
    {
        Playing,
        Recording,
        Unavailable,
        Stopped
    }

    #region Exceptions

    /// <summary>
    /// PlaybackInProgressExeption is thrown if a ConferencePlayer session object is in the venue 
    /// </summary>
    [Serializable]
    public class PlaybackInProgressException : ApplicationException
    {
        public PlaybackInProgressException() : base() { }
        public PlaybackInProgressException(string msg) : base(msg) { }
        public PlaybackInProgressException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// VenueInUseExeption is thrown if any type of session object is in the venue 
    /// </summary>
    [Serializable]
    public class VenueInUseException : ApplicationException
    {
        public VenueInUseException() : base() { }
        public VenueInUseException(string msg) : base(msg) { }
        public VenueInUseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    #endregion Exceptions

}
