#include <streams.h>
#include <initguid.h>

#include "ScreenScraper.h"

/*
    Guidelines for Registering Filters

    The filter registry information determines how the Filter Graph Manager functions during 
    Intelligent Connect. Thus, it affects every application written for DirectShow, not just the 
    ones that will use your filter. You should make sure that your filter behaves correctly, by 
    following these guidelines. 

    1. Do you need the filter data in the registry? For many custom filters, there is no reason to 
    make the filter visible to the Filter Mapper or the System Device Enumerator. As long as you 
    register the DLL, your application can create the filter using CoCreateInstance. In that case, 
    simply omit the AMOVIESETUP_FILTER structure from the factory template. (One drawback is that 
    your filter will not be visible in GraphEdit. To get around this, you can create a private 
    "Testing" category using the IFilterMapper2::CreateCategory method. You should only do this for 
    debug builds.)

    2.  Choose the correct filter category. The default "DirectShow Filters" category is for general
    purpose filters. Whenever appropriate, register your filter in a more specific category. When 
    IFilterMapper2 searches for a filter, it ignores any category whose merit is MERIT_DO_NOT_USE or 
    less. Categories not intended for normal playback have low merit.

    3. Avoid specifying MEDIATYPE_None, MEDIASUBTYPE_None, or GUID_NULL in the AMOVIESETUP_MEDIATYPE
    information for a pin. IFilterMapper2 treats these as wildcards, which can slow the 
    graph-building process.

    4. Choose the lowest merit value possible. Here are some guidelines:

    Type of filter                       Recommended merit

    Default renderer                     MERIT_PREFERRED. For standard media types, however, a custom
                                            renderer should never be the default. 

    Non-default renderer                 MERIT_DO_NOT_USE or MERIT_UNLIKELY 
    Mux                                  MERIT_DO_NOT_USE 
    Decoder                              MERIT_NORMAL 
    Splitter, parser                     MERIT_NORMAL or lower

    Special purpose filter; any filter 
    that is created directly by the      MERIT_DO_NOT_USE 
    application

    Capture                              MERIT_DO_NOT_USE 

    "Fallback" filter; for example, 
    the Color Space Converter Filter     MERIT_UNLIKELY 

    If you are giving a filter a merit of MERIT_DO_NOT_USE, consider whether you need to register 
    this information in the first place. (See item 1.)

    Do not register a filter in the "DirectShow Filters" category that accepts 24-bit RGB. Your 
    filter will interfere with the Color Space Converter filter. 
*/

// List of class IDs and creator functions for the class factory. This
// provides the link between the OLE entry point in the DLL and an object
// being created. The class factory will call the static CreateInstance.
// We provide a set of filters in this one DLL.

CFactoryTemplate g_Templates[1] = 
{
    { 
      L"Cxp Screen Scraper",             // Name
      &CLSID_ScreenScraper,             // CLSID
      CScreenScraperSource::CreateInstance,   // Method to create an instance of MyComponent
      NULL,                             // Initialization function
      NULL                              // Set-up information (for filters)
    },
};

int g_cTemplates = sizeof(g_Templates) / sizeof(g_Templates[0]);    



////////////////////////////////////////////////////////////////////////
//
// Exported entry points for registration and unregistration 
// (in this case they only call through to default implementations).
//
////////////////////////////////////////////////////////////////////////

STDAPI DllRegisterServer()
{
    return AMovieDllRegisterServer2( TRUE );
}

STDAPI DllUnregisterServer()
{
    return AMovieDllRegisterServer2( FALSE );
}