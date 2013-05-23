
//  Microsoft (R) .NET Framework IL Disassembler.  Version 1.1.4322.573
//  Copyright (C) Microsoft Corporation 1998-2002. All rights reserved.

// PE Header:
// Subsystem:                      00000003
// Native entry point address:     0001039e
// Image base:                     00400000
// Section alignment:              00002000
// File alignment:                 00001000
// Stack reserve size:             00100000
// Stack commit size:              00001000
// Directories:                    00000010
// 0        [0       ] address [size] of Export Directory:          
// 10348    [53      ] address [size] of Import Directory:          
// 12000    [378     ] address [size] of Resource Directory:        
// 0        [0       ] address [size] of Exception Directory:       
// 0        [0       ] address [size] of Security Directory:        
// 14000    [c       ] address [size] of Base Relocation Table:     
// 0        [0       ] address [size] of Debug Directory:           
// 0        [0       ] address [size] of Architecture Specific:     
// 0        [0       ] address [size] of Global Pointer:            
// 0        [0       ] address [size] of TLS Directory:             
// 0        [0       ] address [size] of Load Config Directory:     
// 0        [0       ] address [size] of Bound Import Directory:    
// 2000     [8       ] address [size] of Import Address Table:      
// 0        [0       ] address [size] of Delay Load IAT:            
// 2008     [48      ] address [size] of CLR Header:               

// Import Address Table
//     mscoree.dll
//              00002000 Import Address Table
//              0001038e Import Name Table
//              0        time date stamp
//              0        Index of first forwarder reference
//
//                    0  _CorDllMain

// Delay Load Import Address Table
// No data.
// CLR Header:
// 72       Header Size
// 2        Major Runtime Version
// 0        Minor Runtime Version
// 1        Flags
// 0        Entrypoint Token
// 2050     [e2f8    ] address [size] of Metadata Directory:        
// 0        [0       ] address [size] of Resources Directory:       
// 0        [0       ] address [size] of Strong Name Signature:     
// 0        [0       ] address [size] of CodeManager Table:         
// 0        [0       ] address [size] of VTableFixups Directory:    
// 0        [0       ] address [size] of Export Address Table:      
// 0        [0       ] address [size] of Precompile Header:         
// Code Manager Table:
//  default
// Export Address Table Jumps:
// No data.

.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 1:0:5000:0
}
.assembly extern /*23000002*/ stdole
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:3300:0
}
.assembly /*20000001*/ devaxcorex.interop
{
  .custom /*0C000001:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 39 65 30 32 66 66 38 2D 34 39 66 31   // ..$29e02ff8-49f1
                                                                                                                                                                   2D 34 33 39 64 2D 38 35 66 31 2D 62 32 37 61 32   // -439d-85f1-b27a2
                                                                                                                                                                   32 66 33 37 37 63 38 00 00 )                      // 2f377c8..
  .custom /*0C000002:0A000005*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ImportedFromTypeLibAttribute/* 0100000A */::.ctor(string) /* 0A000005 */ = ( 01 00 0A 64 65 76 61 78 63 6F 72 65 78 00 00 )    // ...devaxcorex..
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module devaxcorex.dll
// MVID: {00C44165-CDC1-4906-826F-C3574DCCA0CA}
.imagebase 0x00400000
.subsystem 0x00000003
.file alignment 4096
.corflags 0x00000001
// Image base: 0x03080000
//
// ============== CLASS STRUCTURE DECLARATION ==================
//
.namespace MSR.LST.MDShow
{
  .class /*02000002*/ interface public abstract auto ansi import ICreateDevEnum
  {
  } // end of class ICreateDevEnum

  .class /*02000003*/ interface public abstract auto ansi import IEnumMoniker
  {
  } // end of class IEnumMoniker

  .class /*02000004*/ interface public abstract auto ansi import IMoniker
         implements MSR.LST.MDShow.IPersistStream/* 02000005 */
  {
  } // end of class IMoniker

  .class /*02000005*/ interface public abstract auto ansi import IPersistStream
         implements MSR.LST.MDShow.IPersist/* 02000006 */
  {
  } // end of class IPersistStream

  .class /*02000006*/ interface public abstract auto ansi import IPersist
  {
  } // end of class IPersist

  .class /*02000007*/ interface public abstract auto ansi import IStream
         implements MSR.LST.MDShow.ISequentialStream/* 02000008 */
  {
  } // end of class IStream

  .class /*02000008*/ interface public abstract auto ansi import ISequentialStream
  {
  } // end of class ISequentialStream

  .class /*02000009*/ public sequential ansi sealed beforefieldinit _LARGE_INTEGER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _LARGE_INTEGER

  .class /*0200000A*/ public sequential ansi sealed beforefieldinit _ULARGE_INTEGER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _ULARGE_INTEGER

  .class /*0200000B*/ public sequential ansi sealed beforefieldinit tagSTATSTG
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagSTATSTG

  .class /*0200000C*/ public sequential ansi sealed beforefieldinit _FILETIME
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _FILETIME

  .class /*0200000D*/ interface public abstract auto ansi import IBindCtx
  {
  } // end of class IBindCtx

  .class /*0200000E*/ public sequential ansi sealed beforefieldinit tagBIND_OPTS2
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagBIND_OPTS2

  .class /*0200000F*/ public sequential ansi sealed beforefieldinit _COSERVERINFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _COSERVERINFO

  .class /*02000010*/ public sequential ansi sealed beforefieldinit _COAUTHINFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _COAUTHINFO

  .class /*02000011*/ public sequential ansi sealed beforefieldinit _COAUTHIDENTITY
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _COAUTHIDENTITY

  .class /*02000012*/ interface public abstract auto ansi import IRunningObjectTable
  {
  } // end of class IRunningObjectTable

  .class /*02000013*/ interface public abstract auto ansi import IEnumString
  {
  } // end of class IEnumString

  .class /*02000014*/ public sequential ansi sealed beforefieldinit _AMMediaType
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _AMMediaType

  .class /*02000015*/ public auto ansi sealed _PinDirection
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _PinDirection

  .class /*02000016*/ public sequential ansi sealed beforefieldinit _AllocatorProperties
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _AllocatorProperties

  .class /*02000017*/ interface public abstract auto ansi import IAMovieSetup
  {
  } // end of class IAMovieSetup

  .class /*02000018*/ interface public abstract auto ansi import IEnumFilters
  {
  } // end of class IEnumFilters

  .class /*02000019*/ interface public abstract auto ansi import IBaseFilter
         implements MSR.LST.MDShow.IMediaFilter/* 0200001A */
  {
  } // end of class IBaseFilter

  .class /*0200001A*/ interface public abstract auto ansi import IMediaFilter
         implements MSR.LST.MDShow.IPersist/* 02000006 */
  {
  } // end of class IMediaFilter

  .class /*0200001B*/ public auto ansi sealed _FilterState
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _FilterState

  .class /*0200001C*/ interface public abstract auto ansi import IReferenceClock
  {
  } // end of class IReferenceClock

  .class /*0200001D*/ interface public abstract auto ansi import IEnumPins
  {
  } // end of class IEnumPins

  .class /*0200001E*/ interface public abstract auto ansi import IPin
  {
  } // end of class IPin

  .class /*0200001F*/ public sequential ansi sealed beforefieldinit _PinInfo
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _PinInfo

  .class /*02000020*/ interface public abstract auto ansi import IEnumMediaTypes
  {
  } // end of class IEnumMediaTypes

  .class /*02000021*/ public sequential ansi sealed beforefieldinit _FilterInfo
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _FilterInfo

  .class /*02000022*/ interface public abstract auto ansi import IFilterGraph
  {
  } // end of class IFilterGraph

  .class /*02000023*/ interface public abstract auto ansi import IMediaSample
  {
  } // end of class IMediaSample

  .class /*02000024*/ interface public abstract auto ansi import IMemAllocator
  {
  } // end of class IMemAllocator

  .class /*02000025*/ interface public abstract auto ansi import IMemAllocatorCallbackTemp
         implements MSR.LST.MDShow.IMemAllocator/* 02000024 */
  {
  } // end of class IMemAllocatorCallbackTemp

  .class /*02000026*/ interface public abstract auto ansi import IMemAllocatorNotifyCallbackTemp
  {
  } // end of class IMemAllocatorNotifyCallbackTemp

  .class /*02000027*/ interface public abstract auto ansi import IMemInputPin
  {
  } // end of class IMemInputPin

  .class /*02000028*/ interface public abstract auto ansi import IReferenceClock2
         implements MSR.LST.MDShow.IReferenceClock/* 0200001C */
  {
  } // end of class IReferenceClock2

  .class /*02000029*/ public auto ansi sealed tagAM_SAMPLE_PROPERTY_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagAM_SAMPLE_PROPERTY_FLAGS

  .class /*0200002A*/ public sequential ansi sealed beforefieldinit tagAM_SAMPLE2_PROPERTIES
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagAM_SAMPLE2_PROPERTIES

  .class /*0200002B*/ interface public abstract auto ansi import IMediaSample2
         implements MSR.LST.MDShow.IMediaSample/* 02000023 */
  {
  } // end of class IMediaSample2

  .class /*0200002C*/ public auto ansi sealed AM_SEEKING_SeekingFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class AM_SEEKING_SeekingFlags

  .class /*0200002D*/ public auto ansi sealed AM_SEEKING_SeekingCapabilities
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class AM_SEEKING_SeekingCapabilities

  .class /*0200002E*/ interface public abstract auto ansi import IMediaSeeking
  {
  } // end of class IMediaSeeking

  .class /*0200002F*/ interface public abstract auto ansi import IEnumRegFilters
  {
  } // end of class IEnumRegFilters

  .class /*02000030*/ public sequential ansi sealed beforefieldinit REGFILTER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class REGFILTER

  .class /*02000031*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0133_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0133_0001

  .class /*02000032*/ interface public abstract auto ansi import IFileSourceFilter
  {
  } // end of class IFileSourceFilter

  .class /*02000033*/ interface public abstract auto ansi import IFileSinkFilter
  {
  } // end of class IFileSinkFilter

  .class /*02000034*/ interface public abstract auto ansi import IFileSinkFilter2
         implements MSR.LST.MDShow.IFileSinkFilter/* 02000033 */
  {
  } // end of class IFileSinkFilter2

  .class /*02000035*/ interface public abstract auto ansi import IGraphBuilder
         implements MSR.LST.MDShow.IFilterGraph/* 02000022 */
  {
  } // end of class IGraphBuilder

  .class /*02000036*/ interface public abstract auto ansi import ICaptureGraphBuilder
  {
  } // end of class ICaptureGraphBuilder

  .class /*02000037*/ interface public abstract auto ansi import IAMCopyCaptureFileProgress
  {
  } // end of class IAMCopyCaptureFileProgress

  .class /*02000038*/ interface public abstract auto ansi import ICaptureGraphBuilder2
  {
  } // end of class ICaptureGraphBuilder2

  .class /*02000039*/ interface public abstract auto ansi import IFilterMapper
  {
  } // end of class IFilterMapper

  .class /*0200003A*/ interface public abstract auto ansi import IFilterMapper2
  {
  } // end of class IFilterMapper2

  .class /*0200003B*/ public sequential ansi sealed beforefieldinit REGFILTER2
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class REGFILTER2

  .class /*0200003C*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0006
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0006

  .class /*0200003D*/ public explicit ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0007
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0007

  .class /*0200003E*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0008
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0008

  .class /*0200003F*/ public sequential ansi sealed beforefieldinit REGFILTERPINS
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class REGFILTERPINS

  .class /*02000040*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0002
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0002

  .class /*02000041*/ public sequential ansi sealed beforefieldinit REGPINTYPES
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class REGPINTYPES

  .class /*02000042*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0001

  .class /*02000043*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0009
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0009

  .class /*02000044*/ public sequential ansi sealed beforefieldinit REGFILTERPINS2
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class REGFILTERPINS2

  .class /*02000045*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0005
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0005

  .class /*02000046*/ public sequential ansi sealed beforefieldinit REGPINMEDIUM
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class REGPINMEDIUM

  .class /*02000047*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0003
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0003

  .class /*02000048*/ interface public abstract auto ansi import IMediaEventSink
  {
  } // end of class IMediaEventSink

  .class /*02000049*/ interface public abstract auto ansi import IOverlay
  {
  } // end of class IOverlay

  .class /*0200004A*/ public sequential ansi sealed beforefieldinit tagPALETTEENTRY
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagPALETTEENTRY

  .class /*0200004B*/ public sequential ansi sealed beforefieldinit tagCOLORKEY
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagCOLORKEY

  .class /*0200004C*/ public sequential ansi sealed beforefieldinit _RemotableHandle
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _RemotableHandle

  .class /*0200004D*/ public explicit ansi sealed __MIDL_IWinTypes_0009
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL_IWinTypes_0009

  .class /*0200004E*/ public sequential ansi sealed beforefieldinit tagRECT
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagRECT

  .class /*0200004F*/ public sequential ansi sealed beforefieldinit _RGNDATA
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _RGNDATA

  .class /*02000050*/ public sequential ansi sealed beforefieldinit _RGNDATAHEADER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class _RGNDATAHEADER

  .class /*02000051*/ interface public abstract auto ansi import IOverlayNotify
  {
  } // end of class IOverlayNotify

  .class /*02000052*/ interface public abstract auto ansi import IOverlayNotify2
         implements MSR.LST.MDShow.IOverlayNotify/* 02000051 */
  {
  } // end of class IOverlayNotify2

  .class /*02000053*/ interface public abstract auto ansi import IQualityControl
  {
  } // end of class IQualityControl

  .class /*02000054*/ public sequential ansi sealed beforefieldinit tagQuality
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagQuality

  .class /*02000055*/ public auto ansi sealed tagQualityMessageType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagQualityMessageType

  .class /*02000056*/ interface public abstract auto ansi import ISeekingPassThru
  {
  } // end of class ISeekingPassThru

  .class /*02000057*/ interface public abstract auto ansi import IAMStreamConfig
  {
  } // end of class IAMStreamConfig

  .class /*02000058*/ interface public abstract auto ansi import IAMDevMemoryAllocator
  {
  } // end of class IAMDevMemoryAllocator

  .class /*02000059*/ interface public abstract auto ansi import IAMDevMemoryControl
  {
  } // end of class IAMDevMemoryControl

  .class /*0200005A*/ interface public abstract auto ansi import IConfigInterleaving
  {
  } // end of class IConfigInterleaving

  .class /*0200005B*/ public auto ansi sealed InterleavingMode
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class InterleavingMode

  .class /*0200005C*/ public auto ansi sealed __MIDL_IConfigInterleaving_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL_IConfigInterleaving_0001

  .class /*0200005D*/ interface public abstract auto ansi import IConfigAviMux
  {
  } // end of class IConfigAviMux

  .class /*0200005E*/ interface public abstract auto ansi import IAMVideoCompression
  {
  } // end of class IAMVideoCompression

  .class /*0200005F*/ interface public abstract auto ansi import IAMVfwCaptureDialogs
  {
  } // end of class IAMVfwCaptureDialogs

  .class /*02000060*/ interface public abstract auto ansi import IAMVfwCompressDialogs
  {
  } // end of class IAMVfwCompressDialogs

  .class /*02000061*/ interface public abstract auto ansi import IAMDroppedFrames
  {
  } // end of class IAMDroppedFrames

  .class /*02000062*/ interface public abstract auto ansi import IAMAudioInputMixer
  {
  } // end of class IAMAudioInputMixer

  .class /*02000063*/ interface public abstract auto ansi import IAMBufferNegotiation
  {
  } // end of class IAMBufferNegotiation

  .class /*02000064*/ interface public abstract auto ansi import IAMAnalogVideoDecoder
  {
  } // end of class IAMAnalogVideoDecoder

  .class /*02000065*/ interface public abstract auto ansi import IAMVideoProcAmp
  {
  } // end of class IAMVideoProcAmp

  .class /*02000066*/ interface public abstract auto ansi import IAMAnalogVideoEncoder
  {
  } // end of class IAMAnalogVideoEncoder

  .class /*02000067*/ interface public abstract auto ansi import IAMCameraControl
  {
  } // end of class IAMCameraControl

  .class /*02000068*/ interface public abstract auto ansi import IAMCrossbar
  {
  } // end of class IAMCrossbar

  .class /*02000069*/ interface public abstract auto ansi import IAMTuner
  {
  } // end of class IAMTuner

  .class /*0200006A*/ public auto ansi sealed tagAMTunerModeType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagAMTunerModeType

  .class /*0200006B*/ interface public abstract auto ansi import IAMTunerNotification
  {
  } // end of class IAMTunerNotification

  .class /*0200006C*/ public auto ansi sealed tagAMTunerEventType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagAMTunerEventType

  .class /*0200006D*/ interface public abstract auto ansi import IAMTVTuner
         implements MSR.LST.MDShow.IAMTuner/* 02000069 */
  {
  } // end of class IAMTVTuner

  .class /*0200006E*/ public auto ansi sealed tagTunerInputType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagTunerInputType

  .class /*0200006F*/ interface public abstract auto ansi import IKsPropertySet
  {
  } // end of class IKsPropertySet

  .class /*02000070*/ interface public abstract auto ansi import IAMPhysicalPinInfo
  {
  } // end of class IAMPhysicalPinInfo

  .class /*02000071*/ interface public abstract auto ansi import IAMExtDevice
  {
  } // end of class IAMExtDevice

  .class /*02000072*/ interface public abstract auto ansi import IAMExtTransport
  {
  } // end of class IAMExtTransport

  .class /*02000073*/ interface public abstract auto ansi import IAMTimecodeReader
  {
  } // end of class IAMTimecodeReader

  .class /*02000074*/ public sequential ansi sealed beforefieldinit tagTIMECODE_SAMPLE
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagTIMECODE_SAMPLE

  .class /*02000075*/ public sequential ansi sealed beforefieldinit tagTIMECODE
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagTIMECODE

  .class /*02000076*/ interface public abstract auto ansi import IAMTimecodeGenerator
  {
  } // end of class IAMTimecodeGenerator

  .class /*02000077*/ interface public abstract auto ansi import IAMTimecodeDisplay
  {
  } // end of class IAMTimecodeDisplay

  .class /*02000078*/ interface public abstract auto ansi import IDrawVideoImage
  {
  } // end of class IDrawVideoImage

  .class /*02000079*/ interface public abstract auto ansi import IDecimateVideoImage
  {
  } // end of class IDecimateVideoImage

  .class /*0200007A*/ interface public abstract auto ansi import IAMVideoDecimationProperties
  {
  } // end of class IAMVideoDecimationProperties

  .class /*0200007B*/ public auto ansi sealed _DECIMATION_USAGE
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _DECIMATION_USAGE

  .class /*0200007C*/ interface public abstract auto ansi import IAMLatency
  {
  } // end of class IAMLatency

  .class /*0200007D*/ interface public abstract auto ansi import IAMPushSource
         implements MSR.LST.MDShow.IAMLatency/* 0200007C */
  {
  } // end of class IAMPushSource

  .class /*0200007E*/ interface public abstract auto ansi import IAMAudioRendererStats
  {
  } // end of class IAMAudioRendererStats

  .class /*0200007F*/ interface public abstract auto ansi import IAMGraphStreams
  {
  } // end of class IAMGraphStreams

  .class /*02000080*/ interface public abstract auto ansi import IAMOverlayFX
  {
  } // end of class IAMOverlayFX

  .class /*02000081*/ interface public abstract auto ansi import IAMOpenProgress
  {
  } // end of class IAMOpenProgress

  .class /*02000082*/ interface public abstract auto ansi import IMpeg2Demultiplexer
  {
  } // end of class IMpeg2Demultiplexer

  .class /*02000083*/ interface public abstract auto ansi import IMPEG2StreamIdMap
  {
  } // end of class IMPEG2StreamIdMap

  .class /*02000084*/ interface public abstract auto ansi import IEnumStreamIdMap
  {
  } // end of class IEnumStreamIdMap

  .class /*02000085*/ public sequential ansi sealed beforefieldinit STREAM_ID_MAP
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class STREAM_ID_MAP

  .class /*02000086*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0358_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0358_0001

  .class /*02000087*/ interface public abstract auto ansi import IAMClockSlave
  {
  } // end of class IAMClockSlave

  .class /*02000088*/ interface public abstract auto ansi import IEncoderAPI
  {
  } // end of class IEncoderAPI

  .class /*02000089*/ interface public abstract auto ansi import IVideoEncoder
         implements MSR.LST.MDShow.IEncoderAPI/* 02000088 */
  {
  } // end of class IVideoEncoder

  .class /*0200008A*/ interface public abstract auto ansi import IAMGraphBuilderCallback
  {
  } // end of class IAMGraphBuilderCallback

  .class /*0200008B*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004

  .class /*0200008C*/ interface public abstract auto ansi import IFilterMapper3
         implements MSR.LST.MDShow.IFilterMapper2/* 0200003A */
  {
  } // end of class IFilterMapper3

  .class /*0200008D*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0001

  .class /*0200008E*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002

  .class /*0200008F*/ public auto ansi sealed AM_FILESINK_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class AM_FILESINK_FLAGS

  .class /*02000090*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0148_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0148_0001

  .class /*02000091*/ public auto ansi sealed _AM_RENSDEREXFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AM_RENSDEREXFLAGS

  .class /*02000092*/ interface public abstract auto ansi import IFilterGraph2
         implements MSR.LST.MDShow.IGraphBuilder/* 02000035 */
  {
  } // end of class IFilterGraph2

  .class /*02000093*/ interface public abstract auto ansi import IStreamBuilder
  {
  } // end of class IStreamBuilder

  .class /*02000094*/ interface public abstract auto ansi import IAsyncReader
  {
  } // end of class IAsyncReader

  .class /*02000095*/ interface public abstract auto ansi import IGraphVersion
  {
  } // end of class IGraphVersion

  .class /*02000096*/ interface public abstract auto ansi import IResourceConsumer
  {
  } // end of class IResourceConsumer

  .class /*02000097*/ interface public abstract auto ansi import IResourceManager
  {
  } // end of class IResourceManager

  .class /*02000098*/ interface public abstract auto ansi import IDistributorNotify
  {
  } // end of class IDistributorNotify

  .class /*02000099*/ public auto ansi sealed AM_STREAM_INFO_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class AM_STREAM_INFO_FLAGS

  .class /*0200009A*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001

  .class /*0200009B*/ public sequential ansi sealed beforefieldinit AM_STREAM_INFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class AM_STREAM_INFO

  .class /*0200009C*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0002
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0002

  .class /*0200009D*/ interface public abstract auto ansi import IAMStreamControl
  {
  } // end of class IAMStreamControl

  .class /*0200009E*/ public auto ansi sealed CompressionCaps
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class CompressionCaps

  .class /*0200009F*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001

  .class /*020000A0*/ public auto ansi sealed VfwCaptureDialogs
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class VfwCaptureDialogs

  .class /*020000A1*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0001

  .class /*020000A2*/ public auto ansi sealed VfwCompressDialogs
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class VfwCompressDialogs

  .class /*020000A3*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002

  .class /*020000A4*/ public auto ansi sealed tagAnalogVideoStandard
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagAnalogVideoStandard

  .class /*020000A5*/ public auto ansi sealed VideoCopyProtectionType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class VideoCopyProtectionType

  .class /*020000A6*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0172_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0172_0001

  .class /*020000A7*/ public auto ansi sealed tagPhysicalConnectorType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagPhysicalConnectorType

  .class /*020000A8*/ public auto ansi sealed tagVideoProcAmpProperty
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagVideoProcAmpProperty

  .class /*020000A9*/ public auto ansi sealed tagVideoProcAmpFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagVideoProcAmpFlags

  .class /*020000AA*/ public auto ansi sealed tagCameraControlProperty
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagCameraControlProperty

  .class /*020000AB*/ public auto ansi sealed tagCameraControlFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagCameraControlFlags

  .class /*020000AC*/ public auto ansi sealed tagVideoControlFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagVideoControlFlags

  .class /*020000AD*/ interface public abstract auto ansi import IAMVideoControl
  {
  } // end of class IAMVideoControl

  .class /*020000AE*/ public sequential ansi sealed beforefieldinit tagSIZE
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class tagSIZE

  .class /*020000AF*/ public auto ansi sealed tagAMTunerSubChannel
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagAMTunerSubChannel

  .class /*020000B0*/ public auto ansi sealed tagAMTunerSignalStrength
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagAMTunerSignalStrength

  .class /*020000B1*/ interface public abstract auto ansi import IBPCSatelliteTuner
         implements MSR.LST.MDShow.IAMTuner/* 02000069 */
  {
  } // end of class IBPCSatelliteTuner

  .class /*020000B2*/ public auto ansi sealed tagTVAudioMode
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagTVAudioMode

  .class /*020000B3*/ public auto ansi sealed tagAMTVAudioEventType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class tagAMTVAudioEventType

  .class /*020000B4*/ interface public abstract auto ansi import IAMTVAudioNotification
  {
  } // end of class IAMTVAudioNotification

  .class /*020000B5*/ interface public abstract auto ansi import IAMTVAudio
  {
  } // end of class IAMTVAudio

  .class /*020000B6*/ public auto ansi sealed AMPROPERTY_PIN
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class AMPROPERTY_PIN

  .class /*020000B7*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0184_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0184_0001

  .class /*020000B8*/ interface public abstract auto ansi import IMediaPropertyBag
         implements MSR.LST.MDShow.IPropertyBag/* 020000B9 */
  {
  } // end of class IMediaPropertyBag

  .class /*020000B9*/ interface public abstract auto ansi import IPropertyBag
  {
  } // end of class IPropertyBag

  .class /*020000BA*/ interface public abstract auto ansi import IErrorLog
  {
  } // end of class IErrorLog

  .class /*020000BB*/ interface public abstract auto ansi import IPersistMediaPropertyBag
         implements MSR.LST.MDShow.IPersist/* 02000006 */
  {
  } // end of class IPersistMediaPropertyBag

  .class /*020000BC*/ public auto ansi sealed _AMSTREAMSELECTINFOFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AMSTREAMSELECTINFOFLAGS

  .class /*020000BD*/ public auto ansi sealed _AMSTREAMSELECTENABLEFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AMSTREAMSELECTENABLEFLAGS

  .class /*020000BE*/ interface public abstract auto ansi import IAMStreamSelect
  {
  } // end of class IAMStreamSelect

  .class /*020000BF*/ public auto ansi sealed _AMRESCTL_RESERVEFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AMRESCTL_RESERVEFLAGS

  .class /*020000C0*/ interface public abstract auto ansi import IAMResourceControl
  {
  } // end of class IAMResourceControl

  .class /*020000C1*/ interface public abstract auto ansi import IAMClockAdjust
  {
  } // end of class IAMClockAdjust

  .class /*020000C2*/ public auto ansi sealed _AM_FILTER_MISC_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AM_FILTER_MISC_FLAGS

  .class /*020000C3*/ interface public abstract auto ansi import IAMFilterMiscFlags
  {
  } // end of class IAMFilterMiscFlags

  .class /*020000C4*/ interface public abstract auto ansi import IVideoFrameStep
  {
  } // end of class IVideoFrameStep

  .class /*020000C5*/ public auto ansi sealed _AM_PUSHSOURCE_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AM_PUSHSOURCE_FLAGS

  .class /*020000C6*/ interface public abstract auto ansi import IAMDeviceRemoval
  {
  } // end of class IAMDeviceRemoval

  .class /*020000C7*/ public sequential ansi sealed beforefieldinit DVINFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class DVINFO

  .class /*020000C8*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0348_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0348_0001

  .class /*020000C9*/ public auto ansi sealed _DVENCODERRESOLUTION
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _DVENCODERRESOLUTION

  .class /*020000CA*/ public auto ansi sealed _DVENCODERVIDEOFORMAT
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _DVENCODERVIDEOFORMAT

  .class /*020000CB*/ public auto ansi sealed _DVENCODERFORMAT
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _DVENCODERFORMAT

  .class /*020000CC*/ interface public abstract auto ansi import IDVEnc
  {
  } // end of class IDVEnc

  .class /*020000CD*/ public auto ansi sealed _DVDECODERRESOLUTION
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _DVDECODERRESOLUTION

  .class /*020000CE*/ public auto ansi sealed _DVRESOLUTION
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _DVRESOLUTION

  .class /*020000CF*/ interface public abstract auto ansi import IIPDVDec
  {
  } // end of class IIPDVDec

  .class /*020000D0*/ interface public abstract auto ansi import IDVRGB219
  {
  } // end of class IDVRGB219

  .class /*020000D1*/ interface public abstract auto ansi import IDVSplitter
  {
  } // end of class IDVSplitter

  .class /*020000D2*/ public auto ansi sealed _AM_AUDIO_RENDERER_STAT_PARAM
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AM_AUDIO_RENDERER_STAT_PARAM

  .class /*020000D3*/ public auto ansi sealed _AM_INTF_SEARCH_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class _AM_INTF_SEARCH_FLAGS

  .class /*020000D4*/ public auto ansi sealed AMOVERLAYFX
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class AMOVERLAYFX

  .class /*020000D5*/ interface public abstract auto ansi import IRegisterServiceProvider
  {
  } // end of class IRegisterServiceProvider

  .class /*020000D6*/ public sequential ansi sealed beforefieldinit CodecAPIEventData
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
  } // end of class CodecAPIEventData

  .class /*020000D7*/ interface public abstract auto ansi import ICodecAPI
  {
  } // end of class ICodecAPI

  .class /*020000D8*/ interface public abstract auto ansi import IGetCapabilitiesKey
  {
  } // end of class IGetCapabilitiesKey

  .class /*020000D9*/ public auto ansi sealed VIDEOENCODER_BITRATE_MODE
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class VIDEOENCODER_BITRATE_MODE

  .class /*020000DA*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0367_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0367_0001

  .class /*020000DB*/ interface public abstract auto ansi import IAMDecoderCaps
  {
  } // end of class IAMDecoderCaps

} // end of namespace MSR.LST.MDShow


// =============================================================


// =============== GLOBAL FIELDS AND METHODS ===================


// =============================================================


// =============== CLASS MEMBERS DECLARATION ===================
//   note that class flags, 'extends' and 'implements' clauses
//          are provided here for information only

