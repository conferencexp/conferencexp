
//  Microsoft (R) .NET Framework IL Disassembler.  Version 1.1.4322.573
//  Copyright (C) Microsoft Corporation 1998-2002. All rights reserved.

.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 1:0:5000:0
}
.assembly filters.interop
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.ImportedFromTypeLibAttribute::.ctor(string) = ( 01 00 0A 63 78 70 66 69 6C 74 65 72 73 00 00 )    // ...cxpfilters..
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 32 31 65 39 38 38 66 63 2D 66 62 66 36   // ..$21e988fc-fbf6
                                                                                                  2D 34 30 36 31 2D 38 31 37 33 2D 62 62 65 35 61   // -4061-8173-bbe5a
                                                                                                  35 36 33 61 65 38 38 00 00 )                      // 563ae88..
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module cxpfilters.dll
// MVID: {046391E2-1716-48AE-86B3-922B35F6DB58}
.imagebase 0x00400000
.subsystem 0x00000003
.file alignment 512
.corflags 0x00000001
// Image base: 0x02c40000
//
// ============== CLASS STRUCTURE DECLARATION ==================
//
.namespace MSR.LST.MDShow.Filters
{
  .class interface public abstract auto ansi import IRtpSource
  {
  } // end of class IRtpSource

  .class public auto ansi import RtpSourceClass
         extends [mscorlib]System.Object
         implements MSR.LST.MDShow.Filters.IRtpSource,
                    MSR.LST.MDShow.Filters.RtpSource
  {
  } // end of class RtpSourceClass

  .class interface public abstract auto ansi import RtpSource
         implements MSR.LST.MDShow.Filters.IRtpSource
  {
  } // end of class RtpSource

  .class interface public abstract auto ansi import IRtpRenderer
  {
  } // end of class IRtpRenderer

  .class public auto ansi import RtpRendererClass
         extends [mscorlib]System.Object
         implements MSR.LST.MDShow.Filters.IRtpRenderer,
                    MSR.LST.MDShow.Filters.RtpRenderer
  {
  } // end of class RtpRendererClass

  .class interface public abstract auto ansi import RtpRenderer
         implements MSR.LST.MDShow.Filters.IRtpRenderer
  {
  } // end of class RtpRenderer

  .class interface public abstract auto ansi import IScreenScraper
  {
  } // end of class IScreenScraper

  .class public sequential ansi sealed beforefieldinit _RemotableHandle
         extends [mscorlib]System.ValueType
  {
  } // end of class _RemotableHandle

  .class public explicit ansi sealed __MIDL_IWinTypes_0009
         extends [mscorlib]System.ValueType
  {
  } // end of class __MIDL_IWinTypes_0009

  .class public auto ansi import ScreenScraperClass
         extends [mscorlib]System.Object
         implements MSR.LST.MDShow.Filters.IScreenScraper,
                    MSR.LST.MDShow.Filters.ScreenScraper
  {
  } // end of class ScreenScraperClass

  .class interface public abstract auto ansi import ScreenScraper
         implements MSR.LST.MDShow.Filters.IScreenScraper
  {
  } // end of class ScreenScraper

} // end of namespace MSR.LST.MDShow.Filters


// =============================================================


// =============== GLOBAL FIELDS AND METHODS ===================


// =============================================================


// =============== CLASS MEMBERS DECLARATION ===================
//   note that class flags, 'extends' and 'implements' clauses
//          are provided here for information only

