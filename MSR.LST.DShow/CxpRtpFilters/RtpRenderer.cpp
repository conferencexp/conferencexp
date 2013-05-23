// Note: We do not support loading our filters in GraphEdt.exe
// In order to use the filter, you have to provide it with an appropriately initialized RtpSender

#include <streams.h>        // DShow base classes
#include "RtpRenderer.h"


CRtpRenderer::CRtpRenderer(TCHAR *tszName, LPUNKNOWN punk, HRESULT *phr)
        :   CBaseRenderer (CLSID_RtpRenderer, tszName, punk, phr),
            m_cbHeader(0)
{
    eventLog = gcnew EventLog("Filters", ".", "RtpRenderer");

    // Items that get written to network
    // 0 - length of header (moot since they are all fixed size, kept to be backward compatible with 3.0)
    // 1 - SampleProperties
    // 2 - MediaType
    // 3 - FormatType
    // 4 - Sample data
    int ptrCount = 5;

    ptrs = gcnew array<IntPtr>(ptrCount);
    ptrLengths = gcnew array<Int32>(ptrCount);

    ptrs[0] = IntPtr(&m_cbHeader);
    ptrLengths[0] = sizeof(m_cbHeader);

    ptrs[1] = IntPtr(&m_SampleProps);
    ptrLengths[1] = sizeof(m_SampleProps);

    ptrs[2] = IntPtr(&m_mt);
    ptrLengths[2] = sizeof(m_mt);

	m_flags = 0;
}

CRtpRenderer::~CRtpRenderer(){}

STDMETHODIMP CRtpRenderer::Initialize(IUnknown* pRtpSender)
{
    CAutoLock cAutolock(&m_cSharedState);

    try
    {
        rtpSender = dynamic_cast<RtpSender^>(Marshal::GetObjectForIUnknown(IntPtr(pRtpSender)));
    }
    catch(Exception^ e)
    {
        eventLog->WriteEntry(e->ToString(), EventLogEntryType::Error);
        return E_FAIL;
    }

    return NOERROR;
}

STDMETHODIMP CRtpRenderer::Initialize2(IUnknown* pRtpSender, byte flags) {
    CAutoLock cAutolock(&m_cSharedState);
	HRESULT hr = Initialize(pRtpSender);
	if (hr == NOERROR) {
		m_flags = flags;
	}
	return hr;
}

HRESULT CRtpRenderer::CheckMediaType(const CMediaType* pmt)
{
	CAutoLock cAutoLock(&m_cSharedState);

	// For video, if the low order bit of the flags is set, constrain the input to be a VIDEOINFOHEADER format
	// This was useful for working with the Morgan MJPEG codec
	if ((*(pmt->Type()) == MEDIATYPE_Video) && ((m_flags & 1) != 0)) {
		if (*(pmt->FormatType()) == FORMAT_VideoInfo) {
			eventLog->WriteEntry("CheckMediaType accepting VideoInfo.", EventLogEntryType::Information);
			return NOERROR;
		}
		eventLog->WriteEntry("CheckMediaType rejecting proposed type.", EventLogEntryType::Information);
		return E_FAIL;
	}

	// For other major types or without the flag, accept everything.
	return NOERROR;
}

HRESULT CRtpRenderer::SetMediaType(const CMediaType* pmt)
{
    CAutoLock cAutolock(&m_cSharedState);

    m_mt = *pmt;

    ptrs[3] = IntPtr(m_mt.pbFormat);
    ptrLengths[3] = m_mt.cbFormat;

    // We need to emulate the way BufferChunk writes integers in order to keep compatibility with
    // CXP 3.0.  Must cast the input, not the output.
    m_cbHeader = IPAddress::HostToNetworkOrder((Int16)(ptrLengths[1] + ptrLengths[2] + ptrLengths[3]));

    return NOERROR;
}

HRESULT CRtpRenderer::DoRenderSample(IMediaSample *pMediaSample) {

    ValidateReadPtr(pMediaSample, sizeof(IMediaSample));

    // Retrieve the properties of this sample, to send across the wire
    IMediaSample2 *pMediaSample2 = 0;
    HRESULT hr = pMediaSample->QueryInterface(IID_IMediaSample2, (void **)&pMediaSample2);
    if (FAILED(hr) )
    {
        eventLog->WriteEntry("Failed to QI for IMediaSample2 in DoRenderSample", EventLogEntryType::Error);
        return hr;
    }

    
    hr = pMediaSample2->GetProperties(sizeof(m_SampleProps), (byte*)&m_SampleProps);
    pMediaSample2->Release();
    if ( FAILED(hr))
    {
        eventLog->WriteEntry("Failed to get properties in DoRenderSample", EventLogEntryType::Error);
        return hr;
    }
    
    // Add media sample data
    byte* pbPayload = 0;
    hr = pMediaSample->GetPointer(&pbPayload);
    if ( FAILED(hr))
    {
        eventLog->WriteEntry("Failed to get pointer from IMediaSample in DoRendersample", EventLogEntryType::Error);
        return hr;
    }
    
    ptrs[4] = IntPtr(pbPayload);
    ptrLengths[4] = pMediaSample->GetActualDataLength();

    try
    {
        // We don't need to prepend the lengths, because each item is a fixed length except for
        // the payload, which comes last.
        rtpSender->Send(ptrs, ptrLengths, false);
    }
    catch (Exception^ e)
    {
        eventLog->WriteEntry(e->ToString(), EventLogEntryType::Error);
        return E_FAIL;
    }

    return hr;
}

///////////////////////////////////////
//  COM hand-holding
///////////////////////////////////////

CUnknown * WINAPI CRtpRenderer::CreateInstance(LPUNKNOWN punk, HRESULT *phr) 
{
    return new CRtpRenderer(NAME("RtpRenderer"), punk, phr );
}

STDMETHODIMP CRtpRenderer::NonDelegatingQueryInterface(REFIID riid, void **ppv)
{
    CheckPointer(ppv,E_POINTER);

    if (riid == IID_IRtpRenderer) 
    {
        return GetInterface((IRtpRenderer *) this, ppv);
    }
    else 
    {
        return CBaseRenderer::NonDelegatingQueryInterface(riid, ppv);
    }
}