.namespace MSR.LST.MDShow
{
  .class /*02000002*/ interface public abstract auto ansi import ICreateDevEnum
  {
    .custom /*0C000003:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000004:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 39 38 34 30 38 32 32 2D 35 42 38 34   // ..$29840822-5B84
                                                                                                                                                                     2D 31 31 44 30 2D 42 44 33 42 2D 30 30 41 30 43   // -11D0-BD3B-00A0C
                                                                                                                                                                     39 31 31 43 45 38 36 00 00 )                      // 911CE86..
    .method /*06000001*/ public hidebysig newslot abstract virtual 
            instance void  CreateClassEnumerator([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& clsidDeviceClass,
                                                 [out] class MSR.LST.MDShow.IEnumMoniker/* 02000003 */&  marshal( interface) ppenumMoniker,
                                                 [in] unsigned int32 dwFlags) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 10 12 0C 09
    {
    } // end of method ICreateDevEnum::CreateClassEnumerator

  } // end of class ICreateDevEnum

  .class /*02000003*/ interface public abstract auto ansi import IEnumMoniker
  {
    .custom /*0C000005:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 31 30 32 2D 30 30 30 30   // ..$00000102-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .custom /*0C000006:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000002*/ public hidebysig newslot abstract virtual 
            instance void  RemoteNext([in] unsigned int32 celt,
                                      [out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) rgelt,
                                      [out] unsigned int32& pceltFetched) runtime managed internalcall
    // SIG: 20 03 01 09 10 12 10 10 09
    {
    } // end of method IEnumMoniker::RemoteNext

    .method /*06000003*/ public hidebysig newslot abstract virtual 
            instance void  Skip([in] unsigned int32 celt) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IEnumMoniker::Skip

    .method /*06000004*/ public hidebysig newslot abstract virtual 
            instance void  Reset() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IEnumMoniker::Reset

    .method /*06000005*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IEnumMoniker/* 02000003 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 0C
    {
    } // end of method IEnumMoniker::Clone

  } // end of class IEnumMoniker

  .class /*02000004*/ interface public abstract auto ansi import IMoniker
         implements MSR.LST.MDShow.IPersistStream/* 02000005 */
  {
    .custom /*0C000007:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000008:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 30 30 46 2D 30 30 30 30   // ..$0000000F-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .method /*06000006*/ public hidebysig newslot abstract virtual 
            instance void  GetClassID([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pClassID) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IMoniker::GetClassID

    .method /*06000007*/ public hidebysig newslot abstract virtual 
            instance void  IsDirty() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMoniker::IsDirty

    .method /*06000008*/ public hidebysig newslot abstract virtual 
            instance void  Load([in] class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) pstm) runtime managed internalcall
    // SIG: 20 01 01 12 1C
    {
    } // end of method IMoniker::Load

    .method /*06000009*/ public hidebysig newslot abstract virtual 
            instance void  Save([in] class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) pstm,
                                [in] int32 fClearDirty) runtime managed internalcall
    // SIG: 20 02 01 12 1C 08
    {
    } // end of method IMoniker::Save

    .method /*0600000A*/ public hidebysig newslot abstract virtual 
            instance void  GetSizeMax([out] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */& pcbSize) runtime managed internalcall
    // SIG: 20 01 01 10 11 28
    {
    } // end of method IMoniker::GetSizeMax

    .method /*0600000B*/ public hidebysig newslot abstract virtual 
            instance void  RemoteBindToObject([in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pbc,
                                              [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkToLeft,
                                              [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& riidResult,
                                              [out] object&  marshal( iunknown) ppvResult) runtime managed internalcall
    // SIG: 20 04 01 12 34 12 10 10 11 11 10 1C
    {
    } // end of method IMoniker::RemoteBindToObject

    .method /*0600000C*/ public hidebysig newslot abstract virtual 
            instance void  RemoteBindToStorage([in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pbc,
                                               [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkToLeft,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& riid,
                                               [out] object&  marshal( iunknown) ppvObj) runtime managed internalcall
    // SIG: 20 04 01 12 34 12 10 10 11 11 10 1C
    {
    } // end of method IMoniker::RemoteBindToStorage

    .method /*0600000D*/ public hidebysig newslot abstract virtual 
            instance void  Reduce([in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pbc,
                                  [in] unsigned int32 dwReduceHowFar,
                                  [in][out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppmkToLeft,
                                  [out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppmkReduced) runtime managed internalcall
    // SIG: 20 04 01 12 34 09 10 12 10 10 12 10
    {
    } // end of method IMoniker::Reduce

    .method /*0600000E*/ public hidebysig newslot abstract virtual 
            instance void  ComposeWith([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkRight,
                                       [in] int32 fOnlyIfNotGeneric,
                                       [out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppmkComposite) runtime managed internalcall
    // SIG: 20 03 01 12 10 08 10 12 10
    {
    } // end of method IMoniker::ComposeWith

    .method /*0600000F*/ public hidebysig newslot abstract virtual 
            instance void  Enum([in] int32 fForward,
                                [out] class MSR.LST.MDShow.IEnumMoniker/* 02000003 */&  marshal( interface) ppenumMoniker) runtime managed internalcall
    // SIG: 20 02 01 08 10 12 0C
    {
    } // end of method IMoniker::Enum

    .method /*06000010*/ public hidebysig newslot abstract virtual 
            instance void  IsEqual([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkOtherMoniker) runtime managed internalcall
    // SIG: 20 01 01 12 10
    {
    } // end of method IMoniker::IsEqual

    .method /*06000011*/ public hidebysig newslot abstract virtual 
            instance void  Hash([out] unsigned int32& pdwHash) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IMoniker::Hash

    .method /*06000012*/ public hidebysig newslot abstract virtual 
            instance void  IsRunning([in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pbc,
                                     [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkToLeft,
                                     [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkNewlyRunning) runtime managed internalcall
    // SIG: 20 03 01 12 34 12 10 12 10
    {
    } // end of method IMoniker::IsRunning

    .method /*06000013*/ public hidebysig newslot abstract virtual 
            instance void  GetTimeOfLastChange([in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pbc,
                                               [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkToLeft,
                                               [out] valuetype MSR.LST.MDShow._FILETIME/* 0200000C */& pfiletime) runtime managed internalcall
    // SIG: 20 03 01 12 34 12 10 10 11 30
    {
    } // end of method IMoniker::GetTimeOfLastChange

    .method /*06000014*/ public hidebysig newslot abstract virtual 
            instance void  Inverse([out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppmk) runtime managed internalcall
    // SIG: 20 01 01 10 12 10
    {
    } // end of method IMoniker::Inverse

    .method /*06000015*/ public hidebysig newslot abstract virtual 
            instance void  CommonPrefixWith([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkOther,
                                            [out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppmkPrefix) runtime managed internalcall
    // SIG: 20 02 01 12 10 10 12 10
    {
    } // end of method IMoniker::CommonPrefixWith

    .method /*06000016*/ public hidebysig newslot abstract virtual 
            instance void  RelativePathTo([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkOther,
                                          [out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppmkRelPath) runtime managed internalcall
    // SIG: 20 02 01 12 10 10 12 10
    {
    } // end of method IMoniker::RelativePathTo

    .method /*06000017*/ public hidebysig newslot abstract virtual 
            instance void  GetDisplayName([in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pbc,
                                          [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkToLeft,
                                          [out] string&  marshal( lpwstr) ppszDisplayName) runtime managed internalcall
    // SIG: 20 03 01 12 34 12 10 10 0E
    {
    } // end of method IMoniker::GetDisplayName

    .method /*06000018*/ public hidebysig newslot abstract virtual 
            instance void  ParseDisplayName([in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pbc,
                                            [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkToLeft,
                                            [in] string  marshal( lpwstr) pszDisplayName,
                                            [out] unsigned int32& pchEaten,
                                            [out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppmkOut) runtime managed internalcall
    // SIG: 20 05 01 12 34 12 10 0E 10 09 10 12 10
    {
    } // end of method IMoniker::ParseDisplayName

    .method /*06000019*/ public hidebysig newslot abstract virtual 
            instance void  IsSystemMoniker([out] unsigned int32& pdwMksys) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IMoniker::IsSystemMoniker

  } // end of class IMoniker

  .class /*02000005*/ interface public abstract auto ansi import IPersistStream
         implements MSR.LST.MDShow.IPersist/* 02000006 */
  {
    .custom /*0C000009:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 31 30 39 2D 30 30 30 30   // ..$00000109-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .custom /*0C00000A:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600001A*/ public hidebysig newslot abstract virtual 
            instance void  GetClassID([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pClassID) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IPersistStream::GetClassID

    .method /*0600001B*/ public hidebysig newslot abstract virtual 
            instance void  IsDirty() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IPersistStream::IsDirty

    .method /*0600001C*/ public hidebysig newslot abstract virtual 
            instance void  Load([in] class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) pstm) runtime managed internalcall
    // SIG: 20 01 01 12 1C
    {
    } // end of method IPersistStream::Load

    .method /*0600001D*/ public hidebysig newslot abstract virtual 
            instance void  Save([in] class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) pstm,
                                [in] int32 fClearDirty) runtime managed internalcall
    // SIG: 20 02 01 12 1C 08
    {
    } // end of method IPersistStream::Save

    .method /*0600001E*/ public hidebysig newslot abstract virtual 
            instance void  GetSizeMax([out] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */& pcbSize) runtime managed internalcall
    // SIG: 20 01 01 10 11 28
    {
    } // end of method IPersistStream::GetSizeMax

  } // end of class IPersistStream

  .class /*02000006*/ interface public abstract auto ansi import IPersist
  {
    .custom /*0C00000B:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00000C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 31 30 43 2D 30 30 30 30   // ..$0000010C-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .method /*0600001F*/ public hidebysig newslot abstract virtual 
            instance void  GetClassID([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pClassID) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IPersist::GetClassID

  } // end of class IPersist

  .class /*02000007*/ interface public abstract auto ansi import IStream
         implements MSR.LST.MDShow.ISequentialStream/* 02000008 */
  {
    .custom /*0C00000D:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00000E:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 30 30 43 2D 30 30 30 30   // ..$0000000C-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .method /*06000020*/ public hidebysig newslot abstract virtual 
            instance void  RemoteRead([out] unsigned int8& pv,
                                      [in] unsigned int32 cb,
                                      [out] unsigned int32& pcbRead) runtime managed internalcall
    // SIG: 20 03 01 10 05 09 10 09
    {
    } // end of method IStream::RemoteRead

    .method /*06000021*/ public hidebysig newslot abstract virtual 
            instance void  RemoteWrite([in] unsigned int8& pv,
                                       [in] unsigned int32 cb,
                                       [out] unsigned int32& pcbWritten) runtime managed internalcall
    // SIG: 20 03 01 10 05 09 10 09
    {
    } // end of method IStream::RemoteWrite

    .method /*06000022*/ public hidebysig newslot abstract virtual 
            instance void  RemoteSeek([in] valuetype MSR.LST.MDShow._LARGE_INTEGER/* 02000009 */ dlibMove,
                                      [in] unsigned int32 dwOrigin,
                                      [out] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */& plibNewPosition) runtime managed internalcall
    // SIG: 20 03 01 11 24 09 10 11 28
    {
    } // end of method IStream::RemoteSeek

    .method /*06000023*/ public hidebysig newslot abstract virtual 
            instance void  SetSize([in] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */ libNewSize) runtime managed internalcall
    // SIG: 20 01 01 11 28
    {
    } // end of method IStream::SetSize

    .method /*06000024*/ public hidebysig newslot abstract virtual 
            instance void  RemoteCopyTo([in] class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) pstm,
                                        [in] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */ cb,
                                        [out] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */& pcbRead,
                                        [out] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */& pcbWritten) runtime managed internalcall
    // SIG: 20 04 01 12 1C 11 28 10 11 28 10 11 28
    {
    } // end of method IStream::RemoteCopyTo

    .method /*06000025*/ public hidebysig newslot abstract virtual 
            instance void  Commit([in] unsigned int32 grfCommitFlags) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IStream::Commit

    .method /*06000026*/ public hidebysig newslot abstract virtual 
            instance void  Revert() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IStream::Revert

    .method /*06000027*/ public hidebysig newslot abstract virtual 
            instance void  LockRegion([in] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */ libOffset,
                                      [in] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */ cb,
                                      [in] unsigned int32 dwLockType) runtime managed internalcall
    // SIG: 20 03 01 11 28 11 28 09
    {
    } // end of method IStream::LockRegion

    .method /*06000028*/ public hidebysig newslot abstract virtual 
            instance void  UnlockRegion([in] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */ libOffset,
                                        [in] valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */ cb,
                                        [in] unsigned int32 dwLockType) runtime managed internalcall
    // SIG: 20 03 01 11 28 11 28 09
    {
    } // end of method IStream::UnlockRegion

    .method /*06000029*/ public hidebysig newslot abstract virtual 
            instance void  Stat([out] valuetype MSR.LST.MDShow.tagSTATSTG/* 0200000B */& pstatstg,
                                [in] unsigned int32 grfStatFlag) runtime managed internalcall
    // SIG: 20 02 01 10 11 2C 09
    {
    } // end of method IStream::Stat

    .method /*0600002A*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IStream/* 02000007 */&  marshal( interface) ppstm) runtime managed internalcall
    // SIG: 20 01 01 10 12 1C
    {
    } // end of method IStream::Clone

  } // end of class IStream

  .class /*02000008*/ interface public abstract auto ansi import ISequentialStream
  {
    .custom /*0C00000F:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000010:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 43 37 33 33 41 33 30 2D 32 41 31 43   // ..$0C733A30-2A1C
                                                                                                                                                                     2D 31 31 43 45 2D 41 44 45 35 2D 30 30 41 41 30   // -11CE-ADE5-00AA0
                                                                                                                                                                     30 34 34 37 37 33 44 00 00 )                      // 044773D..
    .method /*0600002B*/ public hidebysig newslot abstract virtual 
            instance void  RemoteRead([out] unsigned int8& pv,
                                      [in] unsigned int32 cb,
                                      [out] unsigned int32& pcbRead) runtime managed internalcall
    // SIG: 20 03 01 10 05 09 10 09
    {
    } // end of method ISequentialStream::RemoteRead

    .method /*0600002C*/ public hidebysig newslot abstract virtual 
            instance void  RemoteWrite([in] unsigned int8& pv,
                                       [in] unsigned int32 cb,
                                       [out] unsigned int32& pcbWritten) runtime managed internalcall
    // SIG: 20 03 01 10 05 09 10 09
    {
    } // end of method ISequentialStream::RemoteWrite

  } // end of class ISequentialStream

  .class /*02000009*/ public sequential ansi sealed beforefieldinit _LARGE_INTEGER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .field /*04000001*/ public int64 QuadPart
  } // end of class _LARGE_INTEGER

  .class /*0200000A*/ public sequential ansi sealed beforefieldinit _ULARGE_INTEGER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .field /*04000002*/ public unsigned int64 QuadPart
  } // end of class _ULARGE_INTEGER

  .class /*0200000B*/ public sequential ansi sealed beforefieldinit tagSTATSTG
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .field /*04000003*/ public  marshal( lpwstr) string pwcsName
    .field /*04000004*/ public unsigned int32 type
    .field /*04000005*/ public valuetype MSR.LST.MDShow._ULARGE_INTEGER/* 0200000A */ cbSize
    .field /*04000006*/ public valuetype MSR.LST.MDShow._FILETIME/* 0200000C */ mtime
    .field /*04000007*/ public valuetype MSR.LST.MDShow._FILETIME/* 0200000C */ ctime
    .field /*04000008*/ public valuetype MSR.LST.MDShow._FILETIME/* 0200000C */ atime
    .field /*04000009*/ public unsigned int32 grfMode
    .field /*0400000A*/ public unsigned int32 grfLocksSupported
    .field /*0400000B*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ 'clsid'
    .field /*0400000C*/ public unsigned int32 grfStateBits
    .field /*0400000D*/ public unsigned int32 reserved
  } // end of class tagSTATSTG

  .class /*0200000C*/ public sequential ansi sealed beforefieldinit _FILETIME
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*0400000E*/ public unsigned int32 dwLowDateTime
    .field /*0400000F*/ public unsigned int32 dwHighDateTime
  } // end of class _FILETIME

  .class /*0200000D*/ interface public abstract auto ansi import IBindCtx
  {
    .custom /*0C000011:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000012:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 30 30 45 2D 30 30 30 30   // ..$0000000E-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .method /*0600002D*/ public hidebysig newslot abstract virtual 
            instance void  RegisterObjectBound([in] object  marshal( iunknown) punk) runtime managed internalcall
    // SIG: 20 01 01 1C
    {
    } // end of method IBindCtx::RegisterObjectBound

    .method /*0600002E*/ public hidebysig newslot abstract virtual 
            instance void  RevokeObjectBound([in] object  marshal( iunknown) punk) runtime managed internalcall
    // SIG: 20 01 01 1C
    {
    } // end of method IBindCtx::RevokeObjectBound

    .method /*0600002F*/ public hidebysig newslot abstract virtual 
            instance void  ReleaseBoundObjects() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IBindCtx::ReleaseBoundObjects

    .method /*06000030*/ public hidebysig newslot abstract virtual 
            instance void  RemoteSetBindOptions([in] valuetype MSR.LST.MDShow.tagBIND_OPTS2/* 0200000E */& pbindopts) runtime managed internalcall
    // SIG: 20 01 01 10 11 38
    {
    } // end of method IBindCtx::RemoteSetBindOptions

    .method /*06000031*/ public hidebysig newslot abstract virtual 
            instance void  RemoteGetBindOptions([in][out] valuetype MSR.LST.MDShow.tagBIND_OPTS2/* 0200000E */& pbindopts) runtime managed internalcall
    // SIG: 20 01 01 10 11 38
    {
    } // end of method IBindCtx::RemoteGetBindOptions

    .method /*06000032*/ public hidebysig newslot abstract virtual 
            instance void  GetRunningObjectTable([out] class MSR.LST.MDShow.IRunningObjectTable/* 02000012 */&  marshal( interface) pprot) runtime managed internalcall
    // SIG: 20 01 01 10 12 48
    {
    } // end of method IBindCtx::GetRunningObjectTable

    .method /*06000033*/ public hidebysig newslot abstract virtual 
            instance void  RegisterObjectParam([in] string  marshal( lpwstr) pszKey,
                                               [in] object  marshal( iunknown) punk) runtime managed internalcall
    // SIG: 20 02 01 0E 1C
    {
    } // end of method IBindCtx::RegisterObjectParam

    .method /*06000034*/ public hidebysig newslot abstract virtual 
            instance void  GetObjectParam([in] string  marshal( lpwstr) pszKey,
                                          [out] object&  marshal( iunknown) ppunk) runtime managed internalcall
    // SIG: 20 02 01 0E 10 1C
    {
    } // end of method IBindCtx::GetObjectParam

    .method /*06000035*/ public hidebysig newslot abstract virtual 
            instance void  EnumObjectParam([out] class MSR.LST.MDShow.IEnumString/* 02000013 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 4C
    {
    } // end of method IBindCtx::EnumObjectParam

    .method /*06000036*/ public hidebysig newslot abstract virtual 
            instance void  RevokeObjectParam([in] string  marshal( lpwstr) pszKey) runtime managed internalcall
    // SIG: 20 01 01 0E
    {
    } // end of method IBindCtx::RevokeObjectParam

  } // end of class IBindCtx

  .class /*0200000E*/ public sequential ansi sealed beforefieldinit tagBIND_OPTS2
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000013:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000010*/ public unsigned int32 cbStruct
    .field /*04000011*/ public unsigned int32 grfFlags
    .field /*04000012*/ public unsigned int32 grfMode
    .field /*04000013*/ public unsigned int32 dwTickCountDeadline
    .field /*04000014*/ public unsigned int32 dwTrackFlags
    .field /*04000015*/ public unsigned int32 dwClassContext
    .field /*04000016*/ public unsigned int32 locale
    .field /*04000017*/ public native int pServerInfo
    .custom /*0C00001C:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class tagBIND_OPTS2

  .class /*0200000F*/ public sequential ansi sealed beforefieldinit _COSERVERINFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000014:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000018*/ public unsigned int32 dwReserved1
    .field /*04000019*/ public  marshal( lpwstr) string pwszName
    .field /*0400001A*/ public native int pAuthInfo
    .custom /*0C000023:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400001B*/ public unsigned int32 dwReserved2
  } // end of class _COSERVERINFO

  .class /*02000010*/ public sequential ansi sealed beforefieldinit _COAUTHINFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000015:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400001C*/ public unsigned int32 dwAuthnSvc
    .field /*0400001D*/ public unsigned int32 dwAuthzSvc
    .field /*0400001E*/ public  marshal( lpwstr) string pwszServerPrincName
    .field /*0400001F*/ public unsigned int32 dwAuthnLevel
    .field /*04000020*/ public unsigned int32 dwImpersonationLevel
    .field /*04000021*/ public native int pAuthIdentityData
    .custom /*0C00002F:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000022*/ public unsigned int32 dwCapabilities
  } // end of class _COAUTHINFO

  .class /*02000011*/ public sequential ansi sealed beforefieldinit _COAUTHIDENTITY
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000016:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000023*/ public native int User
    .custom /*0C000032:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000024*/ public unsigned int32 UserLength
    .field /*04000025*/ public native int Domain
    .custom /*0C000038:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000026*/ public unsigned int32 DomainLength
    .field /*04000027*/ public native int Password
    .custom /*0C00003D:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000028*/ public unsigned int32 PasswordLength
    .field /*04000029*/ public unsigned int32 Flags
  } // end of class _COAUTHIDENTITY

  .class /*02000012*/ interface public abstract auto ansi import IRunningObjectTable
  {
    .custom /*0C000017:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000018:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 30 31 30 2D 30 30 30 30   // ..$00000010-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .method /*06000037*/ public hidebysig newslot abstract virtual 
            instance void  Register([in] unsigned int32 grfFlags,
                                    [in] object  marshal( iunknown) punkObject,
                                    [in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkObjectName,
                                    [out] unsigned int32& pdwRegister) runtime managed internalcall
    // SIG: 20 04 01 09 1C 12 10 10 09
    {
    } // end of method IRunningObjectTable::Register

    .method /*06000038*/ public hidebysig newslot abstract virtual 
            instance void  Revoke([in] unsigned int32 dwRegister) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IRunningObjectTable::Revoke

    .method /*06000039*/ public hidebysig newslot abstract virtual 
            instance void  IsRunning([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkObjectName) runtime managed internalcall
    // SIG: 20 01 01 12 10
    {
    } // end of method IRunningObjectTable::IsRunning

    .method /*0600003A*/ public hidebysig newslot abstract virtual 
            instance void  GetObject([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkObjectName,
                                     [out] object&  marshal( iunknown) ppunkObject) runtime managed internalcall
    // SIG: 20 02 01 12 10 10 1C
    {
    } // end of method IRunningObjectTable::GetObject

    .method /*0600003B*/ public hidebysig newslot abstract virtual 
            instance void  NoteChangeTime([in] unsigned int32 dwRegister,
                                          [in] valuetype MSR.LST.MDShow._FILETIME/* 0200000C */& pfiletime) runtime managed internalcall
    // SIG: 20 02 01 09 10 11 30
    {
    } // end of method IRunningObjectTable::NoteChangeTime

    .method /*0600003C*/ public hidebysig newslot abstract virtual 
            instance void  GetTimeOfLastChange([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pmkObjectName,
                                               [out] valuetype MSR.LST.MDShow._FILETIME/* 0200000C */& pfiletime) runtime managed internalcall
    // SIG: 20 02 01 12 10 10 11 30
    {
    } // end of method IRunningObjectTable::GetTimeOfLastChange

    .method /*0600003D*/ public hidebysig newslot abstract virtual 
            instance void  EnumRunning([out] class MSR.LST.MDShow.IEnumMoniker/* 02000003 */&  marshal( interface) ppenumMoniker) runtime managed internalcall
    // SIG: 20 01 01 10 12 0C
    {
    } // end of method IRunningObjectTable::EnumRunning

  } // end of class IRunningObjectTable

  .class /*02000013*/ interface public abstract auto ansi import IEnumString
  {
    .custom /*0C000019:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 30 30 30 31 30 31 2D 30 30 30 30   // ..$00000101-0000
                                                                                                                                                                     2D 30 30 30 30 2D 43 30 30 30 2D 30 30 30 30 30   // -0000-C000-00000
                                                                                                                                                                     30 30 30 30 30 34 36 00 00 )                      // 0000046..
    .custom /*0C00001A:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600003E*/ public hidebysig newslot abstract virtual 
            instance void  RemoteNext([in] unsigned int32 celt,
                                      [out] string&  marshal( lpwstr) rgelt,
                                      [out] unsigned int32& pceltFetched) runtime managed internalcall
    // SIG: 20 03 01 09 10 0E 10 09
    {
    } // end of method IEnumString::RemoteNext

    .method /*0600003F*/ public hidebysig newslot abstract virtual 
            instance void  Skip([in] unsigned int32 celt) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IEnumString::Skip

    .method /*06000040*/ public hidebysig newslot abstract virtual 
            instance void  Reset() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IEnumString::Reset

    .method /*06000041*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IEnumString/* 02000013 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 4C
    {
    } // end of method IEnumString::Clone

  } // end of class IEnumString

  .class /*02000014*/ public sequential ansi sealed beforefieldinit _AMMediaType
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C00001B:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400002A*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ majortype
    .field /*0400002B*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ subtype
    .field /*0400002C*/ public int32 bFixedSizeSamples
    .field /*0400002D*/ public int32 bTemporalCompression
    .field /*0400002E*/ public unsigned int32 lSampleSize
    .field /*0400002F*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ formattype
    .field /*04000030*/ public  marshal( iunknown) object punk
    .field /*04000031*/ public unsigned int32 cbFormat
    .field /*04000032*/ public native int pbFormat
    .custom /*0C00004A:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class _AMMediaType

  .class /*02000015*/ public auto ansi sealed _PinDirection
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000033*/ public specialname rtspecialname int32 value__
    .field /*04000034*/ public static literal valuetype MSR.LST.MDShow._PinDirection/* 02000015 */ PINDIR_INPUT = int32(0x00000000)
    .field /*04000035*/ public static literal valuetype MSR.LST.MDShow._PinDirection/* 02000015 */ PINDIR_OUTPUT = int32(0x00000001)
  } // end of class _PinDirection

  .class /*02000016*/ public sequential ansi sealed beforefieldinit _AllocatorProperties
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*04000036*/ public int32 cBuffers
    .field /*04000037*/ public int32 cbBuffer
    .field /*04000038*/ public int32 cbAlign
    .field /*04000039*/ public int32 cbPrefix
  } // end of class _AllocatorProperties

  .class /*02000017*/ interface public abstract auto ansi import IAMovieSetup
  {
    .custom /*0C00001D:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 41 33 44 38 43 45 43 30 2D 37 45 35 41   // ..$A3D8CEC0-7E5A
                                                                                                                                                                     2D 31 31 43 46 2D 42 42 43 35 2D 30 30 38 30 35   // -11CF-BBC5-00805
                                                                                                                                                                     46 36 43 45 46 32 30 00 00 )                      // F6CEF20..
    .custom /*0C00001E:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000042*/ public hidebysig newslot abstract virtual 
            instance void  Register() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMovieSetup::Register

    .method /*06000043*/ public hidebysig newslot abstract virtual 
            instance void  Unregister() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMovieSetup::Unregister

  } // end of class IAMovieSetup

  .class /*02000018*/ interface public abstract auto ansi import IEnumFilters
  {
    .custom /*0C00001F:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 33 2D 30 41 44 34   // ..$56A86893-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C000020:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000044*/ public hidebysig newslot abstract virtual 
            instance void  Next([in] unsigned int32 cFilters,
                                [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppFilter,
                                [out] unsigned int32& pcFetched) runtime managed internalcall
    // SIG: 20 03 01 09 10 12 64 10 09
    {
    } // end of method IEnumFilters::Next

    .method /*06000045*/ public hidebysig newslot abstract virtual 
            instance void  Skip([in] unsigned int32 cFilters) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IEnumFilters::Skip

    .method /*06000046*/ public hidebysig newslot abstract virtual 
            instance void  Reset() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IEnumFilters::Reset

    .method /*06000047*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IEnumFilters/* 02000018 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 60
    {
    } // end of method IEnumFilters::Clone

  } // end of class IEnumFilters

  .class /*02000019*/ interface public abstract auto ansi import IBaseFilter
         implements MSR.LST.MDShow.IMediaFilter/* 0200001A */
  {
    .custom /*0C000021:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000022:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 35 2D 30 41 44 34   // ..$56A86895-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*06000048*/ public hidebysig newslot abstract virtual 
            instance void  GetClassID([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pClassID) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IBaseFilter::GetClassID

    .method /*06000049*/ public hidebysig newslot abstract virtual 
            instance void  Stop() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IBaseFilter::Stop

    .method /*0600004A*/ public hidebysig newslot abstract virtual 
            instance void  Pause() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IBaseFilter::Pause

    .method /*0600004B*/ public hidebysig newslot abstract virtual 
            instance void  Run(int64 tStart) runtime managed internalcall
    // SIG: 20 01 01 0A
    {
    } // end of method IBaseFilter::Run

    .method /*0600004C*/ public hidebysig newslot abstract virtual 
            instance void  GetState([in] unsigned int32 dwMilliSecsTimeout,
                                    [out] valuetype MSR.LST.MDShow._FilterState/* 0200001B */& State) runtime managed internalcall
    // SIG: 20 02 01 09 10 11 6C
    {
    } // end of method IBaseFilter::GetState

    .method /*0600004D*/ public hidebysig newslot abstract virtual 
            instance void  SetSyncSource([in] class MSR.LST.MDShow.IReferenceClock/* 0200001C */  marshal( interface) pClock) runtime managed internalcall
    // SIG: 20 01 01 12 70
    {
    } // end of method IBaseFilter::SetSyncSource

    .method /*0600004E*/ public hidebysig newslot abstract virtual 
            instance void  GetSyncSource([out] class MSR.LST.MDShow.IReferenceClock/* 0200001C */&  marshal( interface) pClock) runtime managed internalcall
    // SIG: 20 01 01 10 12 70
    {
    } // end of method IBaseFilter::GetSyncSource

    .method /*0600004F*/ public hidebysig newslot abstract virtual 
            instance void  EnumPins([out] class MSR.LST.MDShow.IEnumPins/* 0200001D */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 74
    {
    } // end of method IBaseFilter::EnumPins

    .method /*06000050*/ public hidebysig newslot abstract virtual 
            instance void  FindPin([in] string  marshal( lpwstr) Id,
                                   [out] class MSR.LST.MDShow.IPin/* 0200001E */&  marshal( interface) ppPin) runtime managed internalcall
    // SIG: 20 02 01 0E 10 12 78
    {
    } // end of method IBaseFilter::FindPin

    .method /*06000051*/ public hidebysig newslot abstract virtual 
            instance void  QueryFilterInfo([out] valuetype MSR.LST.MDShow._FilterInfo/* 02000021 */& pInfo) runtime managed internalcall
    // SIG: 20 01 01 10 11 80 84
    {
    } // end of method IBaseFilter::QueryFilterInfo

    .method /*06000052*/ public hidebysig newslot abstract virtual 
            instance void  JoinFilterGraph([in] class MSR.LST.MDShow.IFilterGraph/* 02000022 */  marshal( interface) pGraph,
                                           [in] string  marshal( lpwstr) pName) runtime managed internalcall
    // SIG: 20 02 01 12 80 88 0E
    {
    } // end of method IBaseFilter::JoinFilterGraph

    .method /*06000053*/ public hidebysig newslot abstract virtual 
            instance void  QueryVendorInfo([out] string&  marshal( lpwstr) pVendorInfo) runtime managed internalcall
    // SIG: 20 01 01 10 0E
    {
    } // end of method IBaseFilter::QueryVendorInfo

  } // end of class IBaseFilter

  .class /*0200001A*/ interface public abstract auto ansi import IMediaFilter
         implements MSR.LST.MDShow.IPersist/* 02000006 */
  {
    .custom /*0C000024:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 39 2D 30 41 44 34   // ..$56A86899-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C000025:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000054*/ public hidebysig newslot abstract virtual 
            instance void  GetClassID([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pClassID) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IMediaFilter::GetClassID

    .method /*06000055*/ public hidebysig newslot abstract virtual 
            instance void  Stop() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaFilter::Stop

    .method /*06000056*/ public hidebysig newslot abstract virtual 
            instance void  Pause() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaFilter::Pause

    .method /*06000057*/ public hidebysig newslot abstract virtual 
            instance void  Run(int64 tStart) runtime managed internalcall
    // SIG: 20 01 01 0A
    {
    } // end of method IMediaFilter::Run

    .method /*06000058*/ public hidebysig newslot abstract virtual 
            instance void  GetState([in] unsigned int32 dwMilliSecsTimeout,
                                    [out] valuetype MSR.LST.MDShow._FilterState/* 0200001B */& State) runtime managed internalcall
    // SIG: 20 02 01 09 10 11 6C
    {
    } // end of method IMediaFilter::GetState

    .method /*06000059*/ public hidebysig newslot abstract virtual 
            instance void  SetSyncSource([in] class MSR.LST.MDShow.IReferenceClock/* 0200001C */  marshal( interface) pClock) runtime managed internalcall
    // SIG: 20 01 01 12 70
    {
    } // end of method IMediaFilter::SetSyncSource

    .method /*0600005A*/ public hidebysig newslot abstract virtual 
            instance void  GetSyncSource([out] class MSR.LST.MDShow.IReferenceClock/* 0200001C */&  marshal( interface) pClock) runtime managed internalcall
    // SIG: 20 01 01 10 12 70
    {
    } // end of method IMediaFilter::GetSyncSource

  } // end of class IMediaFilter

  .class /*0200001B*/ public auto ansi sealed _FilterState
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400003A*/ public specialname rtspecialname int32 value__
    .field /*0400003B*/ public static literal valuetype MSR.LST.MDShow._FilterState/* 0200001B */ State_Stopped = int32(0x00000000)
    .field /*0400003C*/ public static literal valuetype MSR.LST.MDShow._FilterState/* 0200001B */ State_Paused = int32(0x00000001)
    .field /*0400003D*/ public static literal valuetype MSR.LST.MDShow._FilterState/* 0200001B */ State_Running = int32(0x00000002)
  } // end of class _FilterState

  .class /*0200001C*/ interface public abstract auto ansi import IReferenceClock
  {
    .custom /*0C000026:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 37 2D 30 41 44 34   // ..$56A86897-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C000027:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600005B*/ public hidebysig newslot abstract virtual 
            instance void  GetTime([out] int64& pTime) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IReferenceClock::GetTime

    .method /*0600005C*/ public hidebysig newslot abstract virtual 
            instance void  AdviseTime([in] int64 baseTime,
                                      [in] int64 streamTime,
                                      [in] unsigned int32 hEvent,
                                      [out] unsigned int32& pdwAdviseCookie) runtime managed internalcall
    // SIG: 20 04 01 0A 0A 09 10 09
    {
      .param [3]/*08000091*/ 
      .custom /*0C0000E1:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
      .param [4]/*08000092*/ 
      .custom /*0C0000E4:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IReferenceClock::AdviseTime

    .method /*0600005D*/ public hidebysig newslot abstract virtual 
            instance void  AdvisePeriodic([in] int64 startTime,
                                          [in] int64 periodTime,
                                          [in] unsigned int32 hSemaphore,
                                          [out] unsigned int32& pdwAdviseCookie) runtime managed internalcall
    // SIG: 20 04 01 0A 0A 09 10 09
    {
      .param [3]/*08000095*/ 
      .custom /*0C0000ED:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
      .param [4]/*08000096*/ 
      .custom /*0C0000F2:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IReferenceClock::AdvisePeriodic

    .method /*0600005E*/ public hidebysig newslot abstract virtual 
            instance void  Unadvise([in] unsigned int32 dwAdviseCookie) runtime managed internalcall
    // SIG: 20 01 01 09
    {
      .param [1]/*08000097*/ 
      .custom /*0C0000F6:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IReferenceClock::Unadvise

  } // end of class IReferenceClock

  .class /*0200001D*/ interface public abstract auto ansi import IEnumPins
  {
    .custom /*0C000028:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 32 2D 30 41 44 34   // ..$56A86892-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C000029:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600005F*/ public hidebysig newslot abstract virtual 
            instance void  Next([in] unsigned int32 cPins,
                                [out] class MSR.LST.MDShow.IPin/* 0200001E */&  marshal( interface) ppPins,
                                [out] unsigned int32& pcFetched) runtime managed internalcall
    // SIG: 20 03 01 09 10 12 78 10 09
    {
    } // end of method IEnumPins::Next

    .method /*06000060*/ public hidebysig newslot abstract virtual 
            instance void  Skip([in] unsigned int32 cPins) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IEnumPins::Skip

    .method /*06000061*/ public hidebysig newslot abstract virtual 
            instance void  Reset() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IEnumPins::Reset

    .method /*06000062*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IEnumPins/* 0200001D */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 74
    {
    } // end of method IEnumPins::Clone

  } // end of class IEnumPins

  .class /*0200001E*/ interface public abstract auto ansi import IPin
  {
    .custom /*0C00002A:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00002B:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 31 2D 30 41 44 34   // ..$56A86891-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*06000063*/ public hidebysig newslot abstract virtual 
            instance void  Connect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pReceivePin,
                                   [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 12 78 10 11 50
    {
    } // end of method IPin::Connect

    .method /*06000064*/ public hidebysig newslot abstract virtual 
            instance void  ReceiveConnection([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pConnector,
                                             [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 12 78 10 11 50
    {
    } // end of method IPin::ReceiveConnection

    .method /*06000065*/ public hidebysig newslot abstract virtual 
            instance void  Disconnect() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IPin::Disconnect

    .method /*06000066*/ public hidebysig newslot abstract virtual 
            instance void  ConnectedTo([out] class MSR.LST.MDShow.IPin/* 0200001E */&  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 01 01 10 12 78
    {
    } // end of method IPin::ConnectedTo

    .method /*06000067*/ public hidebysig newslot abstract virtual 
            instance void  ConnectionMediaType([out] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 01 01 10 11 50
    {
    } // end of method IPin::ConnectionMediaType

    .method /*06000068*/ public hidebysig newslot abstract virtual 
            instance void  QueryPinInfo([out] valuetype MSR.LST.MDShow._PinInfo/* 0200001F */& pInfo) runtime managed internalcall
    // SIG: 20 01 01 10 11 7C
    {
    } // end of method IPin::QueryPinInfo

    .method /*06000069*/ public hidebysig newslot abstract virtual 
            instance void  QueryDirection([out] valuetype MSR.LST.MDShow._PinDirection/* 02000015 */& pPinDir) runtime managed internalcall
    // SIG: 20 01 01 10 11 54
    {
    } // end of method IPin::QueryDirection

    .method /*0600006A*/ public hidebysig newslot abstract virtual 
            instance void  QueryId([out] string&  marshal( lpwstr) Id) runtime managed internalcall
    // SIG: 20 01 01 10 0E
    {
    } // end of method IPin::QueryId

    .method /*0600006B*/ public hidebysig newslot abstract virtual 
            instance void  QueryAccept([in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 01 01 10 11 50
    {
    } // end of method IPin::QueryAccept

    .method /*0600006C*/ public hidebysig newslot abstract virtual 
            instance void  EnumMediaTypes([out] class MSR.LST.MDShow.IEnumMediaTypes/* 02000020 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 80 80
    {
    } // end of method IPin::EnumMediaTypes

    .method /*0600006D*/ public hidebysig newslot abstract virtual 
            instance void  QueryInternalConnections([out] class MSR.LST.MDShow.IPin/* 0200001E */&  marshal( interface) apPin,
                                                    [in][out] unsigned int32& nPin) runtime managed internalcall
    // SIG: 20 02 01 10 12 78 10 09
    {
    } // end of method IPin::QueryInternalConnections

    .method /*0600006E*/ public hidebysig newslot abstract virtual 
            instance void  EndOfStream() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IPin::EndOfStream

    .method /*0600006F*/ public hidebysig newslot abstract virtual 
            instance void  BeginFlush() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IPin::BeginFlush

    .method /*06000070*/ public hidebysig newslot abstract virtual 
            instance void  EndFlush() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IPin::EndFlush

    .method /*06000071*/ public hidebysig newslot abstract virtual 
            instance void  NewSegment([in] int64 tStart,
                                      [in] int64 tStop,
                                      [in] float64 dRate) runtime managed internalcall
    // SIG: 20 03 01 0A 0A 0D
    {
    } // end of method IPin::NewSegment

  } // end of class IPin

  .class /*0200001F*/ public sequential ansi sealed beforefieldinit _PinInfo
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*0400003E*/ public  marshal( interface) class MSR.LST.MDShow.IBaseFilter/* 02000019 */ pFilter
    .field /*0400003F*/ public valuetype MSR.LST.MDShow._PinDirection/* 02000015 */ dir
    .field /*04000040*/ public  marshal( fixed array [128]) unsigned int16[] achName
  } // end of class _PinInfo

  .class /*02000020*/ interface public abstract auto ansi import IEnumMediaTypes
  {
    .custom /*0C00002C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 38 39 43 33 31 30 34 30 2D 38 34 36 42   // ..$89C31040-846B
                                                                                                                                                                     2D 31 31 43 45 2D 39 37 44 33 2D 30 30 41 41 30   // -11CE-97D3-00AA0
                                                                                                                                                                     30 35 35 35 39 35 41 00 00 )                      // 055595A..
    .custom /*0C00002D:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00002E:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*06000072*/ public hidebysig newslot abstract virtual 
            instance void  Next([in] unsigned int32 cMediaTypes,
                                [out] native int ppMediaTypes,
                                [out] unsigned int32& pcFetched) runtime managed internalcall
    // SIG: 20 03 01 09 18 10 09
    {
    } // end of method IEnumMediaTypes::Next

    .method /*06000073*/ public hidebysig newslot abstract virtual 
            instance void  Skip([in] unsigned int32 cMediaTypes) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IEnumMediaTypes::Skip

    .method /*06000074*/ public hidebysig newslot abstract virtual 
            instance void  Reset() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IEnumMediaTypes::Reset

    .method /*06000075*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IEnumMediaTypes/* 02000020 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 80 80
    {
    } // end of method IEnumMediaTypes::Clone

  } // end of class IEnumMediaTypes

  .class /*02000021*/ public sequential ansi sealed beforefieldinit _FilterInfo
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*04000041*/ public  marshal( fixed array [128]) unsigned int16[] achName
    .field /*04000042*/ public  marshal( interface) class MSR.LST.MDShow.IFilterGraph/* 02000022 */ pGraph
  } // end of class _FilterInfo

  .class /*02000022*/ interface public abstract auto ansi import IFilterGraph
  {
    .custom /*0C000030:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 46 2D 30 41 44 34   // ..$56A8689F-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C000031:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000076*/ public hidebysig newslot abstract virtual 
            instance void  AddFilter([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter,
                                     [in] string  marshal( lpwstr) pName) runtime managed internalcall
    // SIG: 20 02 01 12 64 0E
    {
    } // end of method IFilterGraph::AddFilter

    .method /*06000077*/ public hidebysig newslot abstract virtual 
            instance void  RemoveFilter([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter) runtime managed internalcall
    // SIG: 20 01 01 12 64
    {
    } // end of method IFilterGraph::RemoveFilter

    .method /*06000078*/ public hidebysig newslot abstract virtual 
            instance void  EnumFilters([out] class MSR.LST.MDShow.IEnumFilters/* 02000018 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 60
    {
    } // end of method IFilterGraph::EnumFilters

    .method /*06000079*/ public hidebysig newslot abstract virtual 
            instance void  FindFilterByName([in] string  marshal( lpwstr) pName,
                                            [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppFilter) runtime managed internalcall
    // SIG: 20 02 01 0E 10 12 64
    {
    } // end of method IFilterGraph::FindFilterByName

    .method /*0600007A*/ public hidebysig newslot abstract virtual 
            instance void  ConnectDirect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                         [in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinIn,
                                         [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 03 01 12 78 12 78 10 11 50
    {
    } // end of method IFilterGraph::ConnectDirect

    .method /*0600007B*/ public hidebysig newslot abstract virtual 
            instance void  Reconnect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IFilterGraph::Reconnect

    .method /*0600007C*/ public hidebysig newslot abstract virtual 
            instance void  Disconnect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IFilterGraph::Disconnect

    .method /*0600007D*/ public hidebysig newslot abstract virtual 
            instance void  SetDefaultSyncSource() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IFilterGraph::SetDefaultSyncSource

  } // end of class IFilterGraph

  .class /*02000023*/ interface public abstract auto ansi import IMediaSample
  {
    .custom /*0C000033:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C000034:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000035:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 41 2D 30 41 44 34   // ..$56A8689A-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*0600007E*/ public hidebysig newslot abstract virtual 
            instance void  GetPointer([out] native int ppBuffer) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IMediaSample::GetPointer

    .method /*0600007F*/ public hidebysig newslot abstract virtual 
            instance int32  GetSize() runtime managed preservesig internalcall
    // SIG: 20 00 08
    {
    } // end of method IMediaSample::GetSize

    .method /*06000080*/ public hidebysig newslot abstract virtual 
            instance void  GetTime([out] int64& pTimeStart,
                                   [out] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample::GetTime

    .method /*06000081*/ public hidebysig newslot abstract virtual 
            instance void  SetTime([in] int64& pTimeStart,
                                   [in] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample::SetTime

    .method /*06000082*/ public hidebysig newslot abstract virtual 
            instance void  IsSyncPoint() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaSample::IsSyncPoint

    .method /*06000083*/ public hidebysig newslot abstract virtual 
            instance void  SetSyncPoint(int32 bIsSyncPoint) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample::SetSyncPoint

    .method /*06000084*/ public hidebysig newslot abstract virtual 
            instance void  IsPreroll() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaSample::IsPreroll

    .method /*06000085*/ public hidebysig newslot abstract virtual 
            instance void  SetPreroll(int32 bIsPreroll) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample::SetPreroll

    .method /*06000086*/ public hidebysig newslot abstract virtual 
            instance int32  GetActualDataLength() runtime managed preservesig internalcall
    // SIG: 20 00 08
    {
    } // end of method IMediaSample::GetActualDataLength

    .method /*06000087*/ public hidebysig newslot abstract virtual 
            instance void  SetActualDataLength(int32 __MIDL_0010) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample::SetActualDataLength

    .method /*06000088*/ public hidebysig newslot abstract virtual 
            instance void  GetMediaType(native int ppMediaType) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IMediaSample::GetMediaType

    .method /*06000089*/ public hidebysig newslot abstract virtual 
            instance void  SetMediaType(valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pMediaType) runtime managed internalcall
    // SIG: 20 01 01 10 11 50
    {
    } // end of method IMediaSample::SetMediaType

    .method /*0600008A*/ public hidebysig newslot abstract virtual 
            instance void  IsDiscontinuity() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaSample::IsDiscontinuity

    .method /*0600008B*/ public hidebysig newslot abstract virtual 
            instance void  SetDiscontinuity(int32 bDiscontinuity) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample::SetDiscontinuity

    .method /*0600008C*/ public hidebysig newslot abstract virtual 
            instance void  GetMediaTime([out] int64& pTimeStart,
                                        [out] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample::GetMediaTime

    .method /*0600008D*/ public hidebysig newslot abstract virtual 
            instance void  SetMediaTime([in] int64& pTimeStart,
                                        [in] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample::SetMediaTime

  } // end of class IMediaSample

  .class /*02000024*/ interface public abstract auto ansi import IMemAllocator
  {
    .custom /*0C000036:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000037:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 43 2D 30 41 44 34   // ..$56A8689C-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*0600008E*/ public hidebysig newslot abstract virtual 
            instance void  SetProperties([in] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pRequest,
                                         [out] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pActual) runtime managed internalcall
    // SIG: 20 02 01 10 11 58 10 11 58
    {
    } // end of method IMemAllocator::SetProperties

    .method /*0600008F*/ public hidebysig newslot abstract virtual 
            instance void  GetProperties([out] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pProps) runtime managed internalcall
    // SIG: 20 01 01 10 11 58
    {
    } // end of method IMemAllocator::GetProperties

    .method /*06000090*/ public hidebysig newslot abstract virtual 
            instance void  Commit() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMemAllocator::Commit

    .method /*06000091*/ public hidebysig newslot abstract virtual 
            instance void  Decommit() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMemAllocator::Decommit

    .method /*06000092*/ public hidebysig newslot abstract virtual 
            instance void  GetBuffer([out] class MSR.LST.MDShow.IMediaSample/* 02000023 */&  marshal( interface) ppBuffer,
                                     [in] int64& pStartTime,
                                     [in] int64& pEndTime,
                                     [in] unsigned int32 dwFlags) runtime managed internalcall
    // SIG: 20 04 01 10 12 80 8C 10 0A 10 0A 09
    {
    } // end of method IMemAllocator::GetBuffer

    .method /*06000093*/ public hidebysig newslot abstract virtual 
            instance void  ReleaseBuffer([in] class MSR.LST.MDShow.IMediaSample/* 02000023 */  marshal( interface) pBuffer) runtime managed internalcall
    // SIG: 20 01 01 12 80 8C
    {
    } // end of method IMemAllocator::ReleaseBuffer

  } // end of class IMemAllocator

  .class /*02000025*/ interface public abstract auto ansi import IMemAllocatorCallbackTemp
         implements MSR.LST.MDShow.IMemAllocator/* 02000024 */
  {
    .custom /*0C000039:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00003A:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 37 39 41 30 43 46 30 2D 43 31 44 45   // ..$379A0CF0-C1DE
                                                                                                                                                                     2D 31 31 44 32 2D 41 42 46 35 2D 30 30 41 30 43   // -11D2-ABF5-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .method /*06000094*/ public hidebysig newslot abstract virtual 
            instance void  SetProperties([in] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pRequest,
                                         [out] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pActual) runtime managed internalcall
    // SIG: 20 02 01 10 11 58 10 11 58
    {
    } // end of method IMemAllocatorCallbackTemp::SetProperties

    .method /*06000095*/ public hidebysig newslot abstract virtual 
            instance void  GetProperties([out] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pProps) runtime managed internalcall
    // SIG: 20 01 01 10 11 58
    {
    } // end of method IMemAllocatorCallbackTemp::GetProperties

    .method /*06000096*/ public hidebysig newslot abstract virtual 
            instance void  Commit() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMemAllocatorCallbackTemp::Commit

    .method /*06000097*/ public hidebysig newslot abstract virtual 
            instance void  Decommit() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMemAllocatorCallbackTemp::Decommit

    .method /*06000098*/ public hidebysig newslot abstract virtual 
            instance void  GetBuffer([out] class MSR.LST.MDShow.IMediaSample/* 02000023 */&  marshal( interface) ppBuffer,
                                     [in] int64& pStartTime,
                                     [in] int64& pEndTime,
                                     [in] unsigned int32 dwFlags) runtime managed internalcall
    // SIG: 20 04 01 10 12 80 8C 10 0A 10 0A 09
    {
    } // end of method IMemAllocatorCallbackTemp::GetBuffer

    .method /*06000099*/ public hidebysig newslot abstract virtual 
            instance void  ReleaseBuffer([in] class MSR.LST.MDShow.IMediaSample/* 02000023 */  marshal( interface) pBuffer) runtime managed internalcall
    // SIG: 20 01 01 12 80 8C
    {
    } // end of method IMemAllocatorCallbackTemp::ReleaseBuffer

    .method /*0600009A*/ public hidebysig newslot abstract virtual 
            instance void  SetNotify([in] class MSR.LST.MDShow.IMemAllocatorNotifyCallbackTemp/* 02000026 */  marshal( interface) pNotify) runtime managed internalcall
    // SIG: 20 01 01 12 80 98
    {
    } // end of method IMemAllocatorCallbackTemp::SetNotify

    .method /*0600009B*/ public hidebysig newslot abstract virtual 
            instance void  GetFreeCount([out] int32& plBuffersFree) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IMemAllocatorCallbackTemp::GetFreeCount

  } // end of class IMemAllocatorCallbackTemp

  .class /*02000026*/ interface public abstract auto ansi import IMemAllocatorNotifyCallbackTemp
  {
    .custom /*0C00003B:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00003C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 32 39 38 30 42 33 30 2D 43 31 44 45   // ..$92980B30-C1DE
                                                                                                                                                                     2D 31 31 44 32 2D 41 42 46 35 2D 30 30 41 30 43   // -11D2-ABF5-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .method /*0600009C*/ public hidebysig newslot abstract virtual 
            instance void  NotifyRelease() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMemAllocatorNotifyCallbackTemp::NotifyRelease

  } // end of class IMemAllocatorNotifyCallbackTemp

  .class /*02000027*/ interface public abstract auto ansi import IMemInputPin
  {
    .custom /*0C00003E:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 39 44 2D 30 41 44 34   // ..$56A8689D-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C00003F:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600009D*/ public hidebysig newslot abstract virtual 
            instance void  GetAllocator([out] class MSR.LST.MDShow.IMemAllocator/* 02000024 */&  marshal( interface) ppAllocator) runtime managed internalcall
    // SIG: 20 01 01 10 12 80 90
    {
    } // end of method IMemInputPin::GetAllocator

    .method /*0600009E*/ public hidebysig newslot abstract virtual 
            instance void  NotifyAllocator([in] class MSR.LST.MDShow.IMemAllocator/* 02000024 */  marshal( interface) pAllocator,
                                           [in] int32 bReadOnly) runtime managed internalcall
    // SIG: 20 02 01 12 80 90 08
    {
    } // end of method IMemInputPin::NotifyAllocator

    .method /*0600009F*/ public hidebysig newslot abstract virtual 
            instance void  GetAllocatorRequirements([out] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pProps) runtime managed internalcall
    // SIG: 20 01 01 10 11 58
    {
    } // end of method IMemInputPin::GetAllocatorRequirements

    .method /*060000A0*/ public hidebysig newslot abstract virtual 
            instance void  Receive([in] class MSR.LST.MDShow.IMediaSample/* 02000023 */  marshal( interface) pSample) runtime managed internalcall
    // SIG: 20 01 01 12 80 8C
    {
    } // end of method IMemInputPin::Receive

    .method /*060000A1*/ public hidebysig newslot abstract virtual 
            instance void  ReceiveMultiple([in] class MSR.LST.MDShow.IMediaSample/* 02000023 */&  marshal( interface) pSamples,
                                           [in] int32 nSamples,
                                           [out] int32& nSamplesProcessed) runtime managed internalcall
    // SIG: 20 03 01 10 12 80 8C 08 10 08
    {
    } // end of method IMemInputPin::ReceiveMultiple

    .method /*060000A2*/ public hidebysig newslot abstract virtual 
            instance void  ReceiveCanBlock() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMemInputPin::ReceiveCanBlock

  } // end of class IMemInputPin

  .class /*02000028*/ interface public abstract auto ansi import IReferenceClock2
         implements MSR.LST.MDShow.IReferenceClock/* 0200001C */
  {
    .custom /*0C000040:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000041:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 36 42 37 33 38 38 35 2D 43 32 43 38   // ..$36B73885-C2C8
                                                                                                                                                                     2D 31 31 43 46 2D 38 42 34 36 2D 30 30 38 30 35   // -11CF-8B46-00805
                                                                                                                                                                     46 36 43 45 46 36 30 00 00 )                      // F6CEF60..
    .method /*060000A3*/ public hidebysig newslot abstract virtual 
            instance void  GetTime([out] int64& pTime) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IReferenceClock2::GetTime

    .method /*060000A4*/ public hidebysig newslot abstract virtual 
            instance void  AdviseTime([in] int64 baseTime,
                                      [in] int64 streamTime,
                                      [in] unsigned int32 hEvent,
                                      [out] unsigned int32& pdwAdviseCookie) runtime managed internalcall
    // SIG: 20 04 01 0A 0A 09 10 09
    {
      .param [3]/*080000E9*/ 
      .custom /*0C000132:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
      .param [4]/*080000EA*/ 
      .custom /*0C000133:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IReferenceClock2::AdviseTime

    .method /*060000A5*/ public hidebysig newslot abstract virtual 
            instance void  AdvisePeriodic([in] int64 startTime,
                                          [in] int64 periodTime,
                                          [in] unsigned int32 hSemaphore,
                                          [out] unsigned int32& pdwAdviseCookie) runtime managed internalcall
    // SIG: 20 04 01 0A 0A 09 10 09
    {
      .param [3]/*080000ED*/ 
      .custom /*0C000134:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
      .param [4]/*080000EE*/ 
      .custom /*0C000135:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IReferenceClock2::AdvisePeriodic

    .method /*060000A6*/ public hidebysig newslot abstract virtual 
            instance void  Unadvise([in] unsigned int32 dwAdviseCookie) runtime managed internalcall
    // SIG: 20 01 01 09
    {
      .param [1]/*080000EF*/ 
      .custom /*0C000136:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IReferenceClock2::Unadvise

  } // end of class IReferenceClock2

  .class /*02000029*/ public auto ansi sealed tagAM_SAMPLE_PROPERTY_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000043*/ public specialname rtspecialname int32 value__
    .field /*04000044*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_SPLICEPOINT = int32(0x00000001)
    .field /*04000045*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_PREROLL = int32(0x00000002)
    .field /*04000046*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_DATADISCONTINUITY = int32(0x00000004)
    .field /*04000047*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_TYPECHANGED = int32(0x00000008)
    .field /*04000048*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_TIMEVALID = int32(0x00000010)
    .field /*04000049*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_TIMEDISCONTINUITY = int32(0x00000040)
    .field /*0400004A*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_FLUSH_ON_PAUSE = int32(0x00000080)
    .field /*0400004B*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_STOPVALID = int32(0x00000100)
    .field /*0400004C*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_SAMPLE_ENDOFSTREAM = int32(0x00000200)
    .field /*0400004D*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_STREAM_MEDIA = int32(0x00000000)
    .field /*0400004E*/ public static literal valuetype MSR.LST.MDShow.tagAM_SAMPLE_PROPERTY_FLAGS/* 02000029 */ AM_STREAM_CONTROL = int32(0x00000001)
  } // end of class tagAM_SAMPLE_PROPERTY_FLAGS

  .class /*0200002A*/ public sequential ansi sealed beforefieldinit tagAM_SAMPLE2_PROPERTIES
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .custom /*0C000042:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400004F*/ public unsigned int32 cbData
    .field /*04000050*/ public unsigned int32 dwTypeSpecificFlags
    .field /*04000051*/ public unsigned int32 dwSampleFlags
    .field /*04000052*/ public int32 lActual
    .field /*04000053*/ public int64 tStart
    .field /*04000054*/ public int64 tStop
    .field /*04000055*/ public unsigned int32 dwStreamId
    .field /*04000056*/ public native int pMediaType
    .custom /*0C000071:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000057*/ public native int pbBuffer
    .custom /*0C000074:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000058*/ public int32 cbBuffer
  } // end of class tagAM_SAMPLE2_PROPERTIES

  .class /*0200002B*/ interface public abstract auto ansi import IMediaSample2
         implements MSR.LST.MDShow.IMediaSample/* 02000023 */
  {
    .custom /*0C000043:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000044:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 36 42 37 33 38 38 34 2D 43 32 43 38   // ..$36B73884-C2C8
                                                                                                                                                                     2D 31 31 43 46 2D 38 42 34 36 2D 30 30 38 30 35   // -11CF-8B46-00805
                                                                                                                                                                     46 36 43 45 46 36 30 00 00 )                      // F6CEF60..
    .method /*060000A7*/ public hidebysig newslot abstract virtual 
            instance void  GetPointer([out] native int ppBuffer) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IMediaSample2::GetPointer

    .method /*060000A8*/ public hidebysig newslot abstract virtual 
            instance int32  GetSize() runtime managed preservesig internalcall
    // SIG: 20 00 08
    {
    } // end of method IMediaSample2::GetSize

    .method /*060000A9*/ public hidebysig newslot abstract virtual 
            instance void  GetTime([out] int64& pTimeStart,
                                   [out] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample2::GetTime

    .method /*060000AA*/ public hidebysig newslot abstract virtual 
            instance void  SetTime([in] int64& pTimeStart,
                                   [in] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample2::SetTime

    .method /*060000AB*/ public hidebysig newslot abstract virtual 
            instance void  IsSyncPoint() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaSample2::IsSyncPoint

    .method /*060000AC*/ public hidebysig newslot abstract virtual 
            instance void  SetSyncPoint(int32 bIsSyncPoint) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample2::SetSyncPoint

    .method /*060000AD*/ public hidebysig newslot abstract virtual 
            instance void  IsPreroll() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaSample2::IsPreroll

    .method /*060000AE*/ public hidebysig newslot abstract virtual 
            instance void  SetPreroll(int32 bIsPreroll) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample2::SetPreroll

    .method /*060000AF*/ public hidebysig newslot abstract virtual 
            instance int32  GetActualDataLength() runtime managed preservesig internalcall
    // SIG: 20 00 08
    {
    } // end of method IMediaSample2::GetActualDataLength

    .method /*060000B0*/ public hidebysig newslot abstract virtual 
            instance void  SetActualDataLength(int32 __MIDL_0010) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample2::SetActualDataLength

    .method /*060000B1*/ public hidebysig newslot abstract virtual 
            instance void  GetMediaType(native int ppMediaType) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IMediaSample2::GetMediaType

    .method /*060000B2*/ public hidebysig newslot abstract virtual 
            instance void  SetMediaType(valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pMediaType) runtime managed internalcall
    // SIG: 20 01 01 10 11 50
    {
    } // end of method IMediaSample2::SetMediaType

    .method /*060000B3*/ public hidebysig newslot abstract virtual 
            instance void  IsDiscontinuity() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IMediaSample2::IsDiscontinuity

    .method /*060000B4*/ public hidebysig newslot abstract virtual 
            instance void  SetDiscontinuity(int32 bDiscontinuity) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IMediaSample2::SetDiscontinuity

    .method /*060000B5*/ public hidebysig newslot abstract virtual 
            instance void  GetMediaTime([out] int64& pTimeStart,
                                        [out] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample2::GetMediaTime

    .method /*060000B6*/ public hidebysig newslot abstract virtual 
            instance void  SetMediaTime([in] int64& pTimeStart,
                                        [in] int64& pTimeEnd) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSample2::SetMediaTime

    .method /*060000B7*/ public hidebysig newslot abstract virtual 
            instance void  GetProperties([in] unsigned int32 cbProperties,
                                         [out] unsigned int8& pbProperties) runtime managed internalcall
    // SIG: 20 02 01 09 10 05
    {
    } // end of method IMediaSample2::GetProperties

    .method /*060000B8*/ public hidebysig newslot abstract virtual 
            instance void  SetProperties([in] unsigned int32 cbProperties,
                                         [in] unsigned int8& pbProperties) runtime managed internalcall
    // SIG: 20 02 01 09 10 05
    {
    } // end of method IMediaSample2::SetProperties

  } // end of class IMediaSample2

  .class /*0200002C*/ public auto ansi sealed AM_SEEKING_SeekingFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000059*/ public specialname rtspecialname int32 value__
    .field /*0400005A*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_NoPositioning = int32(0x00000000)
    .field /*0400005B*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_AbsolutePositioning = int32(0x00000001)
    .field /*0400005C*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_RelativePositioning = int32(0x00000002)
    .field /*0400005D*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_IncrementalPositioning = int32(0x00000003)
    .field /*0400005E*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_PositioningBitsMask = int32(0x00000003)
    .field /*0400005F*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_SeekToKeyFrame = int32(0x00000004)
    .field /*04000060*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_ReturnTime = int32(0x00000008)
    .field /*04000061*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_Segment = int32(0x00000010)
    .field /*04000062*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingFlags/* 0200002C */ AM_SEEKING_NoFlush = int32(0x00000020)
  } // end of class AM_SEEKING_SeekingFlags

  .class /*0200002D*/ public auto ansi sealed AM_SEEKING_SeekingCapabilities
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000063*/ public specialname rtspecialname int32 value__
    .field /*04000064*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanSeekAbsolute = int32(0x00000001)
    .field /*04000065*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanSeekForwards = int32(0x00000002)
    .field /*04000066*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanSeekBackwards = int32(0x00000004)
    .field /*04000067*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanGetCurrentPos = int32(0x00000008)
    .field /*04000068*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanGetStopPos = int32(0x00000010)
    .field /*04000069*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanGetDuration = int32(0x00000020)
    .field /*0400006A*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanPlayBackwards = int32(0x00000040)
    .field /*0400006B*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_CanDoSegments = int32(0x00000080)
    .field /*0400006C*/ public static literal valuetype MSR.LST.MDShow.AM_SEEKING_SeekingCapabilities/* 0200002D */ AM_SEEKING_Source = int32(0x00000100)
  } // end of class AM_SEEKING_SeekingCapabilities

  .class /*0200002E*/ interface public abstract auto ansi import IMediaSeeking
  {
    .custom /*0C000045:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 36 42 37 33 38 38 30 2D 43 32 43 38   // ..$36B73880-C2C8
                                                                                                                                                                     2D 31 31 43 46 2D 38 42 34 36 2D 30 30 38 30 35   // -11CF-8B46-00805
                                                                                                                                                                     46 36 43 45 46 36 30 00 00 )                      // F6CEF60..
    .custom /*0C000046:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060000B9*/ public hidebysig newslot abstract virtual 
            instance void  GetCapabilities([out] unsigned int32& pCapabilities) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IMediaSeeking::GetCapabilities

    .method /*060000BA*/ public hidebysig newslot abstract virtual 
            instance void  CheckCapabilities([in][out] unsigned int32& pCapabilities) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IMediaSeeking::CheckCapabilities

    .method /*060000BB*/ public hidebysig newslot abstract virtual 
            instance void  IsFormatSupported([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pFormat) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IMediaSeeking::IsFormatSupported

    .method /*060000BC*/ public hidebysig newslot abstract virtual 
            instance void  QueryPreferredFormat([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pFormat) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IMediaSeeking::QueryPreferredFormat

    .method /*060000BD*/ public hidebysig newslot abstract virtual 
            instance void  GetTimeFormat([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pFormat) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IMediaSeeking::GetTimeFormat

    .method /*060000BE*/ public hidebysig newslot abstract virtual 
            instance void  IsUsingTimeFormat([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pFormat) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IMediaSeeking::IsUsingTimeFormat

    .method /*060000BF*/ public hidebysig newslot abstract virtual 
            instance void  SetTimeFormat([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pFormat) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IMediaSeeking::SetTimeFormat

    .method /*060000C0*/ public hidebysig newslot abstract virtual 
            instance void  GetDuration([out] int64& pDuration) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IMediaSeeking::GetDuration

    .method /*060000C1*/ public hidebysig newslot abstract virtual 
            instance void  GetStopPosition([out] int64& pStop) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IMediaSeeking::GetStopPosition

    .method /*060000C2*/ public hidebysig newslot abstract virtual 
            instance void  GetCurrentPosition([out] int64& pCurrent) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IMediaSeeking::GetCurrentPosition

    .method /*060000C3*/ public hidebysig newslot abstract virtual 
            instance void  ConvertTimeFormat([out] int64& pTarget,
                                             [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pTargetFormat,
                                             [in] int64 Source,
                                             [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pSourceFormat) runtime managed internalcall
    // SIG: 20 04 01 10 0A 10 11 11 0A 10 11 11
    {
    } // end of method IMediaSeeking::ConvertTimeFormat

    .method /*060000C4*/ public hidebysig newslot abstract virtual 
            instance void  SetPositions([in][out] int64& pCurrent,
                                        [in] unsigned int32 dwCurrentFlags,
                                        [in][out] int64& pStop,
                                        [in] unsigned int32 dwStopFlags) runtime managed internalcall
    // SIG: 20 04 01 10 0A 09 10 0A 09
    {
    } // end of method IMediaSeeking::SetPositions

    .method /*060000C5*/ public hidebysig newslot abstract virtual 
            instance void  GetPositions([out] int64& pCurrent,
                                        [out] int64& pStop) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSeeking::GetPositions

    .method /*060000C6*/ public hidebysig newslot abstract virtual 
            instance void  GetAvailable([out] int64& pEarliest,
                                        [out] int64& pLatest) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IMediaSeeking::GetAvailable

    .method /*060000C7*/ public hidebysig newslot abstract virtual 
            instance void  SetRate([in] float64 dRate) runtime managed internalcall
    // SIG: 20 01 01 0D
    {
    } // end of method IMediaSeeking::SetRate

    .method /*060000C8*/ public hidebysig newslot abstract virtual 
            instance void  GetRate([out] float64& pdRate) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IMediaSeeking::GetRate

    .method /*060000C9*/ public hidebysig newslot abstract virtual 
            instance void  GetPreroll([out] int64& pllPreroll) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IMediaSeeking::GetPreroll

  } // end of class IMediaSeeking

  .class /*0200002F*/ interface public abstract auto ansi import IEnumRegFilters
  {
    .custom /*0C000047:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C000048:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000049:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 34 2D 30 41 44 34   // ..$56A868A4-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*060000CA*/ public hidebysig newslot abstract virtual 
            instance void  Next([in] unsigned int32 cFilters,
                                [out] native int apRegFilter,
                                [out] unsigned int32& pcFetched) runtime managed internalcall
    // SIG: 20 03 01 09 18 10 09
    {
      .param [2]/*0800011D*/ 
      .custom /*0C000137:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 52 45 47 46 49 4C 54 45 52 00 00 )          // w.REGFILTER..
    } // end of method IEnumRegFilters::Next

    .method /*060000CB*/ public hidebysig newslot abstract virtual 
            instance void  Skip([in] unsigned int32 cFilters) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IEnumRegFilters::Skip

    .method /*060000CC*/ public hidebysig newslot abstract virtual 
            instance void  Reset() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IEnumRegFilters::Reset

    .method /*060000CD*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IEnumRegFilters/* 0200002F */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 80 BC
    {
    } // end of method IEnumRegFilters::Clone

  } // end of class IEnumRegFilters

  .class /*02000030*/ public sequential ansi sealed beforefieldinit REGFILTER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*0400006D*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ 'clsid'
    .field /*0400006E*/ public  marshal( lpwstr) string Name
  } // end of class REGFILTER

  .class /*02000031*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0133_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*0400006F*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ 'clsid'
    .field /*04000070*/ public  marshal( lpwstr) string Name
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0133_0001

  .class /*02000032*/ interface public abstract auto ansi import IFileSourceFilter
  {
    .custom /*0C00004B:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00004C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 36 2D 30 41 44 34   // ..$56A868A6-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*060000CE*/ public hidebysig newslot abstract virtual 
            instance void  Load([in] string  marshal( lpwstr) pszFileName,
                                [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 0E 10 11 50
    {
    } // end of method IFileSourceFilter::Load

    .method /*060000CF*/ public hidebysig newslot abstract virtual 
            instance void  GetCurFile([out] string&  marshal( lpwstr) ppszFileName,
                                      [out] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 10 0E 10 11 50
    {
    } // end of method IFileSourceFilter::GetCurFile

  } // end of class IFileSourceFilter

  .class /*02000033*/ interface public abstract auto ansi import IFileSinkFilter
  {
    .custom /*0C00004D:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00004E:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 41 32 31 30 34 38 33 30 2D 37 43 37 30   // ..$A2104830-7C70
                                                                                                                                                                     2D 31 31 43 46 2D 38 42 43 45 2D 30 30 41 41 30   // -11CF-8BCE-00AA0
                                                                                                                                                                     30 41 33 46 31 41 36 00 00 )                      // 0A3F1A6..
    .method /*060000D0*/ public hidebysig newslot abstract virtual 
            instance void  SetFileName([in] string  marshal( lpwstr) pszFileName,
                                       [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 0E 10 11 50
    {
    } // end of method IFileSinkFilter::SetFileName

    .method /*060000D1*/ public hidebysig newslot abstract virtual 
            instance void  GetCurFile([out] string&  marshal( lpwstr) ppszFileName,
                                      [out] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 10 0E 10 11 50
    {
    } // end of method IFileSinkFilter::GetCurFile

  } // end of class IFileSinkFilter

  .class /*02000034*/ interface public abstract auto ansi import IFileSinkFilter2
         implements MSR.LST.MDShow.IFileSinkFilter/* 02000033 */
  {
    .custom /*0C00004F:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000050:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 30 38 35 35 42 39 30 2D 43 45 31 42   // ..$00855B90-CE1B
                                                                                                                                                                     2D 31 31 44 30 2D 42 44 34 46 2D 30 30 41 30 43   // -11D0-BD4F-00A0C
                                                                                                                                                                     39 31 31 43 45 38 36 00 00 )                      // 911CE86..
    .method /*060000D2*/ public hidebysig newslot abstract virtual 
            instance void  SetFileName([in] string  marshal( lpwstr) pszFileName,
                                       [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 0E 10 11 50
    {
    } // end of method IFileSinkFilter2::SetFileName

    .method /*060000D3*/ public hidebysig newslot abstract virtual 
            instance void  GetCurFile([out] string&  marshal( lpwstr) ppszFileName,
                                      [out] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 10 0E 10 11 50
    {
    } // end of method IFileSinkFilter2::GetCurFile

    .method /*060000D4*/ public hidebysig newslot abstract virtual 
            instance void  SetMode([in] unsigned int32 dwFlags) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IFileSinkFilter2::SetMode

    .method /*060000D5*/ public hidebysig newslot abstract virtual 
            instance void  GetMode([out] unsigned int32& pdwFlags) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IFileSinkFilter2::GetMode

  } // end of class IFileSinkFilter2

  .class /*02000035*/ interface public abstract auto ansi import IGraphBuilder
         implements MSR.LST.MDShow.IFilterGraph/* 02000022 */
  {
    .custom /*0C000051:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 39 2D 30 41 44 34   // ..$56A868A9-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C000052:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060000D6*/ public hidebysig newslot abstract virtual 
            instance void  AddFilter([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter,
                                     [in] string  marshal( lpwstr) pName) runtime managed internalcall
    // SIG: 20 02 01 12 64 0E
    {
    } // end of method IGraphBuilder::AddFilter

    .method /*060000D7*/ public hidebysig newslot abstract virtual 
            instance void  RemoveFilter([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter) runtime managed internalcall
    // SIG: 20 01 01 12 64
    {
    } // end of method IGraphBuilder::RemoveFilter

    .method /*060000D8*/ public hidebysig newslot abstract virtual 
            instance void  EnumFilters([out] class MSR.LST.MDShow.IEnumFilters/* 02000018 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 60
    {
    } // end of method IGraphBuilder::EnumFilters

    .method /*060000D9*/ public hidebysig newslot abstract virtual 
            instance void  FindFilterByName([in] string  marshal( lpwstr) pName,
                                            [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppFilter) runtime managed internalcall
    // SIG: 20 02 01 0E 10 12 64
    {
    } // end of method IGraphBuilder::FindFilterByName

    .method /*060000DA*/ public hidebysig newslot abstract virtual 
            instance void  ConnectDirect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                         [in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinIn,
                                         [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 03 01 12 78 12 78 10 11 50
    {
    } // end of method IGraphBuilder::ConnectDirect

    .method /*060000DB*/ public hidebysig newslot abstract virtual 
            instance void  Reconnect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IGraphBuilder::Reconnect

    .method /*060000DC*/ public hidebysig newslot abstract virtual 
            instance void  Disconnect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IGraphBuilder::Disconnect

    .method /*060000DD*/ public hidebysig newslot abstract virtual 
            instance void  SetDefaultSyncSource() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IGraphBuilder::SetDefaultSyncSource

    .method /*060000DE*/ public hidebysig newslot abstract virtual 
            instance void  Connect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                   [in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinIn) runtime managed internalcall
    // SIG: 20 02 01 12 78 12 78
    {
    } // end of method IGraphBuilder::Connect

    .method /*060000DF*/ public hidebysig newslot abstract virtual 
            instance void  Render([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IGraphBuilder::Render

    .method /*060000E0*/ public hidebysig newslot abstract virtual 
            instance void  RenderFile([in] string  marshal( lpwstr) lpcwstrFile,
                                      [in] string  marshal( lpwstr) lpcwstrPlayList) runtime managed internalcall
    // SIG: 20 02 01 0E 0E
    {
    } // end of method IGraphBuilder::RenderFile

    .method /*060000E1*/ public hidebysig newslot abstract virtual 
            instance void  AddSourceFilter([in] string  marshal( lpwstr) lpcwstrFileName,
                                           [in] string  marshal( lpwstr) lpcwstrFilterName,
                                           [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppFilter) runtime managed internalcall
    // SIG: 20 03 01 0E 0E 10 12 64
    {
    } // end of method IGraphBuilder::AddSourceFilter

    .method /*060000E2*/ public hidebysig newslot abstract virtual 
            instance void  SetLogFile([in] unsigned int32 hFile) runtime managed internalcall
    // SIG: 20 01 01 09
    {
      .param [1]/*08000142*/ 
      .custom /*0C000138:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IGraphBuilder::SetLogFile

    .method /*060000E3*/ public hidebysig newslot abstract virtual 
            instance void  Abort() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IGraphBuilder::Abort

    .method /*060000E4*/ public hidebysig newslot abstract virtual 
            instance void  ShouldOperationContinue() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IGraphBuilder::ShouldOperationContinue

  } // end of class IGraphBuilder

  .class /*02000036*/ interface public abstract auto ansi import ICaptureGraphBuilder
  {
    .custom /*0C000053:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 42 46 38 37 42 36 45 30 2D 38 43 32 37   // ..$BF87B6E0-8C27
                                                                                                                                                                     2D 31 31 44 30 2D 42 33 46 30 2D 30 30 41 41 30   // -11D0-B3F0-00AA0
                                                                                                                                                                     30 33 37 36 31 43 35 00 00 )                      // 03761C5..
    .custom /*0C000054:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060000E5*/ public hidebysig newslot abstract virtual 
            instance void  SetFiltergraph([in] class MSR.LST.MDShow.IGraphBuilder/* 02000035 */  marshal( interface) pfg) runtime managed internalcall
    // SIG: 20 01 01 12 80 D4
    {
    } // end of method ICaptureGraphBuilder::SetFiltergraph

    .method /*060000E6*/ public hidebysig newslot abstract virtual 
            instance void  GetFiltergraph([out] class MSR.LST.MDShow.IGraphBuilder/* 02000035 */&  marshal( interface) ppfg) runtime managed internalcall
    // SIG: 20 01 01 10 12 80 D4
    {
    } // end of method ICaptureGraphBuilder::GetFiltergraph

    .method /*060000E7*/ public hidebysig newslot abstract virtual 
            instance void  SetOutputFileName([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pType,
                                             [in] string  marshal( lpwstr) lpstrFile,
                                             [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppf,
                                             [out] class MSR.LST.MDShow.IFileSinkFilter/* 02000033 */&  marshal( interface) ppSink) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 0E 10 12 64 10 12 80 CC
    {
    } // end of method ICaptureGraphBuilder::SetOutputFileName

    .method /*060000E8*/ public hidebysig newslot abstract virtual 
            instance void  RemoteFindInterface([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pCategory,
                                               [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pf,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& riid,
                                               [out] object&  marshal( iunknown) ppint) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 12 64 10 11 11 10 1C
    {
    } // end of method ICaptureGraphBuilder::RemoteFindInterface

    .method /*060000E9*/ public hidebysig newslot abstract virtual 
            instance void  RenderStream([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pCategory,
                                        [in] object  marshal( iunknown) pSource,
                                        [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pfCompressor,
                                        [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pfRenderer) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 1C 12 64 12 64
    {
    } // end of method ICaptureGraphBuilder::RenderStream

    .method /*060000EA*/ public hidebysig newslot abstract virtual 
            instance void  ControlStream([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pCategory,
                                         [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter,
                                         [in] int64& pstart,
                                         [in] int64& pStop,
                                         [in] unsigned int16 wStartCookie,
                                         [in] unsigned int16 wStopCookie) runtime managed internalcall
    // SIG: 20 06 01 10 11 11 12 64 10 0A 10 0A 07 07
    {
    } // end of method ICaptureGraphBuilder::ControlStream

    .method /*060000EB*/ public hidebysig newslot abstract virtual 
            instance void  AllocCapFile([in] string  marshal( lpwstr) 'lpstr',
                                        [in] unsigned int64 dwlSize) runtime managed internalcall
    // SIG: 20 02 01 0E 0B
    {
    } // end of method ICaptureGraphBuilder::AllocCapFile

    .method /*060000EC*/ public hidebysig newslot abstract virtual 
            instance void  CopyCaptureFile([in] string  marshal( lpwstr) lpwstrOld,
                                           [in] string  marshal( lpwstr) lpwstrNew,
                                           [in] int32 fAllowEscAbort,
                                           [in] class MSR.LST.MDShow.IAMCopyCaptureFileProgress/* 02000037 */  marshal( interface) pCallback) runtime managed internalcall
    // SIG: 20 04 01 0E 0E 08 12 80 DC
    {
    } // end of method ICaptureGraphBuilder::CopyCaptureFile

  } // end of class ICaptureGraphBuilder

  .class /*02000037*/ interface public abstract auto ansi import IAMCopyCaptureFileProgress
  {
    .custom /*0C000055:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000056:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 37 30 44 31 44 32 30 2D 41 30 36 38   // ..$670D1D20-A068
                                                                                                                                                                     2D 31 31 44 30 2D 42 33 46 30 2D 30 30 41 41 30   // -11D0-B3F0-00AA0
                                                                                                                                                                     30 33 37 36 31 43 35 00 00 )                      // 03761C5..
    .method /*060000ED*/ public hidebysig newslot abstract virtual 
            instance void  Progress([in] int32 iProgress) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMCopyCaptureFileProgress::Progress

  } // end of class IAMCopyCaptureFileProgress

  .class /*02000038*/ interface public abstract auto ansi import ICaptureGraphBuilder2
  {
    .custom /*0C000057:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 33 45 35 41 34 45 30 2D 32 44 35 30   // ..$93E5A4E0-2D50
                                                                                                                                                                     2D 31 31 44 32 2D 41 42 46 41 2D 30 30 41 30 43   // -11D2-ABFA-00A0C
                                                                                                                                                                     39 43 36 45 33 38 44 00 00 )                      // 9C6E38D..
    .custom /*0C000058:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060000EE*/ public hidebysig newslot abstract virtual 
            instance void  SetFiltergraph([in] class MSR.LST.MDShow.IGraphBuilder/* 02000035 */  marshal( interface) pfg) runtime managed internalcall
    // SIG: 20 01 01 12 80 D4
    {
    } // end of method ICaptureGraphBuilder2::SetFiltergraph

    .method /*060000EF*/ public hidebysig newslot abstract virtual 
            instance void  GetFiltergraph([out] class MSR.LST.MDShow.IGraphBuilder/* 02000035 */&  marshal( interface) ppfg) runtime managed internalcall
    // SIG: 20 01 01 10 12 80 D4
    {
    } // end of method ICaptureGraphBuilder2::GetFiltergraph

    .method /*060000F0*/ public hidebysig newslot abstract virtual 
            instance void  SetOutputFileName([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pType,
                                             [in] string  marshal( lpwstr) lpstrFile,
                                             [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppf,
                                             [out] class MSR.LST.MDShow.IFileSinkFilter/* 02000033 */&  marshal( interface) ppSink) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 0E 10 12 64 10 12 80 CC
    {
    } // end of method ICaptureGraphBuilder2::SetOutputFileName

    .method /*060000F1*/ public hidebysig newslot abstract virtual 
            instance void  RemoteFindInterface([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pCategory,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pType,
                                               [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pf,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& riid,
                                               [out] object&  marshal( iunknown) ppint) runtime managed internalcall
    // SIG: 20 05 01 10 11 11 10 11 11 12 64 10 11 11 10 1C
    {
    } // end of method ICaptureGraphBuilder2::RemoteFindInterface

    .method /*060000F2*/ public hidebysig newslot abstract virtual 
            instance void  RenderStream([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pCategory,
                                        [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pType,
                                        [in] object  marshal( iunknown) pSource,
                                        [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pfCompressor,
                                        [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pfRenderer) runtime managed internalcall
    // SIG: 20 05 01 10 11 11 10 11 11 1C 12 64 12 64
    {
    } // end of method ICaptureGraphBuilder2::RenderStream

    .method /*060000F3*/ public hidebysig newslot abstract virtual 
            instance void  ControlStream([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pCategory,
                                         [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pType,
                                         [in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter,
                                         [in] int64& pstart,
                                         [in] int64& pStop,
                                         [in] unsigned int16 wStartCookie,
                                         [in] unsigned int16 wStopCookie) runtime managed internalcall
    // SIG: 20 07 01 10 11 11 10 11 11 12 64 10 0A 10 0A 07 07
    {
    } // end of method ICaptureGraphBuilder2::ControlStream

    .method /*060000F4*/ public hidebysig newslot abstract virtual 
            instance void  AllocCapFile([in] string  marshal( lpwstr) 'lpstr',
                                        [in] unsigned int64 dwlSize) runtime managed internalcall
    // SIG: 20 02 01 0E 0B
    {
    } // end of method ICaptureGraphBuilder2::AllocCapFile

    .method /*060000F5*/ public hidebysig newslot abstract virtual 
            instance void  CopyCaptureFile([in] string  marshal( lpwstr) lpwstrOld,
                                           [in] string  marshal( lpwstr) lpwstrNew,
                                           [in] int32 fAllowEscAbort,
                                           [in] class MSR.LST.MDShow.IAMCopyCaptureFileProgress/* 02000037 */  marshal( interface) pCallback) runtime managed internalcall
    // SIG: 20 04 01 0E 0E 08 12 80 DC
    {
    } // end of method ICaptureGraphBuilder2::CopyCaptureFile

    .method /*060000F6*/ public hidebysig newslot abstract virtual 
            instance void  FindPin([in] object  marshal( iunknown) pSource,
                                   [in] valuetype MSR.LST.MDShow._PinDirection/* 02000015 */ pindir,
                                   [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pCategory,
                                   [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pType,
                                   [in] int32 fUnconnected,
                                   [in] int32 num,
                                   [out] class MSR.LST.MDShow.IPin/* 0200001E */&  marshal( interface) ppPin) runtime managed internalcall
    // SIG: 20 07 01 1C 11 54 10 11 11 10 11 11 08 08 10 12 78
    {
    } // end of method ICaptureGraphBuilder2::FindPin

  } // end of class ICaptureGraphBuilder2

  .class /*02000039*/ interface public abstract auto ansi import IFilterMapper
  {
    .custom /*0C000059:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00005A:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 33 2D 30 41 44 34   // ..$56A868A3-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*060000F7*/ public hidebysig newslot abstract virtual 
            instance void  RegisterFilter([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ 'clsid',
                                          [in] string  marshal( lpwstr) Name,
                                          [in] unsigned int32 dwMerit) runtime managed internalcall
    // SIG: 20 03 01 11 11 0E 09
    {
    } // end of method IFilterMapper::RegisterFilter

    .method /*060000F8*/ public hidebysig newslot abstract virtual 
            instance void  RegisterFilterInstance([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ 'clsid',
                                                  [in] string  marshal( lpwstr) Name,
                                                  [out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& MRId) runtime managed internalcall
    // SIG: 20 03 01 11 11 0E 10 11 11
    {
    } // end of method IFilterMapper::RegisterFilterInstance

    .method /*060000F9*/ public hidebysig newslot abstract virtual 
            instance void  RegisterPin([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ Filter,
                                       [in] string  marshal( lpwstr) Name,
                                       [in] int32 bRendered,
                                       [in] int32 bOutput,
                                       [in] int32 bZero,
                                       [in] int32 bMany,
                                       [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ ConnectsToFilter,
                                       [in] string  marshal( lpwstr) ConnectsToPin) runtime managed internalcall
    // SIG: 20 08 01 11 11 0E 08 08 08 08 11 11 0E
    {
    } // end of method IFilterMapper::RegisterPin

    .method /*060000FA*/ public hidebysig newslot abstract virtual 
            instance void  RegisterPinType([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsFilter,
                                           [in] string  marshal( lpwstr) strName,
                                           [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsMajorType,
                                           [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsSubType) runtime managed internalcall
    // SIG: 20 04 01 11 11 0E 11 11 11 11
    {
    } // end of method IFilterMapper::RegisterPinType

    .method /*060000FB*/ public hidebysig newslot abstract virtual 
            instance void  UnregisterFilter([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ Filter) runtime managed internalcall
    // SIG: 20 01 01 11 11
    {
    } // end of method IFilterMapper::UnregisterFilter

    .method /*060000FC*/ public hidebysig newslot abstract virtual 
            instance void  UnregisterFilterInstance([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ MRId) runtime managed internalcall
    // SIG: 20 01 01 11 11
    {
    } // end of method IFilterMapper::UnregisterFilterInstance

    .method /*060000FD*/ public hidebysig newslot abstract virtual 
            instance void  UnregisterPin([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ Filter,
                                         [in] string  marshal( lpwstr) Name) runtime managed internalcall
    // SIG: 20 02 01 11 11 0E
    {
    } // end of method IFilterMapper::UnregisterPin

    .method /*060000FE*/ public hidebysig newslot abstract virtual 
            instance void  EnumMatchingFilters([out] class MSR.LST.MDShow.IEnumRegFilters/* 0200002F */&  marshal( interface) ppenum,
                                               [in] unsigned int32 dwMerit,
                                               [in] int32 bInputNeeded,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsInMaj,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsInSub,
                                               [in] int32 bRender,
                                               [in] int32 bOututNeeded,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsOutMaj,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsOutSub) runtime managed internalcall
    // SIG: 20 09 01 10 12 80 BC 09 08 11 11 11 11 08 08 11 11 11 11
    {
    } // end of method IFilterMapper::EnumMatchingFilters

  } // end of class IFilterMapper

  .class /*0200003A*/ interface public abstract auto ansi import IFilterMapper2
  {
    .custom /*0C00005B:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 42 37 39 42 42 30 42 30 2D 33 33 43 31   // ..$B79BB0B0-33C1
                                                                                                                                                                     2D 31 31 44 31 2D 41 42 45 31 2D 30 30 41 30 43   // -11D1-ABE1-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .custom /*0C00005C:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060000FF*/ public hidebysig newslot abstract virtual 
            instance void  CreateCategory([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& clsidCategory,
                                          [in] unsigned int32 dwCategoryMerit,
                                          [in] string  marshal( lpwstr) Description) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 09 0E
    {
    } // end of method IFilterMapper2::CreateCategory

    .method /*06000100*/ public hidebysig newslot abstract virtual 
            instance void  UnregisterFilter([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pclsidCategory,
                                            [in] unsigned int16& szInstance,
                                            [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Filter) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 10 07 10 11 11
    {
    } // end of method IFilterMapper2::UnregisterFilter

    .method /*06000101*/ public hidebysig newslot abstract virtual 
            instance void  RegisterFilter([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& clsidFilter,
                                          [in] string  marshal( lpwstr) Name,
                                          [in][out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppMoniker,
                                          [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pclsidCategory,
                                          [in] unsigned int16& szInstance,
                                          [in] valuetype MSR.LST.MDShow.REGFILTER2/* 0200003B */& prf2) runtime managed internalcall
    // SIG: 20 06 01 10 11 11 0E 10 12 10 10 11 11 10 07 10 11 80 EC
    {
      .param [6]/*080001AC*/ 
      .custom /*0C000139:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 19 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 52 45 47 46 49 4C 54 45 52 32 00 00 )       // w.REGFILTER2..
    } // end of method IFilterMapper2::RegisterFilter

    .method /*06000102*/ public hidebysig newslot abstract virtual 
            instance void  EnumMatchingFilters([out] class MSR.LST.MDShow.IEnumMoniker/* 02000003 */&  marshal( interface) ppenum,
                                               [in] unsigned int32 dwFlags,
                                               [in] int32 bExactMatch,
                                               [in] unsigned int32 dwMerit,
                                               [in] int32 bInputNeeded,
                                               [in] unsigned int32 cInputTypes,
                                               valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pInputTypes,
                                               [in] valuetype MSR.LST.MDShow.REGPINMEDIUM/* 02000046 */& pMedIn,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pPinCategoryIn,
                                               [in] int32 bRender,
                                               [in] int32 bOutputNeeded,
                                               [in] unsigned int32 cOutputTypes,
                                               valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pOutputTypes,
                                               [in] valuetype MSR.LST.MDShow.REGPINMEDIUM/* 02000046 */& pMedOut,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pPinCategoryOut) runtime managed internalcall
    // SIG: 20 0F 01 10 12 0C 09 08 09 08 09 10 11 11 10 11 81 18 10 11 11 08 08 09 10 11 11 10 11 81 18 10 11 11
    {
      .param [8]/*080001B4*/ 
      .custom /*0C00013A:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1B 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 52 45 47 50 49 4E 4D 45 44 49 55 4D 00 00 ) // w.REGPINMEDIUM..
      .param [14]/*080001BA*/ 
      .custom /*0C00013B:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1B 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 52 45 47 50 49 4E 4D 45 44 49 55 4D 00 00 ) // w.REGPINMEDIUM..
    } // end of method IFilterMapper2::EnumMatchingFilters

  } // end of class IFilterMapper2

  .class /*0200003B*/ public sequential ansi sealed beforefieldinit REGFILTER2
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*04000071*/ public unsigned int32 dwVersion
    .field /*04000072*/ public unsigned int32 dwMerit
    .field /*04000073*/ public valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0137_0007/* 0200003D */ __MIDL_0014
  } // end of class REGFILTER2

  .class /*0200003C*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0006
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*04000074*/ public unsigned int32 dwVersion
    .field /*04000075*/ public unsigned int32 dwMerit
    .field /*04000076*/ public valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0137_0007/* 0200003D */ __MIDL_0014
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0006

  .class /*0200003D*/ public explicit ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0007
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 8
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0007

  .class /*0200003E*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0008
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C00005D:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000077*/ public unsigned int32 cPins
    .field /*04000078*/ public native int rgPins
    .custom /*0C0000AD:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1C 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 46 49 4C 54 45 52 50 49 4E 53 00   // w.REGFILTERPINS.
                                                                                                                                                                             00 ) 
    .custom /*0C0000AE:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0008

  .class /*0200003F*/ public sequential ansi sealed beforefieldinit REGFILTERPINS
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C00005E:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000079*/ public  marshal( lpwstr) string strName
    .field /*0400007A*/ public int32 bRendered
    .field /*0400007B*/ public int32 bOutput
    .field /*0400007C*/ public int32 bZero
    .field /*0400007D*/ public int32 bMany
    .field /*0400007E*/ public native int clsConnectsToFilter
    .custom /*0C0000B9:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400007F*/ public native int strConnectsToPin
    .custom /*0C0000BC:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000080*/ public unsigned int32 nMediaTypes
    .field /*04000081*/ public native int lpMediaType
    .custom /*0C0000C2:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1A 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 50 49 4E 54 59 50 45 53 00 00 )    // w.REGPINTYPES..
    .custom /*0C0000C3:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class REGFILTERPINS

  .class /*02000040*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0002
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C00005F:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000082*/ public  marshal( lpwstr) string strName
    .field /*04000083*/ public int32 bRendered
    .field /*04000084*/ public int32 bOutput
    .field /*04000085*/ public int32 bZero
    .field /*04000086*/ public int32 bMany
    .field /*04000087*/ public native int clsConnectsToFilter
    .custom /*0C0000CC:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000088*/ public native int strConnectsToPin
    .custom /*0C0000CF:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000089*/ public unsigned int32 nMediaTypes
    .field /*0400008A*/ public native int lpMediaType
    .custom /*0C0000D5:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1A 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 50 49 4E 54 59 50 45 53 00 00 )    // w.REGPINTYPES..
    .custom /*0C0000D6:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0002

  .class /*02000041*/ public sequential ansi sealed beforefieldinit REGPINTYPES
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000060:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400008B*/ public native int clsMajorType
    .custom /*0C0000D9:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400008C*/ public native int clsMinorType
    .custom /*0C0000DA:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class REGPINTYPES

  .class /*02000042*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000061:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400008D*/ public native int clsMajorType
    .custom /*0C0000DD:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400008E*/ public native int clsMinorType
    .custom /*0C0000DE:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0001

  .class /*02000043*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0009
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000062:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*0400008F*/ public unsigned int32 cPins2
    .field /*04000090*/ public native int rgPins2
    .custom /*0C0000DF:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E0:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1D 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 46 49 4C 54 45 52 50 49 4E 53 32   // w.REGFILTERPINS2
                                                                                                                                                                             00 00 ) 
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0009

  .class /*02000044*/ public sequential ansi sealed beforefieldinit REGFILTERPINS2
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000063:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000091*/ public unsigned int32 dwFlags
    .field /*04000092*/ public unsigned int32 cInstances
    .field /*04000093*/ public unsigned int32 nMediaTypes
    .field /*04000094*/ public native int lpMediaType
    .custom /*0C0000E7:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E8:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1A 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 50 49 4E 54 59 50 45 53 00 00 )    // w.REGPINTYPES..
    .field /*04000095*/ public unsigned int32 nMediums
    .field /*04000096*/ public native int lpMedium
    .custom /*0C0000EE:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C0000EF:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1B 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 50 49 4E 4D 45 44 49 55 4D 00 00 ) // w.REGPINMEDIUM..
    .field /*04000097*/ public native int clsPinCategory
    .custom /*0C0000F3:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class REGFILTERPINS2

  .class /*02000045*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0005
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .custom /*0C000064:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .field /*04000098*/ public unsigned int32 dwFlags
    .field /*04000099*/ public unsigned int32 cInstances
    .field /*0400009A*/ public unsigned int32 nMediaTypes
    .field /*0400009B*/ public native int lpMediaType
    .custom /*0C0000F9:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C0000FA:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1A 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 50 49 4E 54 59 50 45 53 00 00 )    // w.REGPINTYPES..
    .field /*0400009C*/ public unsigned int32 nMediums
    .field /*0400009D*/ public native int lpMedium
    .custom /*0C0000FB:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C0000FC:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1B 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                             77 2E 52 45 47 50 49 4E 4D 45 44 49 55 4D 00 00 ) // w.REGPINMEDIUM..
    .field /*0400009E*/ public native int clsPinCategory
    .custom /*0C0000FF:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0005

  .class /*02000046*/ public sequential ansi sealed beforefieldinit REGPINMEDIUM
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*0400009F*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsMedium
    .field /*040000A0*/ public unsigned int32 dw1
    .field /*040000A1*/ public unsigned int32 dw2
  } // end of class REGPINMEDIUM

  .class /*02000047*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0003
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000A2*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ clsMedium
    .field /*040000A3*/ public unsigned int32 dw1
    .field /*040000A4*/ public unsigned int32 dw2
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0003

  .class /*02000048*/ interface public abstract auto ansi import IMediaEventSink
  {
    .custom /*0C000065:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000066:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 32 2D 30 41 44 34   // ..$56A868A2-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*06000103*/ public hidebysig newslot abstract virtual 
            instance void  Notify([in] int32 EventCode,
                                  [in] int32 EventParam1,
                                  [in] int32 EventParam2) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
      .param [2]/*080001BD*/ 
      .custom /*0C00013C:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
      .param [3]/*080001BE*/ 
      .custom /*0C00013D:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    } // end of method IMediaEventSink::Notify

  } // end of class IMediaEventSink

  .class /*02000049*/ interface public abstract auto ansi import IOverlay
  {
    .custom /*0C000067:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000068:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 31 2D 30 41 44 34   // ..$56A868A1-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C000069:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*06000104*/ public hidebysig newslot abstract virtual 
            instance void  GetPalette([out] unsigned int32& pdwColors,
                                      [out] native int ppPalette) runtime managed internalcall
    // SIG: 20 02 01 10 09 18
    {
    } // end of method IOverlay::GetPalette

    .method /*06000105*/ public hidebysig newslot abstract virtual 
            instance void  SetPalette([in] unsigned int32 dwColors,
                                      [in] valuetype MSR.LST.MDShow.tagPALETTEENTRY/* 0200004A */& pPalette) runtime managed internalcall
    // SIG: 20 02 01 09 10 11 81 28
    {
    } // end of method IOverlay::SetPalette

    .method /*06000106*/ public hidebysig newslot abstract virtual 
            instance void  GetDefaultColorKey([out] valuetype MSR.LST.MDShow.tagCOLORKEY/* 0200004B */& pColorKey) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 2C
    {
    } // end of method IOverlay::GetDefaultColorKey

    .method /*06000107*/ public hidebysig newslot abstract virtual 
            instance void  GetColorKey([out] valuetype MSR.LST.MDShow.tagCOLORKEY/* 0200004B */& pColorKey) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 2C
    {
    } // end of method IOverlay::GetColorKey

    .method /*06000108*/ public hidebysig newslot abstract virtual 
            instance void  SetColorKey([in][out] valuetype MSR.LST.MDShow.tagCOLORKEY/* 0200004B */& pColorKey) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 2C
    {
    } // end of method IOverlay::SetColorKey

    .method /*06000109*/ public hidebysig newslot abstract virtual 
            instance void  GetWindowHandle([out] native int pHwnd) runtime managed internalcall
    // SIG: 20 01 01 18
    {
      .param [1]/*080001C6*/ 
      .custom /*0C00013E:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 77 69 72 65 48 57 4E 44 00 00 )             // w.wireHWND..
    } // end of method IOverlay::GetWindowHandle

    .method /*0600010A*/ public hidebysig newslot abstract virtual 
            instance void  GetClipList([out] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pSourceRect,
                                       [out] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pDestinationRect,
                                       [out] native int ppRgnData) runtime managed internalcall
    // SIG: 20 03 01 10 11 81 38 10 11 81 38 18
    {
    } // end of method IOverlay::GetClipList

    .method /*0600010B*/ public hidebysig newslot abstract virtual 
            instance void  GetVideoPosition([out] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pSourceRect,
                                            [out] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pDestinationRect) runtime managed internalcall
    // SIG: 20 02 01 10 11 81 38 10 11 81 38
    {
    } // end of method IOverlay::GetVideoPosition

    .method /*0600010C*/ public hidebysig newslot abstract virtual 
            instance void  Advise([in] class MSR.LST.MDShow.IOverlayNotify/* 02000051 */  marshal( interface) pOverlayNotify,
                                  [in] unsigned int32 dwInterests) runtime managed internalcall
    // SIG: 20 02 01 12 81 44 09
    {
    } // end of method IOverlay::Advise

    .method /*0600010D*/ public hidebysig newslot abstract virtual 
            instance void  Unadvise() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IOverlay::Unadvise

  } // end of class IOverlay

  .class /*0200004A*/ public sequential ansi sealed beforefieldinit tagPALETTEENTRY
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 1
    .size 0
    .field /*040000A5*/ public unsigned int8 peRed
    .field /*040000A6*/ public unsigned int8 peGreen
    .field /*040000A7*/ public unsigned int8 peBlue
    .field /*040000A8*/ public unsigned int8 peFlags
  } // end of class tagPALETTEENTRY

  .class /*0200004B*/ public sequential ansi sealed beforefieldinit tagCOLORKEY
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000A9*/ public unsigned int32 KeyType
    .field /*040000AA*/ public unsigned int32 PaletteIndex
    .field /*040000AB*/ public unsigned int32 LowColorValue
    .field /*040000AC*/ public unsigned int32 HighColorValue
  } // end of class tagCOLORKEY

  .class /*0200004C*/ public sequential ansi sealed beforefieldinit _RemotableHandle
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000AD*/ public int32 fContext
    .field /*040000AE*/ public valuetype MSR.LST.MDShow.__MIDL_IWinTypes_0009/* 0200004D */ u
  } // end of class _RemotableHandle

  .class /*0200004D*/ public explicit ansi sealed __MIDL_IWinTypes_0009
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000AF*/ [0] public int32 hInproc
    .field /*040000B0*/ [0] public int32 hRemote
  } // end of class __MIDL_IWinTypes_0009

  .class /*0200004E*/ public sequential ansi sealed beforefieldinit tagRECT
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000B1*/ public int32 left
    .field /*040000B2*/ public int32 top
    .field /*040000B3*/ public int32 right
    .field /*040000B4*/ public int32 bottom
  } // end of class tagRECT

  .class /*0200004F*/ public sequential ansi sealed beforefieldinit _RGNDATA
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000B5*/ public valuetype MSR.LST.MDShow._RGNDATAHEADER/* 02000050 */ rdh
    .field /*040000B6*/ public  marshal( fixed array [1]) int8[] Buffer
  } // end of class _RGNDATA

  .class /*02000050*/ public sequential ansi sealed beforefieldinit _RGNDATAHEADER
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000B7*/ public unsigned int32 dwSize
    .field /*040000B8*/ public unsigned int32 iType
    .field /*040000B9*/ public unsigned int32 nCount
    .field /*040000BA*/ public unsigned int32 nRgnSize
    .field /*040000BB*/ public valuetype MSR.LST.MDShow.tagRECT/* 0200004E */ rcBound
  } // end of class _RGNDATAHEADER

  .class /*02000051*/ interface public abstract auto ansi import IOverlayNotify
  {
    .custom /*0C00006A:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 30 2D 30 41 44 34   // ..$56A868A0-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C00006B:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600010E*/ public hidebysig newslot abstract virtual 
            instance void  OnPaletteChange([in] unsigned int32 dwColors,
                                           [in] valuetype MSR.LST.MDShow.tagPALETTEENTRY/* 0200004A */& pPalette) runtime managed internalcall
    // SIG: 20 02 01 09 10 11 81 28
    {
    } // end of method IOverlayNotify::OnPaletteChange

    .method /*0600010F*/ public hidebysig newslot abstract virtual 
            instance void  OnClipChange([in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pSourceRect,
                                        [in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pDestinationRect,
                                        [in] valuetype MSR.LST.MDShow._RGNDATA/* 0200004F */& pRgnData) runtime managed internalcall
    // SIG: 20 03 01 10 11 81 38 10 11 81 38 10 11 81 3C
    {
    } // end of method IOverlayNotify::OnClipChange

    .method /*06000110*/ public hidebysig newslot abstract virtual 
            instance void  OnColorKeyChange([in] valuetype MSR.LST.MDShow.tagCOLORKEY/* 0200004B */& pColorKey) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 2C
    {
    } // end of method IOverlayNotify::OnColorKeyChange

    .method /*06000111*/ public hidebysig newslot abstract virtual 
            instance void  OnPositionChange([in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pSourceRect,
                                            [in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pDestinationRect) runtime managed internalcall
    // SIG: 20 02 01 10 11 81 38 10 11 81 38
    {
    } // end of method IOverlayNotify::OnPositionChange

  } // end of class IOverlayNotify

  .class /*02000052*/ interface public abstract auto ansi import IOverlayNotify2
         implements MSR.LST.MDShow.IOverlayNotify/* 02000051 */
  {
    .custom /*0C00006C:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00006D:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 38 30 45 46 41 31 30 2D 44 35 33 35   // ..$680EFA10-D535
                                                                                                                                                                     2D 31 31 44 31 2D 38 37 43 38 2D 30 30 41 30 43   // -11D1-87C8-00A0C
                                                                                                                                                                     39 32 32 33 31 39 36 00 00 )                      // 9223196..
    .custom /*0C00006E:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*06000112*/ public hidebysig newslot abstract virtual 
            instance void  OnPaletteChange([in] unsigned int32 dwColors,
                                           [in] valuetype MSR.LST.MDShow.tagPALETTEENTRY/* 0200004A */& pPalette) runtime managed internalcall
    // SIG: 20 02 01 09 10 11 81 28
    {
    } // end of method IOverlayNotify2::OnPaletteChange

    .method /*06000113*/ public hidebysig newslot abstract virtual 
            instance void  OnClipChange([in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pSourceRect,
                                        [in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pDestinationRect,
                                        [in] valuetype MSR.LST.MDShow._RGNDATA/* 0200004F */& pRgnData) runtime managed internalcall
    // SIG: 20 03 01 10 11 81 38 10 11 81 38 10 11 81 3C
    {
    } // end of method IOverlayNotify2::OnClipChange

    .method /*06000114*/ public hidebysig newslot abstract virtual 
            instance void  OnColorKeyChange([in] valuetype MSR.LST.MDShow.tagCOLORKEY/* 0200004B */& pColorKey) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 2C
    {
    } // end of method IOverlayNotify2::OnColorKeyChange

    .method /*06000115*/ public hidebysig newslot abstract virtual 
            instance void  OnPositionChange([in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pSourceRect,
                                            [in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& pDestinationRect) runtime managed internalcall
    // SIG: 20 02 01 10 11 81 38 10 11 81 38
    {
    } // end of method IOverlayNotify2::OnPositionChange

    .method /*06000116*/ public hidebysig newslot abstract virtual 
            instance void  OnDisplayChange(native int hMonitor) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IOverlayNotify2::OnDisplayChange

  } // end of class IOverlayNotify2

  .class /*02000053*/ interface public abstract auto ansi import IQualityControl
  {
    .custom /*0C00006F:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000070:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 35 2D 30 41 44 34   // ..$56A868A5-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*06000117*/ public hidebysig newslot abstract virtual 
            instance void  Notify([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pSelf,
                                  [in] valuetype MSR.LST.MDShow.tagQuality/* 02000054 */ q) runtime managed internalcall
    // SIG: 20 02 01 12 64 11 81 50
    {
    } // end of method IQualityControl::Notify

    .method /*06000118*/ public hidebysig newslot abstract virtual 
            instance void  SetSink([in] class MSR.LST.MDShow.IQualityControl/* 02000053 */  marshal( interface) piqc) runtime managed internalcall
    // SIG: 20 01 01 12 81 4C
    {
    } // end of method IQualityControl::SetSink

  } // end of class IQualityControl

  .class /*02000054*/ public sequential ansi sealed beforefieldinit tagQuality
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .field /*040000BC*/ public valuetype MSR.LST.MDShow.tagQualityMessageType/* 02000055 */ type
    .field /*040000BD*/ public int32 Proportion
    .field /*040000BE*/ public int64 Late
    .field /*040000BF*/ public int64 TimeStamp
  } // end of class tagQuality

  .class /*02000055*/ public auto ansi sealed tagQualityMessageType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000C0*/ public specialname rtspecialname int32 value__
    .field /*040000C1*/ public static literal valuetype MSR.LST.MDShow.tagQualityMessageType/* 02000055 */ Famine = int32(0x00000000)
    .field /*040000C2*/ public static literal valuetype MSR.LST.MDShow.tagQualityMessageType/* 02000055 */ Flood = int32(0x00000001)
  } // end of class tagQualityMessageType

  .class /*02000056*/ interface public abstract auto ansi import ISeekingPassThru
  {
    .custom /*0C000072:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000073:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 36 42 37 33 38 38 33 2D 43 32 43 38   // ..$36B73883-C2C8
                                                                                                                                                                     2D 31 31 43 46 2D 38 42 34 36 2D 30 30 38 30 35   // -11CF-8B46-00805
                                                                                                                                                                     46 36 43 45 46 36 30 00 00 )                      // F6CEF60..
    .method /*06000119*/ public hidebysig newslot abstract virtual 
            instance void  Init([in] int32 bSupportRendering,
                                [in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 02 01 08 12 78
    {
    } // end of method ISeekingPassThru::Init

  } // end of class ISeekingPassThru

  .class /*02000057*/ interface public abstract auto ansi import IAMStreamConfig
  {
    .custom /*0C000075:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 34 30 2D 33 30 41 43   // ..$C6E13340-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .custom /*0C000076:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C000077:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600011A*/ public hidebysig newslot abstract virtual 
            instance void  SetFormat([in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 01 01 10 11 50
    {
    } // end of method IAMStreamConfig::SetFormat

    .method /*0600011B*/ public hidebysig newslot abstract virtual 
            instance void  GetFormat([out] native int ppmt) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IAMStreamConfig::GetFormat

    .method /*0600011C*/ public hidebysig newslot abstract virtual 
            instance void  GetNumberOfCapabilities([out] int32& piCount,
                                                   [out] int32& piSize) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 08
    {
    } // end of method IAMStreamConfig::GetNumberOfCapabilities

    .method /*0600011D*/ public hidebysig newslot abstract virtual 
            instance void  GetStreamCaps([in] int32 iIndex,
                                         [out] native int ppmt,
                                         [out] unsigned int8& pSCC) runtime managed internalcall
    // SIG: 20 03 01 08 18 10 05
    {
    } // end of method IAMStreamConfig::GetStreamCaps

  } // end of class IAMStreamConfig

  .class /*02000058*/ interface public abstract auto ansi import IAMDevMemoryAllocator
  {
    .custom /*0C000078:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C000079:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 35 34 35 42 46 30 2D 45 37 36 42   // ..$C6545BF0-E76B
                                                                                                                                                                     2D 31 31 44 30 2D 42 44 35 32 2D 30 30 41 30 43   // -11D0-BD52-00A0C
                                                                                                                                                                     39 31 31 43 45 38 36 00 00 )                      // 911CE86..
    .custom /*0C00007A:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600011E*/ public hidebysig newslot abstract virtual 
            instance void  GetInfo([out] unsigned int32& pdwcbTotalFree,
                                   [out] unsigned int32& pdwcbLargestFree,
                                   [out] unsigned int32& pdwcbTotalMemory,
                                   [out] unsigned int32& pdwcbMinimumChunk) runtime managed internalcall
    // SIG: 20 04 01 10 09 10 09 10 09 10 09
    {
    } // end of method IAMDevMemoryAllocator::GetInfo

    .method /*0600011F*/ public hidebysig newslot abstract virtual 
            instance void  CheckMemory([in] unsigned int8& pBuffer) runtime managed internalcall
    // SIG: 20 01 01 10 05
    {
    } // end of method IAMDevMemoryAllocator::CheckMemory

    .method /*06000120*/ public hidebysig newslot abstract virtual 
            instance void  Alloc([out] native int ppBuffer,
                                 [in][out] unsigned int32& pdwcbBuffer) runtime managed internalcall
    // SIG: 20 02 01 18 10 09
    {
    } // end of method IAMDevMemoryAllocator::Alloc

    .method /*06000121*/ public hidebysig newslot abstract virtual 
            instance void  Free([in] unsigned int8& pBuffer) runtime managed internalcall
    // SIG: 20 01 01 10 05
    {
    } // end of method IAMDevMemoryAllocator::Free

    .method /*06000122*/ public hidebysig newslot abstract virtual 
            instance void  GetDevMemoryObject([out] object&  marshal( iunknown) ppUnkInnner,
                                              [in] object  marshal( iunknown) pUnkOuter) runtime managed internalcall
    // SIG: 20 02 01 10 1C 1C
    {
    } // end of method IAMDevMemoryAllocator::GetDevMemoryObject

  } // end of class IAMDevMemoryAllocator

  .class /*02000059*/ interface public abstract auto ansi import IAMDevMemoryControl
  {
    .custom /*0C00007B:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 35 34 35 42 46 31 2D 45 37 36 42   // ..$C6545BF1-E76B
                                                                                                                                                                     2D 31 31 44 30 2D 42 44 35 32 2D 30 30 41 30 43   // -11D0-BD52-00A0C
                                                                                                                                                                     39 31 31 43 45 38 36 00 00 )                      // 911CE86..
    .custom /*0C00007C:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000123*/ public hidebysig newslot abstract virtual 
            instance void  QueryWriteSync() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMDevMemoryControl::QueryWriteSync

    .method /*06000124*/ public hidebysig newslot abstract virtual 
            instance void  WriteSync() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMDevMemoryControl::WriteSync

    .method /*06000125*/ public hidebysig newslot abstract virtual 
            instance void  GetDevId([out] unsigned int32& pdwDevId) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IAMDevMemoryControl::GetDevId

  } // end of class IAMDevMemoryControl

  .class /*0200005A*/ interface public abstract auto ansi import IConfigInterleaving
  {
    .custom /*0C00007D:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 42 45 45 33 44 32 32 30 2D 31 35 37 42   // ..$BEE3D220-157B
                                                                                                                                                                     2D 31 31 44 30 2D 42 44 32 33 2D 30 30 41 30 43   // -11D0-BD23-00A0C
                                                                                                                                                                     39 31 31 43 45 38 36 00 00 )                      // 911CE86..
    .custom /*0C00007E:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000126*/ public hidebysig newslot abstract virtual 
            instance void  put_Mode([in] valuetype MSR.LST.MDShow.InterleavingMode/* 0200005B */ mode) runtime managed internalcall
    // SIG: 20 01 01 11 81 6C
    {
      .param [1]/*080001F6*/ 
      .custom /*0C00013F:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1F 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 49 6E 74 65 72 6C 65 61 76 69 6E 67 4D 6F   // w.InterleavingMo
                                                                                                                                                                               64 65 00 00 )                                     // de..
    } // end of method IConfigInterleaving::put_Mode

    .method /*06000127*/ public hidebysig newslot abstract virtual 
            instance void  get_Mode([out] valuetype MSR.LST.MDShow.InterleavingMode/* 0200005B */& pMode) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 6C
    {
      .param [1]/*080001F7*/ 
      .custom /*0C000140:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1F 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 49 6E 74 65 72 6C 65 61 76 69 6E 67 4D 6F   // w.InterleavingMo
                                                                                                                                                                               64 65 00 00 )                                     // de..
    } // end of method IConfigInterleaving::get_Mode

    .method /*06000128*/ public hidebysig newslot abstract virtual 
            instance void  put_Interleaving([in] int64& prtInterleave,
                                            [in] int64& prtPreroll) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IConfigInterleaving::put_Interleaving

    .method /*06000129*/ public hidebysig newslot abstract virtual 
            instance void  get_Interleaving([out] int64& prtInterleave,
                                            [out] int64& prtPreroll) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IConfigInterleaving::get_Interleaving

  } // end of class IConfigInterleaving

  .class /*0200005B*/ public auto ansi sealed InterleavingMode
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000C3*/ public specialname rtspecialname int32 value__
    .field /*040000C4*/ public static literal valuetype MSR.LST.MDShow.InterleavingMode/* 0200005B */ INTERLEAVE_NONE = int32(0x00000000)
    .field /*040000C5*/ public static literal valuetype MSR.LST.MDShow.InterleavingMode/* 0200005B */ INTERLEAVE_CAPTURE = int32(0x00000001)
    .field /*040000C6*/ public static literal valuetype MSR.LST.MDShow.InterleavingMode/* 0200005B */ INTERLEAVE_FULL = int32(0x00000002)
    .field /*040000C7*/ public static literal valuetype MSR.LST.MDShow.InterleavingMode/* 0200005B */ INTERLEAVE_NONE_BUFFERED = int32(0x00000003)
  } // end of class InterleavingMode

  .class /*0200005C*/ public auto ansi sealed __MIDL_IConfigInterleaving_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000C8*/ public specialname rtspecialname int32 value__
    .field /*040000C9*/ public static literal valuetype MSR.LST.MDShow.__MIDL_IConfigInterleaving_0001/* 0200005C */ INTERLEAVE_NONE = int32(0x00000000)
    .field /*040000CA*/ public static literal valuetype MSR.LST.MDShow.__MIDL_IConfigInterleaving_0001/* 0200005C */ INTERLEAVE_CAPTURE = int32(0x00000001)
    .field /*040000CB*/ public static literal valuetype MSR.LST.MDShow.__MIDL_IConfigInterleaving_0001/* 0200005C */ INTERLEAVE_FULL = int32(0x00000002)
    .field /*040000CC*/ public static literal valuetype MSR.LST.MDShow.__MIDL_IConfigInterleaving_0001/* 0200005C */ INTERLEAVE_NONE_BUFFERED = int32(0x00000003)
  } // end of class __MIDL_IConfigInterleaving_0001

  .class /*0200005D*/ interface public abstract auto ansi import IConfigAviMux
  {
    .custom /*0C00007F:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000080:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 41 43 44 36 41 41 30 2D 46 34 38 32   // ..$5ACD6AA0-F482
                                                                                                                                                                     2D 31 31 43 45 2D 38 42 36 37 2D 30 30 41 41 30   // -11CE-8B67-00AA0
                                                                                                                                                                     30 41 33 46 31 41 36 00 00 )                      // 0A3F1A6..
    .method /*0600012A*/ public hidebysig newslot abstract virtual 
            instance void  SetMasterStream([in] int32 IStream) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IConfigAviMux::SetMasterStream

    .method /*0600012B*/ public hidebysig newslot abstract virtual 
            instance void  GetMasterStream([out] int32& pStream) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IConfigAviMux::GetMasterStream

    .method /*0600012C*/ public hidebysig newslot abstract virtual 
            instance void  SetOutputCompatibilityIndex([in] int32 fOldIndex) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IConfigAviMux::SetOutputCompatibilityIndex

    .method /*0600012D*/ public hidebysig newslot abstract virtual 
            instance void  GetOutputCompatibilityIndex([out] int32& pfOldIndex) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IConfigAviMux::GetOutputCompatibilityIndex

  } // end of class IConfigAviMux

  .class /*0200005E*/ interface public abstract auto ansi import IAMVideoCompression
  {
    .custom /*0C000081:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 34 33 2D 33 30 41 43   // ..$C6E13343-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .custom /*0C000082:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600012E*/ public hidebysig newslot abstract virtual 
            instance void  put_KeyFrameRate([in] int32 KeyFrameRate) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMVideoCompression::put_KeyFrameRate

    .method /*0600012F*/ public hidebysig newslot abstract virtual 
            instance void  get_KeyFrameRate([out] int32& pKeyFrameRate) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMVideoCompression::get_KeyFrameRate

    .method /*06000130*/ public hidebysig newslot abstract virtual 
            instance void  put_PFramesPerKeyFrame([in] int32 PFramesPerKeyFrame) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMVideoCompression::put_PFramesPerKeyFrame

    .method /*06000131*/ public hidebysig newslot abstract virtual 
            instance void  get_PFramesPerKeyFrame([out] int32& pPFramesPerKeyFrame) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMVideoCompression::get_PFramesPerKeyFrame

    .method /*06000132*/ public hidebysig newslot abstract virtual 
            instance void  put_Quality([in] float64 Quality) runtime managed internalcall
    // SIG: 20 01 01 0D
    {
    } // end of method IAMVideoCompression::put_Quality

    .method /*06000133*/ public hidebysig newslot abstract virtual 
            instance void  get_Quality([out] float64& pQuality) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMVideoCompression::get_Quality

    .method /*06000134*/ public hidebysig newslot abstract virtual 
            instance void  put_WindowSize([in] unsigned int64 WindowSize) runtime managed internalcall
    // SIG: 20 01 01 0B
    {
    } // end of method IAMVideoCompression::put_WindowSize

    .method /*06000135*/ public hidebysig newslot abstract virtual 
            instance void  get_WindowSize([out] unsigned int64& pWindowSize) runtime managed internalcall
    // SIG: 20 01 01 10 0B
    {
    } // end of method IAMVideoCompression::get_WindowSize

    .method /*06000136*/ public hidebysig newslot abstract virtual 
            instance void  GetInfo([out] unsigned int16& pszVersion,
                                   [in][out] int32& pcbVersion,
                                   [out] string  marshal( lpwstr) pszDescription,
                                   [in][out] int32& pcbDescription,
                                   [out] int32& pDefaultKeyFrameRate,
                                   [out] int32& pDefaultPFramesPerKey,
                                   [out] float64& pDefaultQuality,
                                   [out] int32& pCapabilities) runtime managed internalcall
    // SIG: 20 08 01 10 07 10 08 0E 10 08 10 08 10 08 10 0D 10 08
    {
    } // end of method IAMVideoCompression::GetInfo

    .method /*06000137*/ public hidebysig newslot abstract virtual 
            instance void  OverrideKeyFrame([in] int32 FrameNumber) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMVideoCompression::OverrideKeyFrame

    .method /*06000138*/ public hidebysig newslot abstract virtual 
            instance void  OverrideFrameSize([in] int32 FrameNumber,
                                             [in] int32 Size) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMVideoCompression::OverrideFrameSize

  } // end of class IAMVideoCompression

  .class /*0200005F*/ interface public abstract auto ansi import IAMVfwCaptureDialogs
  {
    .custom /*0C000083:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000084:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 44 38 44 37 31 35 41 30 2D 36 45 35 45   // ..$D8D715A0-6E5E
                                                                                                                                                                     2D 31 31 44 30 2D 42 33 46 30 2D 30 30 41 41 30   // -11D0-B3F0-00AA0
                                                                                                                                                                     30 33 37 36 31 43 35 00 00 )                      // 03761C5..
    .method /*06000139*/ public hidebysig newslot abstract virtual 
            instance void  HasDialog([in] int32 iDialog) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMVfwCaptureDialogs::HasDialog

    .method /*0600013A*/ public hidebysig newslot abstract virtual 
            instance void  ShowDialog([in] int32 iDialog,
                                      [in] valuetype MSR.LST.MDShow._RemotableHandle/* 0200004C */& hwnd) runtime managed internalcall
    // SIG: 20 02 01 08 10 11 81 30
    {
      .param [2]/*08000215*/ 
      .custom /*0C000141:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 77 69 72 65 48 57 4E 44 00 00 )             // w.wireHWND..
    } // end of method IAMVfwCaptureDialogs::ShowDialog

    .method /*0600013B*/ public hidebysig newslot abstract virtual 
            instance void  SendDriverMessage([in] int32 iDialog,
                                             [in] int32 uMsg,
                                             [in] int32 dw1,
                                             [in] int32 dw2) runtime managed internalcall
    // SIG: 20 04 01 08 08 08 08
    {
    } // end of method IAMVfwCaptureDialogs::SendDriverMessage

  } // end of class IAMVfwCaptureDialogs

  .class /*02000060*/ interface public abstract auto ansi import IAMVfwCompressDialogs
  {
    .custom /*0C000085:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C000086:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 44 38 44 37 31 35 41 33 2D 36 45 35 45   // ..$D8D715A3-6E5E
                                                                                                                                                                     2D 31 31 44 30 2D 42 33 46 30 2D 30 30 41 41 30   // -11D0-B3F0-00AA0
                                                                                                                                                                     30 33 37 36 31 43 35 00 00 )                      // 03761C5..
    .custom /*0C000087:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600013C*/ public hidebysig newslot abstract virtual 
            instance void  ShowDialog([in] int32 iDialog,
                                      [in] valuetype MSR.LST.MDShow._RemotableHandle/* 0200004C */& hwnd) runtime managed internalcall
    // SIG: 20 02 01 08 10 11 81 30
    {
      .param [2]/*0800021B*/ 
      .custom /*0C000142:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 77 69 72 65 48 57 4E 44 00 00 )             // w.wireHWND..
    } // end of method IAMVfwCompressDialogs::ShowDialog

    .method /*0600013D*/ public hidebysig newslot abstract virtual 
            instance void  GetState([out] native int pState,
                                    [in][out] int32& pcbState) runtime managed internalcall
    // SIG: 20 02 01 18 10 08
    {
    } // end of method IAMVfwCompressDialogs::GetState

    .method /*0600013E*/ public hidebysig newslot abstract virtual 
            instance void  SetState([in] native int pState,
                                    [in] int32 cbState) runtime managed internalcall
    // SIG: 20 02 01 18 08
    {
    } // end of method IAMVfwCompressDialogs::SetState

    .method /*0600013F*/ public hidebysig newslot abstract virtual 
            instance void  SendDriverMessage([in] int32 uMsg,
                                             [in] int32 dw1,
                                             [in] int32 dw2) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
    } // end of method IAMVfwCompressDialogs::SendDriverMessage

  } // end of class IAMVfwCompressDialogs

  .class /*02000061*/ interface public abstract auto ansi import IAMDroppedFrames
  {
    .custom /*0C000088:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000089:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 34 34 2D 33 30 41 43   // ..$C6E13344-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .method /*06000140*/ public hidebysig newslot abstract virtual 
            instance void  GetNumDropped([out] int32& plDropped) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMDroppedFrames::GetNumDropped

    .method /*06000141*/ public hidebysig newslot abstract virtual 
            instance void  GetNumNotDropped([out] int32& plNotDropped) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMDroppedFrames::GetNumNotDropped

    .method /*06000142*/ public hidebysig newslot abstract virtual 
            instance void  GetDroppedInfo([in] int32 lSize,
                                          [out] int32& plArray,
                                          [out] int32& plNumCopied) runtime managed internalcall
    // SIG: 20 03 01 08 10 08 10 08
    {
    } // end of method IAMDroppedFrames::GetDroppedInfo

    .method /*06000143*/ public hidebysig newslot abstract virtual 
            instance void  GetAverageFrameSize([out] int32& plAverageSize) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMDroppedFrames::GetAverageFrameSize

  } // end of class IAMDroppedFrames

  .class /*02000062*/ interface public abstract auto ansi import IAMAudioInputMixer
  {
    .custom /*0C00008A:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00008B:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 34 43 33 39 32 32 31 2D 38 33 38 30   // ..$54C39221-8380
                                                                                                                                                                     2D 31 31 44 30 2D 42 33 46 30 2D 30 30 41 41 30   // -11D0-B3F0-00AA0
                                                                                                                                                                     30 33 37 36 31 43 35 00 00 )                      // 03761C5..
    .method /*06000144*/ public hidebysig newslot abstract virtual 
            instance void  put_Enable([in] int32 fEnable) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAudioInputMixer::put_Enable

    .method /*06000145*/ public hidebysig newslot abstract virtual 
            instance void  get_Enable([out] int32& pfEnable) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAudioInputMixer::get_Enable

    .method /*06000146*/ public hidebysig newslot abstract virtual 
            instance void  put_Mono([in] int32 fMono) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAudioInputMixer::put_Mono

    .method /*06000147*/ public hidebysig newslot abstract virtual 
            instance void  get_Mono([out] int32& pfMono) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAudioInputMixer::get_Mono

    .method /*06000148*/ public hidebysig newslot abstract virtual 
            instance void  put_MixLevel([in] float64 Level) runtime managed internalcall
    // SIG: 20 01 01 0D
    {
    } // end of method IAMAudioInputMixer::put_MixLevel

    .method /*06000149*/ public hidebysig newslot abstract virtual 
            instance void  get_MixLevel([out] float64& pLevel) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMAudioInputMixer::get_MixLevel

    .method /*0600014A*/ public hidebysig newslot abstract virtual 
            instance void  put_Pan([in] float64 Pan) runtime managed internalcall
    // SIG: 20 01 01 0D
    {
    } // end of method IAMAudioInputMixer::put_Pan

    .method /*0600014B*/ public hidebysig newslot abstract virtual 
            instance void  get_Pan([out] float64& pPan) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMAudioInputMixer::get_Pan

    .method /*0600014C*/ public hidebysig newslot abstract virtual 
            instance void  put_Loudness([in] int32 fLoudness) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAudioInputMixer::put_Loudness

    .method /*0600014D*/ public hidebysig newslot abstract virtual 
            instance void  get_Loudness([out] int32& pfLoudness) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAudioInputMixer::get_Loudness

    .method /*0600014E*/ public hidebysig newslot abstract virtual 
            instance void  put_Treble([in] float64 Treble) runtime managed internalcall
    // SIG: 20 01 01 0D
    {
    } // end of method IAMAudioInputMixer::put_Treble

    .method /*0600014F*/ public hidebysig newslot abstract virtual 
            instance void  get_Treble([out] float64& pTreble) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMAudioInputMixer::get_Treble

    .method /*06000150*/ public hidebysig newslot abstract virtual 
            instance void  get_TrebleRange([out] float64& pRange) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMAudioInputMixer::get_TrebleRange

    .method /*06000151*/ public hidebysig newslot abstract virtual 
            instance void  put_Bass([in] float64 Bass) runtime managed internalcall
    // SIG: 20 01 01 0D
    {
    } // end of method IAMAudioInputMixer::put_Bass

    .method /*06000152*/ public hidebysig newslot abstract virtual 
            instance void  get_Bass([out] float64& pBass) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMAudioInputMixer::get_Bass

    .method /*06000153*/ public hidebysig newslot abstract virtual 
            instance void  get_BassRange([out] float64& pRange) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMAudioInputMixer::get_BassRange

  } // end of class IAMAudioInputMixer

  .class /*02000063*/ interface public abstract auto ansi import IAMBufferNegotiation
  {
    .custom /*0C00008C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 45 44 37 31 41 30 2D 41 46 35 46   // ..$56ED71A0-AF5F
                                                                                                                                                                     2D 31 31 44 30 2D 42 33 46 30 2D 30 30 41 41 30   // -11D0-B3F0-00AA0
                                                                                                                                                                     30 33 37 36 31 43 35 00 00 )                      // 03761C5..
    .custom /*0C00008D:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000154*/ public hidebysig newslot abstract virtual 
            instance void  SuggestAllocatorProperties([in] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pprop) runtime managed internalcall
    // SIG: 20 01 01 10 11 58
    {
    } // end of method IAMBufferNegotiation::SuggestAllocatorProperties

    .method /*06000155*/ public hidebysig newslot abstract virtual 
            instance void  GetAllocatorProperties([out] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pprop) runtime managed internalcall
    // SIG: 20 01 01 10 11 58
    {
    } // end of method IAMBufferNegotiation::GetAllocatorProperties

  } // end of class IAMBufferNegotiation

  .class /*02000064*/ interface public abstract auto ansi import IAMAnalogVideoDecoder
  {
    .custom /*0C00008E:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00008F:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 35 30 2D 33 30 41 43   // ..$C6E13350-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .method /*06000156*/ public hidebysig newslot abstract virtual 
            instance void  get_AvailableTVFormats([out] int32& lAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoDecoder::get_AvailableTVFormats

    .method /*06000157*/ public hidebysig newslot abstract virtual 
            instance void  put_TVFormat([in] int32 lAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAnalogVideoDecoder::put_TVFormat

    .method /*06000158*/ public hidebysig newslot abstract virtual 
            instance void  get_TVFormat([out] int32& plAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoDecoder::get_TVFormat

    .method /*06000159*/ public hidebysig newslot abstract virtual 
            instance void  get_HorizontalLocked([out] int32& plLocked) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoDecoder::get_HorizontalLocked

    .method /*0600015A*/ public hidebysig newslot abstract virtual 
            instance void  put_VCRHorizontalLocking([in] int32 lVCRHorizontalLocking) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAnalogVideoDecoder::put_VCRHorizontalLocking

    .method /*0600015B*/ public hidebysig newslot abstract virtual 
            instance void  get_VCRHorizontalLocking([out] int32& plVCRHorizontalLocking) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoDecoder::get_VCRHorizontalLocking

    .method /*0600015C*/ public hidebysig newslot abstract virtual 
            instance void  get_NumberOfLines([out] int32& plNumberOfLines) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoDecoder::get_NumberOfLines

    .method /*0600015D*/ public hidebysig newslot abstract virtual 
            instance void  put_OutputEnable([in] int32 lOutputEnable) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAnalogVideoDecoder::put_OutputEnable

    .method /*0600015E*/ public hidebysig newslot abstract virtual 
            instance void  get_OutputEnable([out] int32& plOutputEnable) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoDecoder::get_OutputEnable

  } // end of class IAMAnalogVideoDecoder

  .class /*02000065*/ interface public abstract auto ansi import IAMVideoProcAmp
  {
    .custom /*0C000090:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 36 30 2D 33 30 41 43   // ..$C6E13360-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .custom /*0C000091:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600015F*/ public hidebysig newslot abstract virtual 
            instance void  GetRange([in] int32 Property,
                                    [out] int32& pMin,
                                    [out] int32& pMax,
                                    [out] int32& pSteppingDelta,
                                    [out] int32& pDefault,
                                    [out] int32& pCapsFlags) runtime managed internalcall
    // SIG: 20 06 01 08 10 08 10 08 10 08 10 08 10 08
    {
    } // end of method IAMVideoProcAmp::GetRange

    .method /*06000160*/ public hidebysig newslot abstract virtual 
            instance void  Set([in] int32 Property,
                               [in] int32 lValue,
                               [in] int32 Flags) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
    } // end of method IAMVideoProcAmp::Set

    .method /*06000161*/ public hidebysig newslot abstract virtual 
            instance void  Get([in] int32 Property,
                               [out] int32& lValue,
                               [out] int32& Flags) runtime managed internalcall
    // SIG: 20 03 01 08 10 08 10 08
    {
    } // end of method IAMVideoProcAmp::Get

  } // end of class IAMVideoProcAmp

  .class /*02000066*/ interface public abstract auto ansi import IAMAnalogVideoEncoder
  {
    .custom /*0C000092:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000093:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 42 30 2D 33 30 41 43   // ..$C6E133B0-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .method /*06000162*/ public hidebysig newslot abstract virtual 
            instance void  get_AvailableTVFormats([out] int32& lAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoEncoder::get_AvailableTVFormats

    .method /*06000163*/ public hidebysig newslot abstract virtual 
            instance void  put_TVFormat([in] int32 lAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAnalogVideoEncoder::put_TVFormat

    .method /*06000164*/ public hidebysig newslot abstract virtual 
            instance void  get_TVFormat([out] int32& plAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoEncoder::get_TVFormat

    .method /*06000165*/ public hidebysig newslot abstract virtual 
            instance void  put_CopyProtection([in] int32 lVideoCopyProtection) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAnalogVideoEncoder::put_CopyProtection

    .method /*06000166*/ public hidebysig newslot abstract virtual 
            instance void  get_CopyProtection([out] int32& lVideoCopyProtection) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoEncoder::get_CopyProtection

    .method /*06000167*/ public hidebysig newslot abstract virtual 
            instance void  put_CCEnable([in] int32 lCCEnable) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMAnalogVideoEncoder::put_CCEnable

    .method /*06000168*/ public hidebysig newslot abstract virtual 
            instance void  get_CCEnable([out] int32& lCCEnable) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMAnalogVideoEncoder::get_CCEnable

  } // end of class IAMAnalogVideoEncoder

  .class /*02000067*/ interface public abstract auto ansi import IAMCameraControl
  {
    .custom /*0C000094:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000095:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 37 30 2D 33 30 41 43   // ..$C6E13370-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .method /*06000169*/ public hidebysig newslot abstract virtual 
            instance void  GetRange([in] int32 Property,
                                    [out] int32& pMin,
                                    [out] int32& pMax,
                                    [out] int32& pSteppingDelta,
                                    [out] int32& pDefault,
                                    [out] int32& pCapsFlags) runtime managed internalcall
    // SIG: 20 06 01 08 10 08 10 08 10 08 10 08 10 08
    {
    } // end of method IAMCameraControl::GetRange

    .method /*0600016A*/ public hidebysig newslot abstract virtual 
            instance void  Set([in] int32 Property,
                               [in] int32 lValue,
                               [in] int32 Flags) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
    } // end of method IAMCameraControl::Set

    .method /*0600016B*/ public hidebysig newslot abstract virtual 
            instance void  Get([in] int32 Property,
                               [out] int32& lValue,
                               [out] int32& Flags) runtime managed internalcall
    // SIG: 20 03 01 08 10 08 10 08
    {
    } // end of method IAMCameraControl::Get

  } // end of class IAMCameraControl

  .class /*02000068*/ interface public abstract auto ansi import IAMCrossbar
  {
    .custom /*0C000096:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000097:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 36 45 31 33 33 38 30 2D 33 30 41 43   // ..$C6E13380-30AC
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .method /*0600016C*/ public hidebysig newslot abstract virtual 
            instance void  get_PinCounts([out] int32& OutputPinCount,
                                         [out] int32& InputPinCount) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 08
    {
    } // end of method IAMCrossbar::get_PinCounts

    .method /*0600016D*/ public hidebysig newslot abstract virtual 
            instance void  CanRoute([in] int32 OutputPinIndex,
                                    [in] int32 InputPinIndex) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMCrossbar::CanRoute

    .method /*0600016E*/ public hidebysig newslot abstract virtual 
            instance void  Route([in] int32 OutputPinIndex,
                                 [in] int32 InputPinIndex) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMCrossbar::Route

    .method /*0600016F*/ public hidebysig newslot abstract virtual 
            instance void  get_IsRoutedTo([in] int32 OutputPinIndex,
                                          [out] int32& InputPinIndex) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMCrossbar::get_IsRoutedTo

    .method /*06000170*/ public hidebysig newslot abstract virtual 
            instance void  get_CrossbarPinInfo([in] int32 IsInputPin,
                                               [in] int32 PinIndex,
                                               [out] int32& PinIndexRelated,
                                               [out] int32& PhysicalType) runtime managed internalcall
    // SIG: 20 04 01 08 08 10 08 10 08
    {
    } // end of method IAMCrossbar::get_CrossbarPinInfo

  } // end of class IAMCrossbar

  .class /*02000069*/ interface public abstract auto ansi import IAMTuner
  {
    .custom /*0C000098:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 31 31 41 38 37 36 31 2D 30 33 41 43   // ..$211A8761-03AC
                                                                                                                                                                     2D 31 31 44 31 2D 38 44 31 33 2D 30 30 41 41 30   // -11D1-8D13-00AA0
                                                                                                                                                                     30 42 44 38 33 33 39 00 00 )                      // 0BD8339..
    .custom /*0C000099:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00009A:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*06000171*/ public hidebysig newslot abstract virtual 
            instance void  put_Channel([in] int32 lChannel,
                                       [in] int32 lVideoSubChannel,
                                       [in] int32 lAudioSubChannel) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
    } // end of method IAMTuner::put_Channel

    .method /*06000172*/ public hidebysig newslot abstract virtual 
            instance void  get_Channel([out] int32& plChannel,
                                       [out] int32& plVideoSubChannel,
                                       [out] int32& plAudioSubChannel) runtime managed internalcall
    // SIG: 20 03 01 10 08 10 08 10 08
    {
    } // end of method IAMTuner::get_Channel

    .method /*06000173*/ public hidebysig newslot abstract virtual 
            instance void  ChannelMinMax([out] int32& lChannelMin,
                                         [out] int32& lChannelMax) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 08
    {
    } // end of method IAMTuner::ChannelMinMax

    .method /*06000174*/ public hidebysig newslot abstract virtual 
            instance void  put_CountryCode([in] int32 lCountryCode) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTuner::put_CountryCode

    .method /*06000175*/ public hidebysig newslot abstract virtual 
            instance void  get_CountryCode([out] int32& plCountryCode) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTuner::get_CountryCode

    .method /*06000176*/ public hidebysig newslot abstract virtual 
            instance void  put_TuningSpace([in] int32 lTuningSpace) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTuner::put_TuningSpace

    .method /*06000177*/ public hidebysig newslot abstract virtual 
            instance void  get_TuningSpace([out] int32& plTuningSpace) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTuner::get_TuningSpace

    .method /*06000178*/ public hidebysig newslot abstract virtual 
            instance void  Logon([in] native int hCurrentUser) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IAMTuner::Logon

    .method /*06000179*/ public hidebysig newslot abstract virtual 
            instance void  Logout() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMTuner::Logout

    .method /*0600017A*/ public hidebysig newslot abstract virtual 
            instance void  SignalPresent([out] int32& plSignalStrength) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTuner::SignalPresent

    .method /*0600017B*/ public hidebysig newslot abstract virtual 
            instance void  put_Mode([in] valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ lMode) runtime managed internalcall
    // SIG: 20 01 01 11 81 A8
    {
    } // end of method IAMTuner::put_Mode

    .method /*0600017C*/ public hidebysig newslot abstract virtual 
            instance void  get_Mode([out] valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */& plMode) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 A8
    {
    } // end of method IAMTuner::get_Mode

    .method /*0600017D*/ public hidebysig newslot abstract virtual 
            instance void  GetAvailableModes([out] int32& plModes) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTuner::GetAvailableModes

    .method /*0600017E*/ public hidebysig newslot abstract virtual 
            instance void  RegisterNotificationCallBack([in] class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify,
                                                        [in] int32 lEvents) runtime managed internalcall
    // SIG: 20 02 01 12 81 AC 08
    {
    } // end of method IAMTuner::RegisterNotificationCallBack

    .method /*0600017F*/ public hidebysig newslot abstract virtual 
            instance void  UnRegisterNotificationCallBack([in] class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify) runtime managed internalcall
    // SIG: 20 01 01 12 81 AC
    {
    } // end of method IAMTuner::UnRegisterNotificationCallBack

  } // end of class IAMTuner

  .class /*0200006A*/ public auto ansi sealed tagAMTunerModeType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000CD*/ public specialname rtspecialname int32 value__
    .field /*040000CE*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ AMTUNER_MODE_DEFAULT = int32(0x00000000)
    .field /*040000CF*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ AMTUNER_MODE_TV = int32(0x00000001)
    .field /*040000D0*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ AMTUNER_MODE_FM_RADIO = int32(0x00000002)
    .field /*040000D1*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ AMTUNER_MODE_AM_RADIO = int32(0x00000004)
    .field /*040000D2*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ AMTUNER_MODE_DSS = int32(0x00000008)
  } // end of class tagAMTunerModeType

  .class /*0200006B*/ interface public abstract auto ansi import IAMTunerNotification
  {
    .custom /*0C00009B:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00009C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 31 31 41 38 37 36 30 2D 30 33 41 43   // ..$211A8760-03AC
                                                                                                                                                                     2D 31 31 44 31 2D 38 44 31 33 2D 30 30 41 41 30   // -11D1-8D13-00AA0
                                                                                                                                                                     30 42 44 38 33 33 39 00 00 )                      // 0BD8339..
    .method /*06000180*/ public hidebysig newslot abstract virtual 
            instance void  OnEvent([in] valuetype MSR.LST.MDShow.tagAMTunerEventType/* 0200006C */ Event) runtime managed internalcall
    // SIG: 20 01 01 11 81 B0
    {
    } // end of method IAMTunerNotification::OnEvent

  } // end of class IAMTunerNotification

  .class /*0200006C*/ public auto ansi sealed tagAMTunerEventType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000D3*/ public specialname rtspecialname int32 value__
    .field /*040000D4*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerEventType/* 0200006C */ AMTUNER_EVENT_CHANGED = int32(0x00000001)
  } // end of class tagAMTunerEventType

  .class /*0200006D*/ interface public abstract auto ansi import IAMTVTuner
         implements MSR.LST.MDShow.IAMTuner/* 02000069 */
  {
    .custom /*0C00009D:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 31 31 41 38 37 36 36 2D 30 33 41 43   // ..$211A8766-03AC
                                                                                                                                                                     2D 31 31 44 31 2D 38 44 31 33 2D 30 30 41 41 30   // -11D1-8D13-00AA0
                                                                                                                                                                     30 42 44 38 33 33 39 00 00 )                      // 0BD8339..
    .custom /*0C00009E:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000181*/ public hidebysig newslot abstract virtual 
            instance void  put_Channel([in] int32 lChannel,
                                       [in] int32 lVideoSubChannel,
                                       [in] int32 lAudioSubChannel) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
    } // end of method IAMTVTuner::put_Channel

    .method /*06000182*/ public hidebysig newslot abstract virtual 
            instance void  get_Channel([out] int32& plChannel,
                                       [out] int32& plVideoSubChannel,
                                       [out] int32& plAudioSubChannel) runtime managed internalcall
    // SIG: 20 03 01 10 08 10 08 10 08
    {
    } // end of method IAMTVTuner::get_Channel

    .method /*06000183*/ public hidebysig newslot abstract virtual 
            instance void  ChannelMinMax([out] int32& lChannelMin,
                                         [out] int32& lChannelMax) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 08
    {
    } // end of method IAMTVTuner::ChannelMinMax

    .method /*06000184*/ public hidebysig newslot abstract virtual 
            instance void  put_CountryCode([in] int32 lCountryCode) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTVTuner::put_CountryCode

    .method /*06000185*/ public hidebysig newslot abstract virtual 
            instance void  get_CountryCode([out] int32& plCountryCode) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_CountryCode

    .method /*06000186*/ public hidebysig newslot abstract virtual 
            instance void  put_TuningSpace([in] int32 lTuningSpace) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTVTuner::put_TuningSpace

    .method /*06000187*/ public hidebysig newslot abstract virtual 
            instance void  get_TuningSpace([out] int32& plTuningSpace) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_TuningSpace

    .method /*06000188*/ public hidebysig newslot abstract virtual 
            instance void  Logon([in] native int hCurrentUser) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IAMTVTuner::Logon

    .method /*06000189*/ public hidebysig newslot abstract virtual 
            instance void  Logout() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMTVTuner::Logout

    .method /*0600018A*/ public hidebysig newslot abstract virtual 
            instance void  SignalPresent([out] int32& plSignalStrength) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::SignalPresent

    .method /*0600018B*/ public hidebysig newslot abstract virtual 
            instance void  put_Mode([in] valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ lMode) runtime managed internalcall
    // SIG: 20 01 01 11 81 A8
    {
    } // end of method IAMTVTuner::put_Mode

    .method /*0600018C*/ public hidebysig newslot abstract virtual 
            instance void  get_Mode([out] valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */& plMode) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 A8
    {
    } // end of method IAMTVTuner::get_Mode

    .method /*0600018D*/ public hidebysig newslot abstract virtual 
            instance void  GetAvailableModes([out] int32& plModes) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::GetAvailableModes

    .method /*0600018E*/ public hidebysig newslot abstract virtual 
            instance void  RegisterNotificationCallBack([in] class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify,
                                                        [in] int32 lEvents) runtime managed internalcall
    // SIG: 20 02 01 12 81 AC 08
    {
    } // end of method IAMTVTuner::RegisterNotificationCallBack

    .method /*0600018F*/ public hidebysig newslot abstract virtual 
            instance void  UnRegisterNotificationCallBack([in] class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify) runtime managed internalcall
    // SIG: 20 01 01 12 81 AC
    {
    } // end of method IAMTVTuner::UnRegisterNotificationCallBack

    .method /*06000190*/ public hidebysig newslot abstract virtual 
            instance void  get_AvailableTVFormats([out] int32& lAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_AvailableTVFormats

    .method /*06000191*/ public hidebysig newslot abstract virtual 
            instance void  get_TVFormat([out] int32& plAnalogVideoStandard) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_TVFormat

    .method /*06000192*/ public hidebysig newslot abstract virtual 
            instance void  AutoTune([in] int32 lChannel,
                                    [out] int32& plFoundSignal) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMTVTuner::AutoTune

    .method /*06000193*/ public hidebysig newslot abstract virtual 
            instance void  StoreAutoTune() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMTVTuner::StoreAutoTune

    .method /*06000194*/ public hidebysig newslot abstract virtual 
            instance void  get_NumInputConnections([out] int32& plNumInputConnections) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_NumInputConnections

    .method /*06000195*/ public hidebysig newslot abstract virtual 
            instance void  put_InputType([in] int32 lIndex,
                                         [in] valuetype MSR.LST.MDShow.tagTunerInputType/* 0200006E */ InputType) runtime managed internalcall
    // SIG: 20 02 01 08 11 81 B8
    {
    } // end of method IAMTVTuner::put_InputType

    .method /*06000196*/ public hidebysig newslot abstract virtual 
            instance void  get_InputType([in] int32 lIndex,
                                         [out] valuetype MSR.LST.MDShow.tagTunerInputType/* 0200006E */& pInputType) runtime managed internalcall
    // SIG: 20 02 01 08 10 11 81 B8
    {
    } // end of method IAMTVTuner::get_InputType

    .method /*06000197*/ public hidebysig newslot abstract virtual 
            instance void  put_ConnectInput([in] int32 lIndex) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTVTuner::put_ConnectInput

    .method /*06000198*/ public hidebysig newslot abstract virtual 
            instance void  get_ConnectInput([out] int32& plIndex) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_ConnectInput

    .method /*06000199*/ public hidebysig newslot abstract virtual 
            instance void  get_VideoFrequency([out] int32& lFreq) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_VideoFrequency

    .method /*0600019A*/ public hidebysig newslot abstract virtual 
            instance void  get_AudioFrequency([out] int32& lFreq) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVTuner::get_AudioFrequency

  } // end of class IAMTVTuner

  .class /*0200006E*/ public auto ansi sealed tagTunerInputType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000D5*/ public specialname rtspecialname int32 value__
    .field /*040000D6*/ public static literal valuetype MSR.LST.MDShow.tagTunerInputType/* 0200006E */ TunerInputCable = int32(0x00000000)
    .field /*040000D7*/ public static literal valuetype MSR.LST.MDShow.tagTunerInputType/* 0200006E */ TunerInputAntenna = int32(0x00000001)
  } // end of class tagTunerInputType

  .class /*0200006F*/ interface public abstract auto ansi import IKsPropertySet
  {
    .custom /*0C00009F:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 31 45 46 41 43 33 30 2D 35 31 35 43   // ..$31EFAC30-515C
                                                                                                                                                                     2D 31 31 44 30 2D 41 39 41 41 2D 30 30 41 41 30   // -11D0-A9AA-00AA0
                                                                                                                                                                     30 36 31 42 45 39 33 00 00 )                      // 061BE93..
    .custom /*0C0000A0:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600019B*/ public hidebysig newslot abstract virtual 
            instance void  RemoteSet([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& guidPropSet,
                                     [in] unsigned int32 dwPropID,
                                     [in] unsigned int8& pInstanceData,
                                     [in] unsigned int32 cbInstanceData,
                                     [in] unsigned int8& pPropData,
                                     [in] unsigned int32 cbPropData) runtime managed internalcall
    // SIG: 20 06 01 10 11 11 09 10 05 09 10 05 09
    {
    } // end of method IKsPropertySet::RemoteSet

    .method /*0600019C*/ public hidebysig newslot abstract virtual 
            instance void  RemoteGet([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& guidPropSet,
                                     [in] unsigned int32 dwPropID,
                                     [in] unsigned int8& pInstanceData,
                                     [in] unsigned int32 cbInstanceData,
                                     [out] unsigned int8& pPropData,
                                     [in] unsigned int32 cbPropData,
                                     [out] unsigned int32& pcbReturned) runtime managed internalcall
    // SIG: 20 07 01 10 11 11 09 10 05 09 10 05 09 10 09
    {
    } // end of method IKsPropertySet::RemoteGet

    .method /*0600019D*/ public hidebysig newslot abstract virtual 
            instance void  QuerySupported([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& guidPropSet,
                                          [in] unsigned int32 dwPropID,
                                          [out] unsigned int32& pTypeSupport) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 09 10 09
    {
    } // end of method IKsPropertySet::QuerySupported

  } // end of class IKsPropertySet

  .class /*02000070*/ interface public abstract auto ansi import IAMPhysicalPinInfo
  {
    .custom /*0C0000A1:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000A2:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 46 39 33 38 43 39 39 31 2D 33 30 32 39   // ..$F938C991-3029
                                                                                                                                                                     2D 31 31 43 46 2D 38 43 34 34 2D 30 30 41 41 30   // -11CF-8C44-00AA0
                                                                                                                                                                     30 36 42 36 38 31 34 00 00 )                      // 06B6814..
    .method /*0600019E*/ public hidebysig newslot abstract virtual 
            instance void  GetPhysicalType([out] int32& pType,
                                           [out] string&  marshal( lpwstr) ppszType) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 0E
    {
    } // end of method IAMPhysicalPinInfo::GetPhysicalType

  } // end of class IAMPhysicalPinInfo

  .class /*02000071*/ interface public abstract auto ansi import IAMExtDevice
  {
    .custom /*0C0000A3:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000A4:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 42 35 37 33 30 41 39 30 2D 31 41 32 43   // ..$B5730A90-1A2C
                                                                                                                                                                     2D 31 31 43 46 2D 38 43 32 33 2D 30 30 41 41 30   // -11CF-8C23-00AA0
                                                                                                                                                                     30 36 42 36 38 31 34 00 00 )                      // 06B6814..
    .method /*0600019F*/ public hidebysig newslot abstract virtual 
            instance void  GetCapability([in] int32 Capability,
                                         [out] int32& pValue,
                                         [out] float64& pdblValue) runtime managed internalcall
    // SIG: 20 03 01 08 10 08 10 0D
    {
    } // end of method IAMExtDevice::GetCapability

    .method /*060001A0*/ public hidebysig newslot abstract virtual 
            instance void  get_ExternalDeviceID([out] string&  marshal( lpwstr) ppszData) runtime managed internalcall
    // SIG: 20 01 01 10 0E
    {
    } // end of method IAMExtDevice::get_ExternalDeviceID

    .method /*060001A1*/ public hidebysig newslot abstract virtual 
            instance void  get_ExternalDeviceVersion([out] string&  marshal( lpwstr) ppszData) runtime managed internalcall
    // SIG: 20 01 01 10 0E
    {
    } // end of method IAMExtDevice::get_ExternalDeviceVersion

    .method /*060001A2*/ public hidebysig newslot abstract virtual 
            instance void  put_DevicePower([in] int32 PowerMode) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMExtDevice::put_DevicePower

    .method /*060001A3*/ public hidebysig newslot abstract virtual 
            instance void  get_DevicePower([out] int32& pPowerMode) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMExtDevice::get_DevicePower

    .method /*060001A4*/ public hidebysig newslot abstract virtual 
            instance void  Calibrate([in] unsigned int32 hEvent,
                                     [in] int32 mode,
                                     [out] int32& pStatus) runtime managed internalcall
    // SIG: 20 03 01 09 08 10 08
    {
      .param [1]/*080002BE*/ 
      .custom /*0C000143:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IAMExtDevice::Calibrate

    .method /*060001A5*/ public hidebysig newslot abstract virtual 
            instance void  put_DevicePort([in] int32 DevicePort) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMExtDevice::put_DevicePort

    .method /*060001A6*/ public hidebysig newslot abstract virtual 
            instance void  get_DevicePort([out] int32& pDevicePort) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMExtDevice::get_DevicePort

  } // end of class IAMExtDevice

  .class /*02000072*/ interface public abstract auto ansi import IAMExtTransport
  {
    .custom /*0C0000A5:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000A6:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 41 30 33 43 44 35 46 30 2D 33 30 34 35   // ..$A03CD5F0-3045
                                                                                                                                                                     2D 31 31 43 46 2D 38 43 34 34 2D 30 30 41 41 30   // -11CF-8C44-00AA0
                                                                                                                                                                     30 36 42 36 38 31 34 00 00 )                      // 06B6814..
    .method /*060001A7*/ public hidebysig newslot abstract virtual 
            instance void  GetCapability([in] int32 Capability,
                                         [out] int32& pValue,
                                         [out] float64& pdblValue) runtime managed internalcall
    // SIG: 20 03 01 08 10 08 10 0D
    {
    } // end of method IAMExtTransport::GetCapability

    .method /*060001A8*/ public hidebysig newslot abstract virtual 
            instance void  put_MediaState([in] int32 State) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMExtTransport::put_MediaState

    .method /*060001A9*/ public hidebysig newslot abstract virtual 
            instance void  get_MediaState([out] int32& pState) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMExtTransport::get_MediaState

    .method /*060001AA*/ public hidebysig newslot abstract virtual 
            instance void  put_LocalControl([in] int32 State) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMExtTransport::put_LocalControl

    .method /*060001AB*/ public hidebysig newslot abstract virtual 
            instance void  get_LocalControl([out] int32& pState) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMExtTransport::get_LocalControl

    .method /*060001AC*/ public hidebysig newslot abstract virtual 
            instance void  GetStatus([in] int32 StatusItem,
                                     [out] int32& pValue) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMExtTransport::GetStatus

    .method /*060001AD*/ public hidebysig newslot abstract virtual 
            instance void  GetTransportBasicParameters([in] int32 Param,
                                                       [out] int32& pValue,
                                                       [out] string&  marshal( lpwstr) ppszData) runtime managed internalcall
    // SIG: 20 03 01 08 10 08 10 0E
    {
    } // end of method IAMExtTransport::GetTransportBasicParameters

    .method /*060001AE*/ public hidebysig newslot abstract virtual 
            instance void  SetTransportBasicParameters([in] int32 Param,
                                                       [in] int32 Value,
                                                       [in] string  marshal( lpwstr) pszData) runtime managed internalcall
    // SIG: 20 03 01 08 08 0E
    {
    } // end of method IAMExtTransport::SetTransportBasicParameters

    .method /*060001AF*/ public hidebysig newslot abstract virtual 
            instance void  GetTransportVideoParameters([in] int32 Param,
                                                       [out] int32& pValue) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMExtTransport::GetTransportVideoParameters

    .method /*060001B0*/ public hidebysig newslot abstract virtual 
            instance void  SetTransportVideoParameters([in] int32 Param,
                                                       [in] int32 Value) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMExtTransport::SetTransportVideoParameters

    .method /*060001B1*/ public hidebysig newslot abstract virtual 
            instance void  GetTransportAudioParameters([in] int32 Param,
                                                       [out] int32& pValue) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMExtTransport::GetTransportAudioParameters

    .method /*060001B2*/ public hidebysig newslot abstract virtual 
            instance void  SetTransportAudioParameters([in] int32 Param,
                                                       [in] int32 Value) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMExtTransport::SetTransportAudioParameters

    .method /*060001B3*/ public hidebysig newslot abstract virtual 
            instance void  put_Mode([in] int32 mode) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMExtTransport::put_Mode

    .method /*060001B4*/ public hidebysig newslot abstract virtual 
            instance void  get_Mode([out] int32& pMode) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMExtTransport::get_Mode

    .method /*060001B5*/ public hidebysig newslot abstract virtual 
            instance void  put_Rate([in] float64 dblRate) runtime managed internalcall
    // SIG: 20 01 01 0D
    {
    } // end of method IAMExtTransport::put_Rate

    .method /*060001B6*/ public hidebysig newslot abstract virtual 
            instance void  get_Rate([out] float64& pdblRate) runtime managed internalcall
    // SIG: 20 01 01 10 0D
    {
    } // end of method IAMExtTransport::get_Rate

    .method /*060001B7*/ public hidebysig newslot abstract virtual 
            instance void  GetChase([out] int32& pEnabled,
                                    [out] int32& pOffset,
                                    [out] unsigned int32& phEvent) runtime managed internalcall
    // SIG: 20 03 01 10 08 10 08 10 09
    {
      .param [3]/*080002E0*/ 
      .custom /*0C000144:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IAMExtTransport::GetChase

    .method /*060001B8*/ public hidebysig newslot abstract virtual 
            instance void  SetChase([in] int32 Enable,
                                    [in] int32 Offset,
                                    [in] unsigned int32 hEvent) runtime managed internalcall
    // SIG: 20 03 01 08 08 09
    {
      .param [3]/*080002E3*/ 
      .custom /*0C000145:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IAMExtTransport::SetChase

    .method /*060001B9*/ public hidebysig newslot abstract virtual 
            instance void  GetBump([out] int32& pSpeed,
                                   [out] int32& pDuration) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 08
    {
    } // end of method IAMExtTransport::GetBump

    .method /*060001BA*/ public hidebysig newslot abstract virtual 
            instance void  SetBump([in] int32 Speed,
                                   [in] int32 Duration) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMExtTransport::SetBump

    .method /*060001BB*/ public hidebysig newslot abstract virtual 
            instance void  get_AntiClogControl([out] int32& pEnabled) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMExtTransport::get_AntiClogControl

    .method /*060001BC*/ public hidebysig newslot abstract virtual 
            instance void  put_AntiClogControl([in] int32 Enable) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMExtTransport::put_AntiClogControl

    .method /*060001BD*/ public hidebysig newslot abstract virtual 
            instance void  GetEditPropertySet([in] int32 EditID,
                                              [out] int32& pState) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMExtTransport::GetEditPropertySet

    .method /*060001BE*/ public hidebysig newslot abstract virtual 
            instance void  SetEditPropertySet([in][out] int32& pEditID,
                                              [in] int32 State) runtime managed internalcall
    // SIG: 20 02 01 10 08 08
    {
    } // end of method IAMExtTransport::SetEditPropertySet

    .method /*060001BF*/ public hidebysig newslot abstract virtual 
            instance void  GetEditProperty([in] int32 EditID,
                                           [in] int32 Param,
                                           [out] int32& pValue) runtime managed internalcall
    // SIG: 20 03 01 08 08 10 08
    {
    } // end of method IAMExtTransport::GetEditProperty

    .method /*060001C0*/ public hidebysig newslot abstract virtual 
            instance void  SetEditProperty([in] int32 EditID,
                                           [in] int32 Param,
                                           [in] int32 Value) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
    } // end of method IAMExtTransport::SetEditProperty

    .method /*060001C1*/ public hidebysig newslot abstract virtual 
            instance void  get_EditStart([out] int32& pValue) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMExtTransport::get_EditStart

    .method /*060001C2*/ public hidebysig newslot abstract virtual 
            instance void  put_EditStart([in] int32 Value) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMExtTransport::put_EditStart

  } // end of class IAMExtTransport

  .class /*02000073*/ interface public abstract auto ansi import IAMTimecodeReader
  {
    .custom /*0C0000A7:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 42 34 39 36 43 45 31 2D 38 31 31 42   // ..$9B496CE1-811B
                                                                                                                                                                     2D 31 31 43 46 2D 38 43 37 37 2D 30 30 41 41 30   // -11CF-8C77-00AA0
                                                                                                                                                                     30 36 42 36 38 31 34 00 00 )                      // 06B6814..
    .custom /*0C0000A8:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060001C3*/ public hidebysig newslot abstract virtual 
            instance void  GetTCRMode([in] int32 Param,
                                      [out] int32& pValue) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMTimecodeReader::GetTCRMode

    .method /*060001C4*/ public hidebysig newslot abstract virtual 
            instance void  SetTCRMode([in] int32 Param,
                                      [in] int32 Value) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMTimecodeReader::SetTCRMode

    .method /*060001C5*/ public hidebysig newslot abstract virtual 
            instance void  put_VITCLine([in] int32 Line) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTimecodeReader::put_VITCLine

    .method /*060001C6*/ public hidebysig newslot abstract virtual 
            instance void  get_VITCLine([out] int32& pLine) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTimecodeReader::get_VITCLine

    .method /*060001C7*/ public hidebysig newslot abstract virtual 
            instance void  GetTimecode([out] valuetype MSR.LST.MDShow.tagTIMECODE_SAMPLE/* 02000074 */& pTimecodeSample) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 D0
    {
    } // end of method IAMTimecodeReader::GetTimecode

  } // end of class IAMTimecodeReader

  .class /*02000074*/ public sequential ansi sealed beforefieldinit tagTIMECODE_SAMPLE
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .field /*040000D8*/ public int64 qwTick
    .field /*040000D9*/ public valuetype MSR.LST.MDShow.tagTIMECODE/* 02000075 */ timecode
    .field /*040000DA*/ public unsigned int32 dwUser
    .field /*040000DB*/ public unsigned int32 dwFlags
  } // end of class tagTIMECODE_SAMPLE

  .class /*02000075*/ public sequential ansi sealed beforefieldinit tagTIMECODE
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000DC*/ public unsigned int16 wFrameRate
    .field /*040000DD*/ public unsigned int16 wFrameFract
    .field /*040000DE*/ public unsigned int32 dwFrames
  } // end of class tagTIMECODE

  .class /*02000076*/ interface public abstract auto ansi import IAMTimecodeGenerator
  {
    .custom /*0C0000A9:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 42 34 39 36 43 45 30 2D 38 31 31 42   // ..$9B496CE0-811B
                                                                                                                                                                     2D 31 31 43 46 2D 38 43 37 37 2D 30 30 41 41 30   // -11CF-8C77-00AA0
                                                                                                                                                                     30 36 42 36 38 31 34 00 00 )                      // 06B6814..
    .custom /*0C0000AA:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060001C8*/ public hidebysig newslot abstract virtual 
            instance void  GetTCGMode([in] int32 Param,
                                      [out] int32& pValue) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMTimecodeGenerator::GetTCGMode

    .method /*060001C9*/ public hidebysig newslot abstract virtual 
            instance void  SetTCGMode([in] int32 Param,
                                      [in] int32 Value) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMTimecodeGenerator::SetTCGMode

    .method /*060001CA*/ public hidebysig newslot abstract virtual 
            instance void  put_VITCLine([in] int32 Line) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTimecodeGenerator::put_VITCLine

    .method /*060001CB*/ public hidebysig newslot abstract virtual 
            instance void  get_VITCLine([out] int32& pLine) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTimecodeGenerator::get_VITCLine

    .method /*060001CC*/ public hidebysig newslot abstract virtual 
            instance void  SetTimecode([in] valuetype MSR.LST.MDShow.tagTIMECODE_SAMPLE/* 02000074 */& pTimecodeSample) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 D0
    {
    } // end of method IAMTimecodeGenerator::SetTimecode

    .method /*060001CD*/ public hidebysig newslot abstract virtual 
            instance void  GetTimecode([out] valuetype MSR.LST.MDShow.tagTIMECODE_SAMPLE/* 02000074 */& pTimecodeSample) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 D0
    {
    } // end of method IAMTimecodeGenerator::GetTimecode

  } // end of class IAMTimecodeGenerator

  .class /*02000077*/ interface public abstract auto ansi import IAMTimecodeDisplay
  {
    .custom /*0C0000AB:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000AC:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 42 34 39 36 43 45 32 2D 38 31 31 42   // ..$9B496CE2-811B
                                                                                                                                                                     2D 31 31 43 46 2D 38 43 37 37 2D 30 30 41 41 30   // -11CF-8C77-00AA0
                                                                                                                                                                     30 36 42 36 38 31 34 00 00 )                      // 06B6814..
    .method /*060001CE*/ public hidebysig newslot abstract virtual 
            instance void  GetTCDisplayEnable([out] int32& pState) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTimecodeDisplay::GetTCDisplayEnable

    .method /*060001CF*/ public hidebysig newslot abstract virtual 
            instance void  SetTCDisplayEnable([in] int32 State) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTimecodeDisplay::SetTCDisplayEnable

    .method /*060001D0*/ public hidebysig newslot abstract virtual 
            instance void  GetTCDisplay([in] int32 Param,
                                        [out] int32& pValue) runtime managed internalcall
    // SIG: 20 02 01 08 10 08
    {
    } // end of method IAMTimecodeDisplay::GetTCDisplay

    .method /*060001D1*/ public hidebysig newslot abstract virtual 
            instance void  SetTCDisplay([in] int32 Param,
                                        [in] int32 Value) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IAMTimecodeDisplay::SetTCDisplay

  } // end of class IAMTimecodeDisplay

  .class /*02000078*/ interface public abstract auto ansi import IDrawVideoImage
  {
    .custom /*0C0000AF:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000B0:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 34 38 45 46 42 31 32 30 2D 41 42 34 39   // ..$48EFB120-AB49
                                                                                                                                                                     2D 31 31 44 32 2D 41 45 44 32 2D 30 30 41 30 43   // -11D2-AED2-00A0C
                                                                                                                                                                     39 39 35 45 38 44 35 00 00 )                      // 995E8D5..
    .method /*060001D2*/ public hidebysig newslot abstract virtual 
            instance void  DrawVideoImageBegin() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IDrawVideoImage::DrawVideoImageBegin

    .method /*060001D3*/ public hidebysig newslot abstract virtual 
            instance void  DrawVideoImageEnd() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IDrawVideoImage::DrawVideoImageEnd

    .method /*060001D4*/ public hidebysig newslot abstract virtual 
            instance void  DrawVideoImageDraw([in] valuetype MSR.LST.MDShow._RemotableHandle/* 0200004C */& hdc,
                                              [in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& lprcSrc,
                                              [in] valuetype MSR.LST.MDShow.tagRECT/* 0200004E */& lprcDst) runtime managed internalcall
    // SIG: 20 03 01 10 11 81 30 10 11 81 38 10 11 81 38
    {
      .param [1]/*0800030B*/ 
      .custom /*0C000146:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 16 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 77 69 72 65 48 44 43 00 00 )                // w.wireHDC..
    } // end of method IDrawVideoImage::DrawVideoImageDraw

  } // end of class IDrawVideoImage

  .class /*02000079*/ interface public abstract auto ansi import IDecimateVideoImage
  {
    .custom /*0C0000B1:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 45 35 45 41 33 45 30 2D 45 39 32 34   // ..$2E5EA3E0-E924
                                                                                                                                                                     2D 31 31 44 32 2D 42 36 44 41 2D 30 30 41 30 43   // -11D2-B6DA-00A0C
                                                                                                                                                                     39 39 35 45 38 44 46 00 00 )                      // 995E8DF..
    .custom /*0C0000B2:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060001D5*/ public hidebysig newslot abstract virtual 
            instance void  SetDecimationImageSize([in] int32 lWidth,
                                                  [in] int32 lHeight) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IDecimateVideoImage::SetDecimationImageSize

    .method /*060001D6*/ public hidebysig newslot abstract virtual 
            instance void  ResetDecimationImageSize() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IDecimateVideoImage::ResetDecimationImageSize

  } // end of class IDecimateVideoImage

  .class /*0200007A*/ interface public abstract auto ansi import IAMVideoDecimationProperties
  {
    .custom /*0C0000B3:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000B4:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 30 44 33 32 39 33 30 2D 31 33 44 41   // ..$60D32930-13DA
                                                                                                                                                                     2D 31 31 44 33 2D 39 45 43 36 2D 43 34 46 43 41   // -11D3-9EC6-C4FCA
                                                                                                                                                                     45 46 35 43 37 42 45 00 00 )                      // EF5C7BE..
    .method /*060001D7*/ public hidebysig newslot abstract virtual 
            instance void  QueryDecimationUsage([out] valuetype MSR.LST.MDShow._DECIMATION_USAGE/* 0200007B */& lpUsage) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 EC
    {
    } // end of method IAMVideoDecimationProperties::QueryDecimationUsage

    .method /*060001D8*/ public hidebysig newslot abstract virtual 
            instance void  SetDecimationUsage([in] valuetype MSR.LST.MDShow._DECIMATION_USAGE/* 0200007B */ Usage) runtime managed internalcall
    // SIG: 20 01 01 11 81 EC
    {
    } // end of method IAMVideoDecimationProperties::SetDecimationUsage

  } // end of class IAMVideoDecimationProperties

  .class /*0200007B*/ public auto ansi sealed _DECIMATION_USAGE
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000DF*/ public specialname rtspecialname int32 value__
    .field /*040000E0*/ public static literal valuetype MSR.LST.MDShow._DECIMATION_USAGE/* 0200007B */ DECIMATION_LEGACY = int32(0x00000000)
    .field /*040000E1*/ public static literal valuetype MSR.LST.MDShow._DECIMATION_USAGE/* 0200007B */ DECIMATION_USE_DECODER_ONLY = int32(0x00000001)
    .field /*040000E2*/ public static literal valuetype MSR.LST.MDShow._DECIMATION_USAGE/* 0200007B */ DECIMATION_USE_VIDEOPORT_ONLY = int32(0x00000002)
    .field /*040000E3*/ public static literal valuetype MSR.LST.MDShow._DECIMATION_USAGE/* 0200007B */ DECIMATION_USE_OVERLAY_ONLY = int32(0x00000003)
    .field /*040000E4*/ public static literal valuetype MSR.LST.MDShow._DECIMATION_USAGE/* 0200007B */ DECIMATION_DEFAULT = int32(0x00000004)
  } // end of class _DECIMATION_USAGE

  .class /*0200007C*/ interface public abstract auto ansi import IAMLatency
  {
    .custom /*0C0000B5:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 32 45 41 39 33 42 41 2D 45 43 36 32   // ..$62EA93BA-EC62
                                                                                                                                                                     2D 31 31 44 32 2D 42 37 37 30 2D 30 30 43 30 34   // -11D2-B770-00C04
                                                                                                                                                                     46 42 36 42 44 33 44 00 00 )                      // FB6BD3D..
    .custom /*0C0000B6:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060001D9*/ public hidebysig newslot abstract virtual 
            instance void  GetLatency([in] int64& prtLatency) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IAMLatency::GetLatency

  } // end of class IAMLatency

  .class /*0200007D*/ interface public abstract auto ansi import IAMPushSource
         implements MSR.LST.MDShow.IAMLatency/* 0200007C */
  {
    .custom /*0C0000B7:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000B8:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 46 31 38 35 46 45 37 36 2D 45 36 34 45   // ..$F185FE76-E64E
                                                                                                                                                                     2D 31 31 44 32 2D 42 37 36 45 2D 30 30 43 30 34   // -11D2-B76E-00C04
                                                                                                                                                                     46 42 36 42 44 33 44 00 00 )                      // FB6BD3D..
    .method /*060001DA*/ public hidebysig newslot abstract virtual 
            instance void  GetLatency([in] int64& prtLatency) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IAMPushSource::GetLatency

    .method /*060001DB*/ public hidebysig newslot abstract virtual 
            instance void  GetPushSourceFlags([out] unsigned int32& pFlags) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IAMPushSource::GetPushSourceFlags

    .method /*060001DC*/ public hidebysig newslot abstract virtual 
            instance void  SetPushSourceFlags([in] unsigned int32 Flags) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IAMPushSource::SetPushSourceFlags

    .method /*060001DD*/ public hidebysig newslot abstract virtual 
            instance void  SetStreamOffset([in] int64 rtOffset) runtime managed internalcall
    // SIG: 20 01 01 0A
    {
    } // end of method IAMPushSource::SetStreamOffset

    .method /*060001DE*/ public hidebysig newslot abstract virtual 
            instance void  GetStreamOffset([out] int64& prtOffset) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IAMPushSource::GetStreamOffset

    .method /*060001DF*/ public hidebysig newslot abstract virtual 
            instance void  GetMaxStreamOffset([out] int64& prtMaxOffset) runtime managed internalcall
    // SIG: 20 01 01 10 0A
    {
    } // end of method IAMPushSource::GetMaxStreamOffset

    .method /*060001E0*/ public hidebysig newslot abstract virtual 
            instance void  SetMaxStreamOffset([in] int64 rtMaxOffset) runtime managed internalcall
    // SIG: 20 01 01 0A
    {
    } // end of method IAMPushSource::SetMaxStreamOffset

  } // end of class IAMPushSource

  .class /*0200007E*/ interface public abstract auto ansi import IAMAudioRendererStats
  {
    .custom /*0C0000BA:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000BB:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 32 33 32 30 43 42 32 2D 44 34 31 41   // ..$22320CB2-D41A
                                                                                                                                                                     2D 31 31 44 32 2D 42 46 37 43 2D 44 37 43 42 39   // -11D2-BF7C-D7CB9
                                                                                                                                                                     44 46 30 42 46 39 33 00 00 )                      // DF0BF93..
    .method /*060001E1*/ public hidebysig newslot abstract virtual 
            instance void  GetStatParam([in] unsigned int32 dwParam,
                                        [out] unsigned int32& pdwParam1,
                                        [out] unsigned int32& pdwParam2) runtime managed internalcall
    // SIG: 20 03 01 09 10 09 10 09
    {
    } // end of method IAMAudioRendererStats::GetStatParam

  } // end of class IAMAudioRendererStats

  .class /*0200007F*/ interface public abstract auto ansi import IAMGraphStreams
  {
    .custom /*0C0000BD:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000BE:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 33 32 31 30 35 46 41 2D 30 37 32 45   // ..$632105FA-072E
                                                                                                                                                                     2D 31 31 44 33 2D 38 41 46 39 2D 30 30 43 30 34   // -11D3-8AF9-00C04
                                                                                                                                                                     46 42 36 42 44 33 44 00 00 )                      // FB6BD3D..
    .custom /*0C0000BF:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*060001E2*/ public hidebysig newslot abstract virtual 
            instance void  FindUpstreamInterface([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                                 [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& riid,
                                                 [out] native int& ppvInterface,
                                                 [in] unsigned int32 dwFlags) runtime managed internalcall
    // SIG: 20 04 01 12 78 10 11 11 10 18 09
    {
    } // end of method IAMGraphStreams::FindUpstreamInterface

    .method /*060001E3*/ public hidebysig newslot abstract virtual 
            instance void  SyncUsingStreamOffset([in] int32 bUseStreamOffset) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMGraphStreams::SyncUsingStreamOffset

    .method /*060001E4*/ public hidebysig newslot abstract virtual 
            instance void  SetMaxGraphLatency([in] int64 rtMaxGraphLatency) runtime managed internalcall
    // SIG: 20 01 01 0A
    {
    } // end of method IAMGraphStreams::SetMaxGraphLatency

  } // end of class IAMGraphStreams

  .class /*02000080*/ interface public abstract auto ansi import IAMOverlayFX
  {
    .custom /*0C0000C0:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000C1:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 32 46 41 45 32 35 30 2D 37 45 36 35   // ..$62FAE250-7E65
                                                                                                                                                                     2D 34 34 36 30 2D 42 46 43 39 2D 36 33 39 38 42   // -4460-BFC9-6398B
                                                                                                                                                                     33 32 32 30 37 33 43 00 00 )                      // 322073C..
    .method /*060001E5*/ public hidebysig newslot abstract virtual 
            instance void  QueryOverlayFXCaps([out] unsigned int32& lpdwOverlayFXCaps) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IAMOverlayFX::QueryOverlayFXCaps

    .method /*060001E6*/ public hidebysig newslot abstract virtual 
            instance void  SetOverlayFX([in] unsigned int32 dwOverlayFX) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IAMOverlayFX::SetOverlayFX

    .method /*060001E7*/ public hidebysig newslot abstract virtual 
            instance void  GetOverlayFX([out] unsigned int32& lpdwOverlayFX) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IAMOverlayFX::GetOverlayFX

  } // end of class IAMOverlayFX

  .class /*02000081*/ interface public abstract auto ansi import IAMOpenProgress
  {
    .custom /*0C0000C4:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000C5:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 38 45 31 43 33 39 41 31 2D 44 45 35 33   // ..$8E1C39A1-DE53
                                                                                                                                                                     2D 31 31 43 46 2D 41 41 36 33 2D 30 30 38 30 43   // -11CF-AA63-0080C
                                                                                                                                                                     37 34 34 35 32 38 44 00 00 )                      // 744528D..
    .method /*060001E8*/ public hidebysig newslot abstract virtual 
            instance void  QueryProgress([out] int64& pllTotal,
                                         [out] int64& pllCurrent) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IAMOpenProgress::QueryProgress

    .method /*060001E9*/ public hidebysig newslot abstract virtual 
            instance void  AbortOperation() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMOpenProgress::AbortOperation

  } // end of class IAMOpenProgress

  .class /*02000082*/ interface public abstract auto ansi import IMpeg2Demultiplexer
  {
    .custom /*0C0000C6:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 34 33 36 45 45 45 39 43 2D 32 36 34 46   // ..$436EEE9C-264F
                                                                                                                                                                     2D 34 32 34 32 2D 39 30 45 31 2D 34 45 33 33 30   // -4242-90E1-4E330
                                                                                                                                                                     43 31 30 37 35 31 32 00 00 )                      // C107512..
    .custom /*0C0000C7:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060001EA*/ public hidebysig newslot abstract virtual 
            instance void  CreateOutputPin([in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pMediaType,
                                           [in] string  marshal( lpwstr) pszPinName,
                                           [out] class MSR.LST.MDShow.IPin/* 0200001E */&  marshal( interface) ppIPin) runtime managed internalcall
    // SIG: 20 03 01 10 11 50 0E 10 12 78
    {
    } // end of method IMpeg2Demultiplexer::CreateOutputPin

    .method /*060001EB*/ public hidebysig newslot abstract virtual 
            instance void  SetOutputPinMediaType([in] string  marshal( lpwstr) pszPinName,
                                                 [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pMediaType) runtime managed internalcall
    // SIG: 20 02 01 0E 10 11 50
    {
    } // end of method IMpeg2Demultiplexer::SetOutputPinMediaType

    .method /*060001EC*/ public hidebysig newslot abstract virtual 
            instance void  DeleteOutputPin([in] string  marshal( lpwstr) pszPinName) runtime managed internalcall
    // SIG: 20 01 01 0E
    {
    } // end of method IMpeg2Demultiplexer::DeleteOutputPin

  } // end of class IMpeg2Demultiplexer

  .class /*02000083*/ interface public abstract auto ansi import IMPEG2StreamIdMap
  {
    .custom /*0C0000C8:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000C9:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 44 30 45 30 34 43 34 37 2D 32 35 42 38   // ..$D0E04C47-25B8
                                                                                                                                                                     2D 34 33 36 39 2D 39 32 35 41 2D 33 36 32 41 30   // -4369-925A-362A0
                                                                                                                                                                     31 44 39 35 34 34 34 00 00 )                      // 1D95444..
    .method /*060001ED*/ public hidebysig newslot abstract virtual 
            instance void  MapStreamId([in] unsigned int32 ulStreamId,
                                       [in] unsigned int32 MediaSampleContent,
                                       [in] unsigned int32 ulSubstreamFilterValue,
                                       [in] int32 iDataOffset) runtime managed internalcall
    // SIG: 20 04 01 09 09 09 08
    {
    } // end of method IMPEG2StreamIdMap::MapStreamId

    .method /*060001EE*/ public hidebysig newslot abstract virtual 
            instance void  UnmapStreamId([in] unsigned int32 culStreamId,
                                         [in] unsigned int32& pulStreamId) runtime managed internalcall
    // SIG: 20 02 01 09 10 09
    {
    } // end of method IMPEG2StreamIdMap::UnmapStreamId

    .method /*060001EF*/ public hidebysig newslot abstract virtual 
            instance void  EnumStreamIdMap([out] class MSR.LST.MDShow.IEnumStreamIdMap/* 02000084 */&  marshal( interface) ppIEnumStreamIdMap) runtime managed internalcall
    // SIG: 20 01 01 10 12 82 10
    {
    } // end of method IMPEG2StreamIdMap::EnumStreamIdMap

  } // end of class IMPEG2StreamIdMap

  .class /*02000084*/ interface public abstract auto ansi import IEnumStreamIdMap
  {
    .custom /*0C0000CA:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 34 35 43 31 35 36 36 2D 36 32 30 32   // ..$945C1566-6202
                                                                                                                                                                     2D 34 36 46 43 2D 39 36 43 37 2D 44 38 37 46 32   // -46FC-96C7-D87F2
                                                                                                                                                                     38 39 43 36 35 33 34 00 00 )                      // 89C6534..
    .custom /*0C0000CB:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060001F0*/ public hidebysig newslot abstract virtual 
            instance void  Next([in] unsigned int32 cRequest,
                                [in][out] valuetype MSR.LST.MDShow.STREAM_ID_MAP/* 02000085 */& pStreamIdMap,
                                [out] unsigned int32& pcReceived) runtime managed internalcall
    // SIG: 20 03 01 09 10 11 82 14 10 09
    {
      .param [2]/*08000336*/ 
      .custom /*0C000147:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1C 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 53 54 52 45 41 4D 5F 49 44 5F 4D 41 50 00   // w.STREAM_ID_MAP.
                                                                                                                                                                               00 ) 
    } // end of method IEnumStreamIdMap::Next

    .method /*060001F1*/ public hidebysig newslot abstract virtual 
            instance void  Skip([in] unsigned int32 cRecords) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IEnumStreamIdMap::Skip

    .method /*060001F2*/ public hidebysig newslot abstract virtual 
            instance void  Reset() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IEnumStreamIdMap::Reset

    .method /*060001F3*/ public hidebysig newslot abstract virtual 
            instance void  Clone([out] class MSR.LST.MDShow.IEnumStreamIdMap/* 02000084 */&  marshal( interface) ppIEnumStreamIdMap) runtime managed internalcall
    // SIG: 20 01 01 10 12 82 10
    {
    } // end of method IEnumStreamIdMap::Clone

  } // end of class IEnumStreamIdMap

  .class /*02000085*/ public sequential ansi sealed beforefieldinit STREAM_ID_MAP
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000E5*/ public unsigned int32 stream_id
    .field /*040000E6*/ public unsigned int32 dwMediaSampleContent
    .field /*040000E7*/ public unsigned int32 ulSubstreamFilterValue
    .field /*040000E8*/ public int32 iDataOffset
  } // end of class STREAM_ID_MAP

  .class /*02000086*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0358_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040000E9*/ public unsigned int32 stream_id
    .field /*040000EA*/ public unsigned int32 dwMediaSampleContent
    .field /*040000EB*/ public unsigned int32 ulSubstreamFilterValue
    .field /*040000EC*/ public int32 iDataOffset
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0358_0001

  .class /*02000087*/ interface public abstract auto ansi import IAMClockSlave
  {
    .custom /*0C0000CD:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 46 44 35 32 37 34 31 2D 31 37 36 44   // ..$9FD52741-176D
                                                                                                                                                                     2D 34 42 33 36 2D 38 46 35 31 2D 43 41 38 46 39   // -4B36-8F51-CA8F9
                                                                                                                                                                     33 33 32 32 33 42 45 00 00 )                      // 33223BE..
    .custom /*0C0000CE:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*060001F4*/ public hidebysig newslot abstract virtual 
            instance void  SetErrorTolerance([in] unsigned int32 dwTolerance) runtime managed internalcall
    // SIG: 20 01 01 09
    {
    } // end of method IAMClockSlave::SetErrorTolerance

    .method /*060001F5*/ public hidebysig newslot abstract virtual 
            instance void  GetErrorTolerance([out] unsigned int32& pdwTolerance) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IAMClockSlave::GetErrorTolerance

  } // end of class IAMClockSlave

  .class /*02000088*/ interface public abstract auto ansi import IEncoderAPI
  {
    .custom /*0C0000D0:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 37 30 34 32 33 38 33 39 2D 36 41 43 43   // ..$70423839-6ACC
                                                                                                                                                                     2D 34 42 32 33 2D 42 30 37 39 2D 32 31 44 42 46   // -4B23-B079-21DBF
                                                                                                                                                                     30 38 31 35 36 41 35 00 00 )                      // 08156A5..
    .custom /*0C0000D1:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000D2:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*060001F6*/ public hidebysig newslot abstract virtual 
            instance void  IsSupported([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IEncoderAPI::IsSupported

    .method /*060001F7*/ public hidebysig newslot abstract virtual 
            instance void  IsAvailable([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IEncoderAPI::IsAvailable

    .method /*060001F8*/ public hidebysig newslot abstract virtual 
            instance void  GetParameterRange([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                             [out] object&  marshal( struct) ValueMin,
                                             [out] object&  marshal( struct) ValueMax,
                                             [out] object&  marshal( struct) SteppingDelta) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 10 1C 10 1C 10 1C
    {
    } // end of method IEncoderAPI::GetParameterRange

    .method /*060001F9*/ public hidebysig newslot abstract virtual 
            instance void  GetParameterValues([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                              [out] native int Values,
                                              [out] unsigned int32& ValuesCount) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 18 10 09
    {
    } // end of method IEncoderAPI::GetParameterValues

    .method /*060001FA*/ public hidebysig newslot abstract virtual 
            instance void  GetDefaultValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                           [out] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method IEncoderAPI::GetDefaultValue

    .method /*060001FB*/ public hidebysig newslot abstract virtual 
            instance void  GetValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                    [out] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method IEncoderAPI::GetValue

    .method /*060001FC*/ public hidebysig newslot abstract virtual 
            instance void  SetValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                    [in] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method IEncoderAPI::SetValue

  } // end of class IEncoderAPI

  .class /*02000089*/ interface public abstract auto ansi import IVideoEncoder
         implements MSR.LST.MDShow.IEncoderAPI/* 02000088 */
  {
    .custom /*0C0000D3:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000D4:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 30 32 39 39 37 43 33 42 2D 38 45 31 42   // ..$02997C3B-8E1B
                                                                                                                                                                     2D 34 36 30 45 2D 39 32 37 30 2D 35 34 35 45 30   // -460E-9270-545E0
                                                                                                                                                                     44 45 39 35 36 33 45 00 00 )                      // DE9563E..
    .method /*060001FD*/ public hidebysig newslot abstract virtual 
            instance void  IsSupported([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IVideoEncoder::IsSupported

    .method /*060001FE*/ public hidebysig newslot abstract virtual 
            instance void  IsAvailable([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IVideoEncoder::IsAvailable

    .method /*060001FF*/ public hidebysig newslot abstract virtual 
            instance void  GetParameterRange([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                             [out] object&  marshal( struct) ValueMin,
                                             [out] object&  marshal( struct) ValueMax,
                                             [out] object&  marshal( struct) SteppingDelta) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 10 1C 10 1C 10 1C
    {
    } // end of method IVideoEncoder::GetParameterRange

    .method /*06000200*/ public hidebysig newslot abstract virtual 
            instance void  GetParameterValues([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                              [out] native int Values,
                                              [out] unsigned int32& ValuesCount) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 18 10 09
    {
    } // end of method IVideoEncoder::GetParameterValues

    .method /*06000201*/ public hidebysig newslot abstract virtual 
            instance void  GetDefaultValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                           [out] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method IVideoEncoder::GetDefaultValue

    .method /*06000202*/ public hidebysig newslot abstract virtual 
            instance void  GetValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                    [out] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method IVideoEncoder::GetValue

    .method /*06000203*/ public hidebysig newslot abstract virtual 
            instance void  SetValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                    [in] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method IVideoEncoder::SetValue

  } // end of class IVideoEncoder

  .class /*0200008A*/ interface public abstract auto ansi import IAMGraphBuilderCallback
  {
    .custom /*0C0000D7:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 34 39 39 35 46 35 31 31 2D 39 44 44 42   // ..$4995F511-9DDB
                                                                                                                                                                     2D 34 46 31 32 2D 42 44 33 42 2D 46 30 34 36 31   // -4F12-BD3B-F0461
                                                                                                                                                                     31 38 30 37 42 37 39 00 00 )                      // 1807B79..
    .custom /*0C0000D8:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000204*/ public hidebysig newslot abstract virtual 
            instance void  SelectedFilter([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pMon) runtime managed internalcall
    // SIG: 20 01 01 12 10
    {
    } // end of method IAMGraphBuilderCallback::SelectedFilter

    .method /*06000205*/ public hidebysig newslot abstract virtual 
            instance void  CreatedFilter([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFil) runtime managed internalcall
    // SIG: 20 01 01 12 64
    {
    } // end of method IAMGraphBuilderCallback::CreatedFilter

  } // end of class IAMGraphBuilderCallback

  .class /*0200008B*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000ED*/ public specialname rtspecialname int32 value__
    .field /*040000EE*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004/* 0200008B */ REG_PINFLAG_B_ZERO = int32(0x00000001)
    .field /*040000EF*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004/* 0200008B */ REG_PINFLAG_B_RENDERER = int32(0x00000002)
    .field /*040000F0*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004/* 0200008B */ REG_PINFLAG_B_MANY = int32(0x00000004)
    .field /*040000F1*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004/* 0200008B */ REG_PINFLAG_B_OUTPUT = int32(0x00000008)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0137_0004

  .class /*0200008C*/ interface public abstract auto ansi import IFilterMapper3
         implements MSR.LST.MDShow.IFilterMapper2/* 0200003A */
  {
    .custom /*0C0000DB:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000DC:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 42 37 39 42 42 30 42 31 2D 33 33 43 31   // ..$B79BB0B1-33C1
                                                                                                                                                                     2D 31 31 44 31 2D 41 42 45 31 2D 30 30 41 30 43   // -11D1-ABE1-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .method /*06000206*/ public hidebysig newslot abstract virtual 
            instance void  CreateCategory([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& clsidCategory,
                                          [in] unsigned int32 dwCategoryMerit,
                                          [in] string  marshal( lpwstr) Description) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 09 0E
    {
    } // end of method IFilterMapper3::CreateCategory

    .method /*06000207*/ public hidebysig newslot abstract virtual 
            instance void  UnregisterFilter([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pclsidCategory,
                                            [in] unsigned int16& szInstance,
                                            [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Filter) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 10 07 10 11 11
    {
    } // end of method IFilterMapper3::UnregisterFilter

    .method /*06000208*/ public hidebysig newslot abstract virtual 
            instance void  RegisterFilter([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& clsidFilter,
                                          [in] string  marshal( lpwstr) Name,
                                          [in][out] class MSR.LST.MDShow.IMoniker/* 02000004 */&  marshal( interface) ppMoniker,
                                          [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pclsidCategory,
                                          [in] unsigned int16& szInstance,
                                          [in] valuetype MSR.LST.MDShow.REGFILTER2/* 0200003B */& prf2) runtime managed internalcall
    // SIG: 20 06 01 10 11 11 0E 10 12 10 10 11 11 10 07 10 11 80 EC
    {
      .param [6]/*08000367*/ 
      .custom /*0C000148:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 19 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 52 45 47 46 49 4C 54 45 52 32 00 00 )       // w.REGFILTER2..
    } // end of method IFilterMapper3::RegisterFilter

    .method /*06000209*/ public hidebysig newslot abstract virtual 
            instance void  EnumMatchingFilters([out] class MSR.LST.MDShow.IEnumMoniker/* 02000003 */&  marshal( interface) ppenum,
                                               [in] unsigned int32 dwFlags,
                                               [in] int32 bExactMatch,
                                               [in] unsigned int32 dwMerit,
                                               [in] int32 bInputNeeded,
                                               [in] unsigned int32 cInputTypes,
                                               valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pInputTypes,
                                               [in] valuetype MSR.LST.MDShow.REGPINMEDIUM/* 02000046 */& pMedIn,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pPinCategoryIn,
                                               [in] int32 bRender,
                                               [in] int32 bOutputNeeded,
                                               [in] unsigned int32 cOutputTypes,
                                               valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pOutputTypes,
                                               [in] valuetype MSR.LST.MDShow.REGPINMEDIUM/* 02000046 */& pMedOut,
                                               [in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pPinCategoryOut) runtime managed internalcall
    // SIG: 20 0F 01 10 12 0C 09 08 09 08 09 10 11 11 10 11 81 18 10 11 11 08 08 09 10 11 11 10 11 81 18 10 11 11
    {
      .param [8]/*0800036F*/ 
      .custom /*0C000149:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1B 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 52 45 47 50 49 4E 4D 45 44 49 55 4D 00 00 ) // w.REGPINMEDIUM..
      .param [14]/*08000375*/ 
      .custom /*0C00014A:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1B 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 52 45 47 50 49 4E 4D 45 44 49 55 4D 00 00 ) // w.REGPINMEDIUM..
    } // end of method IFilterMapper3::EnumMatchingFilters

    .method /*0600020A*/ public hidebysig newslot abstract virtual 
            instance void  GetICreateDevEnum([out] class MSR.LST.MDShow.ICreateDevEnum/* 02000002 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 08
    {
    } // end of method IFilterMapper3::GetICreateDevEnum

  } // end of class IFilterMapper3

  .class /*0200008D*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000F2*/ public specialname rtspecialname int32 value__
    .field /*040000F3*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0001/* 0200008D */ CK_NOCOLORKEY = int32(0x00000000)
    .field /*040000F4*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0001/* 0200008D */ CK_INDEX = int32(0x00000001)
    .field /*040000F5*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0001/* 0200008D */ CK_RGB = int32(0x00000002)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0001

  .class /*0200008E*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000F6*/ public specialname rtspecialname int32 value__
    .field /*040000F7*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002/* 0200008E */ ADVISE_NONE = int32(0x00000000)
    .field /*040000F8*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002/* 0200008E */ ADVISE_CLIPPING = int32(0x00000001)
    .field /*040000F9*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002/* 0200008E */ ADVISE_PALETTE = int32(0x00000002)
    .field /*040000FA*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002/* 0200008E */ ADVISE_COLORKEY = int32(0x00000004)
    .field /*040000FB*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002/* 0200008E */ ADVISE_POSITION = int32(0x00000008)
    .field /*040000FC*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002/* 0200008E */ ADVISE_DISPLAY_CHANGE = int32(0x00000010)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0141_0002

  .class /*0200008F*/ public auto ansi sealed AM_FILESINK_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000FD*/ public specialname rtspecialname int32 value__
    .field /*040000FE*/ public static literal valuetype MSR.LST.MDShow.AM_FILESINK_FLAGS/* 0200008F */ AM_FILE_OVERWRITE = int32(0x00000001)
  } // end of class AM_FILESINK_FLAGS

  .class /*02000090*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0148_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040000FF*/ public specialname rtspecialname int32 value__
    .field /*04000100*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0148_0001/* 02000090 */ AM_FILE_OVERWRITE = int32(0x00000001)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0148_0001

  .class /*02000091*/ public auto ansi sealed _AM_RENSDEREXFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000101*/ public specialname rtspecialname int32 value__
    .field /*04000102*/ public static literal valuetype MSR.LST.MDShow._AM_RENSDEREXFLAGS/* 02000091 */ AM_RENDEREX_RENDERTOEXISTINGRENDERERS = int32(0x00000001)
  } // end of class _AM_RENSDEREXFLAGS

  .class /*02000092*/ interface public abstract auto ansi import IFilterGraph2
         implements MSR.LST.MDShow.IGraphBuilder/* 02000035 */
  {
    .custom /*0C0000E2:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000E3:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 36 42 37 33 38 38 32 2D 43 32 43 38   // ..$36B73882-C2C8
                                                                                                                                                                     2D 31 31 43 46 2D 38 42 34 36 2D 30 30 38 30 35   // -11CF-8B46-00805
                                                                                                                                                                     46 36 43 45 46 36 30 00 00 )                      // F6CEF60..
    .method /*0600020B*/ public hidebysig newslot abstract virtual 
            instance void  AddFilter([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter,
                                     [in] string  marshal( lpwstr) pName) runtime managed internalcall
    // SIG: 20 02 01 12 64 0E
    {
    } // end of method IFilterGraph2::AddFilter

    .method /*0600020C*/ public hidebysig newslot abstract virtual 
            instance void  RemoveFilter([in] class MSR.LST.MDShow.IBaseFilter/* 02000019 */  marshal( interface) pFilter) runtime managed internalcall
    // SIG: 20 01 01 12 64
    {
    } // end of method IFilterGraph2::RemoveFilter

    .method /*0600020D*/ public hidebysig newslot abstract virtual 
            instance void  EnumFilters([out] class MSR.LST.MDShow.IEnumFilters/* 02000018 */&  marshal( interface) ppenum) runtime managed internalcall
    // SIG: 20 01 01 10 12 60
    {
    } // end of method IFilterGraph2::EnumFilters

    .method /*0600020E*/ public hidebysig newslot abstract virtual 
            instance void  FindFilterByName([in] string  marshal( lpwstr) pName,
                                            [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppFilter) runtime managed internalcall
    // SIG: 20 02 01 0E 10 12 64
    {
    } // end of method IFilterGraph2::FindFilterByName

    .method /*0600020F*/ public hidebysig newslot abstract virtual 
            instance void  ConnectDirect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                         [in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinIn,
                                         [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 03 01 12 78 12 78 10 11 50
    {
    } // end of method IFilterGraph2::ConnectDirect

    .method /*06000210*/ public hidebysig newslot abstract virtual 
            instance void  Reconnect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IFilterGraph2::Reconnect

    .method /*06000211*/ public hidebysig newslot abstract virtual 
            instance void  Disconnect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IFilterGraph2::Disconnect

    .method /*06000212*/ public hidebysig newslot abstract virtual 
            instance void  SetDefaultSyncSource() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IFilterGraph2::SetDefaultSyncSource

    .method /*06000213*/ public hidebysig newslot abstract virtual 
            instance void  Connect([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                   [in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinIn) runtime managed internalcall
    // SIG: 20 02 01 12 78 12 78
    {
    } // end of method IFilterGraph2::Connect

    .method /*06000214*/ public hidebysig newslot abstract virtual 
            instance void  Render([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut) runtime managed internalcall
    // SIG: 20 01 01 12 78
    {
    } // end of method IFilterGraph2::Render

    .method /*06000215*/ public hidebysig newslot abstract virtual 
            instance void  RenderFile([in] string  marshal( lpwstr) lpcwstrFile,
                                      [in] string  marshal( lpwstr) lpcwstrPlayList) runtime managed internalcall
    // SIG: 20 02 01 0E 0E
    {
    } // end of method IFilterGraph2::RenderFile

    .method /*06000216*/ public hidebysig newslot abstract virtual 
            instance void  AddSourceFilter([in] string  marshal( lpwstr) lpcwstrFileName,
                                           [in] string  marshal( lpwstr) lpcwstrFilterName,
                                           [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppFilter) runtime managed internalcall
    // SIG: 20 03 01 0E 0E 10 12 64
    {
    } // end of method IFilterGraph2::AddSourceFilter

    .method /*06000217*/ public hidebysig newslot abstract virtual 
            instance void  SetLogFile([in] unsigned int32 hFile) runtime managed internalcall
    // SIG: 20 01 01 09
    {
      .param [1]/*0800038B*/ 
      .custom /*0C00014B:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IFilterGraph2::SetLogFile

    .method /*06000218*/ public hidebysig newslot abstract virtual 
            instance void  Abort() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IFilterGraph2::Abort

    .method /*06000219*/ public hidebysig newslot abstract virtual 
            instance void  ShouldOperationContinue() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IFilterGraph2::ShouldOperationContinue

    .method /*0600021A*/ public hidebysig newslot abstract virtual 
            instance void  AddSourceFilterForMoniker([in] class MSR.LST.MDShow.IMoniker/* 02000004 */  marshal( interface) pMoniker,
                                                     [in] class MSR.LST.MDShow.IBindCtx/* 0200000D */  marshal( interface) pCtx,
                                                     [in] string  marshal( lpwstr) lpcwstrFilterName,
                                                     [out] class MSR.LST.MDShow.IBaseFilter/* 02000019 */&  marshal( interface) ppFilter) runtime managed internalcall
    // SIG: 20 04 01 12 10 12 34 0E 10 12 64
    {
    } // end of method IFilterGraph2::AddSourceFilterForMoniker

    .method /*0600021B*/ public hidebysig newslot abstract virtual 
            instance void  ReconnectEx([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                       [in] valuetype MSR.LST.MDShow._AMMediaType/* 02000014 */& pmt) runtime managed internalcall
    // SIG: 20 02 01 12 78 10 11 50
    {
    } // end of method IFilterGraph2::ReconnectEx

    .method /*0600021C*/ public hidebysig newslot abstract virtual 
            instance void  RenderEx([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                    [in] unsigned int32 dwFlags,
                                    [in][out] unsigned int32& pvContext) runtime managed internalcall
    // SIG: 20 03 01 12 78 09 10 09
    {
    } // end of method IFilterGraph2::RenderEx

  } // end of class IFilterGraph2

  .class /*02000093*/ interface public abstract auto ansi import IStreamBuilder
  {
    .custom /*0C0000E5:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000E6:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 42 46 2D 30 41 44 34   // ..$56A868BF-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*0600021D*/ public hidebysig newslot abstract virtual 
            instance void  Render([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                  [in] class MSR.LST.MDShow.IGraphBuilder/* 02000035 */  marshal( interface) pGraph) runtime managed internalcall
    // SIG: 20 02 01 12 78 12 80 D4
    {
    } // end of method IStreamBuilder::Render

    .method /*0600021E*/ public hidebysig newslot abstract virtual 
            instance void  Backout([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) ppinOut,
                                   [in] class MSR.LST.MDShow.IGraphBuilder/* 02000035 */  marshal( interface) pGraph) runtime managed internalcall
    // SIG: 20 02 01 12 78 12 80 D4
    {
    } // end of method IStreamBuilder::Backout

  } // end of class IStreamBuilder

  .class /*02000094*/ interface public abstract auto ansi import IAsyncReader
  {
    .custom /*0C0000E9:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000EA:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 41 2D 30 41 44 34   // ..$56A868AA-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*0600021F*/ public hidebysig newslot abstract virtual 
            instance void  RequestAllocator([in] class MSR.LST.MDShow.IMemAllocator/* 02000024 */  marshal( interface) pPreferred,
                                            [in] valuetype MSR.LST.MDShow._AllocatorProperties/* 02000016 */& pProps,
                                            [out] class MSR.LST.MDShow.IMemAllocator/* 02000024 */&  marshal( interface) ppActual) runtime managed internalcall
    // SIG: 20 03 01 12 80 90 10 11 58 10 12 80 90
    {
    } // end of method IAsyncReader::RequestAllocator

    .method /*06000220*/ public hidebysig newslot abstract virtual 
            instance void  Request([in] class MSR.LST.MDShow.IMediaSample/* 02000023 */  marshal( interface) pSample,
                                   [in] unsigned int32 dwUser) runtime managed internalcall
    // SIG: 20 02 01 12 80 8C 09
    {
      .param [2]/*0800039D*/ 
      .custom /*0C00014C:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IAsyncReader::Request

    .method /*06000221*/ public hidebysig newslot abstract virtual 
            instance void  WaitForNext([in] unsigned int32 dwTimeout,
                                       [out] class MSR.LST.MDShow.IMediaSample/* 02000023 */&  marshal( interface) ppSample,
                                       [out] unsigned int32& pdwUser) runtime managed internalcall
    // SIG: 20 03 01 09 10 12 80 8C 10 09
    {
      .param [3]/*080003A0*/ 
      .custom /*0C00014D:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 18 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 55 4C 4F 4E 47 5F 50 54 52 00 00 )          // w.ULONG_PTR..
    } // end of method IAsyncReader::WaitForNext

    .method /*06000222*/ public hidebysig newslot abstract virtual 
            instance void  SyncReadAligned([in] class MSR.LST.MDShow.IMediaSample/* 02000023 */  marshal( interface) pSample) runtime managed internalcall
    // SIG: 20 01 01 12 80 8C
    {
    } // end of method IAsyncReader::SyncReadAligned

    .method /*06000223*/ public hidebysig newslot abstract virtual 
            instance void  SyncRead([in] int64 llPosition,
                                    [in] int32 lLength,
                                    [out] unsigned int8& pBuffer) runtime managed internalcall
    // SIG: 20 03 01 0A 08 10 05
    {
    } // end of method IAsyncReader::SyncRead

    .method /*06000224*/ public hidebysig newslot abstract virtual 
            instance void  Length([out] int64& pTotal,
                                  [out] int64& pAvailable) runtime managed internalcall
    // SIG: 20 02 01 10 0A 10 0A
    {
    } // end of method IAsyncReader::Length

    .method /*06000225*/ public hidebysig newslot abstract virtual 
            instance void  BeginFlush() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAsyncReader::BeginFlush

    .method /*06000226*/ public hidebysig newslot abstract virtual 
            instance void  EndFlush() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAsyncReader::EndFlush

  } // end of class IAsyncReader

  .class /*02000095*/ interface public abstract auto ansi import IGraphVersion
  {
    .custom /*0C0000EB:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 42 2D 30 41 44 34   // ..$56A868AB-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .custom /*0C0000EC:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000227*/ public hidebysig newslot abstract virtual 
            instance void  QueryVersion(int32& pVersion) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IGraphVersion::QueryVersion

  } // end of class IGraphVersion

  .class /*02000096*/ interface public abstract auto ansi import IResourceConsumer
  {
    .custom /*0C0000F0:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000F1:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 44 2D 30 41 44 34   // ..$56A868AD-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*06000228*/ public hidebysig newslot abstract virtual 
            instance void  AcquireResource([in] int32 idResource) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IResourceConsumer::AcquireResource

    .method /*06000229*/ public hidebysig newslot abstract virtual 
            instance void  ReleaseResource([in] int32 idResource) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IResourceConsumer::ReleaseResource

  } // end of class IResourceConsumer

  .class /*02000097*/ interface public abstract auto ansi import IResourceManager
  {
    .custom /*0C0000F4:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000F5:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 43 2D 30 41 44 34   // ..$56A868AC-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*0600022A*/ public hidebysig newslot abstract virtual 
            instance void  Register([in] string  marshal( lpwstr) pName,
                                    [in] int32 cResource,
                                    [out] int32& plToken) runtime managed internalcall
    // SIG: 20 03 01 0E 08 10 08
    {
    } // end of method IResourceManager::Register

    .method /*0600022B*/ public hidebysig newslot abstract virtual 
            instance void  RegisterGroup([in] string  marshal( lpwstr) pName,
                                         [in] int32 cResource,
                                         [in] int32& palTokens,
                                         [out] int32& plToken) runtime managed internalcall
    // SIG: 20 04 01 0E 08 10 08 10 08
    {
    } // end of method IResourceManager::RegisterGroup

    .method /*0600022C*/ public hidebysig newslot abstract virtual 
            instance void  RequestResource([in] int32 idResource,
                                           [in] object  marshal( iunknown) pFocusObject,
                                           [in] class MSR.LST.MDShow.IResourceConsumer/* 02000096 */  marshal( interface) pConsumer) runtime managed internalcall
    // SIG: 20 03 01 08 1C 12 82 58
    {
    } // end of method IResourceManager::RequestResource

    .method /*0600022D*/ public hidebysig newslot abstract virtual 
            instance void  NotifyAcquire([in] int32 idResource,
                                         [in] class MSR.LST.MDShow.IResourceConsumer/* 02000096 */  marshal( interface) pConsumer,
                                         [in] int32  marshal( error) hr) runtime managed internalcall
    // SIG: 20 03 01 08 12 82 58 08
    {
    } // end of method IResourceManager::NotifyAcquire

    .method /*0600022E*/ public hidebysig newslot abstract virtual 
            instance void  NotifyRelease([in] int32 idResource,
                                         [in] class MSR.LST.MDShow.IResourceConsumer/* 02000096 */  marshal( interface) pConsumer,
                                         [in] int32 bStillWant) runtime managed internalcall
    // SIG: 20 03 01 08 12 82 58 08
    {
    } // end of method IResourceManager::NotifyRelease

    .method /*0600022F*/ public hidebysig newslot abstract virtual 
            instance void  CancelRequest([in] int32 idResource,
                                         [in] class MSR.LST.MDShow.IResourceConsumer/* 02000096 */  marshal( interface) pConsumer) runtime managed internalcall
    // SIG: 20 02 01 08 12 82 58
    {
    } // end of method IResourceManager::CancelRequest

    .method /*06000230*/ public hidebysig newslot abstract virtual 
            instance void  SetFocus([in] object  marshal( iunknown) pFocusObject) runtime managed internalcall
    // SIG: 20 01 01 1C
    {
    } // end of method IResourceManager::SetFocus

    .method /*06000231*/ public hidebysig newslot abstract virtual 
            instance void  ReleaseFocus([in] object  marshal( iunknown) pFocusObject) runtime managed internalcall
    // SIG: 20 01 01 1C
    {
    } // end of method IResourceManager::ReleaseFocus

  } // end of class IResourceManager

  .class /*02000098*/ interface public abstract auto ansi import IDistributorNotify
  {
    .custom /*0C0000F7:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000F8:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 36 41 38 36 38 41 46 2D 30 41 44 34   // ..$56A868AF-0AD4
                                                                                                                                                                     2D 31 31 43 45 2D 42 30 33 41 2D 30 30 32 30 41   // -11CE-B03A-0020A
                                                                                                                                                                     46 30 42 41 37 37 30 00 00 )                      // F0BA770..
    .method /*06000232*/ public hidebysig newslot abstract virtual 
            instance void  Stop() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IDistributorNotify::Stop

    .method /*06000233*/ public hidebysig newslot abstract virtual 
            instance void  Pause() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IDistributorNotify::Pause

    .method /*06000234*/ public hidebysig newslot abstract virtual 
            instance void  Run(int64 tStart) runtime managed internalcall
    // SIG: 20 01 01 0A
    {
    } // end of method IDistributorNotify::Run

    .method /*06000235*/ public hidebysig newslot abstract virtual 
            instance void  SetSyncSource([in] class MSR.LST.MDShow.IReferenceClock/* 0200001C */  marshal( interface) pClock) runtime managed internalcall
    // SIG: 20 01 01 12 70
    {
    } // end of method IDistributorNotify::SetSyncSource

    .method /*06000236*/ public hidebysig newslot abstract virtual 
            instance void  NotifyGraphChange() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IDistributorNotify::NotifyGraphChange

  } // end of class IDistributorNotify

  .class /*02000099*/ public auto ansi sealed AM_STREAM_INFO_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000103*/ public specialname rtspecialname int32 value__
    .field /*04000104*/ public static literal valuetype MSR.LST.MDShow.AM_STREAM_INFO_FLAGS/* 02000099 */ AM_STREAM_INFO_START_DEFINED = int32(0x00000001)
    .field /*04000105*/ public static literal valuetype MSR.LST.MDShow.AM_STREAM_INFO_FLAGS/* 02000099 */ AM_STREAM_INFO_STOP_DEFINED = int32(0x00000002)
    .field /*04000106*/ public static literal valuetype MSR.LST.MDShow.AM_STREAM_INFO_FLAGS/* 02000099 */ AM_STREAM_INFO_DISCARDING = int32(0x00000004)
    .field /*04000107*/ public static literal valuetype MSR.LST.MDShow.AM_STREAM_INFO_FLAGS/* 02000099 */ AM_STREAM_INFO_STOP_SEND_EXTRA = int32(0x00000010)
  } // end of class AM_STREAM_INFO_FLAGS

  .class /*0200009A*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000108*/ public specialname rtspecialname int32 value__
    .field /*04000109*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001/* 0200009A */ AM_STREAM_INFO_START_DEFINED = int32(0x00000001)
    .field /*0400010A*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001/* 0200009A */ AM_STREAM_INFO_STOP_DEFINED = int32(0x00000002)
    .field /*0400010B*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001/* 0200009A */ AM_STREAM_INFO_DISCARDING = int32(0x00000004)
    .field /*0400010C*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001/* 0200009A */ AM_STREAM_INFO_STOP_SEND_EXTRA = int32(0x00000010)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0001

  .class /*0200009B*/ public sequential ansi sealed beforefieldinit AM_STREAM_INFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .field /*0400010D*/ public int64 tStart
    .field /*0400010E*/ public int64 tStop
    .field /*0400010F*/ public unsigned int32 dwStartCookie
    .field /*04000110*/ public unsigned int32 dwStopCookie
    .field /*04000111*/ public unsigned int32 dwFlags
  } // end of class AM_STREAM_INFO

  .class /*0200009C*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0002
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 8
    .size 0
    .field /*04000112*/ public int64 tStart
    .field /*04000113*/ public int64 tStop
    .field /*04000114*/ public unsigned int32 dwStartCookie
    .field /*04000115*/ public unsigned int32 dwStopCookie
    .field /*04000116*/ public unsigned int32 dwFlags
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0159_0002

  .class /*0200009D*/ interface public abstract auto ansi import IAMStreamControl
  {
    .custom /*0C0000FD:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C0000FE:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 36 42 37 33 38 38 31 2D 43 32 43 38   // ..$36B73881-C2C8
                                                                                                                                                                     2D 31 31 43 46 2D 38 42 34 36 2D 30 30 38 30 35   // -11CF-8B46-00805
                                                                                                                                                                     46 36 43 45 46 36 30 00 00 )                      // F6CEF60..
    .method /*06000237*/ public hidebysig newslot abstract virtual 
            instance void  StartAt([in] int64& ptStart,
                                   [in] unsigned int32 dwCookie) runtime managed internalcall
    // SIG: 20 02 01 10 0A 09
    {
    } // end of method IAMStreamControl::StartAt

    .method /*06000238*/ public hidebysig newslot abstract virtual 
            instance void  StopAt([in] int64& ptStop,
                                  [in] int32 bSendExtra,
                                  [in] unsigned int32 dwCookie) runtime managed internalcall
    // SIG: 20 03 01 10 0A 08 09
    {
    } // end of method IAMStreamControl::StopAt

    .method /*06000239*/ public hidebysig newslot abstract virtual 
            instance void  GetInfo([out] valuetype MSR.LST.MDShow.AM_STREAM_INFO/* 0200009B */& pInfo) runtime managed internalcall
    // SIG: 20 01 01 10 11 82 6C
    {
      .param [1]/*080003C5*/ 
      .custom /*0C00014E:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 1D 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 41 4D 5F 53 54 52 45 41 4D 5F 49 4E 46 4F   // w.AM_STREAM_INFO
                                                                                                                                                                               00 00 ) 
    } // end of method IAMStreamControl::GetInfo

  } // end of class IAMStreamControl

  .class /*0200009E*/ public auto ansi sealed CompressionCaps
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000117*/ public specialname rtspecialname int32 value__
    .field /*04000118*/ public static literal valuetype MSR.LST.MDShow.CompressionCaps/* 0200009E */ CompressionCaps_CanQuality = int32(0x00000001)
    .field /*04000119*/ public static literal valuetype MSR.LST.MDShow.CompressionCaps/* 0200009E */ CompressionCaps_CanCrunch = int32(0x00000002)
    .field /*0400011A*/ public static literal valuetype MSR.LST.MDShow.CompressionCaps/* 0200009E */ CompressionCaps_CanKeyFrame = int32(0x00000004)
    .field /*0400011B*/ public static literal valuetype MSR.LST.MDShow.CompressionCaps/* 0200009E */ CompressionCaps_CanBFrame = int32(0x00000008)
    .field /*0400011C*/ public static literal valuetype MSR.LST.MDShow.CompressionCaps/* 0200009E */ CompressionCaps_CanWindow = int32(0x00000010)
  } // end of class CompressionCaps

  .class /*0200009F*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400011D*/ public specialname rtspecialname int32 value__
    .field /*0400011E*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001/* 0200009F */ CompressionCaps_CanQuality = int32(0x00000001)
    .field /*0400011F*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001/* 0200009F */ CompressionCaps_CanCrunch = int32(0x00000002)
    .field /*04000120*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001/* 0200009F */ CompressionCaps_CanKeyFrame = int32(0x00000004)
    .field /*04000121*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001/* 0200009F */ CompressionCaps_CanBFrame = int32(0x00000008)
    .field /*04000122*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001/* 0200009F */ CompressionCaps_CanWindow = int32(0x00000010)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0166_0001

  .class /*020000A0*/ public auto ansi sealed VfwCaptureDialogs
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000123*/ public specialname rtspecialname int32 value__
    .field /*04000124*/ public static literal valuetype MSR.LST.MDShow.VfwCaptureDialogs/* 020000A0 */ VfwCaptureDialog_Source = int32(0x00000001)
    .field /*04000125*/ public static literal valuetype MSR.LST.MDShow.VfwCaptureDialogs/* 020000A0 */ VfwCaptureDialog_Format = int32(0x00000002)
    .field /*04000126*/ public static literal valuetype MSR.LST.MDShow.VfwCaptureDialogs/* 020000A0 */ VfwCaptureDialog_Display = int32(0x00000004)
  } // end of class VfwCaptureDialogs

  .class /*020000A1*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000127*/ public specialname rtspecialname int32 value__
    .field /*04000128*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0167_0001/* 020000A1 */ VfwCaptureDialog_Source = int32(0x00000001)
    .field /*04000129*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0167_0001/* 020000A1 */ VfwCaptureDialog_Format = int32(0x00000002)
    .field /*0400012A*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0167_0001/* 020000A1 */ VfwCaptureDialog_Display = int32(0x00000004)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0001

  .class /*020000A2*/ public auto ansi sealed VfwCompressDialogs
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400012B*/ public specialname rtspecialname int32 value__
    .field /*0400012C*/ public static literal valuetype MSR.LST.MDShow.VfwCompressDialogs/* 020000A2 */ VfwCompressDialog_Config = int32(0x00000001)
    .field /*0400012D*/ public static literal valuetype MSR.LST.MDShow.VfwCompressDialogs/* 020000A2 */ VfwCompressDialog_About = int32(0x00000002)
    .field /*0400012E*/ public static literal valuetype MSR.LST.MDShow.VfwCompressDialogs/* 020000A2 */ VfwCompressDialog_QueryConfig = int32(0x00000004)
    .field /*0400012F*/ public static literal valuetype MSR.LST.MDShow.VfwCompressDialogs/* 020000A2 */ VfwCompressDialog_QueryAbout = int32(0x00000008)
  } // end of class VfwCompressDialogs

  .class /*020000A3*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000130*/ public specialname rtspecialname int32 value__
    .field /*04000131*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002/* 020000A3 */ VfwCompressDialog_Config = int32(0x00000001)
    .field /*04000132*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002/* 020000A3 */ VfwCompressDialog_About = int32(0x00000002)
    .field /*04000133*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002/* 020000A3 */ VfwCompressDialog_QueryConfig = int32(0x00000004)
    .field /*04000134*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002/* 020000A3 */ VfwCompressDialog_QueryAbout = int32(0x00000008)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0167_0002

  .class /*020000A4*/ public auto ansi sealed tagAnalogVideoStandard
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000135*/ public specialname rtspecialname int32 value__
    .field /*04000136*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_None = int32(0x00000000)
    .field /*04000137*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_NTSC_M = int32(0x00000001)
    .field /*04000138*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_NTSC_M_J = int32(0x00000002)
    .field /*04000139*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_NTSC_433 = int32(0x00000004)
    .field /*0400013A*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_B = int32(0x00000010)
    .field /*0400013B*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_D = int32(0x00000020)
    .field /*0400013C*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_G = int32(0x00000040)
    .field /*0400013D*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_H = int32(0x00000080)
    .field /*0400013E*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_I = int32(0x00000100)
    .field /*0400013F*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_M = int32(0x00000200)
    .field /*04000140*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_N = int32(0x00000400)
    .field /*04000141*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_60 = int32(0x00000800)
    .field /*04000142*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_B = int32(0x00001000)
    .field /*04000143*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_D = int32(0x00002000)
    .field /*04000144*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_G = int32(0x00004000)
    .field /*04000145*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_H = int32(0x00008000)
    .field /*04000146*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_K = int32(0x00010000)
    .field /*04000147*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_K1 = int32(0x00020000)
    .field /*04000148*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_L = int32(0x00040000)
    .field /*04000149*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_SECAM_L1 = int32(0x00080000)
    .field /*0400014A*/ public static literal valuetype MSR.LST.MDShow.tagAnalogVideoStandard/* 020000A4 */ AnalogVideo_PAL_N_COMBO = int32(0x00100000)
  } // end of class tagAnalogVideoStandard

  .class /*020000A5*/ public auto ansi sealed VideoCopyProtectionType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400014B*/ public specialname rtspecialname int32 value__
    .field /*0400014C*/ public static literal valuetype MSR.LST.MDShow.VideoCopyProtectionType/* 020000A5 */ VideoCopyProtectionMacrovisionBasic = int32(0x00000000)
    .field /*0400014D*/ public static literal valuetype MSR.LST.MDShow.VideoCopyProtectionType/* 020000A5 */ VideoCopyProtectionMacrovisionCBI = int32(0x00000001)
  } // end of class VideoCopyProtectionType

  .class /*020000A6*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0172_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400014E*/ public specialname rtspecialname int32 value__
    .field /*0400014F*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0172_0001/* 020000A6 */ VideoCopyProtectionMacrovisionBasic = int32(0x00000000)
    .field /*04000150*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0172_0001/* 020000A6 */ VideoCopyProtectionMacrovisionCBI = int32(0x00000001)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0172_0001

  .class /*020000A7*/ public auto ansi sealed tagPhysicalConnectorType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000151*/ public specialname rtspecialname int32 value__
    .field /*04000152*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_Tuner = int32(0x00000001)
    .field /*04000153*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_Composite = int32(0x00000002)
    .field /*04000154*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_SVideo = int32(0x00000003)
    .field /*04000155*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_RGB = int32(0x00000004)
    .field /*04000156*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_YRYBY = int32(0x00000005)
    .field /*04000157*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_SerialDigital = int32(0x00000006)
    .field /*04000158*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_ParallelDigital = int32(0x00000007)
    .field /*04000159*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_SCSI = int32(0x00000008)
    .field /*0400015A*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_AUX = int32(0x00000009)
    .field /*0400015B*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_1394 = int32(0x0000000A)
    .field /*0400015C*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_USB = int32(0x0000000B)
    .field /*0400015D*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_VideoDecoder = int32(0x0000000C)
    .field /*0400015E*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_VideoEncoder = int32(0x0000000D)
    .field /*0400015F*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_SCART = int32(0x0000000E)
    .field /*04000160*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Video_Black = int32(0x0000000F)
    .field /*04000161*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_Tuner = int32(0x00001000)
    .field /*04000162*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_Line = int32(0x00001001)
    .field /*04000163*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_Mic = int32(0x00001002)
    .field /*04000164*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_AESDigital = int32(0x00001003)
    .field /*04000165*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_SPDIFDigital = int32(0x00001004)
    .field /*04000166*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_SCSI = int32(0x00001005)
    .field /*04000167*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_AUX = int32(0x00001006)
    .field /*04000168*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_1394 = int32(0x00001007)
    .field /*04000169*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_USB = int32(0x00001008)
    .field /*0400016A*/ public static literal valuetype MSR.LST.MDShow.tagPhysicalConnectorType/* 020000A7 */ PhysConn_Audio_AudioDecoder = int32(0x00001009)
  } // end of class tagPhysicalConnectorType

  .class /*020000A8*/ public auto ansi sealed tagVideoProcAmpProperty
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400016B*/ public specialname rtspecialname int32 value__
    .field /*0400016C*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_Brightness = int32(0x00000000)
    .field /*0400016D*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_Contrast = int32(0x00000001)
    .field /*0400016E*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_Hue = int32(0x00000002)
    .field /*0400016F*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_Saturation = int32(0x00000003)
    .field /*04000170*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_Sharpness = int32(0x00000004)
    .field /*04000171*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_Gamma = int32(0x00000005)
    .field /*04000172*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_ColorEnable = int32(0x00000006)
    .field /*04000173*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_WhiteBalance = int32(0x00000007)
    .field /*04000174*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_BacklightCompensation = int32(0x00000008)
    .field /*04000175*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpProperty/* 020000A8 */ VideoProcAmp_Gain = int32(0x00000009)
  } // end of class tagVideoProcAmpProperty

  .class /*020000A9*/ public auto ansi sealed tagVideoProcAmpFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000176*/ public specialname rtspecialname int32 value__
    .field /*04000177*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpFlags/* 020000A9 */ VideoProcAmp_Flags_Auto = int32(0x00000001)
    .field /*04000178*/ public static literal valuetype MSR.LST.MDShow.tagVideoProcAmpFlags/* 020000A9 */ VideoProcAmp_Flags_Manual = int32(0x00000002)
  } // end of class tagVideoProcAmpFlags

  .class /*020000AA*/ public auto ansi sealed tagCameraControlProperty
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000179*/ public specialname rtspecialname int32 value__
    .field /*0400017A*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlProperty/* 020000AA */ CameraControl_Pan = int32(0x00000000)
    .field /*0400017B*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlProperty/* 020000AA */ CameraControl_Tilt = int32(0x00000001)
    .field /*0400017C*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlProperty/* 020000AA */ CameraControl_Roll = int32(0x00000002)
    .field /*0400017D*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlProperty/* 020000AA */ CameraControl_Zoom = int32(0x00000003)
    .field /*0400017E*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlProperty/* 020000AA */ CameraControl_Exposure = int32(0x00000004)
    .field /*0400017F*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlProperty/* 020000AA */ CameraControl_Iris = int32(0x00000005)
    .field /*04000180*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlProperty/* 020000AA */ CameraControl_Focus = int32(0x00000006)
  } // end of class tagCameraControlProperty

  .class /*020000AB*/ public auto ansi sealed tagCameraControlFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000181*/ public specialname rtspecialname int32 value__
    .field /*04000182*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlFlags/* 020000AB */ CameraControl_Flags_Auto = int32(0x00000001)
    .field /*04000183*/ public static literal valuetype MSR.LST.MDShow.tagCameraControlFlags/* 020000AB */ CameraControl_Flags_Manual = int32(0x00000002)
  } // end of class tagCameraControlFlags

  .class /*020000AC*/ public auto ansi sealed tagVideoControlFlags
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000184*/ public specialname rtspecialname int32 value__
    .field /*04000185*/ public static literal valuetype MSR.LST.MDShow.tagVideoControlFlags/* 020000AC */ VideoControlFlag_FlipHorizontal = int32(0x00000001)
    .field /*04000186*/ public static literal valuetype MSR.LST.MDShow.tagVideoControlFlags/* 020000AC */ VideoControlFlag_FlipVertical = int32(0x00000002)
    .field /*04000187*/ public static literal valuetype MSR.LST.MDShow.tagVideoControlFlags/* 020000AC */ VideoControlFlag_ExternalTriggerEnable = int32(0x00000004)
    .field /*04000188*/ public static literal valuetype MSR.LST.MDShow.tagVideoControlFlags/* 020000AC */ VideoControlFlag_Trigger = int32(0x00000008)
  } // end of class tagVideoControlFlags

  .class /*020000AD*/ interface public abstract auto ansi import IAMVideoControl
  {
    .custom /*0C000100:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 41 32 45 30 36 37 30 2D 32 38 45 34   // ..$6A2E0670-28E4
                                                                                                                                                                     2D 31 31 44 30 2D 41 31 38 43 2D 30 30 41 30 43   // -11D0-A18C-00A0C
                                                                                                                                                                     39 31 31 38 39 35 36 00 00 )                      // 9118956..
    .custom /*0C000101:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000102:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*0600023A*/ public hidebysig newslot abstract virtual 
            instance void  GetCaps([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                   [out] int32& pCapsFlags) runtime managed internalcall
    // SIG: 20 02 01 12 78 10 08
    {
    } // end of method IAMVideoControl::GetCaps

    .method /*0600023B*/ public hidebysig newslot abstract virtual 
            instance void  SetMode([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                   [in] int32 mode) runtime managed internalcall
    // SIG: 20 02 01 12 78 08
    {
    } // end of method IAMVideoControl::SetMode

    .method /*0600023C*/ public hidebysig newslot abstract virtual 
            instance void  GetMode([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                   [out] int32& mode) runtime managed internalcall
    // SIG: 20 02 01 12 78 10 08
    {
    } // end of method IAMVideoControl::GetMode

    .method /*0600023D*/ public hidebysig newslot abstract virtual 
            instance void  GetCurrentActualFrameRate([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                                     [out] int64& ActualFrameRate) runtime managed internalcall
    // SIG: 20 02 01 12 78 10 0A
    {
    } // end of method IAMVideoControl::GetCurrentActualFrameRate

    .method /*0600023E*/ public hidebysig newslot abstract virtual 
            instance void  GetMaxAvailableFrameRate([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                                    [in] int32 iIndex,
                                                    [in] valuetype MSR.LST.MDShow.tagSIZE/* 020000AE */ Dimensions,
                                                    [out] int64& MaxAvailableFrameRate) runtime managed internalcall
    // SIG: 20 04 01 12 78 08 11 82 B8 10 0A
    {
    } // end of method IAMVideoControl::GetMaxAvailableFrameRate

    .method /*0600023F*/ public hidebysig newslot abstract virtual 
            instance void  GetFrameRateList([in] class MSR.LST.MDShow.IPin/* 0200001E */  marshal( interface) pPin,
                                            [in] int32 iIndex,
                                            [in] valuetype MSR.LST.MDShow.tagSIZE/* 020000AE */ Dimensions,
                                            [out] int32& ListSize,
                                            [out] native int FrameRates) runtime managed internalcall
    // SIG: 20 05 01 12 78 08 11 82 B8 10 08 18
    {
    } // end of method IAMVideoControl::GetFrameRateList

  } // end of class IAMVideoControl

  .class /*020000AE*/ public sequential ansi sealed beforefieldinit tagSIZE
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*04000189*/ public int32 cx
    .field /*0400018A*/ public int32 cy
  } // end of class tagSIZE

  .class /*020000AF*/ public auto ansi sealed tagAMTunerSubChannel
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400018B*/ public specialname rtspecialname int32 value__
    .field /*0400018C*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerSubChannel/* 020000AF */ AMTUNER_SUBCHAN_NO_TUNE = int32(0xFFFFFFFE)
    .field /*0400018D*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerSubChannel/* 020000AF */ AMTUNER_SUBCHAN_DEFAULT = int32(0xFFFFFFFF)
  } // end of class tagAMTunerSubChannel

  .class /*020000B0*/ public auto ansi sealed tagAMTunerSignalStrength
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400018E*/ public specialname rtspecialname int32 value__
    .field /*0400018F*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerSignalStrength/* 020000B0 */ AMTUNER_HASNOSIGNALSTRENGTH = int32(0xFFFFFFFF)
    .field /*04000190*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerSignalStrength/* 020000B0 */ AMTUNER_NOSIGNAL = int32(0x00000000)
    .field /*04000191*/ public static literal valuetype MSR.LST.MDShow.tagAMTunerSignalStrength/* 020000B0 */ AMTUNER_SIGNALPRESENT = int32(0x00000001)
  } // end of class tagAMTunerSignalStrength

  .class /*020000B1*/ interface public abstract auto ansi import IBPCSatelliteTuner
         implements MSR.LST.MDShow.IAMTuner/* 02000069 */
  {
    .custom /*0C000103:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000104:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 31 31 41 38 37 36 35 2D 30 33 41 43   // ..$211A8765-03AC
                                                                                                                                                                     2D 31 31 44 31 2D 38 44 31 33 2D 30 30 41 41 30   // -11D1-8D13-00AA0
                                                                                                                                                                     30 42 44 38 33 33 39 00 00 )                      // 0BD8339..
    .method /*06000240*/ public hidebysig newslot abstract virtual 
            instance void  put_Channel([in] int32 lChannel,
                                       [in] int32 lVideoSubChannel,
                                       [in] int32 lAudioSubChannel) runtime managed internalcall
    // SIG: 20 03 01 08 08 08
    {
    } // end of method IBPCSatelliteTuner::put_Channel

    .method /*06000241*/ public hidebysig newslot abstract virtual 
            instance void  get_Channel([out] int32& plChannel,
                                       [out] int32& plVideoSubChannel,
                                       [out] int32& plAudioSubChannel) runtime managed internalcall
    // SIG: 20 03 01 10 08 10 08 10 08
    {
    } // end of method IBPCSatelliteTuner::get_Channel

    .method /*06000242*/ public hidebysig newslot abstract virtual 
            instance void  ChannelMinMax([out] int32& lChannelMin,
                                         [out] int32& lChannelMax) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 08
    {
    } // end of method IBPCSatelliteTuner::ChannelMinMax

    .method /*06000243*/ public hidebysig newslot abstract virtual 
            instance void  put_CountryCode([in] int32 lCountryCode) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IBPCSatelliteTuner::put_CountryCode

    .method /*06000244*/ public hidebysig newslot abstract virtual 
            instance void  get_CountryCode([out] int32& plCountryCode) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IBPCSatelliteTuner::get_CountryCode

    .method /*06000245*/ public hidebysig newslot abstract virtual 
            instance void  put_TuningSpace([in] int32 lTuningSpace) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IBPCSatelliteTuner::put_TuningSpace

    .method /*06000246*/ public hidebysig newslot abstract virtual 
            instance void  get_TuningSpace([out] int32& plTuningSpace) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IBPCSatelliteTuner::get_TuningSpace

    .method /*06000247*/ public hidebysig newslot abstract virtual 
            instance void  Logon([in] native int hCurrentUser) runtime managed internalcall
    // SIG: 20 01 01 18
    {
    } // end of method IBPCSatelliteTuner::Logon

    .method /*06000248*/ public hidebysig newslot abstract virtual 
            instance void  Logout() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IBPCSatelliteTuner::Logout

    .method /*06000249*/ public hidebysig newslot abstract virtual 
            instance void  SignalPresent([out] int32& plSignalStrength) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IBPCSatelliteTuner::SignalPresent

    .method /*0600024A*/ public hidebysig newslot abstract virtual 
            instance void  put_Mode([in] valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */ lMode) runtime managed internalcall
    // SIG: 20 01 01 11 81 A8
    {
    } // end of method IBPCSatelliteTuner::put_Mode

    .method /*0600024B*/ public hidebysig newslot abstract virtual 
            instance void  get_Mode([out] valuetype MSR.LST.MDShow.tagAMTunerModeType/* 0200006A */& plMode) runtime managed internalcall
    // SIG: 20 01 01 10 11 81 A8
    {
    } // end of method IBPCSatelliteTuner::get_Mode

    .method /*0600024C*/ public hidebysig newslot abstract virtual 
            instance void  GetAvailableModes([out] int32& plModes) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IBPCSatelliteTuner::GetAvailableModes

    .method /*0600024D*/ public hidebysig newslot abstract virtual 
            instance void  RegisterNotificationCallBack([in] class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify,
                                                        [in] int32 lEvents) runtime managed internalcall
    // SIG: 20 02 01 12 81 AC 08
    {
    } // end of method IBPCSatelliteTuner::RegisterNotificationCallBack

    .method /*0600024E*/ public hidebysig newslot abstract virtual 
            instance void  UnRegisterNotificationCallBack([in] class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify) runtime managed internalcall
    // SIG: 20 01 01 12 81 AC
    {
    } // end of method IBPCSatelliteTuner::UnRegisterNotificationCallBack

    .method /*0600024F*/ public hidebysig newslot abstract virtual 
            instance void  get_DefaultSubChannelTypes([out] int32& plDefaultVideoType,
                                                      [out] int32& plDefaultAudioType) runtime managed internalcall
    // SIG: 20 02 01 10 08 10 08
    {
    } // end of method IBPCSatelliteTuner::get_DefaultSubChannelTypes

    .method /*06000250*/ public hidebysig newslot abstract virtual 
            instance void  put_DefaultSubChannelTypes([in] int32 lDefaultVideoType,
                                                      [in] int32 lDefaultAudioType) runtime managed internalcall
    // SIG: 20 02 01 08 08
    {
    } // end of method IBPCSatelliteTuner::put_DefaultSubChannelTypes

    .method /*06000251*/ public hidebysig newslot abstract virtual 
            instance void  IsTapingPermitted() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IBPCSatelliteTuner::IsTapingPermitted

  } // end of class IBPCSatelliteTuner

  .class /*020000B2*/ public auto ansi sealed tagTVAudioMode
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000192*/ public specialname rtspecialname int32 value__
    .field /*04000193*/ public static literal valuetype MSR.LST.MDShow.tagTVAudioMode/* 020000B2 */ AMTVAUDIO_MODE_MONO = int32(0x00000001)
    .field /*04000194*/ public static literal valuetype MSR.LST.MDShow.tagTVAudioMode/* 020000B2 */ AMTVAUDIO_MODE_STEREO = int32(0x00000002)
    .field /*04000195*/ public static literal valuetype MSR.LST.MDShow.tagTVAudioMode/* 020000B2 */ AMTVAUDIO_MODE_LANG_A = int32(0x00000010)
    .field /*04000196*/ public static literal valuetype MSR.LST.MDShow.tagTVAudioMode/* 020000B2 */ AMTVAUDIO_MODE_LANG_B = int32(0x00000020)
    .field /*04000197*/ public static literal valuetype MSR.LST.MDShow.tagTVAudioMode/* 020000B2 */ AMTVAUDIO_MODE_LANG_C = int32(0x00000040)
  } // end of class tagTVAudioMode

  .class /*020000B3*/ public auto ansi sealed tagAMTVAudioEventType
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*04000198*/ public specialname rtspecialname int32 value__
    .field /*04000199*/ public static literal valuetype MSR.LST.MDShow.tagAMTVAudioEventType/* 020000B3 */ AMTVAUDIO_EVENT_CHANGED = int32(0x00000001)
  } // end of class tagAMTVAudioEventType

  .class /*020000B4*/ interface public abstract auto ansi import IAMTVAudioNotification
  {
    .custom /*0C000105:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000106:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 38 33 45 43 31 43 33 33 2D 32 33 44 31   // ..$83EC1C33-23D1
                                                                                                                                                                     2D 31 31 44 31 2D 39 39 45 36 2D 30 30 41 30 43   // -11D1-99E6-00A0C
                                                                                                                                                                     39 35 36 30 32 36 36 00 00 )                      // 9560266..
    .method /*06000252*/ public hidebysig newslot abstract virtual 
            instance void  OnEvent([in] valuetype MSR.LST.MDShow.tagAMTVAudioEventType/* 020000B3 */ Event) runtime managed internalcall
    // SIG: 20 01 01 11 82 CC
    {
    } // end of method IAMTVAudioNotification::OnEvent

  } // end of class IAMTVAudioNotification

  .class /*020000B5*/ interface public abstract auto ansi import IAMTVAudio
  {
    .custom /*0C000107:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 38 33 45 43 31 43 33 30 2D 32 33 44 31   // ..$83EC1C30-23D1
                                                                                                                                                                     2D 31 31 44 31 2D 39 39 45 36 2D 30 30 41 30 43   // -11D1-99E6-00A0C
                                                                                                                                                                     39 35 36 30 32 36 36 00 00 )                      // 9560266..
    .custom /*0C000108:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000253*/ public hidebysig newslot abstract virtual 
            instance void  GetHardwareSupportedTVAudioModes([out] int32& plModes) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVAudio::GetHardwareSupportedTVAudioModes

    .method /*06000254*/ public hidebysig newslot abstract virtual 
            instance void  GetAvailableTVAudioModes([out] int32& plModes) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVAudio::GetAvailableTVAudioModes

    .method /*06000255*/ public hidebysig newslot abstract virtual 
            instance void  get_TVAudioMode([out] int32& plMode) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IAMTVAudio::get_TVAudioMode

    .method /*06000256*/ public hidebysig newslot abstract virtual 
            instance void  put_TVAudioMode([in] int32 lMode) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IAMTVAudio::put_TVAudioMode

    .method /*06000257*/ public hidebysig newslot abstract virtual 
            instance void  RegisterNotificationCallBack([in] class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify,
                                                        [in] int32 lEvents) runtime managed internalcall
    // SIG: 20 02 01 12 81 AC 08
    {
    } // end of method IAMTVAudio::RegisterNotificationCallBack

    .method /*06000258*/ public hidebysig newslot abstract virtual 
            instance void  UnRegisterNotificationCallBack(class MSR.LST.MDShow.IAMTunerNotification/* 0200006B */  marshal( interface) pNotify) runtime managed internalcall
    // SIG: 20 01 01 12 81 AC
    {
    } // end of method IAMTVAudio::UnRegisterNotificationCallBack

  } // end of class IAMTVAudio

  .class /*020000B6*/ public auto ansi sealed AMPROPERTY_PIN
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400019A*/ public specialname rtspecialname int32 value__
    .field /*0400019B*/ public static literal valuetype MSR.LST.MDShow.AMPROPERTY_PIN/* 020000B6 */ AMPROPERTY_PIN_CATEGORY = int32(0x00000000)
    .field /*0400019C*/ public static literal valuetype MSR.LST.MDShow.AMPROPERTY_PIN/* 020000B6 */ AMPROPERTY_PIN_MEDIUM = int32(0x00000001)
  } // end of class AMPROPERTY_PIN

  .class /*020000B7*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0184_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*0400019D*/ public specialname rtspecialname int32 value__
    .field /*0400019E*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0184_0001/* 020000B7 */ AMPROPERTY_PIN_CATEGORY = int32(0x00000000)
    .field /*0400019F*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0184_0001/* 020000B7 */ AMPROPERTY_PIN_MEDIUM = int32(0x00000001)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0184_0001

  .class /*020000B8*/ interface public abstract auto ansi import IMediaPropertyBag
         implements MSR.LST.MDShow.IPropertyBag/* 020000B9 */
  {
    .custom /*0C000109:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00010A:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 36 30 32 35 41 38 38 30 2D 43 30 44 35   // ..$6025A880-C0D5
                                                                                                                                                                     2D 31 31 44 30 2D 42 44 34 45 2D 30 30 41 30 43   // -11D0-BD4E-00A0C
                                                                                                                                                                     39 31 31 43 45 38 36 00 00 )                      // 911CE86..
    .method /*06000259*/ public hidebysig newslot abstract virtual 
            instance void  RemoteRead([in] string  marshal( lpwstr) pszPropName,
                                      [out] object&  marshal( struct) pVar,
                                      [in] class MSR.LST.MDShow.IErrorLog/* 020000BA */  marshal( interface) pErrorLog,
                                      [in] unsigned int32 varType,
                                      [in] object  marshal( iunknown) pUnkObj) runtime managed internalcall
    // SIG: 20 05 01 0E 10 1C 12 82 E8 09 1C
    {
    } // end of method IMediaPropertyBag::RemoteRead

    .method /*0600025A*/ public hidebysig newslot abstract virtual 
            instance void  Write([in] string  marshal( lpwstr) pszPropName,
                                 [in] object&  marshal( struct) pVar) runtime managed internalcall
    // SIG: 20 02 01 0E 10 1C
    {
    } // end of method IMediaPropertyBag::Write

    .method /*0600025B*/ public hidebysig newslot abstract virtual 
            instance void  EnumProperty([in] unsigned int32 iProperty,
                                        [in][out] object&  marshal( struct) pvarPropertyName,
                                        [in][out] object&  marshal( struct) pvarPropertyValue) runtime managed internalcall
    // SIG: 20 03 01 09 10 1C 10 1C
    {
    } // end of method IMediaPropertyBag::EnumProperty

  } // end of class IMediaPropertyBag

  .class /*020000B9*/ interface public abstract auto ansi import IPropertyBag
  {
    .custom /*0C00010B:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00010C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 35 32 37 32 41 30 30 2D 34 32 43 42   // ..$55272A00-42CB
                                                                                                                                                                     2D 31 31 43 45 2D 38 31 33 35 2D 30 30 41 41 30   // -11CE-8135-00AA0
                                                                                                                                                                     30 34 42 42 38 35 31 00 00 )                      // 04BB851..
    .method /*0600025C*/ public hidebysig newslot abstract virtual 
            instance void  RemoteRead([in] string  marshal( lpwstr) pszPropName,
                                      [out] object&  marshal( struct) pVar,
                                      [in] class MSR.LST.MDShow.IErrorLog/* 020000BA */  marshal( interface) pErrorLog,
                                      [in] unsigned int32 varType,
                                      [in] object  marshal( iunknown) pUnkObj) runtime managed internalcall
    // SIG: 20 05 01 0E 10 1C 12 82 E8 09 1C
    {
    } // end of method IPropertyBag::RemoteRead

    .method /*0600025D*/ public hidebysig newslot abstract virtual 
            instance void  Write([in] string  marshal( lpwstr) pszPropName,
                                 [in] object&  marshal( struct) pVar) runtime managed internalcall
    // SIG: 20 02 01 0E 10 1C
    {
    } // end of method IPropertyBag::Write

  } // end of class IPropertyBag

  .class /*020000BA*/ interface public abstract auto ansi import IErrorLog
  {
    .custom /*0C00010D:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 33 31 32 37 43 41 34 30 2D 34 34 36 45   // ..$3127CA40-446E
                                                                                                                                                                     2D 31 31 43 45 2D 38 31 33 35 2D 30 30 41 41 30   // -11CE-8135-00AA0
                                                                                                                                                                     30 34 42 42 38 35 31 00 00 )                      // 04BB851..
    .custom /*0C00010E:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600025E*/ public hidebysig newslot abstract virtual 
            instance void  AddError([in] string  marshal( lpwstr) pszPropName,
                                    [in] valuetype [stdole/* 23000002 */]stdole.EXCEPINFO/* 01000009 */& pExcepInfo) runtime managed internalcall
    // SIG: 20 02 01 0E 10 11 25
    {
    } // end of method IErrorLog::AddError

  } // end of class IErrorLog

  .class /*020000BB*/ interface public abstract auto ansi import IPersistMediaPropertyBag
         implements MSR.LST.MDShow.IPersist/* 02000006 */
  {
    .custom /*0C00010F:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000110:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 37 33 38 45 30 34 30 2D 42 36 37 46   // ..$5738E040-B67F
                                                                                                                                                                     2D 31 31 44 30 2D 42 44 34 44 2D 30 30 41 30 43   // -11D0-BD4D-00A0C
                                                                                                                                                                     39 31 31 43 45 38 36 00 00 )                      // 911CE86..
    .method /*0600025F*/ public hidebysig newslot abstract virtual 
            instance void  GetClassID([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pClassID) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method IPersistMediaPropertyBag::GetClassID

    .method /*06000260*/ public hidebysig newslot abstract virtual 
            instance void  InitNew() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IPersistMediaPropertyBag::InitNew

    .method /*06000261*/ public hidebysig newslot abstract virtual 
            instance void  Load([in] class MSR.LST.MDShow.IMediaPropertyBag/* 020000B8 */  marshal( interface) pPropBag,
                                [in] class MSR.LST.MDShow.IErrorLog/* 020000BA */  marshal( interface) pErrorLog) runtime managed internalcall
    // SIG: 20 02 01 12 82 E0 12 82 E8
    {
    } // end of method IPersistMediaPropertyBag::Load

    .method /*06000262*/ public hidebysig newslot abstract virtual 
            instance void  Save([in] class MSR.LST.MDShow.IMediaPropertyBag/* 020000B8 */  marshal( interface) pPropBag,
                                [in] int32 fClearDirty,
                                [in] int32 fSaveAllProperties) runtime managed internalcall
    // SIG: 20 03 01 12 82 E0 08 08
    {
    } // end of method IPersistMediaPropertyBag::Save

  } // end of class IPersistMediaPropertyBag

  .class /*020000BC*/ public auto ansi sealed _AMSTREAMSELECTINFOFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001A0*/ public specialname rtspecialname int32 value__
    .field /*040001A1*/ public static literal valuetype MSR.LST.MDShow._AMSTREAMSELECTINFOFLAGS/* 020000BC */ AMSTREAMSELECTINFO_ENABLED = int32(0x00000001)
    .field /*040001A2*/ public static literal valuetype MSR.LST.MDShow._AMSTREAMSELECTINFOFLAGS/* 020000BC */ AMSTREAMSELECTINFO_EXCLUSIVE = int32(0x00000002)
  } // end of class _AMSTREAMSELECTINFOFLAGS

  .class /*020000BD*/ public auto ansi sealed _AMSTREAMSELECTENABLEFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001A3*/ public specialname rtspecialname int32 value__
    .field /*040001A4*/ public static literal valuetype MSR.LST.MDShow._AMSTREAMSELECTENABLEFLAGS/* 020000BD */ AMSTREAMSELECTENABLE_ENABLE = int32(0x00000001)
    .field /*040001A5*/ public static literal valuetype MSR.LST.MDShow._AMSTREAMSELECTENABLEFLAGS/* 020000BD */ AMSTREAMSELECTENABLE_ENABLEALL = int32(0x00000002)
  } // end of class _AMSTREAMSELECTENABLEFLAGS

  .class /*020000BE*/ interface public abstract auto ansi import IAMStreamSelect
  {
    .custom /*0C000111:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000112:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C000113:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 31 39 36 30 39 36 30 2D 31 37 46 35   // ..$C1960960-17F5
                                                                                                                                                                     2D 31 31 44 31 2D 41 42 45 31 2D 30 30 41 30 43   // -11D1-ABE1-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .method /*06000263*/ public hidebysig newslot abstract virtual 
            instance void  Count([out] unsigned int32& pcStreams) runtime managed internalcall
    // SIG: 20 01 01 10 09
    {
    } // end of method IAMStreamSelect::Count

    .method /*06000264*/ public hidebysig newslot abstract virtual 
            instance void  Info([in] int32 lIndex,
                                [out] native int ppmt,
                                [out] unsigned int32& pdwFlags,
                                [out] unsigned int32& plcid,
                                [out] unsigned int32& pdwGroup,
                                [out] native int ppszName,
                                [out] object&  marshal( iunknown) ppObject,
                                [out] object&  marshal( iunknown) ppunk) runtime managed internalcall
    // SIG: 20 08 01 08 18 10 09 10 09 10 09 18 10 1C 10 1C
    {
    } // end of method IAMStreamSelect::Info

    .method /*06000265*/ public hidebysig newslot abstract virtual 
            instance void  Enable([in] int32 lIndex,
                                  [in] unsigned int32 dwFlags) runtime managed internalcall
    // SIG: 20 02 01 08 09
    {
    } // end of method IAMStreamSelect::Enable

  } // end of class IAMStreamSelect

  .class /*020000BF*/ public auto ansi sealed _AMRESCTL_RESERVEFLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001A6*/ public specialname rtspecialname int32 value__
    .field /*040001A7*/ public static literal valuetype MSR.LST.MDShow._AMRESCTL_RESERVEFLAGS/* 020000BF */ AMRESCTL_RESERVEFLAGS_RESERVE = int32(0x00000000)
    .field /*040001A8*/ public static literal valuetype MSR.LST.MDShow._AMRESCTL_RESERVEFLAGS/* 020000BF */ AMRESCTL_RESERVEFLAGS_UNRESERVE = int32(0x00000001)
  } // end of class _AMRESCTL_RESERVEFLAGS

  .class /*020000C0*/ interface public abstract auto ansi import IAMResourceControl
  {
    .custom /*0C000114:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000115:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 38 33 38 39 44 32 44 30 2D 37 37 44 37   // ..$8389D2D0-77D7
                                                                                                                                                                     2D 31 31 44 31 2D 41 42 45 36 2D 30 30 41 30 43   // -11D1-ABE6-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .custom /*0C000116:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*06000266*/ public hidebysig newslot abstract virtual 
            instance void  Reserve([in] unsigned int32 dwFlags,
                                   [in] native int pvReserved) runtime managed internalcall
    // SIG: 20 02 01 09 18
    {
    } // end of method IAMResourceControl::Reserve

  } // end of class IAMResourceControl

  .class /*020000C1*/ interface public abstract auto ansi import IAMClockAdjust
  {
    .custom /*0C000117:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 34 44 35 34 36 36 42 30 2D 41 34 39 43   // ..$4D5466B0-A49C
                                                                                                                                                                     2D 31 31 44 31 2D 41 42 45 38 2D 30 30 41 30 43   // -11D1-ABE8-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .custom /*0C000118:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000267*/ public hidebysig newslot abstract virtual 
            instance void  SetClockDelta([in] int64 rtDelta) runtime managed internalcall
    // SIG: 20 01 01 0A
    {
    } // end of method IAMClockAdjust::SetClockDelta

  } // end of class IAMClockAdjust

  .class /*020000C2*/ public auto ansi sealed _AM_FILTER_MISC_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001A9*/ public specialname rtspecialname int32 value__
    .field /*040001AA*/ public static literal valuetype MSR.LST.MDShow._AM_FILTER_MISC_FLAGS/* 020000C2 */ AM_FILTER_MISC_FLAGS_IS_RENDERER = int32(0x00000001)
    .field /*040001AB*/ public static literal valuetype MSR.LST.MDShow._AM_FILTER_MISC_FLAGS/* 020000C2 */ AM_FILTER_MISC_FLAGS_IS_SOURCE = int32(0x00000002)
  } // end of class _AM_FILTER_MISC_FLAGS

  .class /*020000C3*/ interface public abstract auto ansi import IAMFilterMiscFlags
  {
    .custom /*0C000119:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00011A:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 32 44 44 37 34 39 35 30 2D 41 38 39 30   // ..$2DD74950-A890
                                                                                                                                                                     2D 31 31 44 31 2D 41 42 45 38 2D 30 30 41 30 43   // -11D1-ABE8-00A0C
                                                                                                                                                                     39 30 35 46 33 37 35 00 00 )                      // 905F375..
    .method /*06000268*/ public hidebysig newslot abstract virtual 
            instance unsigned int32  GetMiscFlags() runtime managed preservesig internalcall
    // SIG: 20 00 09
    {
    } // end of method IAMFilterMiscFlags::GetMiscFlags

  } // end of class IAMFilterMiscFlags

  .class /*020000C4*/ interface public abstract auto ansi import IVideoFrameStep
  {
    .custom /*0C00011B:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00011C:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 45 34 36 41 39 37 38 37 2D 32 42 37 31   // ..$E46A9787-2B71
                                                                                                                                                                     2D 34 34 34 44 2D 41 34 42 35 2D 31 46 41 42 37   // -444D-A4B5-1FAB7
                                                                                                                                                                     42 37 30 38 44 36 41 00 00 )                      // B708D6A..
    .method /*06000269*/ public hidebysig newslot abstract virtual 
            instance void  Step(unsigned int32 dwFrames,
                                object  marshal( iunknown) pStepObject) runtime managed internalcall
    // SIG: 20 02 01 09 1C
    {
    } // end of method IVideoFrameStep::Step

    .method /*0600026A*/ public hidebysig newslot abstract virtual 
            instance void  CanStep(int32 bMultiple,
                                   object  marshal( iunknown) pStepObject) runtime managed internalcall
    // SIG: 20 02 01 08 1C
    {
    } // end of method IVideoFrameStep::CanStep

    .method /*0600026B*/ public hidebysig newslot abstract virtual 
            instance void  CancelStep() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IVideoFrameStep::CancelStep

  } // end of class IVideoFrameStep

  .class /*020000C5*/ public auto ansi sealed _AM_PUSHSOURCE_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001AC*/ public specialname rtspecialname int32 value__
    .field /*040001AD*/ public static literal valuetype MSR.LST.MDShow._AM_PUSHSOURCE_FLAGS/* 020000C5 */ AM_PUSHSOURCECAPS_INTERNAL_RM = int32(0x00000001)
    .field /*040001AE*/ public static literal valuetype MSR.LST.MDShow._AM_PUSHSOURCE_FLAGS/* 020000C5 */ AM_PUSHSOURCECAPS_NOT_LIVE = int32(0x00000002)
    .field /*040001AF*/ public static literal valuetype MSR.LST.MDShow._AM_PUSHSOURCE_FLAGS/* 020000C5 */ AM_PUSHSOURCECAPS_PRIVATE_CLOCK = int32(0x00000004)
    .field /*040001B0*/ public static literal valuetype MSR.LST.MDShow._AM_PUSHSOURCE_FLAGS/* 020000C5 */ AM_PUSHSOURCEREQS_USE_STREAM_CLOCK = int32(0x00010000)
  } // end of class _AM_PUSHSOURCE_FLAGS

  .class /*020000C6*/ interface public abstract auto ansi import IAMDeviceRemoval
  {
    .custom /*0C00011D:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C00011E:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 46 39 30 41 36 31 33 30 2D 42 36 35 38   // ..$F90A6130-B658
                                                                                                                                                                     2D 31 31 44 32 2D 41 45 34 39 2D 30 30 30 30 46   // -11D2-AE49-0000F
                                                                                                                                                                     38 37 35 34 42 39 39 00 00 )                      // 8754B99..
    .custom /*0C00011F:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*0600026C*/ public hidebysig newslot abstract virtual 
            instance void  DeviceInfo([out] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& pclsidInterfaceClass,
                                      [out] native int pwszSymbolicLink) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 18
    {
    } // end of method IAMDeviceRemoval::DeviceInfo

    .method /*0600026D*/ public hidebysig newslot abstract virtual 
            instance void  Reassociate() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMDeviceRemoval::Reassociate

    .method /*0600026E*/ public hidebysig newslot abstract virtual 
            instance void  Disassociate() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method IAMDeviceRemoval::Disassociate

  } // end of class IAMDeviceRemoval

  .class /*020000C7*/ public sequential ansi sealed beforefieldinit DVINFO
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040001B1*/ public unsigned int32 dwDVAAuxSrc
    .field /*040001B2*/ public unsigned int32 dwDVAAuxCtl
    .field /*040001B3*/ public unsigned int32 dwDVAAuxSrc1
    .field /*040001B4*/ public unsigned int32 dwDVAAuxCtl1
    .field /*040001B5*/ public unsigned int32 dwDVVAuxSrc
    .field /*040001B6*/ public unsigned int32 dwDVVAuxCtl
    .field /*040001B7*/ public  marshal( fixed array [2]) unsigned int32[] dwDVReserved
  } // end of class DVINFO

  .class /*020000C8*/ public sequential ansi sealed beforefieldinit __MIDL___MIDL_itf_devaxcorex2Einterop_0348_0001
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040001B8*/ public unsigned int32 dwDVAAuxSrc
    .field /*040001B9*/ public unsigned int32 dwDVAAuxCtl
    .field /*040001BA*/ public unsigned int32 dwDVAAuxSrc1
    .field /*040001BB*/ public unsigned int32 dwDVAAuxCtl1
    .field /*040001BC*/ public unsigned int32 dwDVVAuxSrc
    .field /*040001BD*/ public unsigned int32 dwDVVAuxCtl
    .field /*040001BE*/ public  marshal( fixed array [2]) unsigned int32[] dwDVReserved
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0348_0001

  .class /*020000C9*/ public auto ansi sealed _DVENCODERRESOLUTION
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001BF*/ public specialname rtspecialname int32 value__
    .field /*040001C0*/ public static literal valuetype MSR.LST.MDShow._DVENCODERRESOLUTION/* 020000C9 */ DVENCODERRESOLUTION_720x480 = int32(0x000007DC)
    .field /*040001C1*/ public static literal valuetype MSR.LST.MDShow._DVENCODERRESOLUTION/* 020000C9 */ DVENCODERRESOLUTION_360x240 = int32(0x000007DD)
    .field /*040001C2*/ public static literal valuetype MSR.LST.MDShow._DVENCODERRESOLUTION/* 020000C9 */ DVENCODERRESOLUTION_180x120 = int32(0x000007DE)
    .field /*040001C3*/ public static literal valuetype MSR.LST.MDShow._DVENCODERRESOLUTION/* 020000C9 */ DVENCODERRESOLUTION_88x60 = int32(0x000007DF)
  } // end of class _DVENCODERRESOLUTION

  .class /*020000CA*/ public auto ansi sealed _DVENCODERVIDEOFORMAT
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001C4*/ public specialname rtspecialname int32 value__
    .field /*040001C5*/ public static literal valuetype MSR.LST.MDShow._DVENCODERVIDEOFORMAT/* 020000CA */ DVENCODERVIDEOFORMAT_NTSC = int32(0x000007D0)
    .field /*040001C6*/ public static literal valuetype MSR.LST.MDShow._DVENCODERVIDEOFORMAT/* 020000CA */ DVENCODERVIDEOFORMAT_PAL = int32(0x000007D1)
  } // end of class _DVENCODERVIDEOFORMAT

  .class /*020000CB*/ public auto ansi sealed _DVENCODERFORMAT
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001C7*/ public specialname rtspecialname int32 value__
    .field /*040001C8*/ public static literal valuetype MSR.LST.MDShow._DVENCODERFORMAT/* 020000CB */ DVENCODERFORMAT_DVSD = int32(0x000007D7)
    .field /*040001C9*/ public static literal valuetype MSR.LST.MDShow._DVENCODERFORMAT/* 020000CB */ DVENCODERFORMAT_DVHD = int32(0x000007D8)
    .field /*040001CA*/ public static literal valuetype MSR.LST.MDShow._DVENCODERFORMAT/* 020000CB */ DVENCODERFORMAT_DVSL = int32(0x000007D9)
  } // end of class _DVENCODERFORMAT

  .class /*020000CC*/ interface public abstract auto ansi import IDVEnc
  {
    .custom /*0C000120:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000121:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 44 31 38 45 31 37 41 30 2D 41 41 43 42   // ..$D18E17A0-AACB
                                                                                                                                                                     2D 31 31 44 30 2D 41 46 42 30 2D 30 30 41 41 30   // -11D0-AFB0-00AA0
                                                                                                                                                                     30 42 36 37 41 34 32 00 00 )                      // 0B67A42..
    .method /*0600026F*/ public hidebysig newslot abstract virtual 
            instance void  get_IFormatResolution([out] int32& VideoFormat,
                                                 [out] int32& DVFormat,
                                                 [out] int32& Resolution,
                                                 [in] unsigned int8 fDVInfo,
                                                 [out] valuetype MSR.LST.MDShow.DVINFO/* 020000C7 */& sDVInfo) runtime managed internalcall
    // SIG: 20 05 01 10 08 10 08 10 08 05 10 11 83 1C
    {
      .param [5]/*08000428*/ 
      .custom /*0C00014F:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 15 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 44 56 49 4E 46 4F 00 00 )                   // w.DVINFO..
    } // end of method IDVEnc::get_IFormatResolution

    .method /*06000270*/ public hidebysig newslot abstract virtual 
            instance void  put_IFormatResolution([in] int32 VideoFormat,
                                                 [in] int32 DVFormat,
                                                 [in] int32 Resolution,
                                                 [in] unsigned int8 fDVInfo,
                                                 [in] valuetype MSR.LST.MDShow.DVINFO/* 020000C7 */& sDVInfo) runtime managed internalcall
    // SIG: 20 05 01 08 08 08 05 10 11 83 1C
    {
      .param [5]/*0800042D*/ 
      .custom /*0C000150:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 15 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 44 56 49 4E 46 4F 00 00 )                   // w.DVINFO..
    } // end of method IDVEnc::put_IFormatResolution

  } // end of class IDVEnc

  .class /*020000CD*/ public auto ansi sealed _DVDECODERRESOLUTION
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001CB*/ public specialname rtspecialname int32 value__
    .field /*040001CC*/ public static literal valuetype MSR.LST.MDShow._DVDECODERRESOLUTION/* 020000CD */ DVDECODERRESOLUTION_720x480 = int32(0x000003E8)
    .field /*040001CD*/ public static literal valuetype MSR.LST.MDShow._DVDECODERRESOLUTION/* 020000CD */ DVDECODERRESOLUTION_360x240 = int32(0x000003E9)
    .field /*040001CE*/ public static literal valuetype MSR.LST.MDShow._DVDECODERRESOLUTION/* 020000CD */ DVDECODERRESOLUTION_180x120 = int32(0x000003EA)
    .field /*040001CF*/ public static literal valuetype MSR.LST.MDShow._DVDECODERRESOLUTION/* 020000CD */ DVDECODERRESOLUTION_88x60 = int32(0x000003EB)
  } // end of class _DVDECODERRESOLUTION

  .class /*020000CE*/ public auto ansi sealed _DVRESOLUTION
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001D0*/ public specialname rtspecialname int32 value__
    .field /*040001D1*/ public static literal valuetype MSR.LST.MDShow._DVRESOLUTION/* 020000CE */ DVRESOLUTION_FULL = int32(0x000003E8)
    .field /*040001D2*/ public static literal valuetype MSR.LST.MDShow._DVRESOLUTION/* 020000CE */ DVRESOLUTION_HALF = int32(0x000003E9)
    .field /*040001D3*/ public static literal valuetype MSR.LST.MDShow._DVRESOLUTION/* 020000CE */ DVRESOLUTION_QUARTER = int32(0x000003EA)
    .field /*040001D4*/ public static literal valuetype MSR.LST.MDShow._DVRESOLUTION/* 020000CE */ DVRESOLUTION_DC = int32(0x000003EB)
  } // end of class _DVRESOLUTION

  .class /*020000CF*/ interface public abstract auto ansi import IIPDVDec
  {
    .custom /*0C000122:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000123:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 42 38 45 38 42 44 36 30 2D 30 42 46 45   // ..$B8E8BD60-0BFE
                                                                                                                                                                     2D 31 31 44 30 2D 41 46 39 31 2D 30 30 41 41 30   // -11D0-AF91-00AA0
                                                                                                                                                                     30 42 36 37 41 34 32 00 00 )                      // 0B67A42..
    .method /*06000271*/ public hidebysig newslot abstract virtual 
            instance void  get_IPDisplay([out] int32& displayPix) runtime managed internalcall
    // SIG: 20 01 01 10 08
    {
    } // end of method IIPDVDec::get_IPDisplay

    .method /*06000272*/ public hidebysig newslot abstract virtual 
            instance void  put_IPDisplay([in] int32 displayPix) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IIPDVDec::put_IPDisplay

  } // end of class IIPDVDec

  .class /*020000D0*/ interface public abstract auto ansi import IDVRGB219
  {
    .custom /*0C000124:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 35 38 34 37 33 41 31 39 2D 32 42 43 38   // ..$58473A19-2BC8
                                                                                                                                                                     2D 34 36 36 33 2D 38 30 31 32 2D 32 35 46 38 31   // -4663-8012-25F81
                                                                                                                                                                     42 41 42 44 44 44 31 00 00 )                      // BABDDD1..
    .custom /*0C000125:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000273*/ public hidebysig newslot abstract virtual 
            instance void  SetRGB219([in] int32 bState) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IDVRGB219::SetRGB219

  } // end of class IDVRGB219

  .class /*020000D1*/ interface public abstract auto ansi import IDVSplitter
  {
    .custom /*0C000126:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 32 41 33 41 33 30 32 2D 44 41 37 43   // ..$92A3A302-DA7C
                                                                                                                                                                     2D 34 41 31 46 2D 42 41 37 45 2D 31 38 30 32 42   // -4A1F-BA7E-1802B
                                                                                                                                                                     42 35 44 32 44 30 32 00 00 )                      // B5D2D02..
    .custom /*0C000127:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000274*/ public hidebysig newslot abstract virtual 
            instance void  DiscardAlternateVideoFrames([in] int32 nDiscard) runtime managed internalcall
    // SIG: 20 01 01 08
    {
    } // end of method IDVSplitter::DiscardAlternateVideoFrames

  } // end of class IDVSplitter

  .class /*020000D2*/ public auto ansi sealed _AM_AUDIO_RENDERER_STAT_PARAM
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001D5*/ public specialname rtspecialname int32 value__
    .field /*040001D6*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_BREAK_COUNT = int32(0x00000001)
    .field /*040001D7*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_SLAVE_MODE = int32(0x00000002)
    .field /*040001D8*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_SILENCE_DUR = int32(0x00000003)
    .field /*040001D9*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_LAST_BUFFER_DUR = int32(0x00000004)
    .field /*040001DA*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_DISCONTINUITIES = int32(0x00000005)
    .field /*040001DB*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_SLAVE_RATE = int32(0x00000006)
    .field /*040001DC*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_SLAVE_DROPWRITE_DUR = int32(0x00000007)
    .field /*040001DD*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_SLAVE_HIGHLOWERROR = int32(0x00000008)
    .field /*040001DE*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_SLAVE_LASTHIGHLOWERROR = int32(0x00000009)
    .field /*040001DF*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_SLAVE_ACCUMERROR = int32(0x0000000A)
    .field /*040001E0*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_BUFFERFULLNESS = int32(0x0000000B)
    .field /*040001E1*/ public static literal valuetype MSR.LST.MDShow._AM_AUDIO_RENDERER_STAT_PARAM/* 020000D2 */ AM_AUDREND_STAT_PARAM_JITTER = int32(0x0000000C)
  } // end of class _AM_AUDIO_RENDERER_STAT_PARAM

  .class /*020000D3*/ public auto ansi sealed _AM_INTF_SEARCH_FLAGS
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001E2*/ public specialname rtspecialname int32 value__
    .field /*040001E3*/ public static literal valuetype MSR.LST.MDShow._AM_INTF_SEARCH_FLAGS/* 020000D3 */ AM_INTF_SEARCH_INPUT_PIN = int32(0x00000001)
    .field /*040001E4*/ public static literal valuetype MSR.LST.MDShow._AM_INTF_SEARCH_FLAGS/* 020000D3 */ AM_INTF_SEARCH_OUTPUT_PIN = int32(0x00000002)
    .field /*040001E5*/ public static literal valuetype MSR.LST.MDShow._AM_INTF_SEARCH_FLAGS/* 020000D3 */ AM_INTF_SEARCH_FILTER = int32(0x00000004)
  } // end of class _AM_INTF_SEARCH_FLAGS

  .class /*020000D4*/ public auto ansi sealed AMOVERLAYFX
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001E6*/ public specialname rtspecialname int32 value__
    .field /*040001E7*/ public static literal valuetype MSR.LST.MDShow.AMOVERLAYFX/* 020000D4 */ AMOVERFX_NOFX = int32(0x00000000)
    .field /*040001E8*/ public static literal valuetype MSR.LST.MDShow.AMOVERLAYFX/* 020000D4 */ AMOVERFX_MIRRORLEFTRIGHT = int32(0x00000002)
    .field /*040001E9*/ public static literal valuetype MSR.LST.MDShow.AMOVERLAYFX/* 020000D4 */ AMOVERFX_MIRRORUPDOWN = int32(0x00000004)
    .field /*040001EA*/ public static literal valuetype MSR.LST.MDShow.AMOVERLAYFX/* 020000D4 */ AMOVERFX_DEINTERLACE = int32(0x00000008)
  } // end of class AMOVERLAYFX

  .class /*020000D5*/ interface public abstract auto ansi import IRegisterServiceProvider
  {
    .custom /*0C000128:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 37 42 33 41 32 46 30 31 2D 30 37 35 31   // ..$7B3A2F01-0751
                                                                                                                                                                     2D 34 38 44 44 2D 42 35 35 36 2D 30 30 34 37 38   // -48DD-B556-00478
                                                                                                                                                                     35 31 37 31 43 35 34 00 00 )                      // 5171C54..
    .custom /*0C000129:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000275*/ public hidebysig newslot abstract virtual 
            instance void  RegisterService([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& guidService,
                                           [in] object  marshal( iunknown) punkObject) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 1C
    {
    } // end of method IRegisterServiceProvider::RegisterService

  } // end of class IRegisterServiceProvider

  .class /*020000D6*/ public sequential ansi sealed beforefieldinit CodecAPIEventData
         extends [mscorlib/* 23000001 */]System.ValueType/* 01000005 */
  {
    .pack 4
    .size 0
    .field /*040001EB*/ public valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */ guid
    .field /*040001EC*/ public unsigned int32 dataLength
    .field /*040001ED*/ public  marshal( fixed array [3]) unsigned int32[] reserved
  } // end of class CodecAPIEventData

  .class /*020000D7*/ interface public abstract auto ansi import ICodecAPI
  {
    .custom /*0C00012A:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 39 30 31 44 42 34 43 37 2D 33 31 43 45   // ..$901DB4C7-31CE
                                                                                                                                                                     2D 34 31 41 32 2D 38 35 44 43 2D 38 46 41 30 42   // -41A2-85DC-8FA0B
                                                                                                                                                                     46 34 31 42 38 44 41 00 00 )                      // F41B8DA..
    .custom /*0C00012B:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .custom /*0C00012C:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .method /*06000276*/ public hidebysig newslot abstract virtual 
            instance void  IsSupported([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method ICodecAPI::IsSupported

    .method /*06000277*/ public hidebysig newslot abstract virtual 
            instance void  IsModifiable([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method ICodecAPI::IsModifiable

    .method /*06000278*/ public hidebysig newslot abstract virtual 
            instance void  GetParameterRange([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                             [out] object&  marshal( struct) ValueMin,
                                             [out] object&  marshal( struct) ValueMax,
                                             [out] object&  marshal( struct) SteppingDelta) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 10 1C 10 1C 10 1C
    {
    } // end of method ICodecAPI::GetParameterRange

    .method /*06000279*/ public hidebysig newslot abstract virtual 
            instance void  GetParameterValues([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                              [out] native int Values,
                                              [out] unsigned int32& ValuesCount) runtime managed internalcall
    // SIG: 20 03 01 10 11 11 18 10 09
    {
    } // end of method ICodecAPI::GetParameterValues

    .method /*0600027A*/ public hidebysig newslot abstract virtual 
            instance void  GetDefaultValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                           [out] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method ICodecAPI::GetDefaultValue

    .method /*0600027B*/ public hidebysig newslot abstract virtual 
            instance void  GetValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                    [out] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method ICodecAPI::GetValue

    .method /*0600027C*/ public hidebysig newslot abstract virtual 
            instance void  SetValue([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                    [in] object&  marshal( struct) Value) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 10 1C
    {
    } // end of method ICodecAPI::SetValue

    .method /*0600027D*/ public hidebysig newslot abstract virtual 
            instance void  RegisterForEvent([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                            [in] int32 userData) runtime managed internalcall
    // SIG: 20 02 01 10 11 11 08
    {
      .param [2]/*08000444*/ 
      .custom /*0C000151:0A000004*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComAliasNameAttribute/* 01000008 */::.ctor(string) /* 0A000004 */ = ( 01 00 17 4D 53 52 2E 4C 53 54 2E 4D 44 53 68 6F   // ...MSR.LST.MDSho
                                                                                                                                                                               77 2E 4C 4F 4E 47 5F 50 54 52 00 00 )             // w.LONG_PTR..
    } // end of method ICodecAPI::RegisterForEvent

    .method /*0600027E*/ public hidebysig newslot abstract virtual 
            instance void  UnregisterForEvent([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api) runtime managed internalcall
    // SIG: 20 01 01 10 11 11
    {
    } // end of method ICodecAPI::UnregisterForEvent

    .method /*0600027F*/ public hidebysig newslot abstract virtual 
            instance void  SetAllDefaults() runtime managed internalcall
    // SIG: 20 00 01
    {
    } // end of method ICodecAPI::SetAllDefaults

    .method /*06000280*/ public hidebysig newslot abstract virtual 
            instance void  SetValueWithNotify([in] valuetype [mscorlib/* 23000001 */]System.Guid/* 01000004 */& Api,
                                              [in] object&  marshal( struct) Value,
                                              [out] native int ChangedParam,
                                              [out] unsigned int32& ChangedParamCount) runtime managed internalcall
    // SIG: 20 04 01 10 11 11 10 1C 18 10 09
    {
    } // end of method ICodecAPI::SetValueWithNotify

    .method /*06000281*/ public hidebysig newslot abstract virtual 
            instance void  SetAllDefaultsWithNotify([out] native int ChangedParam,
                                                    [out] unsigned int32& ChangedParamCount) runtime managed internalcall
    // SIG: 20 02 01 18 10 09
    {
    } // end of method ICodecAPI::SetAllDefaultsWithNotify

    .method /*06000282*/ public hidebysig newslot abstract virtual 
            instance void  GetAllSettings([in] class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) __MIDL_0016) runtime managed internalcall
    // SIG: 20 01 01 12 1C
    {
    } // end of method ICodecAPI::GetAllSettings

    .method /*06000283*/ public hidebysig newslot abstract virtual 
            instance void  SetAllSettings([in] class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) __MIDL_0017) runtime managed internalcall
    // SIG: 20 01 01 12 1C
    {
    } // end of method ICodecAPI::SetAllSettings

    .method /*06000284*/ public hidebysig newslot abstract virtual 
            instance void  SetAllSettingsWithNotify(class MSR.LST.MDShow.IStream/* 02000007 */  marshal( interface) __MIDL_0018,
                                                    [out] native int ChangedParam,
                                                    [out] unsigned int32& ChangedParamCount) runtime managed internalcall
    // SIG: 20 03 01 12 1C 18 10 09
    {
    } // end of method ICodecAPI::SetAllSettingsWithNotify

  } // end of class ICodecAPI

  .class /*020000D8*/ interface public abstract auto ansi import IGetCapabilitiesKey
  {
    .custom /*0C00012D:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 41 38 38 30 39 32 32 32 2D 30 37 42 42   // ..$A8809222-07BB
                                                                                                                                                                     2D 34 38 45 41 2D 39 35 31 43 2D 33 33 31 35 38   // -48EA-951C-33158
                                                                                                                                                                     31 30 30 36 32 35 42 00 00 )                      // 100625B..
    .custom /*0C00012E:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C00012F:0A000003*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.ComConversionLossAttribute/* 01000006 */::.ctor() /* 0A000003 */ = ( 01 00 00 00 ) 
    .method /*06000285*/ public hidebysig newslot abstract virtual 
            instance void  GetCapabilitiesKey([out] native int& pHKey) runtime managed internalcall
    // SIG: 20 01 01 10 18
    {
    } // end of method IGetCapabilitiesKey::GetCapabilitiesKey

  } // end of class IGetCapabilitiesKey

  .class /*020000D9*/ public auto ansi sealed VIDEOENCODER_BITRATE_MODE
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001EE*/ public specialname rtspecialname int32 value__
    .field /*040001EF*/ public static literal valuetype MSR.LST.MDShow.VIDEOENCODER_BITRATE_MODE/* 020000D9 */ ConstantBitRate = int32(0x00000000)
    .field /*040001F0*/ public static literal valuetype MSR.LST.MDShow.VIDEOENCODER_BITRATE_MODE/* 020000D9 */ VariableBitRateAverage = int32(0x00000001)
    .field /*040001F1*/ public static literal valuetype MSR.LST.MDShow.VIDEOENCODER_BITRATE_MODE/* 020000D9 */ VariableBitRatePeak = int32(0x00000002)
  } // end of class VIDEOENCODER_BITRATE_MODE

  .class /*020000DA*/ public auto ansi sealed __MIDL___MIDL_itf_devaxcorex2Einterop_0367_0001
         extends [mscorlib/* 23000001 */]System.Enum/* 01000007 */
  {
    .field /*040001F2*/ public specialname rtspecialname int32 value__
    .field /*040001F3*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0367_0001/* 020000DA */ ConstantBitRate = int32(0x00000000)
    .field /*040001F4*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0367_0001/* 020000DA */ VariableBitRateAverage = int32(0x00000001)
    .field /*040001F5*/ public static literal valuetype MSR.LST.MDShow.__MIDL___MIDL_itf_devaxcorex2Einterop_0367_0001/* 020000DA */ VariableBitRatePeak = int32(0x00000002)
  } // end of class __MIDL___MIDL_itf_devaxcorex2Einterop_0367_0001

  .class /*020000DB*/ interface public abstract auto ansi import IAMDecoderCaps
  {
    .custom /*0C000130:0A000002*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.InterfaceTypeAttribute/* 01000003 */::.ctor(int16) /* 0A000002 */ = ( 01 00 01 00 00 00 ) 
    .custom /*0C000131:0A000001*/ instance void [mscorlib/* 23000001 */]System.Runtime.InteropServices.GuidAttribute/* 01000002 */::.ctor(string) /* 0A000001 */ = ( 01 00 24 43 30 44 46 46 34 36 37 2D 44 34 39 39   // ..$C0DFF467-D499
                                                                                                                                                                     2D 34 39 38 36 2D 39 37 32 42 2D 45 31 44 39 30   // -4986-972B-E1D90
                                                                                                                                                                     39 30 46 41 39 34 31 00 00 )                      // 90FA941..
    .method /*06000286*/ public hidebysig newslot abstract virtual 
            instance void  GetDecoderCaps([in] unsigned int32 dwCapIndex,
                                          [out] unsigned int32& lpdwCap) runtime managed internalcall
    // SIG: 20 02 01 09 10 09
    {
    } // end of method IAMDecoderCaps::GetDecoderCaps

  } // end of class IAMDecoderCaps


// =============================================================

} // end of namespace MSR.LST.MDShow

//*********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file D:\sd\LST\Source\MSR.LST.MDShow\Interop\devaxcorex.interop.res