.namespace MSR.LST.MDShow.Filters
{
  .class interface public abstract auto ansi import IRtpSource
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 37 32 45 34 32 41 36 31 2D 36 41 41 31   // ..$72E42A61-6AA1
                                                                                                    2D 34 35 41 44 2D 42 41 32 31 2D 37 33 30 36 35   // -45AD-BA21-73065
                                                                                                    41 38 39 37 46 35 43 00 00 )                      // A897F5C..
    .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 80 00 00 00 ) 
    .custom instance void [mscorlib]System.Runtime.InteropServices.InterfaceTypeAttribute::.ctor(int16) = ( 01 00 01 00 00 00 ) 
    .method public hidebysig newslot abstract virtual 
            instance void  Initialize([in] object  marshal( iunknown) pRtpStream) runtime managed internalcall
    {
    } // end of method IRtpSource::Initialize

  } // end of class IRtpSource

  .class public auto ansi import RtpSourceClass
         extends [mscorlib]System.Object
         implements MSR.LST.MDShow.Filters.IRtpSource,
                    MSR.LST.MDShow.Filters.RtpSource
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 02 00 00 00 ) 
    .custom instance void [mscorlib]System.Runtime.InteropServices.ClassInterfaceAttribute::.ctor(int16) = ( 01 00 00 00 00 00 ) 
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 31 35 38 43 34 34 32 31 2D 39 34 35 46   // ..$158C4421-945F
                                                                                                    2D 34 38 32 36 2D 38 38 35 31 2D 32 34 35 39 44   // -4826-8851-2459D
                                                                                                    39 32 43 43 46 30 37 00 00 )                      // 92CCF07..
    .method public specialname rtspecialname 
            instance void  .ctor() runtime managed internalcall
    {
    } // end of method RtpSourceClass::.ctor

    .method public hidebysig newslot virtual 
            instance void  Initialize([in] object  marshal( iunknown) pRtpStream) runtime managed internalcall
    {
      .override MSR.LST.MDShow.Filters.IRtpSource::Initialize
    } // end of method RtpSourceClass::Initialize

  } // end of class RtpSourceClass

  .class interface public abstract auto ansi import RtpSource
         implements MSR.LST.MDShow.Filters.IRtpSource
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.CoClassAttribute::.ctor(class [mscorlib]System.Type) = ( 01 00 25 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ..%MSR.LST.MDSho
                                                                                                                            77 2E 46 69 6C 74 65 72 73 2E 52 74 70 53 6F 75   // w.Filters.RtpSou
                                                                                                                            72 63 65 43 6C 61 73 73 00 00 )                   // rceClass..
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 37 32 45 34 32 41 36 31 2D 36 41 41 31   // ..$72E42A61-6AA1
                                                                                                    2D 34 35 41 44 2D 42 41 32 31 2D 37 33 30 36 35   // -45AD-BA21-73065
                                                                                                    41 38 39 37 46 35 43 00 00 )                      // A897F5C..
  } // end of class RtpSource

  .class interface public abstract auto ansi import IRtpRenderer
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 41 33 44 43 30 42 38 2D 36 33 44 35   // ..$5A3DC0B8-63D5
                                                                                                    2D 34 34 30 42 2D 38 33 34 44 2D 30 37 35 43 34   // -440B-834D-075C4
                                                                                                    42 43 35 45 37 42 37 00 00 )                      // BC5E7B7..
    .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 80 00 00 00 ) 
    .custom instance void [mscorlib]System.Runtime.InteropServices.InterfaceTypeAttribute::.ctor(int16) = ( 01 00 01 00 00 00 ) 
    .method public hidebysig newslot abstract virtual 
            instance void  Initialize([in] object  marshal( iunknown) pRtpSender) runtime managed internalcall
    {
    } // end of method IRtpRenderer::Initialize

  } // end of class IRtpRenderer

  .class public auto ansi import RtpRendererClass
         extends [mscorlib]System.Object
         implements MSR.LST.MDShow.Filters.IRtpRenderer,
                    MSR.LST.MDShow.Filters.RtpRenderer
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 02 00 00 00 ) 
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 43 43 37 43 45 39 46 37 2D 30 39 32 37   // ..$CC7CE9F7-0927
                                                                                                    2D 34 42 45 36 2D 38 34 45 37 2D 33 30 35 43 34   // -4BE6-84E7-305C4
                                                                                                    45 34 35 44 33 45 31 00 00 )                      // E45D3E1..
    .custom instance void [mscorlib]System.Runtime.InteropServices.ClassInterfaceAttribute::.ctor(int16) = ( 01 00 00 00 00 00 ) 
    .method public specialname rtspecialname 
            instance void  .ctor() runtime managed internalcall
    {
    } // end of method RtpRendererClass::.ctor

    .method public hidebysig newslot virtual 
            instance void  Initialize([in] object  marshal( iunknown) pRtpSender) runtime managed internalcall
    {
      .override MSR.LST.MDShow.Filters.IRtpRenderer::Initialize
    } // end of method RtpRendererClass::Initialize

  } // end of class RtpRendererClass

  .class interface public abstract auto ansi import RtpRenderer
         implements MSR.LST.MDShow.Filters.IRtpRenderer
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 41 33 44 43 30 42 38 2D 36 33 44 35   // ..$5A3DC0B8-63D5
                                                                                                    2D 34 34 30 42 2D 38 33 34 44 2D 30 37 35 43 34   // -440B-834D-075C4
                                                                                                    42 43 35 45 37 42 37 00 00 )                      // BC5E7B7..
    .custom instance void [mscorlib]System.Runtime.InteropServices.CoClassAttribute::.ctor(class [mscorlib]System.Type) = ( 01 00 27 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ..'MSR.LST.MDSho
                                                                                                                            77 2E 46 69 6C 74 65 72 73 2E 52 74 70 52 65 6E   // w.Filters.RtpRen
                                                                                                                            64 65 72 65 72 43 6C 61 73 73 00 00 )             // dererClass..
  } // end of class RtpRenderer

  .class interface public abstract auto ansi import IScreenScraper
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 41 44 42 43 46 33 33 46 2D 37 30 41 33   // ..$ADBCF33F-70A3
                                                                                                    2D 34 32 39 30 2D 41 38 30 31 2D 37 42 34 44 34   // -4290-A801-7B4D4
                                                                                                    33 46 46 30 42 41 43 00 00 )                      // 3FF0BAC..
    .custom instance void [mscorlib]System.Runtime.InteropServices.InterfaceTypeAttribute::.ctor(int16) = ( 01 00 01 00 00 00 ) 
    .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 80 00 00 00 ) 
    .method public hidebysig newslot abstract virtual 
            instance void  Handle([in] valuetype MSR.LST.MDShow.Filters._RemotableHandle& hWnd) runtime managed internalcall
    {
      .param [1]
      .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 1F 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                              77 2E 46 69 6C 74 65 72 73 2E 77 69 72 65 48 57   // w.Filters.wireHW
                                                                                                              4E 44 00 00 )                                     // ND..
    } // end of method IScreenScraper::Handle

    .method public hidebysig newslot abstract virtual 
            instance void  FrameRate([in] int32 FrameRate) runtime managed internalcall
    {
    } // end of method IScreenScraper::FrameRate

  } // end of class IScreenScraper

  .class public sequential ansi sealed beforefieldinit _RemotableHandle
         extends [mscorlib]System.ValueType
  {
    .pack 4
    .size 0
    .field public int32 fContext
    .field public valuetype MSR.LST.MDShow.Filters.__MIDL_IWinTypes_0009 u
  } // end of class _RemotableHandle

  .class public explicit ansi sealed __MIDL_IWinTypes_0009
         extends [mscorlib]System.ValueType
  {
    .pack 4
    .size 0
    .field [0] public int32 hInproc
    .field [0] public int32 hRemote
  } // end of class __MIDL_IWinTypes_0009

  .class public auto ansi import ScreenScraperClass
         extends [mscorlib]System.Object
         implements MSR.LST.MDShow.Filters.IScreenScraper,
                    MSR.LST.MDShow.Filters.ScreenScraper
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 02 00 00 00 ) 
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 36 36 42 41 35 39 36 35 2D 33 30 39 32   // ..$66BA5965-3092
                                                                                                    2D 34 32 32 33 2D 38 36 34 39 2D 34 39 36 45 37   // -4223-8649-496E7
                                                                                                    41 42 36 37 46 32 35 00 00 )                      // AB67F25..
    .custom instance void [mscorlib]System.Runtime.InteropServices.ClassInterfaceAttribute::.ctor(int16) = ( 01 00 00 00 00 00 ) 
    .method public specialname rtspecialname 
            instance void  .ctor() runtime managed internalcall
    {
    } // end of method ScreenScraperClass::.ctor

    .method public hidebysig newslot virtual 
            instance void  Handle([in] valuetype MSR.LST.MDShow.Filters._RemotableHandle& hWnd) runtime managed internalcall
    {
      .param [1]
      .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 1F 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                              77 2E 46 69 6C 74 65 72 73 2E 77 69 72 65 48 57   // w.Filters.wireHW
                                                                                                              4E 44 00 00 )                                     // ND..
      .override MSR.LST.MDShow.Filters.IScreenScraper::Handle
    } // end of method ScreenScraperClass::Handle

    .method public hidebysig newslot virtual 
            instance void  FrameRate([in] int32 FrameRate) runtime managed internalcall
    {
      .override MSR.LST.MDShow.Filters.IScreenScraper::FrameRate
    } // end of method ScreenScraperClass::FrameRate

  } // end of class ScreenScraperClass

  .class interface public abstract auto ansi import ScreenScraper
         implements MSR.LST.MDShow.Filters.IScreenScraper
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 41 44 42 43 46 33 33 46 2D 37 30 41 33   // ..$ADBCF33F-70A3
                                                                                                    2D 34 32 39 30 2D 41 38 30 31 2D 37 42 34 44 34   // -4290-A801-7B4D4
                                                                                                    33 46 46 30 42 41 43 00 00 )                      // 3FF0BAC..
    .custom instance void [mscorlib]System.Runtime.InteropServices.CoClassAttribute::.ctor(class [mscorlib]System.Type) = ( 01 00 29 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ..)MSR.LST.MDSho
                                                                                                                            77 2E 46 69 6C 74 65 72 73 2E 53 63 72 65 65 6E   // w.Filters.Screen
                                                                                                                            53 63 72 61 70 65 72 43 6C 61 73 73 00 00 )       // ScraperClass..
  } // end of class ScreenScraper


// =============================================================

} // end of namespace MSR.LST.MDShow.Filters

//*********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file D:\Jason\SD\lst\Source\MSR.LST.MDShow\Interop\filters.interop.orig.res
