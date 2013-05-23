using System;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// This is the base exception from which all others inherit
    /// </summary>
    public class RtpException : ApplicationException
    {
        public RtpException(){}
        public RtpException(string message) : base(message){}
        public RtpException(string message, Exception inner) : base(message, inner){}
    }

    /// <summary>
    /// This exception is thrown when the RtpListener runs out of buffers
    /// </summary>
    public class PoolExhaustedException : RtpException
    {
        public PoolExhaustedException(){}
        public PoolExhaustedException(string message) : base(message){}
        public PoolExhaustedException(string message, Exception inner) : base(message, inner){}
    }

    /// <summary>
    /// This exception is thrown when a frame that is too large is sent or received
    /// </summary>
    public class FrameTooLargeException : RtpException
    {
        public FrameTooLargeException(){}
        public FrameTooLargeException(string message) : base(message){}
        public FrameTooLargeException(string message, Exception inner) : base(message, inner){}
    }

    /// <summary>
    /// This exception is thrown when a frame is incomplete but someone has asked for the data
    /// </summary>
    public class FrameIncompleteException : RtpException
    {
        public FrameIncompleteException(){}
        public FrameIncompleteException(string message) : base(message){}
        public FrameIncompleteException(string message, Exception inner) : base(message, inner){}
    }

    /// <summary>
    /// This exception is thrown when a packet with an incorrect timestamp is added to a frame
    /// </summary>
    public class IncorrectTimestampException : RtpException
    {
        public IncorrectTimestampException(){}
        public IncorrectTimestampException(string message) : base(message){}
        public IncorrectTimestampException(string message, Exception inner) : base(message, inner){}
    }

    /// <summary>
    /// This exception is thrown when a duplicate packet arrives and it is not expected
    /// </summary>
    public class DuplicatePacketException : RtpException
    {
        public DuplicatePacketException(){}
        public DuplicatePacketException(string message) : base(message){}
        public DuplicatePacketException(string message, Exception inner) : base(message, inner){}
    }

    /// <summary>
    /// This exception is thrown when the NextFrame method is unblocked from a manual call or the
    /// Dispose method.
    /// </summary>
    public class NextFrameUnblockedException : RtpException
    {
        public NextFrameUnblockedException(){}
        public NextFrameUnblockedException(string message) : base(message){}
        public NextFrameUnblockedException(string message, Exception inner) : base(message, inner){}
    }
}
