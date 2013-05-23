using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

using LstSocks = MSR.LST.Net.Sockets;

using MSR.LST.Net.Sockets;
using System.Configuration;


namespace MSR.LST.Net
{
    /// <summary>
    /// Multicast Listener is a high level class that is used to simplify the process of listening for multicast UDP packets.
    /// It does all the work for you to join the multicast group and set the proper socket settings for correct operation.
    /// 
    /// In the future, this will become more polymorphic so that you can select from one of a a number of protocols that implement the
    /// "INetworkListener" interface, such as NetworkListenerTcp, NetworkListenerUdpUnicast, NetworkListenerPgm.
    /// </summary>
    /// <example>
    /// ...
    /// MulticastUdpListener mcListener = new MulticastUdpListener(endPoint);
    /// mcListener.Receive(packetBuffer);
    /// mcListener.Displose();
    /// mcListener = null;
    /// ...
    /// </example>
    [ComVisible(false)]
    public class UdpListener : INetworkListener
    {

        #region Asynchronous ReceiveFrom functionality
        private class asyncReceiveState
        {
            internal asyncReceiveState(MSR.LST.Net.Sockets.Socket sock, BufferChunk bufferChunk, Queue queue, ReceivedFromCallback receivedFromCallback)
            {
                this.sock = sock;
                this.bufferChunk = bufferChunk;
                this.queue = queue;
                this.receivedFromCallback = receivedFromCallback;
            }

            internal MSR.LST.Net.Sockets.Socket sock = null;
            internal BufferChunk bufferChunk = null;
            internal Queue queue = null;
            internal ReceivedFromCallback receivedFromCallback = null;
        }

        public void AsyncReceiveFrom(Queue queueOfBufferChunks, ReceivedFromCallback callback)
        {
            if (disposed) // This continues in the background, so shut down if disposed
            {
                return;
            }
            // Set up the state
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderEP = (EndPoint)sender;
            BufferChunk bc = (BufferChunk)queueOfBufferChunks.Dequeue();
            asyncReceiveState ars = new asyncReceiveState(this.sock, bc, queueOfBufferChunks, callback);

            // Start the ReceiveFrom
            sock.BeginReceiveFrom(bc, ref senderEP, new AsyncCallback(asyncReceiveCallback), ars);
        }

        private void asyncReceiveCallback(IAsyncResult ar)
        {
            if (disposed) // This continues in the background, so shut down if disposed
            {
                return;
            }
            // Set up the state
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderEP = (EndPoint)sender;
            asyncReceiveState ars = (asyncReceiveState)ar.AsyncState;

            // Receive the packet
            try
            {
                ars.bufferChunk.Length = ars.sock.EndReceiveFrom(ar, ref senderEP);

                // We get back a size of 0 when the socket read fails due to timeout
                if (ars.bufferChunk.Length > 0)
                {
                    // Send the data back to the app that called AsyncReceiveFrom
                    ars.receivedFromCallback(ars.bufferChunk, senderEP);
                }
            }
            catch (Exception e)
            {
                //Pri2: This used to throw a network timeout event into the EventQueue when it was a synchronous receive
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }

            // Start another pending network read
            AsyncReceiveFrom(ars.queue, ars.receivedFromCallback);
        }
        #endregion
        #region Private Properties
        /// <summary>
        /// bool to tell if the Dispose method has been called, per the IDisposable pattern
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// Socket upon which the Multicast Listener works.  Note that we use the MSR.LST version of Socket in order to get BufferChunk support.
        /// </summary>
        private MSR.LST.Net.Sockets.Socket sock = null;

        /// <summary>
        /// Hold on to the multicastInterface so we can query it at a later time for diagnostic purposes
        /// </summary>
        private IPAddress externalInterface = null;

        /// <summary>
        /// Background worker that connects to a UDP reflector
        /// </summary>
        private UdpReflectorJoiner joiner = null;

        private Random rnd = new Random();
        
        /// <summary>
        /// This is the address from which we will receive traffic.  For multicast sockets, this is a multicast
        /// address.  For reflector-based sockets, this is the address of the unicast reflector.
        /// </summary>
        private IPEndPoint nextHopEP = null;

