# ManagedBass

[![Chat on Gitter](https://img.shields.io/gitter/room/MathewSachin/ManagedBass.svg?style=flat-square)](https://gitter.im/MathewSachin/ManagedBass)
[![Build Status](https://img.shields.io/appveyor/ci/MathewSachin/ManagedBass/master.svg?style=flat-square)](https://ci.appveyor.com/project/MathewSachin/ManagedBass)

[(c) 2016 Mathew Sachin](LICENSE.md). All Rights Reserved.  
Free Open-Source Cross-Platform .Net Wrapper for Un4seen Bass and its AddOns.

Bass and its Add-Ons can be downloaded at http://un4seen.com/  
ManagedBass is targeted for **Any CPU**, but bass Libraries(.dll/.so/.dylib/.a) are separate for x86, x64, ARM, etc.  
Download the versions you need.

> For latest info: Follow [ManagedBass Blog](https://managedbass.wordpress.com)

See the [Sample Repositories](https://github.com/ManagedBass) for examples.

Features
-----------------------------------------
* Free, Open-Source, No-Registration required.

* Simplified Member Names.  
  e.g. Instead of `BassWma.BASS_WMA_EncodeOpenFile()` you could use `BassWma.EncodeOpenFile();`

* **NEW**: Introducing Separate classes for Plugins.

* ManagedBass also provides completely managed types for your use along with the wrapper types.  
  Namespaces are grouped by AddOns or Features.

* Now, available in different flavors for Windows, Mac, Linux, Xamarin.Android and Xamarin.iOS.  
  **NEW**: A Portable Class Library which can target Windows Store is also included.

Bass and other Add-Ons are trademarks of their respective owners: **Un4Seen Bass - (c) Ian Luck**

Getting Started
-----------------------------------------
* Install the NuGet package
```powershell
Install-Package ManagedBass
```

* Download the BASS libraries from http://un4seen.com and place them in Build Output Directory.
