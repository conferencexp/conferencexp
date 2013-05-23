
//  Microsoft (R) .NET Framework IL Disassembler.  Version 2.0.50727.42
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v1.1.4322
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 1:0:5000:0
}
.assembly control.interop
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 61 38 36 38 62 30 2D 30 61 64 34   // ..$56a868b0-0ad4
                                                                                                  2D 31 31 63 65 2D 62 30 33 61 2D 30 30 32 30 61   // -11ce-b03a-0020a
                                                                                                  66 30 62 61 37 37 30 00 00 )                      // f0ba770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.ImportedFromTypeLibAttribute::.ctor(string) = ( 01 00 0D 51 75 61 72 74 7A 54 79 70 65 4C 69 62   // ...QuartzTypeLib
                                                                                                                 00 00 ) 
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module QuartzTypeLib.dll
// MVID: {52BD996F-8F14-4513-ABE4-7421E8A171D9}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x03080000


// =============== CLASS MEMBERS DECLARATION ===================

.class interface public abstract auto ansi import MSR.LST.MDShow.IAMCollection
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 39 2D 30 41 44 34   // ..$56A868B9-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Count() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IAMCollection::get_Count

  .method public hidebysig newslot abstract virtual 
          instance void  Item([in] int32 lItem,
                              [out] object&  marshal( iunknown ) ppUnk) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IAMCollection::Item

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( iunknown ) 
          get__NewEnum() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IAMCollection::get__NewEnum

  .property int32 Count()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IAMCollection::get_Count()
  } // end of property IAMCollection::Count
  .property object _NewEnum()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IAMCollection::get__NewEnum()
  } // end of property IAMCollection::_NewEnum
} // end of class MSR.LST.MDShow.IAMCollection

.class interface public abstract auto ansi import MSR.LST.MDShow.IMediaControl
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 31 2D 30 41 44 34   // ..$56A868B1-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot abstract virtual 
          instance void  Run() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::Run

  .method public hidebysig newslot abstract virtual 
          instance void  Pause() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::Pause

  .method public hidebysig newslot abstract virtual 
          instance void  Stop() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::Stop

  .method public hidebysig newslot abstract virtual 
          instance void  GetState([in] int32 msTimeout,
                                  [out] int32& pfs) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::GetState

  .method public hidebysig newslot abstract virtual 
          instance void  RenderFile([in] string  marshal( bstr) strFilename) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::RenderFile

  .method public hidebysig newslot abstract virtual 
          instance void  AddSourceFilter([in] string  marshal( bstr) strFilename,
                                         [out] object&  marshal( idispatch ) ppUnk) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::AddSourceFilter

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( idispatch ) 
          get_FilterCollection() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::get_FilterCollection

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( idispatch ) 
          get_RegFilterCollection() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::get_RegFilterCollection

  .method public hidebysig newslot abstract virtual 
          instance void  StopWhenReady() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 08 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaControl::StopWhenReady

  .property object FilterCollection()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IMediaControl::get_FilterCollection()
  } // end of property IMediaControl::FilterCollection
  .property object RegFilterCollection()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IMediaControl::get_RegFilterCollection()
  } // end of property IMediaControl::RegFilterCollection
} // end of class MSR.LST.MDShow.IMediaControl

.class interface public abstract auto ansi import MSR.LST.MDShow.IMediaEvent
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 36 2D 30 41 44 34   // ..$56A868B6-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .method public hidebysig newslot abstract virtual 
          instance void  GetEventHandle([out] int32& hEvent) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IMediaEvent::GetEventHandle

  .method public hidebysig newslot abstract virtual 
          instance void  GetEvent([out] int32& lEventCode,
                                  [out] int32& lParam1,
                                  [out] int32& lParam2,
                                  [in] int32 msTimeout) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .param [2]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IMediaEvent::GetEvent

  .method public hidebysig newslot abstract virtual 
          instance void  WaitForCompletion([in] int32 msTimeout,
                                           [out] int32& pEvCode) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaEvent::WaitForCompletion

  .method public hidebysig newslot abstract virtual 
          instance void  CancelDefaultHandling([in] int32 lEvCode) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaEvent::CancelDefaultHandling

  .method public hidebysig newslot abstract virtual 
          instance void  RestoreDefaultHandling([in] int32 lEvCode) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaEvent::RestoreDefaultHandling

  .method public hidebysig newslot abstract virtual 
          instance void  FreeEventParams([in] int32 lEvCode,
                                         [in] int32 lParam1,
                                         [in] int32 lParam2) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
    .param [2]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IMediaEvent::FreeEventParams

} // end of class MSR.LST.MDShow.IMediaEvent

.class interface public abstract auto ansi import MSR.LST.MDShow.IMediaEventEx
       implements MSR.LST.MDShow.IMediaEvent
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 43 30 2D 30 41 44 34   // ..$56A868C0-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 00 10 00 00 ) 
  .method public hidebysig newslot abstract virtual 
          instance void  GetEventHandle([out] int32& hEvent) runtime managed internalcall
  {
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IMediaEventEx::GetEventHandle

  .method public hidebysig newslot abstract virtual 
          instance void  GetEvent([out] int32& lEventCode,
                                  [out] int32& lParam1,
                                  [out] int32& lParam2,
                                  [in] int32 msTimeout) runtime managed internalcall
  {
    .param [2]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IMediaEventEx::GetEvent

  .method public hidebysig newslot abstract virtual 
          instance void  WaitForCompletion([in] int32 msTimeout,
                                           [out] int32& pEvCode) runtime managed internalcall
  {
  } // end of method IMediaEventEx::WaitForCompletion

  .method public hidebysig newslot abstract virtual 
          instance void  CancelDefaultHandling([in] int32 lEvCode) runtime managed internalcall
  {
  } // end of method IMediaEventEx::CancelDefaultHandling

  .method public hidebysig newslot abstract virtual 
          instance void  RestoreDefaultHandling([in] int32 lEvCode) runtime managed internalcall
  {
  } // end of method IMediaEventEx::RestoreDefaultHandling

  .method public hidebysig newslot abstract virtual 
          instance void  FreeEventParams([in] int32 lEvCode,
                                         [in] int32 lParam1,
                                         [in] int32 lParam2) runtime managed internalcall
  {
    .param [2]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IMediaEventEx::FreeEventParams

  .method public hidebysig newslot abstract virtual 
          instance void  SetNotifyWindow([in] int32 hwnd,
                                         [in] int32 lMsg,
                                         [in] int32 lInstanceData) runtime managed internalcall
  {
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IMediaEventEx::SetNotifyWindow

  .method public hidebysig newslot abstract virtual 
          instance void  SetNotifyFlags([in] int32 lNoNotifyFlags) runtime managed internalcall
  {
  } // end of method IMediaEventEx::SetNotifyFlags

  .method public hidebysig newslot abstract virtual 
          instance void  GetNotifyFlags([out] int32& lplNoNotifyFlags) runtime managed internalcall
  {
  } // end of method IMediaEventEx::GetNotifyFlags

} // end of class MSR.LST.MDShow.IMediaEventEx

.class interface public abstract auto ansi import MSR.LST.MDShow.IMediaPosition
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 32 2D 30 41 44 34   // ..$56A868B2-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .method public hidebysig newslot specialname abstract virtual 
          instance float64  get_Duration() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::get_Duration

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_CurrentPosition([in] float64 pllTime) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::set_CurrentPosition

  .method public hidebysig newslot specialname abstract virtual 
          instance float64  get_CurrentPosition() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::get_CurrentPosition

  .method public hidebysig newslot specialname abstract virtual 
          instance float64  get_StopTime() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::get_StopTime

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_StopTime([in] float64 pllTime) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::set_StopTime

  .method public hidebysig newslot specialname abstract virtual 
          instance float64  get_PrerollTime() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::get_PrerollTime

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_PrerollTime([in] float64 pllTime) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::set_PrerollTime

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Rate([in] float64 pdRate) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::set_Rate

  .method public hidebysig newslot specialname abstract virtual 
          instance float64  get_Rate() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::get_Rate

  .method public hidebysig newslot abstract virtual 
          instance int32  CanSeekForward() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 09 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::CanSeekForward

  .method public hidebysig newslot abstract virtual 
          instance int32  CanSeekBackward() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0A 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaPosition::CanSeekBackward

  .property float64 Duration()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance float64 MSR.LST.MDShow.IMediaPosition::get_Duration()
  } // end of property IMediaPosition::Duration
  .property float64 CurrentPosition()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .get instance float64 MSR.LST.MDShow.IMediaPosition::get_CurrentPosition()
    .set instance void MSR.LST.MDShow.IMediaPosition::set_CurrentPosition(float64)
  } // end of property IMediaPosition::CurrentPosition
  .property float64 StopTime()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
    .get instance float64 MSR.LST.MDShow.IMediaPosition::get_StopTime()
    .set instance void MSR.LST.MDShow.IMediaPosition::set_StopTime(float64)
  } // end of property IMediaPosition::StopTime
  .property float64 PrerollTime()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
    .get instance float64 MSR.LST.MDShow.IMediaPosition::get_PrerollTime()
    .set instance void MSR.LST.MDShow.IMediaPosition::set_PrerollTime(float64)
  } // end of property IMediaPosition::PrerollTime
  .property float64 Rate()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
    .get instance float64 MSR.LST.MDShow.IMediaPosition::get_Rate()
    .set instance void MSR.LST.MDShow.IMediaPosition::set_Rate(float64)
  } // end of property IMediaPosition::Rate
} // end of class MSR.LST.MDShow.IMediaPosition

