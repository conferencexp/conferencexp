using System;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    internal class AppConfig
    {
        private const string reflector = "MSR.LST.Reflector.";
        public const string MCLoopbackOff = reflector + "MCLoopbackOff";
        public const string UnicastPort = reflector + "UnicastRTPListenPort";
        public const string MultipleInterfaceSupport = reflector + "MultipleInterfaceSupport";
        public const string MulticastInterfaceRouteIndex = reflector + "MulticastInterfaceRouteIndex";
        public const string MulticastInterfaceIP = reflector + "MulticastInterfaceIP";
        public const string UnicastInterfaceIP = reflector + "UnicastInterfaceIP";
        public const string IPv6MulticastInterfaceIP = reflector + "IPv6MulticastInterfaceIP";
        public const string IPv6UnicastInterfaceIP = reflector + "IPv6UnicastInterfaceIP";
        public const string IPv6Support = reflector + "IPv6Support";
        public const string IPv4Support = reflector + "IPv4Support";
        public const string TimeOutMinutes = reflector + "TimeOutMinutes";
        public const string UICulture = reflector + "UICulture";
        public const string SendMulticast = reflector + "SendMulticast";

        public const string ParentReflector = reflector + "MultiReflector";
       // public const string ParentReflectorPort = reflector + "MultiReflectorPort";


        private AppConfig(){}
    }
}
