using System;
using System.Runtime.Serialization;


namespace MSR.LST.ConferenceXP.VenueService
{
    [Serializable]
    public class VenueServiceException : ApplicationException
    {
        public VenueServiceException( string message) : base(message) {}
        public VenueServiceException() : base() {}
        public VenueServiceException( string message, Exception inner) : base(message, inner) {}
        protected VenueServiceException( SerializationInfo si, StreamingContext sc ) : base(si,sc) {}
    }
}
