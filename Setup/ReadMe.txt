Vista Build Warnings
====================

ConferenceXP Setup files build error-free and warning-free on WindowsXP, but each has the following build warning on Vista:

WARNING: 'FirewallAPI.dll' should be excluded because its source file 'H:\Windows\system32\FirewallAPI.dll' is under Windows System File Protection.

If the FirewallAPI.dll references are excluded in Vista, the build breaks on XP since FirewallAPI.dll is new to Vista and the setup files cannot detect the dependency that was excluded. As long as ConferenceXP is being developed for both the XP and Vista platforms, the build warnings on Vista will have to be ignored. The only drawback to doing so is that the MSI's from Vista will be a bit larger and that they will be shipping a system file that is never used on XP. However, once XP is phased out, the FirewallAPI.dll references in Vista should be excluded.