        /// <summary>
        /// The multicast address we are listening to.  For native multicast, this is the same as nextHopEP.
        /// For reflector-based sockets, this is the actual multicast address.
        /// </summary>
        private IPEndPoint multicastEP = null;

        #endregion

        #region Constructors 

        /// <summary>
        /// This constructor is used for non-reflector sessions; 
        /// if necessary, it joins the appropriate multicast group.
        /// </summary>
        /// <param name="multicastEP"></param>
        /// <param name="timeoutMilliseconds"></param>
    
        public UdpListener(System.Net.IPEndPoint nextHopEP, int timeoutMilliseconds)
        {
            LstSocks.Socket.SockInterfacePair sip = LstSocks.Socket.GetSharedSocket(nextHopEP);
            this.sock = sip.sock;
            this.externalInterface = sip.extInterface;
            this.nextHopEP = nextHopEP;
            this.multicastEP = nextHopEP;
            
            lock(sip)
            {
                if(!sip.Initialized)
                {
                    try
                    {
                        InitializeSocket(nextHopEP.Port, timeoutMilliseconds);

                        if(Utility.IsMulticast(nextHopEP.Address))
                        {
                            if(nextHopEP.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                // Join the IPv6 Multicast group
                                sock.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership,
                                    new IPv6MulticastOption(nextHopEP.Address));
                            } 
                            else 
                            {
                                // Join the IPv4 Multicast group
                                sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                                    new MulticastOption(nextHopEP.Address));
                            }

                        }

                        sip.Initialized = true;
                    } 
                    catch
                    {
                        this.Dispose();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Common initialization for unicast (reflector) and multicast sockets
        /// </summary>
        /// <param name="port"> The local port number on which packets will be received.  This can be
        /// zero to indicate a "don't care" port should be selected.
        /// </param>
        /// <param name="timeoutMilliseconds"></param>
        private void InitializeSocket(int port, int timeoutMilliseconds)
        {
            // Set the timeout on the socket
            if (timeoutMilliseconds > 0)
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeoutMilliseconds);

            // Set the socket to send & receive from this endpoint
            sock.Bind(new IPEndPoint(externalInterface, port));

            // Make room for 80 packets plus some overhead
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1500 * 80);
        }

        /// <summary>
        /// This constructor is for use with a unicast reflector.  This constructor sends JOIN messages
        /// to the given reflector endpoint.
        /// 
        /// </summary>
        /// <param name="reflectorEP"></param>
        /// <param name="multicastEP"></param>
        /// <param name="timeout"></param>
        public UdpListener(IPEndPoint multicastEP, IPEndPoint reflectorEP, int timeout)
        {
                    
            LstSocks.Socket.SockInterfacePair sip = LstSocks.Socket.GetSharedSocket(reflectorEP);
            this.sock = sip.sock;
            this.externalInterface = sip.extInterface;
            this.nextHopEP = reflectorEP;
            this.multicastEP = multicastEP;

            // check whether we must "force" the local port to equal the remote port -- this is done
            // for clients behind picky firewalls.  If not, use port number 0 to indicate a "don't care"

            int port = 0;

            try
            {
                string forcePort = ConfigurationManager.AppSettings["MSR.LST.Net.ForceLocalUnicastPorts"];
                bool force = Boolean.Parse(forcePort);
                if (force)
                    port = reflectorEP.Port;
            }
            catch (Exception) { }

            lock (sip)
            {
                if (!sip.Initialized)
                {
                    try
                    {
                        InitializeSocket(port, timeout);

                        // notify the reflector of our presence...
                        joiner = new UdpReflectorJoiner(nextHopEP, multicastEP);                        
                        joiner.Start();

                        sip.Initialized = true;
                    }
                    catch
                    {
                        this.Dispose();
                        throw;
                    }
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
                    // if we are using a reflector, send a tear-down message
                    if (!multicastEP.Equals(nextHopEP))
                    {
                        try
                        {
                            if (joiner != null)
                                joiner.Terminate();

                            // send a LEAVE message; this might get lost, but that doesn't matter
                            // because the reflector wil time out.
                            UdpSender sender = new UdpSender(nextHopEP, 64);
                            UdpReflectorMessage message = new UdpReflectorMessage(UdpReflectorMessageType.LEAVE, multicastEP);

                            sender.Send(message.ToBufferChunk());
                            sender.Dispose();
                        }
                        catch (Exception) { }
                    }

                    LstSocks.Socket.ReleaseSharedSocket(nextHopEP, sock);
                    sock = null;
                }
            }
        }
        /// <summary>
        /// Destructor -- needed because we hold on to an expensive resource, a network socket.  Note that this just calls Dispose.
        /// </summary>
        ~UdpListener()
        {
            Dispose();
        }

#endregion

        


