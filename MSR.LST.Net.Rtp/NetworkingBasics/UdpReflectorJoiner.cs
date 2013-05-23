using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;

namespace MSR.LST.Net
{

    public class UdpReflectorJoiner
      
    {

        //private readonly int REFLECTOR_JOIN_ATTEMPTS = 4;
        private readonly int JOIN_MESSAGE_DELAY = 2000; // ms

        private readonly IPEndPoint reflectorEP;
        private readonly IPEndPoint multicastEP;

        private volatile bool alive = true;

        public UdpReflectorJoiner(IPEndPoint reflectorEP,IPEndPoint multicastEP)
        {
            this.multicastEP = multicastEP;
            this.reflectorEP = reflectorEP;
        }

        //public UdpReflectorJoiner(IPEndPoint reflectorEP, IPEndPoint multicastEP,Socket socket)
        //{
        //    this.multicastEP = multicastEP;
        //    this.reflectorEP = reflectorEP;
        //    this.socket = socket;
        //}

        public void Terminate()
        {
            alive = false;
        }

        public void Start()
        {
            Thread thread = new Thread(SendJoinMessages);
            thread.IsBackground = true;
            thread.Start();
        }

        ~UdpReflectorJoiner()
        {
            alive = false;
        }

        /// <summary>
        /// Send  UDP join messages to the reflector.  There is no acknoweledgement, so we send 
        /// a series of these messages, pausing briefly in between.
        /// 
        /// A bit of ugliness: the joiner works over both C#'s built in socket, as well as CXP's 
        /// UDPSender.  If the class was initialized without a Socket, then we create a locale UdpSender.
        /// This is necessary because the client uses UdpSender, whereas the reflector uses raw sockets.
        /// </summary>
        private void SendJoinMessages()
        {
            Debug.Assert(reflectorEP != null);
            Debug.Assert(multicastEP != null);
            
            UdpSender sender = null;

            try
            {
                sender = new UdpSender(this.reflectorEP, 64);
                sender.DisableLoopback();

                while (alive)
                {
                    UdpReflectorMessage rjm = new UdpReflectorMessage(UdpReflectorMessageType.JOIN, multicastEP);
                    BufferChunk bufferChunk = rjm.ToBufferChunk();
                   
                   
                    // UdpSender, as used by CXPClient
                    sender.Send(bufferChunk);
                    

                    Thread.Sleep(JOIN_MESSAGE_DELAY);
                }
            }
            catch
            {
                
            }
            finally
            {
                if (sender != null)
                    sender.Dispose();
            }
        }
    }
}
