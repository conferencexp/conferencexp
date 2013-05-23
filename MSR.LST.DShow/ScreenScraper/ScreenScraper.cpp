//-----------------------------------------------------------------------------
// File: ScreenScraper.cpp
//
// Desc: Provides an image of the user's desktop or a chosen window as a 
// continuously updating stream.
//-----------------------------------------------------------------------------
#include <streams.h>
#include "ScreenScraper.h"

//-----------------------------------------------------------------------------
// CScreenScraperPin Class
//-----------------------------------------------------------------------------
CScreenScraperPin::CScreenScraperPin(HRESULT *phr, CSource *pFilter)
        : CSourceStream(NAME("Screen Scraper"), phr, pFilter, L"Out"),
        m_hWnd(0),
        m_rtFrameRate(UNITS / 1), // Capture and display desktop 1 time per second
        m_lastFrame(0)
{
    // Get the device context of the main display
    HDC hDC = CreateDC(TEXT("DISPLAY"), NULL, NULL, NULL);

    // Get the dimensions of the main desktop window
    // Don't understand why this isn't set properly in CImageDisplay
    m_ScreenWidth  = GetDeviceCaps(hDC, HORZRES);
    m_ScreenHeight = GetDeviceCaps(hDC, VERTRES);

    // Release the device context
    DeleteDC(hDC);
}


//
// GetMediaType
//
// Prefer 1 format - whatever the display is set to
//
HRESULT CScreenScraperPin::GetMediaType(CMediaType *pmt)
{
    CheckPointer(pmt,E_POINTER);
    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // Set properties according to the display settings
    pmt->SetFormat((BYTE*)m_Display.GetDisplayFormat(), sizeof(VIDEOINFO));

    VIDEOINFO* pvi = (VIDEOINFO*)pmt->Format();
    pvi->bmiHeader.biWidth      = m_ScreenWidth;
    pvi->bmiHeader.biHeight     = m_ScreenHeight;
    pvi->bmiHeader.biSizeImage  = GetBitmapSize(&pvi->bmiHeader);

    pmt->SetType(&MEDIATYPE_Video);
    pmt->SetFormatType(&FORMAT_VideoInfo);
    pmt->SetTemporalCompression(FALSE);

    // Work out the GUID for the subtype from the header info.
    const GUID SubTypeGUID = GetBitmapSubtype(&pvi->bmiHeader);
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);

    return NOERROR;
}


//
// CheckMediaType
//
// We only accept media types compatible with the screen
// Returns E_INVALIDARG if the mediatype is not acceptable
//
HRESULT CScreenScraperPin::CheckMediaType(const CMediaType *pMediaType)
{
    CheckPointer(pMediaType,E_POINTER);

    return m_Display.CheckMediaType(pMediaType);
}


//
// SetMediaType
//
// Called when a media type is agreed between filters
//
HRESULT CScreenScraperPin::SetMediaType(const CMediaType *pMediaType)
{
    CAutoLock cAutoLock(m_pFilter->pStateLock());

    HRESULT hr = CheckMediaType(pMediaType);

    if(SUCCEEDED(hr))
    {
        Console::WriteLine(MSR::LST::MDShow::MediaType::Dump(IntPtr((void*)pMediaType)));

        // Pass the call up to my base class to actually set it
        hr = CSourceStream::SetMediaType(pMediaType);
    }

    return hr;
}


//
// DecideBufferSize
//
// This will always be called after the format has been sucessfully
// negotiated. So we have a look at m_mt to see what size image we agreed.
// Then we can ask for buffers of the correct size to contain them.
//
HRESULT CScreenScraperPin::DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pProperties)
{
    CheckPointer(pAlloc, E_POINTER);
    CheckPointer(pProperties, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // We want to allocate enough space for a full desktop scrape on the primary monitor, 
    // for which we already stored the important values in the constructor
    ALLOCATOR_PROPERTIES Actual;
    pAlloc->GetProperties(&Actual);

    int maxBytes = ((VIDEOINFO*)m_mt.Format())->bmiHeader.biSizeImage;
    
    if(Actual.cbBuffer < maxBytes || Actual.cBuffers < BUFFER_COUNT)
    {
        pProperties->cBuffers = BUFFER_COUNT;
        pProperties->cbBuffer = maxBytes;

        // Ask the allocator to reserve us some sample memory. NOTE: the function
        // can succeed (return NOERROR) but still not have allocated the
        // memory that we requested, so we must check we got whatever we wanted.
        HRESULT hr = pAlloc->SetProperties(pProperties,&Actual);
        if(FAILED(hr))
        {
            return hr;
        }

        // Is this allocator unsuitable?
        if(Actual.cbBuffer < maxBytes || Actual.cbBuffer < pProperties->cbBuffer)
        {
            return E_FAIL;
        }
    }

    return NOERROR;

} // DecideBufferSize


// This is where we insert the DIB bits into the video stream.
// FillBuffer is called once for every sample in the stream.
HRESULT CScreenScraperPin::FillBuffer(IMediaSample *pSample)
{
    CheckPointer(pSample, E_POINTER);
    CAutoLock cAutoLockShared(&m_cSharedState);

    // Multithreaded, store a local copy each time through
    HWND hWnd = m_hWnd; 

    RECT rectWnd;
    GetWindowRect(hWnd, &rectWnd);

    LONG width = rectWnd.right - rectWnd.left;
    LONG height = rectWnd.bottom - rectWnd.top;

    VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)m_mt.pbFormat;
    BITMAPINFOHEADER* pBmih = &(pVih)->bmiHeader; 

    // Future version - Check to see if window size has changed
    if(width != pBmih->biWidth || height != pBmih->biHeight)
    {
        pVih->rcSource.right = width;
        pVih->rcSource.bottom = height;
        pVih->rcTarget.right = width;
        pVih->rcTarget.bottom = height;

        pBmih->biWidth = width;
        pBmih->biHeight = height;

        pBmih->biSizeImage  = GetBitmapSize(pBmih);
        m_mt.SetSampleSize(pBmih->biSizeImage);

        pSample->SetMediaType(&m_mt);
    } 
    
    // Access the sample's data buffer
    BYTE *pData;
    pSample->GetPointer(&pData);
    
    // Copy window data to sample
    CopyBitmapToSample(hWnd, width, height, pData, (BITMAPINFO *)pBmih);
    pSample->SetActualDataLength(m_mt.GetSampleSize());

    // Set the timestamps that will govern playback frame rate.
    REFERENCE_TIME rtStart = m_lastFrame;
    m_lastFrame += m_rtFrameRate;

    pSample->SetTime(&rtStart, &m_lastFrame);

    // Set TRUE on every sample for uncompressed frames
    pSample->SetSyncPoint(TRUE);

    return S_OK;
}