.class interface public abstract auto ansi import MSR.LST.MDShow.IBasicAudio
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 33 2D 30 41 44 34   // ..$56A868B3-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Volume([in] int32 plVolume) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicAudio::set_Volume

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Volume() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicAudio::get_Volume

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Balance([in] int32 plBalance) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicAudio::set_Balance

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Balance() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicAudio::get_Balance

  .property int32 Volume()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicAudio::set_Volume(int32)
    .get instance int32 MSR.LST.MDShow.IBasicAudio::get_Volume()
  } // end of property IBasicAudio::Volume
  .property int32 Balance()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicAudio::set_Balance(int32)
    .get instance int32 MSR.LST.MDShow.IBasicAudio::get_Balance()
  } // end of property IBasicAudio::Balance
} // end of class MSR.LST.MDShow.IBasicAudio

.class interface public abstract auto ansi import MSR.LST.MDShow.IVideoWindow
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 34 2D 30 41 44 34   // ..$56A868B4-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Caption([in] string  marshal( bstr) strCaption) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_Caption

  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_Caption() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_Caption

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_WindowStyle([in] int32 WindowStyle) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_WindowStyle

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_WindowStyle() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_WindowStyle

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_WindowStyleEx([in] int32 WindowStyleEx) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_WindowStyleEx

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_WindowStyleEx() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_WindowStyleEx

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_AutoShow([in] int32 AutoShow) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_AutoShow

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_AutoShow() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_AutoShow

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_WindowState([in] int32 WindowState) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 08 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_WindowState

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_WindowState() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 08 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_WindowState

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_BackgroundPalette([in] int32 pBackgroundPalette) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0A 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_BackgroundPalette

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_BackgroundPalette() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0A 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_BackgroundPalette

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Visible([in] int32 pVisible) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0C 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_Visible

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Visible() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0C 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_Visible

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Left([in] int32 pLeft) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0E 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_Left

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Left() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0E 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_Left

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Width([in] int32 pWidth) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 10 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_Width

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Width() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 10 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_Width

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Top([in] int32 pTop) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 12 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_Top

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Top() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 12 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_Top

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Height([in] int32 pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 14 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_Height

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Height() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 14 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_Height

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Owner([in] int32 Owner) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 16 00 02 60 00 00 )                         // .....`..
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IVideoWindow::set_Owner

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Owner() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 16 00 02 60 00 00 )                         // .....`..
    .param [0]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IVideoWindow::get_Owner

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_MessageDrain([in] int32 Drain) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 18 00 02 60 00 00 )                         // .....`..
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IVideoWindow::set_MessageDrain

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_MessageDrain() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 18 00 02 60 00 00 )                         // .....`..
    .param [0]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IVideoWindow::get_MessageDrain

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_BorderColor() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1A 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_BorderColor

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_BorderColor([in] int32 Color) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1A 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_BorderColor

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_FullScreenMode() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1C 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::get_FullScreenMode

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_FullScreenMode([in] int32 FullScreenMode) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1C 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::set_FullScreenMode

  .method public hidebysig newslot abstract virtual 
          instance void  SetWindowForeground([in] int32 Focus) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1E 00 02 60 00 00 )                         // .....`..
  } // end of method IVideoWindow::SetWindowForeground

  .method public hidebysig newslot abstract virtual 
          instance void  NotifyOwnerMessage([in] int32 hwnd,
                                            [in] int32 uMsg,
                                            [in] int32 wParam,
                                            [in] int32 lParam) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1F 00 02 60 00 00 )                         // .....`..
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [4]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
  } // end of method IVideoWindow::NotifyOwnerMessage

  .method public hidebysig newslot abstract virtual 
          instance void  SetWindowPosition([in] int32 Left,
                                           [in] int32 Top,
                                           [in] int32 Width,
                                           [in] int32 Height) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 20 00 02 60 00 00 )                         // .. ..`..
  } // end of method IVideoWindow::SetWindowPosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetWindowPosition([out] int32& pLeft,
                                           [out] int32& pTop,
                                           [out] int32& pWidth,
                                           [out] int32& pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 21 00 02 60 00 00 )                         // ..!..`..
  } // end of method IVideoWindow::GetWindowPosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetMinIdealImageSize([out] int32& pWidth,
                                              [out] int32& pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 22 00 02 60 00 00 )                         // .."..`..
  } // end of method IVideoWindow::GetMinIdealImageSize

  .method public hidebysig newslot abstract virtual 
          instance void  GetMaxIdealImageSize([out] int32& pWidth,
                                              [out] int32& pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 23 00 02 60 00 00 )                         // ..#..`..
  } // end of method IVideoWindow::GetMaxIdealImageSize

  .method public hidebysig newslot abstract virtual 
          instance void  GetRestorePosition([out] int32& pLeft,
                                            [out] int32& pTop,
                                            [out] int32& pWidth,
                                            [out] int32& pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 24 00 02 60 00 00 )                         // ..$..`..
  } // end of method IVideoWindow::GetRestorePosition

  .method public hidebysig newslot abstract virtual 
          instance void  HideCursor([in] int32 HideCursor) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 25 00 02 60 00 00 )                         // ..%..`..
  } // end of method IVideoWindow::HideCursor

  .method public hidebysig newslot abstract virtual 
          instance void  IsCursorHidden([out] int32& CursorHidden) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 26 00 02 60 00 00 )                         // ..&..`..
  } // end of method IVideoWindow::IsCursorHidden

  .property string Caption()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IVideoWindow::get_Caption()
    .set instance void MSR.LST.MDShow.IVideoWindow::set_Caption(string)
  } // end of property IVideoWindow::Caption
  .property int32 WindowStyle()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_WindowStyle(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_WindowStyle()
  } // end of property IVideoWindow::WindowStyle
  .property int32 WindowStyleEx()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_WindowStyleEx()
    .set instance void MSR.LST.MDShow.IVideoWindow::set_WindowStyleEx(int32)
  } // end of property IVideoWindow::WindowStyleEx
  .property int32 AutoShow()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_AutoShow(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_AutoShow()
  } // end of property IVideoWindow::AutoShow
  .property int32 WindowState()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 08 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_WindowState(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_WindowState()
  } // end of property IVideoWindow::WindowState
  .property int32 BackgroundPalette()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0A 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_BackgroundPalette()
    .set instance void MSR.LST.MDShow.IVideoWindow::set_BackgroundPalette(int32)
  } // end of property IVideoWindow::BackgroundPalette
  .property int32 Visible()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0C 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_Visible(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_Visible()
  } // end of property IVideoWindow::Visible
  .property int32 Left()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0E 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_Left(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_Left()
  } // end of property IVideoWindow::Left
  .property int32 Width()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 10 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_Width()
    .set instance void MSR.LST.MDShow.IVideoWindow::set_Width(int32)
  } // end of property IVideoWindow::Width
  .property int32 Top()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 12 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_Top()
    .set instance void MSR.LST.MDShow.IVideoWindow::set_Top(int32)
  } // end of property IVideoWindow::Top
  .property int32 Height()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 14 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_Height(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_Height()
  } // end of property IVideoWindow::Height
  .property int32 Owner()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 16 00 02 60 00 00 )                         // .....`..
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_Owner()
    .set instance void MSR.LST.MDShow.IVideoWindow::set_Owner(int32)
  } // end of property IVideoWindow::Owner
  .property int32 MessageDrain()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 18 00 02 60 00 00 )                         // .....`..
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_MessageDrain(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_MessageDrain()
  } // end of property IVideoWindow::MessageDrain
  .property int32 BorderColor()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1A 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_BorderColor()
    .set instance void MSR.LST.MDShow.IVideoWindow::set_BorderColor(int32)
  } // end of property IVideoWindow::BorderColor
  .property int32 FullScreenMode()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1C 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IVideoWindow::set_FullScreenMode(int32)
    .get instance int32 MSR.LST.MDShow.IVideoWindow::get_FullScreenMode()
  } // end of property IVideoWindow::FullScreenMode
} // end of class MSR.LST.MDShow.IVideoWindow

