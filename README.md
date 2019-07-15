# ManagedBass
(c) Mathew Sachin  
Free Open-Source Cross-Platform .Net Wrapper for Un4seen Bass and its AddOns.

Bass and its Add-Ons can be downloaded at http://un4seen.com/  
ManagedBass is targeted for **Any CPU**, but bass Libraries(.dll/.so/.dylib/.a) are separate for x86, x64, ARM, etc.  
Download the versions you need.

See the [Sample Repositories](https://github.com/ManagedBass) for examples.

> ~~ManagedBass depends on `ManagedBass.PInvoke` for PInvoke declarations which is quite stable package.~~

> ManagedBass is now provided as a set of packages split per AddOn.

Getting Started
-----------------------------------------
* Install the NuGet package
```powershell
Install-Package ManagedBass
```

* Download the BASS libraries from http://un4seen.com and place them in Build Output Directory.

See https://github.com/ManagedBass/Home for more info.

## Changelog

### v2.0
- Moved from PCL to .Net Standard 1.4.
- Removed Load and Unload methods in support of being cross-platform.
- Removed DynamicLibrary class.

### v1.0
- Split NuGet packages per AddOn.
- No dependency on `ManagedBass.PInvoke`.
- Using C# 7 on Visual Studio 2017.
