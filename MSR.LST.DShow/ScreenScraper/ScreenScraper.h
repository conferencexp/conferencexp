//------------------------------------------------------------------------------
// File: ScreenScraper.h
//
// Desc: Filter for scraping the desktop or a window and turning it into a bitmap
//------------------------------------------------------------------------------
#pragma once

#include "ScreenScraper_i.c"    // IID_ScreenScraper and CLSID_ScreenScraper
#include "ScreenScraper_h.h"    // IScreenScraper

#using <mscorlib.dll>
using namespace System;

//-----------------------------------------------------------------------------
// CScreenScraperPin
//-----------------------------------------------------------------------------
class CScreenScraperPin : public CSourceStream
{
protected:

    static const int BUFFER_COUNT = 1;  // How many buffers we want to use

    HWND m_hWnd;                        // Which window we are taking images of

    // UNITS = 10 ^ 7  
    // UNITS / 30 = 30 fps;
    // UNITS / 20 = 20 fps, etc
    REFERENCE_TIME m_rtFrameRate;       // Desired frame rate
    REFERENCE_TIME m_lastFrame;         // Current frame number, used for framerate control

    CImageDisplay m_Display;            // Figures out our media type for us

    int m_ScreenHeight;                 // Screen height
    int m_ScreenWidth;                  // Screen width

    CCritSec m_cSharedState;            // Protects our internal state

public:

    CScreenScraperPin(HRESULT *phr, CSource *pFilter);
    
    HRESULT GetMediaType(CMediaType *pmt);
    HRESULT CheckMediaType(const CMediaType *pMediaType);
    HRESULT SetMediaType(const CMediaType *pMediaType);

    HRESULT DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pRequest);
    HRESULT FillBuffer(IMediaSample *pSample);
    
    STDMETHODIMP Notify(IBaseFilter* pSender, Quality q)
    {
        // We don't drop frames, we just block
        return E_FAIL;
    }

    STDMETHODIMP FrameRate(int frameRate)
    {
        // Validate it is within the acceptable bounds
        if(frameRate <= 0 || frameRate > 30)
        {
            return E_INVALIDARG;
        }
        
        // Set it
        m_rtFrameRate = UNITS / frameRate;
        return S_OK;
    }
    
    STDMETHODIMP Handle(HWND hWnd)
    {
        m_hWnd = hWnd;
        return S_OK;
    }

private:
    void CopyBitmapToSample(HWND hWnd, int wndWidth, int wndHeight, BYTE* pData, BITMAPINFO* pBMI);
    void DrawMouse(HDC hDC, HWND hWnd);
};

//-----------------------------------------------------------------------------
// CScreenScraperSource
//-----------------------------------------------------------------------------
class CScreenScraperSource : public CSource, IScreenScraper
{
public:
    // COM
    DECLARE_IUNKNOWN;
    static CUnknown * WINAPI CreateInstance(IUnknown *pUnk, HRESULT *phr);
    STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void ** ppv);

    // IScreenScraper
    STDMETHODIMP Handle(HWND hWnd)
    {
        return m_pPin->Handle(hWnd);
    }

    STDMETHODIMP FrameRate(int frameRate)
    {
        return m_pPin->FrameRate(frameRate);
    }

private:
    // Constructor is private because you have to use CreateInstance
    CScreenScraperSource(IUnknown *pUnk, HRESULT *phr);
    ~CScreenScraperSource();

    CScreenScraperPin *m_pPin;
};