.class interface public abstract auto ansi import MSR.LST.MDShow.IBasicVideo
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 35 2D 30 41 44 34   // ..$56A868B5-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot specialname abstract virtual 
          instance float64  get_AvgTimePerFrame() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_AvgTimePerFrame

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_BitRate() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_BitRate

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_BitErrorRate() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_BitErrorRate

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_VideoWidth() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_VideoWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_VideoHeight() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_VideoHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceLeft([in] int32 pSourceLeft) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_SourceLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceLeft() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_SourceLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceWidth([in] int32 pSourceWidth) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_SourceWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceWidth() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_SourceWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceTop([in] int32 pSourceTop) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 09 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_SourceTop

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceTop() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 09 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_SourceTop

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceHeight([in] int32 pSourceHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0B 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_SourceHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceHeight() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0B 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_SourceHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationLeft([in] int32 pDestinationLeft) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0D 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_DestinationLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationLeft() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0D 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_DestinationLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationWidth([in] int32 pDestinationWidth) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0F 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_DestinationWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationWidth() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0F 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_DestinationWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationTop([in] int32 pDestinationTop) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 11 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_DestinationTop

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationTop() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 11 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_DestinationTop

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationHeight([in] int32 pDestinationHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 13 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::set_DestinationHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationHeight() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 13 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::get_DestinationHeight

  .method public hidebysig newslot abstract virtual 
          instance void  SetSourcePosition([in] int32 Left,
                                           [in] int32 Top,
                                           [in] int32 Width,
                                           [in] int32 Height) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 15 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::SetSourcePosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetSourcePosition([out] int32& pLeft,
                                           [out] int32& pTop,
                                           [out] int32& pWidth,
                                           [out] int32& pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 16 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::GetSourcePosition

  .method public hidebysig newslot abstract virtual 
          instance void  SetDefaultSourcePosition() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 17 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::SetDefaultSourcePosition

  .method public hidebysig newslot abstract virtual 
          instance void  SetDestinationPosition([in] int32 Left,
                                                [in] int32 Top,
                                                [in] int32 Width,
                                                [in] int32 Height) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 18 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::SetDestinationPosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetDestinationPosition([out] int32& pLeft,
                                                [out] int32& pTop,
                                                [out] int32& pWidth,
                                                [out] int32& pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 19 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::GetDestinationPosition

  .method public hidebysig newslot abstract virtual 
          instance void  SetDefaultDestinationPosition() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1A 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::SetDefaultDestinationPosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetVideoSize([out] int32& pWidth,
                                      [out] int32& pHeight) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1B 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::GetVideoSize

  .method public hidebysig newslot abstract virtual 
          instance void  GetVideoPaletteEntries([in] int32 StartIndex,
                                                [in] int32 Entries,
                                                [out] int32& pRetrieved,
                                                [out] int32& pPalette) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1C 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::GetVideoPaletteEntries

  .method public hidebysig newslot abstract virtual 
          instance void  GetCurrentImage([in][out] int32& pBufferSize,
                                         native int pDIBImage) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1D 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::GetCurrentImage

  .method public hidebysig newslot abstract virtual 
          instance void  IsUsingDefaultSource() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1E 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::IsUsingDefaultSource

  .method public hidebysig newslot abstract virtual 
          instance void  IsUsingDefaultDestination() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 1F 00 02 60 00 00 )                         // .....`..
  } // end of method IBasicVideo::IsUsingDefaultDestination

  .property float64 AvgTimePerFrame()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance float64 MSR.LST.MDShow.IBasicVideo::get_AvgTimePerFrame()
  } // end of property IBasicVideo::AvgTimePerFrame
  .property int32 BitRate()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_BitRate()
  } // end of property IBasicVideo::BitRate
  .property int32 BitErrorRate()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_BitErrorRate()
  } // end of property IBasicVideo::BitErrorRate
  .property int32 VideoWidth()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_VideoWidth()
  } // end of property IBasicVideo::VideoWidth
  .property int32 VideoHeight()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_VideoHeight()
  } // end of property IBasicVideo::VideoHeight
  .property int32 SourceLeft()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_SourceLeft()
    .set instance void MSR.LST.MDShow.IBasicVideo::set_SourceLeft(int32)
  } // end of property IBasicVideo::SourceLeft
  .property int32 SourceWidth()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_SourceWidth()
    .set instance void MSR.LST.MDShow.IBasicVideo::set_SourceWidth(int32)
  } // end of property IBasicVideo::SourceWidth
  .property int32 SourceTop()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 09 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_SourceTop()
    .set instance void MSR.LST.MDShow.IBasicVideo::set_SourceTop(int32)
  } // end of property IBasicVideo::SourceTop
  .property int32 SourceHeight()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0B 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicVideo::set_SourceHeight(int32)
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_SourceHeight()
  } // end of property IBasicVideo::SourceHeight
  .property int32 DestinationLeft()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0D 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_DestinationLeft()
    .set instance void MSR.LST.MDShow.IBasicVideo::set_DestinationLeft(int32)
  } // end of property IBasicVideo::DestinationLeft
  .property int32 DestinationWidth()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0F 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicVideo::set_DestinationWidth(int32)
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_DestinationWidth()
  } // end of property IBasicVideo::DestinationWidth
  .property int32 DestinationTop()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 11 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_DestinationTop()
    .set instance void MSR.LST.MDShow.IBasicVideo::set_DestinationTop(int32)
  } // end of property IBasicVideo::DestinationTop
  .property int32 DestinationHeight()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 13 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicVideo::set_DestinationHeight(int32)
    .get instance int32 MSR.LST.MDShow.IBasicVideo::get_DestinationHeight()
  } // end of property IBasicVideo::DestinationHeight
} // end of class MSR.LST.MDShow.IBasicVideo

