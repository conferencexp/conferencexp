using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

using LstSocks = MSR.LST.Net.Sockets;

using MSR.LST.Net.Sockets;


namespace MSR.LST.Net
{
    /// <summary>
    /// Multicast Sender is a high level class that is used to simplify the process of sending multicast UDP packets.
    /// It does all the work for you to join the multicast group and set the proper socket settings for correct operation.
    /// </summary>
    [ComVisible(false)]
    public class UdpSender : INetworkSender
    {
        #region Statics

        private const long SecondsSuppressSocketExceptions = 5;

        private static int maxRetries = 4;
        private static int delayBetweenRetries = 500;

        public static int MaxRetries
        {
            get{return maxRetries;}
            set{maxRetries = value;}
        }

        public static int DelayBetweenRetries
        {
            get{return delayBetweenRetries;}
            set{delayBetweenRetries = value;}
        }

        #endregion Statics

        #region Members
        
        /// <summary>
        /// Part of the IDisposable pattern, tells you when the instance has been disposed
        /// </summary>
        private bool disposed = false;
        
        /// <summary>
        /// Socket we send on.  Note that we use the MSR.LST version to get BufferChunk support.
        /// </summary>
        private MSR.LST.Net.Sockets.Socket sock = null;
        
        /// <summary>
        /// Multicast EndPoint we're sending to.
        /// </summary>
        private IPEndPoint endPoint;
        
        /// <summary>
        /// Our loopback address for the unicast case
        /// </summary>
        private IPEndPoint echoEndPoint = null;
        
        /// <summary>
        /// Millisecond delay to add between packet sends, used to govern network throughput on limited networks such as 802.11b
        /// </summary>
        private short delayBetweenPackets = 0;
        
        /// <summary>
        /// Hold the local externalInterface for diagnostic purposes
        /// </summary>
        private IPAddress localRoutingInterface = null;

        /// <summary>
        /// How many times have we retried this packet?
        /// </summary>
        private int retries = 0;
        
        private int lastExceptionNativeErrorCode;
        
        private DateTime lastExceptionTime;

        #if FaultInjection
        private Random rnd = new Random();
        #endif

        #endregion Members

        #region Constructors
        
        /// <summary>
        /// Constructor that binds this object instance to an IPEndPoint.  If you need to change settings dynamically, Dispose and recreate a new object.
        /// </summary>
        /// <param name="endPoint">IPEndPoint where to send the multicast packets -- should be in range 224.0.0.0 to 239.255.255.255</param>
        /// <param name="timeToLive">ushort Time To Live of the packets -- how many routers will we cross -- set to 2 for local or testing</param>
        public UdpSender(System.Net.IPEndPoint destEndPoint, ushort timeToLive)
        {
            this.endPoint = destEndPoint;

            LstSocks.Socket.SockInterfacePair sip = LstSocks.Socket.GetSharedSocket(destEndPoint);
            this.sock = sip.sock;
            this.localRoutingInterface = sip.extInterface;

            if ( Utility.IsMulticast( destEndPoint.Address ) )
            {
                SocketOptionLevel sOL = SocketOptionLevel.IP;
                if (destEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    sOL = SocketOptionLevel.IPv6;
                }
                // Set the TTL
                sock.SetSocketOption(sOL, SocketOptionName.MulticastTimeToLive, timeToLive);
                // Enable Multicast Loopback
                sock.SetSocketOption(sOL, SocketOptionName.MulticastLoopback, 1);
            }
            else
            {
                // Enable Unicast Loopback
                /// Note: If we didn't also instantiate a UdpListener, the socket won't have been bound
                /// to a local interface.  Specifically this comes up when we use the UdpSender to transmit
                /// reports to a diagnostic server.  In this case the loopback is not needed anyway.
                IPEndPoint localEndpoint = (IPEndPoint)sock.LocalEndPoint;
                if (localEndpoint != null) {
                    echoEndPoint = new IPEndPoint(localRoutingInterface, localEndpoint.Port);
                }
            }
        }

        /// <summary>
        /// Dispose per the IDisposable pattern
        /// </summary>
        public void Dispose() 
        {
            GC.SuppressFinalize(this);
            
            if(!disposed) 
            {
                disposed = true;
                if (sock != null)
                {
                    LstSocks.Socket.ReleaseSharedSocket(endPoint, sock);
                    sock = null;
                }
            }
        }