        #region Public Methods

        /// <summary>
        /// Receive a packet into a BufferChunk.  This method is preferred over receiving into a byte[] because you can allocate a large
        /// byte[] in one BufferChunk and continously receiving into the buffer without recreating byte[]s and dealing with the memory allocation
        /// overhead that causes.
        /// 
        /// No int bytes received is returned because the bytes received is stored in BufferChunk.Length.
        /// </summary>
        /// <param name="packetBuffer">BufferChunk</param>
        /// <example>
        /// ...
        /// MulticastUdpListener mcListener = new MulticastUdpListener(endPoint);
        ///
        /// // Allocate a 2K buffer to hold the incoming packet
        /// BufferChunk packetBuffer = new BufferChunk(2000);
        ///
        /// mcListener.Receive(packetBuffer);
        ///
        /// mcListener.Displose();
        /// mcListener = null;
        /// ...
        /// </example>
        public void Receive(BufferChunk packetBuffer)
        {
            if (disposed) throw new ObjectDisposedException(Strings.MulticastUdpListenerAlreadyDisposed);

            sock.Receive(packetBuffer);
#if FaultInjection
            if ( dropPacketsReceivedPercent > 0 )
            {
                while (rnd.Next(0,100) < dropPacketsReceivedPercent)
                {
                    sock.Receive(packetBuffer);
                }
            }
#endif
        }
        /// <summary>
        /// Same as Receive, but you also get an EndPoint containing the sender of the packet.
        /// </summary>
        /// <param name="packetBuffer">BufferChunk</param>
        /// <param name="endPoint">EndPoint</param>
        /// <example>
        /// ...
        /// MulticastUdpListener mcListener = new MulticastUdpListener(endPoint);
        ///
        /// // Allocate a 2K buffer to hold the incoming packet
        /// BufferChunk packetBuffer = new BufferChunk(2000);
        ///
        /// // Allocate a structure to hold the incoming endPoint
        /// EndPoint endPoint;
        ///
        /// mcListener.ReceiveFrom(packetBuffer, endPoint);
        ///
        /// mcListener.Displose();
        /// mcListener = null;
        /// ...
        /// </example>
        public void ReceiveFrom(BufferChunk packetBuffer, out EndPoint endPoint)
        {
            if (disposed) throw new ObjectDisposedException(Strings.MulticastUdpListenerAlreadyDisposed);

            endPoint = new IPEndPoint(externalInterface,0);
            sock.ReceiveFrom(packetBuffer, ref endPoint);
#if FaultInjection
            if ( dropPacketsReceivedPercent > 0 )
            {
                while (rnd.Next(0,100) < dropPacketsReceivedPercent)
                {
                    sock.ReceiveFrom(packetBuffer, ref endPoint);
                }
            }
#endif
        }
        #endregion
        #region Public Properties
        /// <summary>
        /// Get the IP address of the Local Multicast Interface -- used for diagnostic purposes
        /// </summary>
        public IPAddress ExternalInterface
        {
            get
            {
                return externalInterface;
            }
        }
        #endregion
        #region Fault Injection
#if FaultInjection
        internal int dropPacketsReceivedPercent = 0;
        public int DropPacketsReceivedPercent
        {
            get
            {
                return dropPacketsReceivedPercent;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new ArgumentException("DropPacketsReceivedPercent must be between 0 and 100");
                dropPacketsReceivedPercent = value;
            }
        }
#endif
        #endregion
        
    }

}