.class interface public abstract auto ansi import MSR.LST.MDShow.IBasicVideo2
       implements MSR.LST.MDShow.IBasicVideo
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 00 10 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 33 32 39 42 42 33 36 30 2D 46 36 45 41   // ..$329BB360-F6EA
                                                                                                  2D 31 31 44 31 2D 39 30 33 38 2D 30 30 41 30 43   // -11D1-9038-00A0C
                                                                                                  39 36 39 37 32 39 38 00 00 )                      // 9697298..
  .method public hidebysig newslot specialname abstract virtual 
          instance float64  get_AvgTimePerFrame() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_AvgTimePerFrame

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_BitRate() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_BitRate

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_BitErrorRate() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_BitErrorRate

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_VideoWidth() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_VideoWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_VideoHeight() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_VideoHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceLeft([in] int32 pSourceLeft) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_SourceLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceLeft() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_SourceLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceWidth([in] int32 pSourceWidth) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_SourceWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceWidth() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_SourceWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceTop([in] int32 pSourceTop) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_SourceTop

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceTop() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_SourceTop

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_SourceHeight([in] int32 pSourceHeight) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_SourceHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceHeight() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_SourceHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationLeft([in] int32 pDestinationLeft) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_DestinationLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationLeft() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_DestinationLeft

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationWidth([in] int32 pDestinationWidth) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_DestinationWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationWidth() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_DestinationWidth

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationTop([in] int32 pDestinationTop) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_DestinationTop

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationTop() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_DestinationTop

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_DestinationHeight([in] int32 pDestinationHeight) runtime managed internalcall
  {
  } // end of method IBasicVideo2::set_DestinationHeight

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_DestinationHeight() runtime managed internalcall
  {
  } // end of method IBasicVideo2::get_DestinationHeight

  .method public hidebysig newslot abstract virtual 
          instance void  SetSourcePosition([in] int32 Left,
                                           [in] int32 Top,
                                           [in] int32 Width,
                                           [in] int32 Height) runtime managed internalcall
  {
  } // end of method IBasicVideo2::SetSourcePosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetSourcePosition([out] int32& pLeft,
                                           [out] int32& pTop,
                                           [out] int32& pWidth,
                                           [out] int32& pHeight) runtime managed internalcall
  {
  } // end of method IBasicVideo2::GetSourcePosition

  .method public hidebysig newslot abstract virtual 
          instance void  SetDefaultSourcePosition() runtime managed internalcall
  {
  } // end of method IBasicVideo2::SetDefaultSourcePosition

  .method public hidebysig newslot abstract virtual 
          instance void  SetDestinationPosition([in] int32 Left,
                                                [in] int32 Top,
                                                [in] int32 Width,
                                                [in] int32 Height) runtime managed internalcall
  {
  } // end of method IBasicVideo2::SetDestinationPosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetDestinationPosition([out] int32& pLeft,
                                                [out] int32& pTop,
                                                [out] int32& pWidth,
                                                [out] int32& pHeight) runtime managed internalcall
  {
  } // end of method IBasicVideo2::GetDestinationPosition

  .method public hidebysig newslot abstract virtual 
          instance void  SetDefaultDestinationPosition() runtime managed internalcall
  {
  } // end of method IBasicVideo2::SetDefaultDestinationPosition

  .method public hidebysig newslot abstract virtual 
          instance void  GetVideoSize([out] int32& pWidth,
                                      [out] int32& pHeight) runtime managed internalcall
  {
  } // end of method IBasicVideo2::GetVideoSize

  .method public hidebysig newslot abstract virtual 
          instance void  GetVideoPaletteEntries([in] int32 StartIndex,
                                                [in] int32 Entries,
                                                [out] int32& pRetrieved,
                                                [out] int32& pPalette) runtime managed internalcall
  {
  } // end of method IBasicVideo2::GetVideoPaletteEntries

  .method public hidebysig newslot abstract virtual 
          instance void  GetCurrentImage([in][out] int32& pBufferSize,
                                         native int pDIBImage) runtime managed internalcall
  {
  } // end of method IBasicVideo2::GetCurrentImage

  .method public hidebysig newslot abstract virtual 
          instance void  IsUsingDefaultSource() runtime managed internalcall
  {
  } // end of method IBasicVideo2::IsUsingDefaultSource

  .method public hidebysig newslot abstract virtual 
          instance void  IsUsingDefaultDestination() runtime managed internalcall
  {
  } // end of method IBasicVideo2::IsUsingDefaultDestination

  .method public hidebysig newslot abstract virtual 
          instance void  GetPreferredAspectRatio([out] int32& plAspectX,
                                                 [out] int32& plAspectY) runtime managed internalcall
  {
  } // end of method IBasicVideo2::GetPreferredAspectRatio

  .property float64 AvgTimePerFrame()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance float64 MSR.LST.MDShow.IBasicVideo2::get_AvgTimePerFrame()
  } // end of property IBasicVideo2::AvgTimePerFrame
  .property int32 BitRate()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_BitRate()
  } // end of property IBasicVideo2::BitRate
  .property int32 BitErrorRate()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_BitErrorRate()
  } // end of property IBasicVideo2::BitErrorRate
  .property int32 VideoWidth()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_VideoWidth()
  } // end of property IBasicVideo2::VideoWidth
  .property int32 VideoHeight()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_VideoHeight()
  } // end of property IBasicVideo2::VideoHeight
  .property int32 SourceLeft()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_SourceLeft()
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_SourceLeft(int32)
  } // end of property IBasicVideo2::SourceLeft
  .property int32 SourceWidth()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_SourceWidth(int32)
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_SourceWidth()
  } // end of property IBasicVideo2::SourceWidth
  .property int32 SourceTop()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 09 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_SourceTop()
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_SourceTop(int32)
  } // end of property IBasicVideo2::SourceTop
  .property int32 SourceHeight()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0B 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_SourceHeight()
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_SourceHeight(int32)
  } // end of property IBasicVideo2::SourceHeight
  .property int32 DestinationLeft()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0D 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_DestinationLeft(int32)
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_DestinationLeft()
  } // end of property IBasicVideo2::DestinationLeft
  .property int32 DestinationWidth()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0F 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_DestinationWidth()
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_DestinationWidth(int32)
  } // end of property IBasicVideo2::DestinationWidth
  .property int32 DestinationTop()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 11 00 02 60 00 00 )                         // .....`..
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_DestinationTop(int32)
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_DestinationTop()
  } // end of property IBasicVideo2::DestinationTop
  .property int32 DestinationHeight()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 13 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IBasicVideo2::get_DestinationHeight()
    .set instance void MSR.LST.MDShow.IBasicVideo2::set_DestinationHeight(int32)
  } // end of property IBasicVideo2::DestinationHeight
} // end of class MSR.LST.MDShow.IBasicVideo2

.class interface public abstract auto ansi import MSR.LST.MDShow.IDeferredCommand
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.InterfaceTypeAttribute::.ctor(int16) = ( 01 00 01 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 38 2D 30 41 44 34   // ..$56A868B8-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .method public hidebysig newslot abstract virtual 
          instance void  Cancel() runtime managed internalcall
  {
  } // end of method IDeferredCommand::Cancel

  .method public hidebysig newslot abstract virtual 
          instance void  Confidence([out] int32& pConfidence) runtime managed internalcall
  {
  } // end of method IDeferredCommand::Confidence

  .method public hidebysig newslot abstract virtual 
          instance void  Postpone([in] float64 newtime) runtime managed internalcall
  {
  } // end of method IDeferredCommand::Postpone

  .method public hidebysig newslot abstract virtual 
          instance void  GetHResult([out] int32&  marshal( error) phrResult) runtime managed internalcall
  {
  } // end of method IDeferredCommand::GetHResult

} // end of class MSR.LST.MDShow.IDeferredCommand

.class interface public abstract auto ansi import MSR.LST.MDShow.IQueueCommand
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.InterfaceTypeAttribute::.ctor(int16) = ( 01 00 01 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 37 2D 30 41 44 34   // ..$56A868B7-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .method public hidebysig newslot abstract virtual 
          instance void  InvokeAtStreamTime([out] class MSR.LST.MDShow.IDeferredCommand&  marshal( interface ) pCmd,
                                            [in] float64 time,
                                            [in] valuetype [mscorlib]System.Guid& iid,
                                            [in] int32 dispidMethod,
                                            [in] int16 wFlags,
                                            [in] int32 cArgs,
                                            [in] object&  marshal( struct) pDispParams,
                                            [in][out] object&  marshal( struct) pvarResult,
                                            [out] int16& puArgErr) runtime managed internalcall
  {
  } // end of method IQueueCommand::InvokeAtStreamTime

  .method public hidebysig newslot abstract virtual 
          instance void  InvokeAtPresentationTime([out] class MSR.LST.MDShow.IDeferredCommand&  marshal( interface ) pCmd,
                                                  [in] float64 time,
                                                  [in] valuetype [mscorlib]System.Guid& iid,
                                                  [in] int32 dispidMethod,
                                                  [in] int16 wFlags,
                                                  [in] int32 cArgs,
                                                  [in] object&  marshal( struct) pDispParams,
                                                  [in][out] object&  marshal( struct) pvarResult,
                                                  [out] int16& puArgErr) runtime managed internalcall
  {
  } // end of method IQueueCommand::InvokeAtPresentationTime

} // end of class MSR.LST.MDShow.IQueueCommand

