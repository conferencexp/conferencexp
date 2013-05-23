using System;


namespace OneNote
{
    using System.Runtime.InteropServices;

    // Declare IDataImporter as a COM interface which 
    // derives from IDispatch interface:
    [Guid("F56A67BB-243F-4927-B987-F57B3D9DBEFE"),
     InterfaceType(ComInterfaceType.InterfaceIsDual)] 
    interface ISimpleImporter
    { 
        // Note that IUnknown Interface members are NOT listed here:
        void Import([In, MarshalAs(UnmanagedType.BStr)] string xml);
        void NavigateToPage([In, MarshalAs(UnmanagedType.BStr)] string path,
                            [In, MarshalAs(UnmanagedType.BStr)] string guid);
    }

    [ComImport, Guid("22148139-F1FC-4EB0-B237-DFCD8A38EFFC")]
    class ImportServer{}
}
