:::: Run this script using the Visual Studio 2010 command prompt.
:::: Pass 'debug' or 'release' on the command line, e.g.: cxpbuild.bat release

@if NOT DEFINED VCINSTALLDIR (
@echo.
@echo Please build from a Visual Studio 2010 command prompt
@exit /b 1
)

@if "%1"=="" goto :ParameterErr
@if /I %1==debug goto :BeginBuild
@if /I %1==release goto :BeginBuild

:ParameterErr
@echo.
@echo Please pass 'debug' or 'release' on the command line, e.g.: cxpbuild.bat release
@exit /b 1

:BeginBuild

:::: Core API

msbuild .\Solutions\ConferenceAPI.sln /t:Build /p:Configuration=%1
::msbuild .\Solutions\RtpAPI_Samples.sln /t:Build /p:Configuration=%1

:::: Services - Archiver, Venue Service, Diagnostic Service and Reflector

msbuild .\Solutions\ServiceCommon.sln /t:Build /p:Configuration=%1
msbuild .\Solutions\Archive.sln /t:Build /p:Configuration=%1
msbuild .\Solutions\VenueService.sln /t:Build /p:Configuration=%1
msbuild .\Solutions\Reflector.sln /t:Build /p:Configuration=%1
msbuild .\Solutions\DiagnosticService.sln /t:Build /p:Configuration=%1

:::: Service setup solutions

devenv .\Solutions\Setup\Archive_Setup.sln /Build %1
devenv .\Solutions\Setup\Reflector_Setup.sln /Build %1
devenv .\Solutions\Setup\VenueService_Setup.sln /Build %1
devenv .\Solutions\Setup\DiagnosticService_Setup.sln /Build %1

:::: Capabilities

msbuild .\Solutions\Capabilities.sln /t:Build /p:Configuration=%1

:::: DirectShow Filters and CXPClient 

:::: These have dependencies on ConferenceAPI, Capabilities and Archiver.
:::: We run the x86 build on both x86 and x64 platforms since codecs are not available for x64.
msbuild .\Solutions\AudioVideo.sln /t:Build /p:Configuration=%1
msbuild .\Solutions\CXPClient.sln /t:Build /p:Configuration=%1;Platform=x86
devenv .\Solutions\Setup\CXPClient_Setup_x86.sln /Build %1

:::: The following are all optional items to build

:::: x64 DirectShow Filters and Native x64 CxpClient

:::: These only work with uncompressed audio/video since the Windows Media codecs we use are not available for x64.
:::: Build this on x64, or using the Cross Tools Command Prompt on x86.
::setlocal
::call "%VCINSTALLDIR%\vcvarsall.bat" x86_amd64
::msbuild .\Solutions\AudioVideo.sln /t:Build /p:Configuration=%1;Platform=x64
::endlocal
::msbuild .\Solutions\CXPClient.sln /t:Build /p:Configuration=%1
::devenv .\Solutions\Setup\CXPClient_Setup_x64.sln /Build %1
::md MSR.LST.ConferenceXP\CXPClient\bin\x64\%1
::xcopy /s /y MSR.LST.ConferenceXP\CXPClient\bin\%1\* MSR.LST.ConferenceXP\CXPClient\bin\x64\%1\*

:::: CxpClient for AnyCPU.  This is not currently working.

::msbuild .\Solutions\CXPClient.sln /t:Build /p:Configuration=%1
::devenv .\Solutions\Setup\CXPClient_Setup.sln /Build %1

:::: Samples

:::: If you want to build the AV_Samples or ConferenceAPI_Samples for a particular
:::: platform, please make sure that is the last one built. 
::msbuild .\Solutions\AudioVideo_Samples.sln /t:Build /p:Configuration=%1
::msbuild .\Solutions\ConferenceAPI_Samples.sln /t:Build /p:Configuration=%1