.class public auto ansi import MSR.LST.MDShow.FilgraphManagerClass
       extends [mscorlib]System.Object
       implements MSR.LST.MDShow.IMediaControl,
                  MSR.LST.MDShow.FilgraphManager,
                  MSR.LST.MDShow.IMediaEvent,
                  MSR.LST.MDShow.IMediaPosition,
                  MSR.LST.MDShow.IBasicAudio,
                  MSR.LST.MDShow.IBasicVideo,
                  MSR.LST.MDShow.IVideoWindow
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.ClassInterfaceAttribute::.ctor(int16) = ( 01 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 45 34 33 36 45 42 42 33 2D 35 32 34 46   // ..$E436EBB3-524F
                                                                                                  2D 31 31 43 45 2D 39 46 35 33 2D 30 30 32 30 41   // -11CE-9F53-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 02 00 00 00 ) 
  .method public specialname rtspecialname 
          instance void  .ctor() runtime managed internalcall
  {
  } // end of method FilgraphManagerClass::.ctor

  .method public hidebysig newslot virtual 
          instance void  Run() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::Run
  } // end of method FilgraphManagerClass::Run

  .method public hidebysig newslot virtual 
          instance void  Pause() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::Pause
  } // end of method FilgraphManagerClass::Pause

  .method public hidebysig newslot virtual 
          instance void  Stop() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::Stop
  } // end of method FilgraphManagerClass::Stop

  .method public hidebysig newslot virtual 
          instance void  GetState([in] int32 msTimeout,
                                  [out] int32& pfs) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::GetState
  } // end of method FilgraphManagerClass::GetState

  .method public hidebysig newslot virtual 
          instance void  RenderFile([in] string  marshal( bstr) strFilename) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::RenderFile
  } // end of method FilgraphManagerClass::RenderFile

  .method public hidebysig newslot virtual 
          instance void  AddSourceFilter([in] string  marshal( bstr) strFilename,
                                         [out] object&  marshal( idispatch ) ppUnk) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::AddSourceFilter
  } // end of method FilgraphManagerClass::AddSourceFilter

  .method public hidebysig newslot specialname virtual 
          instance object 
          marshal( idispatch ) 
          get_FilterCollection() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::get_FilterCollection
  } // end of method FilgraphManagerClass::get_FilterCollection

  .method public hidebysig newslot specialname virtual 
          instance object 
          marshal( idispatch ) 
          get_RegFilterCollection() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::get_RegFilterCollection
  } // end of method FilgraphManagerClass::get_RegFilterCollection

  .method public hidebysig newslot virtual 
          instance void  StopWhenReady() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 08 00 02 60 00 00 )                         // .....`..
    .override MSR.LST.MDShow.IMediaControl::StopWhenReady
  } // end of method FilgraphManagerClass::StopWhenReady

  .method public hidebysig newslot virtual 
          instance void  GetEventHandle([out] int32& hEvent) runtime managed internalcall
  {
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IMediaEvent::GetEventHandle
  } // end of method FilgraphManagerClass::GetEventHandle

  .method public hidebysig newslot virtual 
          instance void  GetEvent([out] int32& lEventCode,
                                  [out] int32& lParam1,
                                  [out] int32& lParam2,
                                  [in] int32 msTimeout) runtime managed internalcall
  {
    .param [2]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IMediaEvent::GetEvent
  } // end of method FilgraphManagerClass::GetEvent

  .method public hidebysig newslot virtual 
          instance void  WaitForCompletion([in] int32 msTimeout,
                                           [out] int32& pEvCode) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaEvent::WaitForCompletion
  } // end of method FilgraphManagerClass::WaitForCompletion

  .method public hidebysig newslot virtual 
          instance void  CancelDefaultHandling([in] int32 lEvCode) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaEvent::CancelDefaultHandling
  } // end of method FilgraphManagerClass::CancelDefaultHandling

  .method public hidebysig newslot virtual 
          instance void  RestoreDefaultHandling([in] int32 lEvCode) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaEvent::RestoreDefaultHandling
  } // end of method FilgraphManagerClass::RestoreDefaultHandling

  .method public hidebysig newslot virtual 
          instance void  FreeEventParams([in] int32 lEvCode,
                                         [in] int32 lParam1,
                                         [in] int32 lParam2) runtime managed internalcall
  {
    .param [2]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IMediaEvent::FreeEventParams
  } // end of method FilgraphManagerClass::FreeEventParams

  .method public hidebysig newslot specialname virtual 
          instance float64  get_Duration() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::get_Duration
  } // end of method FilgraphManagerClass::get_Duration

  .method public hidebysig newslot specialname virtual 
          instance void  set_CurrentPosition([in] float64 pllTime) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::set_CurrentPosition
  } // end of method FilgraphManagerClass::set_CurrentPosition

  .method public hidebysig newslot specialname virtual 
          instance float64  get_CurrentPosition() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::get_CurrentPosition
  } // end of method FilgraphManagerClass::get_CurrentPosition

  .method public hidebysig newslot specialname virtual 
          instance float64  get_StopTime() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::get_StopTime
  } // end of method FilgraphManagerClass::get_StopTime

  .method public hidebysig newslot specialname virtual 
          instance void  set_StopTime([in] float64 pllTime) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::set_StopTime
  } // end of method FilgraphManagerClass::set_StopTime

  .method public hidebysig newslot specialname virtual 
          instance float64  get_PrerollTime() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::get_PrerollTime
  } // end of method FilgraphManagerClass::get_PrerollTime

  .method public hidebysig newslot specialname virtual 
          instance void  set_PrerollTime([in] float64 pllTime) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::set_PrerollTime
  } // end of method FilgraphManagerClass::set_PrerollTime

  .method public hidebysig newslot specialname virtual 
          instance void  set_Rate([in] float64 pdRate) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::set_Rate
  } // end of method FilgraphManagerClass::set_Rate

  .method public hidebysig newslot specialname virtual 
          instance float64  get_Rate() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::get_Rate
  } // end of method FilgraphManagerClass::get_Rate

  .method public hidebysig newslot virtual 
          instance int32  CanSeekForward() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::CanSeekForward
  } // end of method FilgraphManagerClass::CanSeekForward

  .method public hidebysig newslot virtual 
          instance int32  CanSeekBackward() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IMediaPosition::CanSeekBackward
  } // end of method FilgraphManagerClass::CanSeekBackward

  .method public hidebysig newslot specialname virtual 
          instance void  set_Volume([in] int32 plVolume) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicAudio::set_Volume
  } // end of method FilgraphManagerClass::set_Volume

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Volume() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicAudio::get_Volume
  } // end of method FilgraphManagerClass::get_Volume

  .method public hidebysig newslot specialname virtual 
          instance void  set_Balance([in] int32 plBalance) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicAudio::set_Balance
  } // end of method FilgraphManagerClass::set_Balance

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Balance() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicAudio::get_Balance
  } // end of method FilgraphManagerClass::get_Balance

  .method public hidebysig newslot specialname virtual 
          instance float64  get_AvgTimePerFrame() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_AvgTimePerFrame
  } // end of method FilgraphManagerClass::get_AvgTimePerFrame

  .method public hidebysig newslot specialname virtual 
          instance int32  get_BitRate() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_BitRate
  } // end of method FilgraphManagerClass::get_BitRate

  .method public hidebysig newslot specialname virtual 
          instance int32  get_BitErrorRate() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_BitErrorRate
  } // end of method FilgraphManagerClass::get_BitErrorRate

  .method public hidebysig newslot specialname virtual 
          instance int32  get_VideoWidth() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_VideoWidth
  } // end of method FilgraphManagerClass::get_VideoWidth

  .method public hidebysig newslot specialname virtual 
          instance int32  get_VideoHeight() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_VideoHeight
  } // end of method FilgraphManagerClass::get_VideoHeight

  .method public hidebysig newslot specialname virtual 
          instance void  set_SourceLeft([in] int32 pSourceLeft) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_SourceLeft
  } // end of method FilgraphManagerClass::set_SourceLeft

  .method public hidebysig newslot specialname virtual 
          instance int32  get_SourceLeft() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_SourceLeft
  } // end of method FilgraphManagerClass::get_SourceLeft

  .method public hidebysig newslot specialname virtual 
          instance void  set_SourceWidth([in] int32 pSourceWidth) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_SourceWidth
  } // end of method FilgraphManagerClass::set_SourceWidth

  .method public hidebysig newslot specialname virtual 
          instance int32  get_SourceWidth() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_SourceWidth
  } // end of method FilgraphManagerClass::get_SourceWidth

  .method public hidebysig newslot specialname virtual 
          instance void  set_SourceTop([in] int32 pSourceTop) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_SourceTop
  } // end of method FilgraphManagerClass::set_SourceTop

  .method public hidebysig newslot specialname virtual 
          instance int32  get_SourceTop() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_SourceTop
  } // end of method FilgraphManagerClass::get_SourceTop

  .method public hidebysig newslot specialname virtual 
          instance void  set_SourceHeight([in] int32 pSourceHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_SourceHeight
  } // end of method FilgraphManagerClass::set_SourceHeight

  .method public hidebysig newslot specialname virtual 
          instance int32  get_SourceHeight() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_SourceHeight
  } // end of method FilgraphManagerClass::get_SourceHeight

  .method public hidebysig newslot specialname virtual 
          instance void  set_DestinationLeft([in] int32 pDestinationLeft) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_DestinationLeft
  } // end of method FilgraphManagerClass::set_DestinationLeft

  .method public hidebysig newslot specialname virtual 
          instance int32  get_DestinationLeft() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_DestinationLeft
  } // end of method FilgraphManagerClass::get_DestinationLeft

  .method public hidebysig newslot specialname virtual 
          instance void  set_DestinationWidth([in] int32 pDestinationWidth) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_DestinationWidth
  } // end of method FilgraphManagerClass::set_DestinationWidth

  .method public hidebysig newslot specialname virtual 
          instance int32  get_DestinationWidth() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_DestinationWidth
  } // end of method FilgraphManagerClass::get_DestinationWidth

  .method public hidebysig newslot specialname virtual 
          instance void  set_DestinationTop([in] int32 pDestinationTop) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_DestinationTop
  } // end of method FilgraphManagerClass::set_DestinationTop

  .method public hidebysig newslot specialname virtual 
          instance int32  get_DestinationTop() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_DestinationTop
  } // end of method FilgraphManagerClass::get_DestinationTop

  .method public hidebysig newslot specialname virtual 
          instance void  set_DestinationHeight([in] int32 pDestinationHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::set_DestinationHeight
  } // end of method FilgraphManagerClass::set_DestinationHeight

  .method public hidebysig newslot specialname virtual 
          instance int32  get_DestinationHeight() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::get_DestinationHeight
  } // end of method FilgraphManagerClass::get_DestinationHeight

  .method public hidebysig newslot virtual 
          instance void  SetSourcePosition([in] int32 Left,
                                           [in] int32 Top,
                                           [in] int32 Width,
                                           [in] int32 Height) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::SetSourcePosition
  } // end of method FilgraphManagerClass::SetSourcePosition

  .method public hidebysig newslot virtual 
          instance void  GetSourcePosition([out] int32& pLeft,
                                           [out] int32& pTop,
                                           [out] int32& pWidth,
                                           [out] int32& pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::GetSourcePosition
  } // end of method FilgraphManagerClass::GetSourcePosition

  .method public hidebysig newslot virtual 
          instance void  SetDefaultSourcePosition() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::SetDefaultSourcePosition
  } // end of method FilgraphManagerClass::SetDefaultSourcePosition

  .method public hidebysig newslot virtual 
          instance void  SetDestinationPosition([in] int32 Left,
                                                [in] int32 Top,
                                                [in] int32 Width,
                                                [in] int32 Height) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::SetDestinationPosition
  } // end of method FilgraphManagerClass::SetDestinationPosition

  .method public hidebysig newslot virtual 
          instance void  GetDestinationPosition([out] int32& pLeft,
                                                [out] int32& pTop,
                                                [out] int32& pWidth,
                                                [out] int32& pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::GetDestinationPosition
  } // end of method FilgraphManagerClass::GetDestinationPosition

  .method public hidebysig newslot virtual 
          instance void  SetDefaultDestinationPosition() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::SetDefaultDestinationPosition
  } // end of method FilgraphManagerClass::SetDefaultDestinationPosition

  .method public hidebysig newslot virtual 
          instance void  GetVideoSize([out] int32& pWidth,
                                      [out] int32& pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::GetVideoSize
  } // end of method FilgraphManagerClass::GetVideoSize

  .method public hidebysig newslot virtual 
          instance void  GetVideoPaletteEntries([in] int32 StartIndex,
                                                [in] int32 Entries,
                                                [out] int32& pRetrieved,
                                                [out] int32& pPalette) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::GetVideoPaletteEntries
  } // end of method FilgraphManagerClass::GetVideoPaletteEntries

  .method public hidebysig newslot virtual 
          instance void  GetCurrentImage([in][out] int32& pBufferSize,
                                         native int pDIBImage) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::GetCurrentImage
  } // end of method FilgraphManagerClass::GetCurrentImage

  .method public hidebysig newslot virtual 
          instance void  IsUsingDefaultSource() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::IsUsingDefaultSource
  } // end of method FilgraphManagerClass::IsUsingDefaultSource

  .method public hidebysig newslot virtual 
          instance void  IsUsingDefaultDestination() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IBasicVideo::IsUsingDefaultDestination
  } // end of method FilgraphManagerClass::IsUsingDefaultDestination

  .method public hidebysig newslot specialname virtual 
          instance void  set_Caption([in] string  marshal( bstr) strCaption) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_Caption
  } // end of method FilgraphManagerClass::set_Caption

  .method public hidebysig newslot specialname virtual 
          instance string 
          marshal( bstr) 
          get_Caption() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_Caption
  } // end of method FilgraphManagerClass::get_Caption

  .method public hidebysig newslot specialname virtual 
          instance void  set_WindowStyle([in] int32 WindowStyle) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_WindowStyle
  } // end of method FilgraphManagerClass::set_WindowStyle

  .method public hidebysig newslot specialname virtual 
          instance int32  get_WindowStyle() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_WindowStyle
  } // end of method FilgraphManagerClass::get_WindowStyle

  .method public hidebysig newslot specialname virtual 
          instance void  set_WindowStyleEx([in] int32 WindowStyleEx) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_WindowStyleEx
  } // end of method FilgraphManagerClass::set_WindowStyleEx

  .method public hidebysig newslot specialname virtual 
          instance int32  get_WindowStyleEx() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_WindowStyleEx
  } // end of method FilgraphManagerClass::get_WindowStyleEx

  .method public hidebysig newslot specialname virtual 
          instance void  set_AutoShow([in] int32 AutoShow) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_AutoShow
  } // end of method FilgraphManagerClass::set_AutoShow

  .method public hidebysig newslot specialname virtual 
          instance int32  get_AutoShow() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_AutoShow
  } // end of method FilgraphManagerClass::get_AutoShow

  .method public hidebysig newslot specialname virtual 
          instance void  set_WindowState([in] int32 WindowState) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_WindowState
  } // end of method FilgraphManagerClass::set_WindowState

  .method public hidebysig newslot specialname virtual 
          instance int32  get_WindowState() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_WindowState
  } // end of method FilgraphManagerClass::get_WindowState

  .method public hidebysig newslot specialname virtual 
          instance void  set_BackgroundPalette([in] int32 pBackgroundPalette) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_BackgroundPalette
  } // end of method FilgraphManagerClass::set_BackgroundPalette

  .method public hidebysig newslot specialname virtual 
          instance int32  get_BackgroundPalette() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_BackgroundPalette
  } // end of method FilgraphManagerClass::get_BackgroundPalette

  .method public hidebysig newslot specialname virtual 
          instance void  set_Visible([in] int32 pVisible) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_Visible
  } // end of method FilgraphManagerClass::set_Visible

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Visible() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_Visible
  } // end of method FilgraphManagerClass::get_Visible

  .method public hidebysig newslot specialname virtual 
          instance void  set_Left([in] int32 pLeft) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_Left
  } // end of method FilgraphManagerClass::set_Left

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Left() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_Left
  } // end of method FilgraphManagerClass::get_Left

  .method public hidebysig newslot specialname virtual 
          instance void  set_Width([in] int32 pWidth) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_Width
  } // end of method FilgraphManagerClass::set_Width

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Width() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_Width
  } // end of method FilgraphManagerClass::get_Width

  .method public hidebysig newslot specialname virtual 
          instance void  set_Top([in] int32 pTop) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_Top
  } // end of method FilgraphManagerClass::set_Top

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Top() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_Top
  } // end of method FilgraphManagerClass::get_Top

  .method public hidebysig newslot specialname virtual 
          instance void  set_Height([in] int32 pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_Height
  } // end of method FilgraphManagerClass::set_Height

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Height() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_Height
  } // end of method FilgraphManagerClass::get_Height

  .method public hidebysig newslot specialname virtual 
          instance void  set_Owner([in] int32 Owner) runtime managed internalcall
  {
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IVideoWindow::set_Owner
  } // end of method FilgraphManagerClass::set_Owner

  .method public hidebysig newslot specialname virtual 
          instance int32  get_Owner() runtime managed internalcall
  {
    .param [0]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IVideoWindow::get_Owner
  } // end of method FilgraphManagerClass::get_Owner

  .method public hidebysig newslot specialname virtual 
          instance void  set_MessageDrain([in] int32 Drain) runtime managed internalcall
  {
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IVideoWindow::set_MessageDrain
  } // end of method FilgraphManagerClass::set_MessageDrain

  .method public hidebysig newslot specialname virtual 
          instance int32  get_MessageDrain() runtime managed internalcall
  {
    .param [0]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IVideoWindow::get_MessageDrain
  } // end of method FilgraphManagerClass::get_MessageDrain

  .method public hidebysig newslot specialname virtual 
          instance int32  get_BorderColor() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_BorderColor
  } // end of method FilgraphManagerClass::get_BorderColor

  .method public hidebysig newslot specialname virtual 
          instance void  set_BorderColor([in] int32 Color) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_BorderColor
  } // end of method FilgraphManagerClass::set_BorderColor

  .method public hidebysig newslot specialname virtual 
          instance int32  get_FullScreenMode() runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::get_FullScreenMode
  } // end of method FilgraphManagerClass::get_FullScreenMode

  .method public hidebysig newslot specialname virtual 
          instance void  set_FullScreenMode([in] int32 FullScreenMode) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::set_FullScreenMode
  } // end of method FilgraphManagerClass::set_FullScreenMode

  .method public hidebysig newslot virtual 
          instance void  SetWindowForeground([in] int32 Focus) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::SetWindowForeground
  } // end of method FilgraphManagerClass::SetWindowForeground

  .method public hidebysig newslot virtual 
          instance void  NotifyOwnerMessage([in] int32 hwnd,
                                            [in] int32 uMsg,
                                            [in] int32 wParam,
                                            [in] int32 lParam) runtime managed internalcall
  {
    .param [1]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [3]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .param [4]
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .override MSR.LST.MDShow.IVideoWindow::NotifyOwnerMessage
  } // end of method FilgraphManagerClass::NotifyOwnerMessage

  .method public hidebysig newslot virtual 
          instance void  SetWindowPosition([in] int32 Left,
                                           [in] int32 Top,
                                           [in] int32 Width,
                                           [in] int32 Height) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::SetWindowPosition
  } // end of method FilgraphManagerClass::SetWindowPosition

  .method public hidebysig newslot virtual 
          instance void  GetWindowPosition([out] int32& pLeft,
                                           [out] int32& pTop,
                                           [out] int32& pWidth,
                                           [out] int32& pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::GetWindowPosition
  } // end of method FilgraphManagerClass::GetWindowPosition

  .method public hidebysig newslot virtual 
          instance void  GetMinIdealImageSize([out] int32& pWidth,
                                              [out] int32& pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::GetMinIdealImageSize
  } // end of method FilgraphManagerClass::GetMinIdealImageSize

  .method public hidebysig newslot virtual 
          instance void  GetMaxIdealImageSize([out] int32& pWidth,
                                              [out] int32& pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::GetMaxIdealImageSize
  } // end of method FilgraphManagerClass::GetMaxIdealImageSize

  .method public hidebysig newslot virtual 
          instance void  GetRestorePosition([out] int32& pLeft,
                                            [out] int32& pTop,
                                            [out] int32& pWidth,
                                            [out] int32& pHeight) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::GetRestorePosition
  } // end of method FilgraphManagerClass::GetRestorePosition

  .method public hidebysig newslot virtual 
          instance void  HideCursor([in] int32 HideCursor) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::HideCursor
  } // end of method FilgraphManagerClass::HideCursor

  .method public hidebysig newslot virtual 
          instance void  IsCursorHidden([out] int32& CursorHidden) runtime managed internalcall
  {
    .override MSR.LST.MDShow.IVideoWindow::IsCursorHidden
  } // end of method FilgraphManagerClass::IsCursorHidden

  .property object FilterCollection()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.FilgraphManagerClass::get_FilterCollection()
  } // end of property FilgraphManagerClass::FilterCollection
  .property object RegFilterCollection()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.FilgraphManagerClass::get_RegFilterCollection()
  } // end of property FilgraphManagerClass::RegFilterCollection
  .property float64 Duration()
  {
    .get instance float64 MSR.LST.MDShow.FilgraphManagerClass::get_Duration()
  } // end of property FilgraphManagerClass::Duration
  .property float64 CurrentPosition()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_CurrentPosition(float64)
    .get instance float64 MSR.LST.MDShow.FilgraphManagerClass::get_CurrentPosition()
  } // end of property FilgraphManagerClass::CurrentPosition
  .property float64 StopTime()
  {
    .get instance float64 MSR.LST.MDShow.FilgraphManagerClass::get_StopTime()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_StopTime(float64)
  } // end of property FilgraphManagerClass::StopTime
  .property float64 PrerollTime()
  {
    .get instance float64 MSR.LST.MDShow.FilgraphManagerClass::get_PrerollTime()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_PrerollTime(float64)
  } // end of property FilgraphManagerClass::PrerollTime
  .property float64 Rate()
  {
    .get instance float64 MSR.LST.MDShow.FilgraphManagerClass::get_Rate()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Rate(float64)
  } // end of property FilgraphManagerClass::Rate
  .property int32 Volume()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Volume(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Volume()
  } // end of property FilgraphManagerClass::Volume
  .property int32 Balance()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Balance(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Balance()
  } // end of property FilgraphManagerClass::Balance
  .property float64 AvgTimePerFrame()
  {
    .get instance float64 MSR.LST.MDShow.FilgraphManagerClass::get_AvgTimePerFrame()
  } // end of property FilgraphManagerClass::AvgTimePerFrame
  .property int32 BitRate()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_BitRate()
  } // end of property FilgraphManagerClass::BitRate
  .property int32 BitErrorRate()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_BitErrorRate()
  } // end of property FilgraphManagerClass::BitErrorRate
  .property int32 VideoWidth()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_VideoWidth()
  } // end of property FilgraphManagerClass::VideoWidth
  .property int32 VideoHeight()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_VideoHeight()
  } // end of property FilgraphManagerClass::VideoHeight
  .property int32 SourceLeft()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_SourceLeft()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_SourceLeft(int32)
  } // end of property FilgraphManagerClass::SourceLeft
  .property int32 SourceWidth()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_SourceWidth(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_SourceWidth()
  } // end of property FilgraphManagerClass::SourceWidth
  .property int32 SourceTop()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_SourceTop(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_SourceTop()
  } // end of property FilgraphManagerClass::SourceTop
  .property int32 SourceHeight()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_SourceHeight()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_SourceHeight(int32)
  } // end of property FilgraphManagerClass::SourceHeight
  .property int32 DestinationLeft()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_DestinationLeft(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_DestinationLeft()
  } // end of property FilgraphManagerClass::DestinationLeft
  .property int32 DestinationWidth()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_DestinationWidth(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_DestinationWidth()
  } // end of property FilgraphManagerClass::DestinationWidth
  .property int32 DestinationTop()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_DestinationTop()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_DestinationTop(int32)
  } // end of property FilgraphManagerClass::DestinationTop
  .property int32 DestinationHeight()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_DestinationHeight()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_DestinationHeight(int32)
  } // end of property FilgraphManagerClass::DestinationHeight
  .property string Caption()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Caption(string)
    .get instance string MSR.LST.MDShow.FilgraphManagerClass::get_Caption()
  } // end of property FilgraphManagerClass::Caption
  .property int32 WindowStyle()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_WindowStyle()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_WindowStyle(int32)
  } // end of property FilgraphManagerClass::WindowStyle
  .property int32 WindowStyleEx()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_WindowStyleEx()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_WindowStyleEx(int32)
  } // end of property FilgraphManagerClass::WindowStyleEx
  .property int32 AutoShow()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_AutoShow()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_AutoShow(int32)
  } // end of property FilgraphManagerClass::AutoShow
  .property int32 WindowState()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_WindowState(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_WindowState()
  } // end of property FilgraphManagerClass::WindowState
  .property int32 BackgroundPalette()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_BackgroundPalette(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_BackgroundPalette()
  } // end of property FilgraphManagerClass::BackgroundPalette
  .property int32 Visible()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Visible()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Visible(int32)
  } // end of property FilgraphManagerClass::Visible
  .property int32 Left()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Left(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Left()
  } // end of property FilgraphManagerClass::Left
  .property int32 Width()
  {
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Width(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Width()
  } // end of property FilgraphManagerClass::Width
  .property int32 Top()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Top()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Top(int32)
  } // end of property FilgraphManagerClass::Top
  .property int32 Height()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Height()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Height(int32)
  } // end of property FilgraphManagerClass::Height
  .property int32 Owner()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_Owner(int32)
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_Owner()
  } // end of property FilgraphManagerClass::Owner
  .property int32 MessageDrain()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.ComAliasNameAttribute::.ctor(string) = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                            77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_MessageDrain()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_MessageDrain(int32)
  } // end of property FilgraphManagerClass::MessageDrain
  .property int32 BorderColor()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_BorderColor()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_BorderColor(int32)
  } // end of property FilgraphManagerClass::BorderColor
  .property int32 FullScreenMode()
  {
    .get instance int32 MSR.LST.MDShow.FilgraphManagerClass::get_FullScreenMode()
    .set instance void MSR.LST.MDShow.FilgraphManagerClass::set_FullScreenMode(int32)
  } // end of property FilgraphManagerClass::FullScreenMode
} // end of class MSR.LST.MDShow.FilgraphManagerClass

