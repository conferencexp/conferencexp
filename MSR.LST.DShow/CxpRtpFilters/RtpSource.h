#pragma once

#include "RtpSource_i.c"    // IID_RtpSource and CLSID_RtpSource
#include "RtpSource_h.h"    // IRtpSource
#include "vcclr.h"          // for gcroot

using namespace System;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;

using namespace MSR::LST;
using namespace MSR::LST::Net::Rtp;

class CRtpSource : public CSource, public IRtpSource
{
public:
    // COM
    DECLARE_IUNKNOWN;
    static CUnknown *WINAPI CreateInstance(LPUNKNOWN punk, HRESULT *phr);
    STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void ** ppv);

    // IRtpSource
    STDMETHODIMP Initialize(IUnknown* pRtpStream);

private:
    CRtpSource(LPUNKNOWN punk, HRESULT *phr);
    ~CRtpSource();

    gcroot<EventLog^> eventLog;
    CCritSec m_cSharedState;
};

class CRtpSourceStream : public CSourceStream
{
public:
    CRtpSourceStream(HRESULT *phr, CRtpSource *pParent, LPCWSTR pPinName);

    gcroot<RtpStream^> rtpStream;

    HRESULT FillBuffer(IMediaSample *pMediaSample);
    HRESULT DecideBufferSize(IMemAllocator *pIMemAlloc, ALLOCATOR_PROPERTIES *pProperties);
    HRESULT GetMediaType(CMediaType *pmt);
	HRESULT CheckMediaType(const CMediaType *pmt);

private:

    const static DWORD BUFFER_SIZE_DEFAULT = 250000;

    // Methods
    HRESULT SniffTheStream(void);
    
    void ReadHeader(BufferChunk^ frame);
    void ReadSampleProps(BufferChunk^ frame, UInt16& cbHeader);
    void ReadMediaType(BufferChunk^ frame, UInt16& cbHeader);

    // Values read from network
    // For order of data, see RtpRenderer.h
    AM_SAMPLE2_PROPERTIES m_RemoteSampleProps;
    CMediaType m_RemoteMediaType;

    DWORD m_bufferSizeMax;

    gcroot<EventLog^> eventLog;
    CCritSec m_cSharedState;
    CRtpSource *pCRtpSource;
};