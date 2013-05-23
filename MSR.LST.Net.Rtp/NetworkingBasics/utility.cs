using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;


namespace MSR.LST.Net
{
 
    /// <summary>
    /// Helper class to contain functions of use when working with mulitcast interfaces.
    /// </summary>
    [ComVisible(false)]
    public class Utility
    {
        //Pri1: Review for Exception Handling & Event Logging
        #region Private Constants
        private const short AF_INET = 2;
        private const short AF_INET6 = 23;
        private const int SIO_ROUTING_INTERFACE_QUERY = unchecked ((int) (0x40000000 | 0x80000000 | 0x08000000 | 20));
        private const string MulticastIPAddress = "233.45.17.171";  // Use the Pipecleaner address
        private const short port = 5004;
        #endregion
        #region members
        private static IPAddress externalInterface = null;
        public static IPAddress multicastIP;
        #endregion
        #region Private Structures
        /// <summary>
        /// DotNet definition of sockaddr_in, the Winsock2 structure that roughly corresponds to an EndPoint structure from System.Net.  Used to interop with Winsock2,
        /// in this case to call IOControl for SIO_ROUTING_INTERFACE_QUERY.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
            private class sockaddr_in
        {
            public short sin_family = AF_INET;
            public short sin_port = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]       
            public byte[] sin_addr = new Byte[4];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]       
            public byte[] sin_zero = new Byte[8];
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
            private class sockaddr_in6
        {
            public short sin_family = AF_INET6;
            public short sin_port = 0;
            public uint sin6_flowinfo = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]       
            public byte[] sin_addr = new Byte[16];
            public uint sin6_scope_id = 0;

        }
        #endregion
        #region Public Static Methods

        /// <summary>
        /// Find the interface we should be binding to to receive a multicast stream by using Socket.IOControl to call
        /// SIO_ROUTING_INTEFACE_QUERY (see WSAIoctl in Winsock2 documentation) passing in a known multicast address.
        /// </summary>
        /// <returns>IPAddress containing the local multicast interface</returns>
        /// <example>
        /// IPAddress ifAddress = MulticastInterface.GetLocalMulticastInterface();
        /// </example>
        public static IPAddress GetLocalMulticastInterface()
        {
            if (externalInterface != null)
            {
                return externalInterface;
            }
            else if (multicastIP != null)
            {
                return GetLocalRoutingInterface(multicastIP);
            }
            else
            {
                return null;
            }
        }

        //Removed, replace this functionality by using bind(IPAddress.Any, ... or IPAddress.IPv6Any);
        public static IPAddress GetLocalRoutingInterface(IPAddress ipAddress)
        {
            Socket sock = null;
            IntPtr ptrInAddr = IntPtr.Zero;
            IntPtr ptrOutAddr = IntPtr.Zero;

            if ( ipAddress.AddressFamily == AddressFamily.InterNetwork )
            {
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sockaddr_in inAddr = new sockaddr_in();
                sockaddr_in outAddr = new sockaddr_in();

                try 
                {
                    ipAddress.GetAddressBytes().CopyTo(inAddr.sin_addr, 0);

                    // create a sockaddr_in function for our destination IP address
                    inAddr.sin_port = IPAddress.HostToNetworkOrder(port);

                    // create an block of unmanaged memory for use by Marshal.StructureToPtr.  We seem to need to do this, even though
                    // StructureToPtr will go ahead and release/reallocate the memory
                    ptrInAddr = Marshal.AllocCoTaskMem(Marshal.SizeOf(inAddr));

                    // Copy inAddr from managed to unmanaged
                    Marshal.StructureToPtr(inAddr, ptrInAddr, false);

                    // Create a managed byte array to hold the structure, but in byte array form
                    byte[] byteInAddr = new byte[Marshal.SizeOf(inAddr)];

                    // Copy the structure from unmanaged ptr into managed byte array
                    Marshal.Copy(ptrInAddr, byteInAddr, 0, byteInAddr.Length);

                    // Create a second managed byte array to hold the output sockaddr_in structure
                    byte[] byteOutAddr = new byte[Marshal.SizeOf(inAddr)];

                    // Make the call to IOControl, asking for the Interface we should use if we want to route a packet to inAddr
                    sock.IOControl(
                        SIO_ROUTING_INTERFACE_QUERY,
                        byteInAddr,
                        byteOutAddr
                        );

                    // create the memory placeholder for our local interface
          
                    // Copy the results from the byteOutAddr into an unmanaged pointer
                    ptrOutAddr = Marshal.AllocCoTaskMem(Marshal.SizeOf(outAddr));
                    Marshal.Copy(byteOutAddr, 0, ptrOutAddr, byteOutAddr.Length);
          
                    // Copy the data from the unmanaged pointer to the ourAddr structure
                    Marshal.PtrToStructure(ptrOutAddr, outAddr);
                }
                catch(SocketException se)
                {
                    // Perhaps there were no interfaces present, AKA No Network Adapters enabled/installed and connected to media (wired or wireless)
                    EventLog.WriteEntry("RtpSession", se.ToString(), EventLogEntryType.Warning, 99);
                   
                }
                finally
                {
                    // Release the socket
                    sock = null;

                    Marshal.FreeCoTaskMem(ptrInAddr);
                    Marshal.FreeCoTaskMem(ptrOutAddr);
                }

                // Return an IPAddress structure that is initialized with the value of the IP address contained in the outAddr structure
                if (outAddr != null)
                {
                    int len = outAddr.sin_addr.Length;

                    // Have to convert the byte[] to a uint.  It turns out that the IPAddress ctor won't create an IPv4 address from four bytes, it only uses the byte[] ctor for 16 byte IPv6 construction?!?
                    uint ipAsInt = 0;

                    for (int i = len; i > 0; i--)
                    {
                        ipAsInt = ipAsInt * 256 + outAddr.sin_addr[i - 1];
                    }
                    externalInterface = new IPAddress(ipAsInt);
                    return(new IPAddress(ipAsInt));
                }
                else
                {
                    return null;
                }

            }

            if ( Socket.OSSupportsIPv6 && ipAddress.AddressFamily == AddressFamily.InterNetworkV6 )
            {
                sock = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
                sockaddr_in6 inAddr = new sockaddr_in6();
                sockaddr_in6 outAddr = new sockaddr_in6();

                try 
                {
                    ipAddress.GetAddressBytes().CopyTo(inAddr.sin_addr, 0);

                    // create a sockaddr_in function for our destination IP address
                    inAddr.sin_port = IPAddress.HostToNetworkOrder(port);

                    // create an block of unmanaged memory for use by Marshal.StructureToPtr.  We seem to need to do this, even though
                    // StructureToPtr will go ahead and release/reallocate the memory
                    ptrInAddr = Marshal.AllocCoTaskMem(Marshal.SizeOf(inAddr));

                    // Copy inAddr from managed to unmanaged
                    Marshal.StructureToPtr(inAddr, ptrInAddr, false);

                    // Create a managed byte array to hold the structure, but in byte array form
                    byte[] byteInAddr = new byte[Marshal.SizeOf(inAddr)];

                    // Copy the structure from unmanaged ptr into managed byte array
                    Marshal.Copy(ptrInAddr, byteInAddr, 0, byteInAddr.Length);

                    // Create a second managed byte array to hold the output sockaddr_in structure
                    byte[] byteOutAddr = new byte[Marshal.SizeOf(inAddr)];

                    // Make the call to IOControl, asking for the Interface we should use if we want to route a packet to inAddr

                    sock.IOControl(
                        SIO_ROUTING_INTERFACE_QUERY,
                        byteInAddr,
                        byteOutAddr
                        );

                    // create the memory placeholder for our local interface
                    // Copy the results from the byteOutAddr into an unmanaged pointer
                    ptrOutAddr = Marshal.AllocCoTaskMem(Marshal.SizeOf(outAddr));
                    Marshal.Copy(byteOutAddr, 0, ptrOutAddr, byteOutAddr.Length);
          
                    // Copy the data from the unmanaged pointer to the ourAddr structure
                    Marshal.PtrToStructure(ptrOutAddr, outAddr);
                }
                catch (SocketException se)
                {
                    // Perhaps there were no interfaces present, AKA No Network Adapters enabled/installed and connected to media (wired or wireless)
                    EventLog.WriteEntry("RtpSession", se.ToString(), EventLogEntryType.Warning, 99);
                }
                finally
                {
                    // Release the socket
                    sock = null;

                    Marshal.FreeCoTaskMem(ptrInAddr);
                    Marshal.FreeCoTaskMem(ptrOutAddr);
                }

                // Return an IPAddress structure that is initialized with the value of the IP address contained in the outAddr structure
                if (outAddr != null)
                {
                    externalInterface = new IPAddress(outAddr.sin_addr, outAddr.sin6_scope_id);
                    return (new IPAddress(outAddr.sin_addr, outAddr.sin6_scope_id));
                }
                else
                {
                    return null;
                }

            }
            return null;

        }
        
        /// <summary>
        /// Checks if the IP is a multicast IP
        /// </summary>
        /// <remarks>
        /// Works for both IPv4 and IPv6
        /// </remarks>
        public static bool IsMulticast(IPAddress ipAddress)
        {
            bool isMulticast;

            if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                // In IPv6 Multicast addresses first byte is 0xFF
                byte[] ipv6Bytes = ipAddress.GetAddressBytes();
                isMulticast = (ipv6Bytes[0] == 0xff);
            }
            else // IPv4
            {
                Debug.Assert(ipAddress.AddressFamily == AddressFamily.InterNetwork);

                // In IPv4 Multicast addresses first byte is between 224 and 239
                byte[] addressBytes = ipAddress.GetAddressBytes();
                isMulticast = (addressBytes[0] >= 224) && (addressBytes[0] <= 239);
            }

            return isMulticast;
        }

        /// <summary>
        /// Checks if the IP is a multicast IP
        /// </summary>
        /// <remarks>
        /// Works for both IPv4 and IPv6.  Note that this overload is slightly less effecient than the
        /// alternative, as only the IPAddress is needed to determine if an IP is multicast.
        /// </remarks>
        public static bool IsMulticast(IPEndPoint ipEndPoint)
        {
            return IsMulticast(ipEndPoint.Address);
        }

        #endregion
    }
}
