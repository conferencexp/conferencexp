using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Net;


namespace MSR.LST.Net.Heartbeat {
    class HeartbeatServer: IDisposable {

        private UdpSender m_UdpSender;
        private bool m_ThreadStop;
        private bool m_Disposed;
        private Thread m_Thread;
        private readonly IPEndPoint m_IpEp;
        private readonly BufferChunk m_PacketBuffer;
        private readonly int m_Period;

        public HeartbeatServer(String addr, int port, uint cookie, int period, ushort ttl) {
            m_Period = period;
            m_Thread = null;
            m_Disposed = false;

            //Set up the send buffer
            byte[] buffer = new byte[sizeof(uint)];
            m_PacketBuffer = new BufferChunk(buffer);
            m_PacketBuffer.SetUInt32(0, cookie);

            //Create the Sender
            IPAddress ipaddr = IPAddress.Parse(addr);
            m_IpEp = new IPEndPoint(ipaddr, port);
            m_UdpSender = new UdpSender(m_IpEp, ttl);
        }

        internal void Start() {
            if (!m_Disposed) {
                m_ThreadStop = false;
                m_Thread = new Thread(RunThread);
                m_Thread.Name = "Heartbeat Service Thread";
                m_Thread.Start();
            }
        }

        internal void Stop() {
            m_ThreadStop = true;
            if (m_Thread != null) {
                if (!m_Thread.Join(10000)) {
                    m_Thread.Abort();
                }
                m_Thread = null;
            }
        }

        private void RunThread() {
            while (!m_ThreadStop) {
                try {
                    m_UdpSender.Send(m_PacketBuffer);
                }
                catch (Exception e) {      
                    HeartbeatService.ServiceEventLog.WriteEntry(e.ToString(),EventLogEntryType.Warning);
                    return;
                }
                Thread.Sleep(m_Period);
            }
        }

        #region IDisposable Members

        public void Dispose() {
            if (!m_Disposed) {
                Stop();
                if (m_UdpSender != null) {
                    m_UdpSender.Dispose();
                    m_UdpSender = null;
                }
                m_Disposed = true;
            }
        }

        #endregion
    }
}
