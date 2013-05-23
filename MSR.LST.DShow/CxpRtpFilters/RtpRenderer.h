#pragma once

#include "RtpRenderer_i.c"  // IID_RtpRenderer and CLSID_RtpRenderer
#include "RtpRenderer_h.h"  // IRtpRenderer
#include "vcclr.h"          // for gcroot

using namespace System;
using namespace System::Net;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;
using namespace MSR::LST::Net::Rtp;

class CRtpRenderer : public CBaseRenderer, public IRtpRenderer
{
public:

    static CUnknown *WINAPI CreateInstance(LPUNKNOWN punk, HRESULT *phr);
    DECLARE_IUNKNOWN;

private:

    // Methods
    CRtpRenderer(TCHAR *tszName, LPUNKNOWN punk, HRESULT *phr);
    ~CRtpRenderer();

    HRESULT CheckMediaType(const CMediaType* pmt);
    HRESULT SetMediaType(const CMediaType* pmt);
    HRESULT DoRenderSample(IMediaSample *pSample);

    STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void ** ppv);
    STDMETHODIMP Initialize(IUnknown* pRtpSender);
    STDMETHODIMP Initialize2(IUnknown* pRtpSender, byte flags);

    // Members
    gcroot<RtpSender^> rtpSender;
    gcroot<EventLog^> eventLog;

    gcroot<array<IntPtr>^> ptrs;
    gcroot<array<Int32>^> ptrLengths;

    // Items that get written to network
    // 0 - length of header (moot since they are all fixed size, kept to be backward compatible with 3.0)
    // 1 - SampleProperties
    // 2 - MediaType
    // 3 - FormatType
    // 4 - Sample data
    Int16 m_cbHeader;
    AM_SAMPLE2_PROPERTIES m_SampleProps;
    CMediaType m_mt;

	//  Flags used to influence behavior of the filter on behalf of certain special case scenarios.
	//	bit 0: If set, accept only VIDEOINFOHEADER formats when connecting, otherwise accept any.
	byte m_flags;

    CCritSec m_cSharedState;
};