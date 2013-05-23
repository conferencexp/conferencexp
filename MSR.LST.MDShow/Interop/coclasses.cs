using System;


namespace MSR.LST.MDShow
{
    // [HKEY_CLASSES_ROOT\CLSID\{62BE5D10-60EB-11d0-BD3B-00A0C911CE86}]
    // @="System Device Enumerator"
    // "Version"=dword:00000007
    //
    // [HKEY_CLASSES_ROOT\CLSID\{62BE5D10-60EB-11d0-BD3B-00A0C911CE86}\InprocServer32]
    // @="C:\\WINDOWS\\System32\\devenum.dll"
    // "ThreadingModel"="Both"
    class CreateDeviceEnumClass
    {
        private static Guid CLSID_DeviceEnum = new Guid("62BE5D10-60EB-11d0-BD3B-00A0C911CE86");

        public static ICreateDevEnum CreateInstance()
        {
            return (ICreateDevEnum)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_DeviceEnum, true));
        }
    }

    // [HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{BF87B6E1-8C27-11D0-B3F0-00AA003761C5}]
    // @="Capture Graph Builder 2"
    //
    // [HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{BF87B6E1-8C27-11D0-B3F0-00AA003761C5}\InprocServer32]
    // @="C:\\WINDOWS\\System32\\qcap.dll"
    // "ThreadingModel"="Both"
    public abstract class CaptureGraphBuilder2Class
    {
        private static Guid CLSID_CaptureGraphBuilder2 = new Guid("BF87B6E1-8C27-11D0-B3F0-00AA003761C5");

        public static ICaptureGraphBuilder2 CreateInstance()
        {
            return (ICaptureGraphBuilder2)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_CaptureGraphBuilder2, true));
        }
    }

    // [HKEY_CLASSES_ROOT\CLSID\{CDA42200-BD88-11d0-BD4E-00A0C911CE86}]
    // @="Filter Mapper2"
    //
    // [HKEY_CLASSES_ROOT\CLSID\{CDA42200-BD88-11d0-BD4E-00A0C911CE86}\InprocServer32]
    // @="C:\\WINDOWS\\System32\\quartz.dll"
    // "ThreadingModel"="Both"
    public abstract class FilterMapper2Class
    {
        private static Guid CLSID_FilterMapper2 = new Guid("CDA42200-BD88-11d0-BD4E-00A0C911CE86");

        public static IFilterMapper2 CreateInstance()
        {
            return (IFilterMapper2)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_FilterMapper2, true));
        }
    }

    /// <summary>
    /// Class for creating the RtpRenderer DShow filter
    /// 
    /// Guid was taken from RtpRenderer.idl in the DShow\CxpRtpFilters folder
    /// </summary>
    public abstract class RtpRendererClass
    {
        private static Guid CLSID_RtpRenderer = new Guid("CC7CE9F7-0927-4be6-84E7-305C4E45D3E1");

        public static IBaseFilter CreateInstance()
        {
            return (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_RtpRenderer, true));
        }
    }

    /// <summary>
    /// Class for creating the RtpSource DShow filter
    /// 
    /// Guid was taken from RtpSource.idl in the DShow\CxpRtpFilters folder
    /// </summary>
    public abstract class RtpSourceClass
    {
        private static Guid CLSID_RtpSource = new Guid("158C4421-945F-4826-8851-2459D92CCF07");

        public static IBaseFilter CreateInstance()
        {
            return (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_RtpSource, true));
        }
    }

    /// <summary>
    /// Class for creating the CLSID_ScreenScraper DShow filter
    /// 
    /// Guid was taken from CLSID_ScreenScraper.idl in the DShow\ScreenScraper folder
    /// </summary>
    public abstract class ScreenScraperClass
    {
        private static Guid CLSID_ScreenScraper = new Guid("66BA5965-3092-4223-8649-496E7AB67F25");

        public static IBaseFilter CreateInstance()
        {
            return (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_ScreenScraper, true));
        }
    }
}