.class interface public abstract auto ansi import MSR.LST.MDShow.FilgraphManager
       implements MSR.LST.MDShow.IMediaControl
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.CoClassAttribute::.ctor(class [mscorlib]System.Type) = ( 01 00 23 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ..#MSR.LST.MDSho
                                                                                                                          77 2E 46 69 6C 67 72 61 70 68 4D 61 6E 61 67 65   // w.FilgraphManage
                                                                                                                          72 43 6C 61 73 73 00 00 )                         // rClass..
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 31 2D 30 41 44 34   // ..$56A868B1-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
} // end of class MSR.LST.MDShow.FilgraphManager

.class interface public abstract auto ansi import MSR.LST.MDShow.IFilterInfo
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 41 2D 30 41 44 34   // ..$56A868BA-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .method public hidebysig newslot abstract virtual 
          instance void  FindPin([in] string  marshal( bstr) strPinID,
                                 [out] object&  marshal( idispatch ) ppUnk) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::FindPin

  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_Name() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::get_Name

  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_VendorInfo() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::get_VendorInfo

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( iunknown ) 
          get_Filter() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::get_Filter

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( idispatch ) 
          get_Pins() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::get_Pins

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_IsFileSource() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::get_IsFileSource

  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_Filename() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::get_Filename

  .method public hidebysig newslot specialname abstract virtual 
          instance void  set_Filename([in] string  marshal( bstr) pstrFilename) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
  } // end of method IFilterInfo::set_Filename

  .property string Name()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IFilterInfo::get_Name()
  } // end of property IFilterInfo::Name
  .property string VendorInfo()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IFilterInfo::get_VendorInfo()
  } // end of property IFilterInfo::VendorInfo
  .property object Filter()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IFilterInfo::get_Filter()
  } // end of property IFilterInfo::Filter
  .property object Pins()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IFilterInfo::get_Pins()
  } // end of property IFilterInfo::Pins
  .property int32 IsFileSource()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IFilterInfo::get_IsFileSource()
  } // end of property IFilterInfo::IsFileSource
  .property string Filename()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IFilterInfo::get_Filename()
    .set instance void MSR.LST.MDShow.IFilterInfo::set_Filename(string)
  } // end of property IFilterInfo::Filename
} // end of class MSR.LST.MDShow.IFilterInfo