void CScreenScraperPin::CopyBitmapToSample(HWND hWnd, int wndWidth, int wndHeight, BYTE* pData, BITMAPINFO* pBMI)
{
    // Create compatible device context and compatible bitmap
    HDC hWndDC = GetWindowDC(hWnd);
    HDC hMemDC = CreateCompatibleDC(hWndDC);
    HBITMAP hMemBmp = CreateCompatibleBitmap(hWndDC, wndWidth, wndHeight);

    // Select the bitmap into the device context and copy bitmap from WndDC to MemDC
    HBITMAP hOldBmp = (HBITMAP)SelectObject(hMemDC, hMemBmp);
    BitBlt(hMemDC, 0, 0, wndWidth, wndHeight, hWndDC, 0, 0, SRCCOPY | CAPTUREBLT);
    
    // Add the mouse to the picture
    DrawMouse(hMemDC, hWnd);

    // Copy the bitmap bytes into our own buffer
    // According to documentation, when using GetDIBits, the bitmap should not be selected into
    // a device context.  Also, it is good practice to put the objects back the way you found them.
    hMemBmp = (HBITMAP)SelectObject(hMemDC, hOldBmp);
    GetDIBits(hMemDC, hMemBmp, 0, wndHeight, pData, pBMI, DIB_RGB_COLORS);

    // Clean up, in reverse order
    DeleteObject(hMemBmp);
    DeleteDC(hMemDC);
    ReleaseDC(hWnd, hWndDC);
}

void CScreenScraperPin::DrawMouse(HDC hDC, HWND hWnd)
{
    // Get information about system cursor
    CURSORINFO ci;
    ci.cbSize = sizeof(CURSORINFO);
    GetCursorInfo(&ci);
    
    // Bounds of the window
    RECT rectWnd;
    GetWindowRect(hWnd, &rectWnd);

    // If the mouse is in the window
    if(PtInRect(&rectWnd, ci.ptScreenPos))
    {
        ICONINFO iconinfo;
        iconinfo.hbmMask = NULL;
        iconinfo.hbmColor = NULL;

        if(GetIconInfo(ci.hCursor, &iconinfo))
        {
            DrawIcon(hDC, ci.ptScreenPos.x - iconinfo.xHotspot, ci.ptScreenPos.y - iconinfo.yHotspot, ci.hCursor);

            DeleteObject(iconinfo.hbmColor);
            DeleteObject(iconinfo.hbmMask);
        }
    }
}

//-----------------------------------------------------------------------------
// CScreenScraperSource Class
//-----------------------------------------------------------------------------
CScreenScraperSource::CScreenScraperSource(IUnknown *pUnk, HRESULT *phr)
           : CSource(NAME("ScreenScraperSource"), pUnk, CLSID_ScreenScraper)
{
    // The pin magically adds itself to our pin array.
    m_pPin = new CScreenScraperPin(phr, this);

    if (phr)
    {
        if (m_pPin == NULL)
            *phr = E_OUTOFMEMORY;
        else
            *phr = S_OK;
    }  
}

CScreenScraperSource::~CScreenScraperSource()
{
    delete m_pPin;
}

CUnknown * WINAPI CScreenScraperSource::CreateInstance(IUnknown *pUnk, HRESULT *phr)
{
    CScreenScraperSource *pNewFilter = new CScreenScraperSource(pUnk, phr );

    if (phr)
    {
        if (pNewFilter == NULL) 
            *phr = E_OUTOFMEMORY;
        else
            *phr = S_OK;
    }
    return pNewFilter;
}

STDMETHODIMP CScreenScraperSource::NonDelegatingQueryInterface(REFIID riid, void **ppv)
{
    CheckPointer(ppv,E_POINTER);

    if(riid == IID_IScreenScraper) {
        return GetInterface((IScreenScraper *) this, ppv);
    } else {
        return CSource::NonDelegatingQueryInterface(riid, ppv);
    }
}