        /// <summary>
        /// Destructor -- needed because we hold on to an expensive resource, a network socket.  Note that this just calls Dispose.
        /// </summary>
        ~UdpSender()
        {
            Dispose();
        }


        #endregion
        
        #region Public Methods

        /// <summary>
        /// Disable unicast loopback
        /// </summary>
        /// 
        public void DisableLoopback()
        {
            echoEndPoint = null;
        }

        /// <summary>
        /// Send a BufferChunk.  This method is preferred over sending a byte[] because you can allocate a large
        /// byte[] in one BufferChunk and continously send the buffer without recreating byte[]s and dealing with the memory allocation
        /// overhead that causes.
        /// </summary>
        /// <param name="packetBuffer">BufferChunk to send</param>
        public void Send(BufferChunk packetBuffer)
        {
            if (disposed) throw new ObjectDisposedException(Strings.UdpSenderAlreadyDisposed);

            try
            {
#if FaultInjection

                if (dropPacketsSentPercent == 0)
                {
                    // Send an echo signal back if we're sending out to a unicast address.  This mimicks the behavior of multicast with MulticastLoopback == true
                    if (echoEndPoint != null)
                        sock.SendTo(packetBuffer, echoEndPoint);

                    sock.SendTo(packetBuffer, endPoint);
                }
                else
                {
                    // Send an echo signal back if we're sending out to a unicast address.  This mimicks the behavior of multicast with MulticastLoopback == true
                    if (echoEndPoint != null)
                    {
                        if (rnd.Next(0, 100) >= dropPacketsSentPercent)
                        {
                            sock.SendTo(packetBuffer, echoEndPoint);
                        }
                    }

                    if (rnd.Next(0, 100) >= dropPacketsSentPercent)
                    {
                        sock.SendTo(packetBuffer, endPoint);
                    }
                }
#else
                // Send an echo signal back if we're sending out to a unicast address.  This mimicks the behavior of multicast with MulticastLoopback == true
                if (echoEndPoint != null)
                    sock.SendTo(packetBuffer, echoEndPoint);

                // Reset the retry counter
                retries = 0;

                // Come back to here in order to try resending to network
                RetrySend:
                try
                {
                    sock.SendTo(packetBuffer, endPoint);
                    if(delayBetweenPackets != 0)
                    {
                        Thread.Sleep(delayBetweenPackets); // To control bandwidth
                    }
                }
                catch (SocketException se)
                {
                    // Look for a WSACancelBlockingCall (Error code 10004)
                    // We might just be in the middle of changing access points
                    // This is a problem for the Intel 2200BG card
                    if (se.ErrorCode == 10004) 
                    {
                        if(retries++ < MaxRetries)
                        {
                            Thread.Sleep(DelayBetweenRetries);
                            goto RetrySend;
                        }
                    }
 
                    // Wrong error code or we have retried max times
                    throw;
                }
#endif
            }
            catch (SocketException se)
            {
                // Suppress the SocketException if the SocketException.NativeErrorCode is the same and the last exception occured within the exception suppression period
                if (lastExceptionNativeErrorCode == se.NativeErrorCode)
                {
                    if (lastExceptionTime.AddSeconds(SecondsSuppressSocketExceptions) < DateTime.Now)
                    {
                        lastExceptionTime = DateTime.Now;
                        throw;
                    }
                }
                else
                {
                    lastExceptionNativeErrorCode = se.NativeErrorCode;
                    lastExceptionTime = DateTime.Now;
                    throw;
                }
            }
        }

        #endregion

        #region Public Properties
        
        /// <summary>
        /// Get the local MulticastInterface for diagnostic purposes
        /// </summary>
        public IPAddress ExternalInterface
        {
            get
            {
                return localRoutingInterface;
            }
        }

        public short DelayBetweenPackets
        {
            get
            {
                return delayBetweenPackets;
            }
            set
            {
                if ( delayBetweenPackets < 0 || delayBetweenPackets > 30 )
                {
                    throw new ArgumentException(Strings.DelayBetweenPacketsRange);
                }

                delayBetweenPackets = value;
            }
        }
        
        
        #endregion
        
        #region Fault Injection
#if FaultInjection
        internal int dropPacketsSentPercent = 0;
        public int DropPacketsSentPercent
        {
            get
            {
                return dropPacketsSentPercent;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new ArgumentException("DropPacketsSentPercent must be between 0 and 100");
                dropPacketsSentPercent = value;
            }
        }
#endif
        #endregion
    }
}