.class interface public abstract auto ansi import MSR.LST.MDShow.IRegFilterInfo
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 42 2D 30 41 44 34   // ..$56A868BB-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_Name() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IRegFilterInfo::get_Name

  .method public hidebysig newslot abstract virtual 
          instance void  Filter([out] object&  marshal( idispatch ) ppUnk) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IRegFilterInfo::Filter

  .property string Name()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IRegFilterInfo::get_Name()
  } // end of property IRegFilterInfo::Name
} // end of class MSR.LST.MDShow.IRegFilterInfo

.class interface public abstract auto ansi import MSR.LST.MDShow.IMediaTypeInfo
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 43 2D 30 41 44 34   // ..$56A868BC-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_Type() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaTypeInfo::get_Type

  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_Subtype() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IMediaTypeInfo::get_Subtype

  .property string Type()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IMediaTypeInfo::get_Type()
  } // end of property IMediaTypeInfo::Type
  .property string Subtype()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IMediaTypeInfo::get_Subtype()
  } // end of property IMediaTypeInfo::Subtype
} // end of class MSR.LST.MDShow.IMediaTypeInfo

.class interface public abstract auto ansi import MSR.LST.MDShow.IPinInfo
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 36 41 38 36 38 42 44 2D 30 41 44 34   // ..$56A868BD-0AD4
                                                                                                  2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                  46 30 42 41 37 37 30 00 00 )                      // F0BA770..
  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( iunknown ) 
          get_Pin() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_Pin

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( idispatch ) 
          get_ConnectedTo() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_ConnectedTo

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( idispatch ) 
          get_ConnectionMediaType() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_ConnectionMediaType

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( idispatch ) 
          get_FilterInfo() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_FilterInfo

  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_Name() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_Name

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Direction() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_Direction

  .method public hidebysig newslot specialname abstract virtual 
          instance string 
          marshal( bstr) 
          get_PinID() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_PinID

  .method public hidebysig newslot specialname abstract virtual 
          instance object 
          marshal( idispatch ) 
          get_MediaTypes() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::get_MediaTypes

  .method public hidebysig newslot abstract virtual 
          instance void  Connect([in] object  marshal( iunknown ) pPin) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 08 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::Connect

  .method public hidebysig newslot abstract virtual 
          instance void  ConnectDirect([in] object  marshal( iunknown ) pPin) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 09 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::ConnectDirect

  .method public hidebysig newslot abstract virtual 
          instance void  ConnectWithType([in] object  marshal( iunknown ) pPin,
                                         [in] object  marshal( idispatch ) pMediaType) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0A 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::ConnectWithType

  .method public hidebysig newslot abstract virtual 
          instance void  Disconnect() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0B 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::Disconnect

  .method public hidebysig newslot abstract virtual 
          instance void  Render() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 0C 00 02 60 00 00 )                         // .....`..
  } // end of method IPinInfo::Render

  .property object Pin()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IPinInfo::get_Pin()
  } // end of property IPinInfo::Pin
  .property object ConnectedTo()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IPinInfo::get_ConnectedTo()
  } // end of property IPinInfo::ConnectedTo
  .property object ConnectionMediaType()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IPinInfo::get_ConnectionMediaType()
  } // end of property IPinInfo::ConnectionMediaType
  .property object FilterInfo()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IPinInfo::get_FilterInfo()
  } // end of property IPinInfo::FilterInfo
  .property string Name()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IPinInfo::get_Name()
  } // end of property IPinInfo::Name
  .property int32 Direction()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IPinInfo::get_Direction()
  } // end of property IPinInfo::Direction
  .property string PinID()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 06 00 02 60 00 00 )                         // .....`..
    .get instance string MSR.LST.MDShow.IPinInfo::get_PinID()
  } // end of property IPinInfo::PinID
  .property object MediaTypes()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 07 00 02 60 00 00 )                         // .....`..
    .get instance object MSR.LST.MDShow.IPinInfo::get_MediaTypes()
  } // end of property IPinInfo::MediaTypes
} // end of class MSR.LST.MDShow.IPinInfo

