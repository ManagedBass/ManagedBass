# ManagedBass

[![Chat on Gitter](https://img.shields.io/gitter/room/MathewSachin/ManagedBass.svg?style=flat-square)](https://gitter.im/MathewSachin/ManagedBass)
[![Build Status](https://img.shields.io/appveyor/ci/MathewSachin/ManagedBass/master.svg?style=flat-square)](https://ci.appveyor.com/project/MathewSachin/ManagedBass)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ManagedBass.svg?style=flat-square)](https://www.nuget.org/Packages/ManagedBass)

[(c) 2016 Mathew Sachin](LICENSE.md). All Rights Reserved.  
Free Open-Source Cross-Platform .Net Wrapper for Un4seen Bass and its AddOns.

Bass and its Add-Ons can be downloaded at http://un4seen.com/  
ManagedBass is targeted for **Any CPU**, but bass Libraries(dll/so/dylib) are separate for x86, x64, ARM, etc.  
Download the versions you need.

> For latest info: Follow [ManagedBass Blog](https://managedbass.wordpress.com)

Features
-----------------------------------------
* **FREE**: You get freedom from the licensing of Bass.Net.  
  BASS and other ADDONS STILL NEED TO BE LICENSED.  
  You don't have to suppress Popups by providing a Registration Key.

* **Method Names** have been **simplified** using `EntryPoint` Parameter of `DllImport`.  
  e.g. Instead of `BassWma.BASS_WMA_EncodeOpenFile()` you could use `BassWma.EncodeOpenFile();`

* **Plugin Add-Ons** are wrapped in a light-weight way as instances of the Plugin class.  
  Most Plugins are supported.  
  Their Specific functions are not wrapped due to lack of productivity.

* ManagedBass provides completely managed types for your use along with the Native wrapper types.  
  Namespaces are grouped by AddOns or Features.

* Now, available in different flavors for Windows, Mac, Linux, Xamarin.Android and Xamarin.iOS.

Bass and other Add-Ons are trademarks of their respective owners: **Un4Seen Bass - (c) Ian Luck**
