using System;
using System.Collections;
using System.Net;


namespace MSR.LST.Net
{
    public delegate void ReceivedFromCallback (BufferChunk bufferChunk, EndPoint endPont);
    public interface INetworkListener : IDisposable
    {
        void Receive(BufferChunk packetBuffer);
        void ReceiveFrom(BufferChunk packetBuffer, out EndPoint sender);
        void AsyncReceiveFrom(Queue queue, ReceivedFromCallback callback);
        IPAddress ExternalInterface { get; }
#if FaultInjection
        int DropPacketsReceivedPercent { get; set; }
#endif
    }

    public interface INetworkSender : IDisposable
    {
        void Send(BufferChunk packetBuffer);
        IPAddress ExternalInterface { get; }
        short DelayBetweenPackets { get; set; }
#if FaultInjection
        int DropPacketsSentPercent { get; set; }
#endif
    }
}
