// A couple of years ago we converted our filters from being purely native to
// mixed native / managed.  Additionally, we inherit from DirectShow base 
// classes which are native and COM based.
//
// In previous versions of VS (2002, 2003) there was an infamous "loader lock"
// problem.  The basic idea is that managed code cannot run during native
// initialization or it risks a deadlock during loading.  However, native code
// can run during managed initialization because the managed side can handle
// *some* native initialization for us.  In our case, some ATL code has a
// global with a destructor, which apparently the managed side won't handle
// for us if we don’t use the normal CRT entrypoint.
//
// We were receiving warnings during our build alerting us to the potential of
// encountering the "loader lock" issue.  But we could not figure out how to 
// resolve the warnings, and since the code kept working, we ignored it.  In
// VS 2005 the code finally quit working, as the build tools and runtime 
// tightened their detection.
//
// It turns out the solution is actually quite simple though.  Rather than using
// the default DirectShow entry point DllEntryPoint (located in 
// baseclasses\dllentry.cpp) we provide a standard entry point (DllMain) that 
// VC knows about so that it can handle the native code initialization.  From 
// there we call DirectShow's entry point.  The whole project, except for the 
// current file are compiled with /clr.  This file is compiled native (right 
// click on the file and choose properties).
//
// So we are no longer ignoring linker warnings LNK4210 and LNK4243.  However, 
// we are now ignoring compiler warning C4739: __asm causes native code 
// generation in baseclasses\wxutil.h, line 391

// So that the linker will look for this method elsewhere
extern "C" int __stdcall DllEntryPoint(void *, unsigned, void *);

int __stdcall DllMain( void * dllHandle, unsigned reason, void * reserved)
{
    // VC has handled the native initialization, forward on to DirectShow
    return DllEntryPoint(dllHandle, reason, reserved);
}