.class interface public abstract auto ansi import MSR.LST.MDShow.IAMStats
{
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 42 43 39 42 43 46 38 30 2D 44 43 44 32   // ..$BC9BCF80-DCD2
                                                                                                  2D 31 31 44 32 2D 41 42 46 36 2D 30 30 41 30 43   // -11D2-ABF6-00A0C
                                                                                                  39 30 35 46 33 37 35 00 00 )                      // 905F375..
  .custom instance void [mscorlib]System.Runtime.InteropServices.TypeLibTypeAttribute::.ctor(int16) = ( 01 00 40 10 00 00 )                               // ..@...
  .method public hidebysig newslot abstract virtual 
          instance void  Reset() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 00 00 02 60 00 00 )                         // .....`..
  } // end of method IAMStats::Reset

  .method public hidebysig newslot specialname abstract virtual 
          instance int32  get_Count() runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
  } // end of method IAMStats::get_Count

  .method public hidebysig newslot abstract virtual 
          instance void  GetValueByIndex([in] int32 lIndex,
                                         [out] string&  marshal( bstr) szName,
                                         [out] int32& lCount,
                                         [out] float64& dLast,
                                         [out] float64& dAverage,
                                         [out] float64& dStdDev,
                                         [out] float64& dMin,
                                         [out] float64& dMax) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 02 00 02 60 00 00 )                         // .....`..
  } // end of method IAMStats::GetValueByIndex

  .method public hidebysig newslot abstract virtual 
          instance void  GetValueByName([in] string  marshal( bstr) szName,
                                        [out] int32& lIndex,
                                        [out] int32& lCount,
                                        [out] float64& dLast,
                                        [out] float64& dAverage,
                                        [out] float64& dStdDev,
                                        [out] float64& dMin,
                                        [out] float64& dMax) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 03 00 02 60 00 00 )                         // .....`..
  } // end of method IAMStats::GetValueByName

  .method public hidebysig newslot abstract virtual 
          instance void  GetIndex([in] string  marshal( bstr) szName,
                                  [in] int32 lCreate,
                                  [out] int32& plIndex) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 04 00 02 60 00 00 )                         // .....`..
  } // end of method IAMStats::GetIndex

  .method public hidebysig newslot abstract virtual 
          instance void  AddValue([in] int32 lIndex,
                                  [in] float64 dValue) runtime managed internalcall
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 05 00 02 60 00 00 )                         // .....`..
  } // end of method IAMStats::AddValue

  .property int32 Count()
  {
    .custom instance void [mscorlib]System.Runtime.InteropServices.DispIdAttribute::.ctor(int32) = ( 01 00 01 00 02 60 00 00 )                         // .....`..
    .get instance int32 MSR.LST.MDShow.IAMStats::get_Count()
  } // end of property IAMStats::Count
} // end of class MSR.LST.MDShow.IAMStats


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file D:\SD\Main\MSR.LST.MDShow\Interop\control.interop.orig.res
