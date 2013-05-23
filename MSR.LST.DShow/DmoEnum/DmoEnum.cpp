// DmoEnum.cpp : Defines the entry point for the console application.
//
#include "dmo.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace MSR
{
    namespace LST
    {
        namespace MDShow
        {
            public ref struct AvailableDMO
            {
                AvailableDMO(System::String^ name, System::Guid^ guid)
                {
                    this->Name = name;
                    this->Guid = guid;
                }

                System::String^ Name;
                System::Guid^ Guid;
            };

            public ref class DMOEnumerator
            {
                public:

                    static List<AvailableDMO^>^ Enumerate(
                        System::Guid category, 
                        System::Guid majorType, 
                        System::Guid subType,
                        System::Boolean inputMediaType)
                    {
                        List<AvailableDMO^>^ dmos = gcnew List<AvailableDMO^>();

                        // Create the partial media type
                        DMO_PARTIAL_MEDIATYPE partialMT;
                        Marshal::StructureToPtr(majorType, IntPtr(&partialMT.type), false);
                        Marshal::StructureToPtr(subType, IntPtr(&partialMT.subtype), false);

                        // Determine whether this is for input or output types
                        int inCount = 0;
                        DMO_PARTIAL_MEDIATYPE* pInPartialMT = 0;

                        int outCount = 0;
                        DMO_PARTIAL_MEDIATYPE* pOutPartialMT = 0;

                        if(inputMediaType)
                        {
                            inCount = 1;
                            pInPartialMT = &partialMT;
                        }
                        else
                        {
                            outCount = 1;
                            pOutPartialMT = &partialMT;
                        }

                        // Enumerate
                        GUID nCategory;
                        Marshal::StructureToPtr(category, IntPtr(&nCategory), false);

                        IEnumDMO* pEnum = NULL; 
                        HRESULT hr = DMOEnum(
                            nCategory,                              // Category
                            0,                                      // Included keyed DMOs
                            inCount, pInPartialMT,                  // Input types
                            outCount, pOutPartialMT,                // Output types
                            &pEnum);

                        if (SUCCEEDED(hr)) 
                        {
                            CLSID clsidDMO;
                            WCHAR* wszName;

                            do
                            {
                                hr = pEnum->Next(1, &clsidDMO, &wszName, NULL);

                                if (hr == S_OK) 
                                {
                                    String^ name = gcnew System::String(wszName);
                                    Guid^ g = gcnew Guid(clsidDMO.Data1, clsidDMO.Data2,
                                        clsidDMO.Data3, clsidDMO.Data4[0], clsidDMO.Data4[1],
                                        clsidDMO.Data4[2], clsidDMO.Data4[3],
                                        clsidDMO.Data4[4], clsidDMO.Data4[5],
                                        clsidDMO.Data4[6], clsidDMO.Data4[7]);

                                    dmos->Add(gcnew AvailableDMO(name, g));

                                    // Remember to release wszName!
                                    CoTaskMemFree(wszName);
                                }
                            } while (hr == S_OK);

                            pEnum->Release();
                        }

                        return dmos;
                    }
            };
        }
    }
}