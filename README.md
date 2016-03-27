# ManagedBass

[![Join the chat at https://gitter.im/MathewSachin/ManagedBass](https://img.shields.io/gitter/room/MathewSachin/ManagedBass.svg?style=flat-square)](https://gitter.im/MathewSachin/ManagedBass?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Build Status](https://img.shields.io/appveyor/ci/MathewSachin/ManagedBass/master.svg?style=flat-square)](https://ci.appveyor.com/project/MathewSachin/ManagedBass)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ManagedBass.svg?style=flat-square)](https://www.nuget.org/Packages/ManagedBass)

(c) 2016 Mathew Sachin. All Rights Reserved.  
Free Open-Source Cross-Platform .Net Wrapper for Un4seen Bass and its AddOns.

Bass and its Add-Ons can be downloaded at http://un4seen.com/  
ManagedBass is targeted for **Any CPU**, but bass Libraries(dll/so/dylib) are separate for x86, x64, ARM, etc.  
Download the versions you need.

Features
-----------------------------------------
* **FREE**: You get freedom from the licensing of Bass.Net.  
  BASS and other ADDONS STILL NEED TO BE LICENSED.  
  You don't have to suppress Popups by providing a Registration Key.

* **Method Names** have been **simplified** in Dynamics using `EntryPoint` Parameter of `DllImport`.  
  e.g. Instead of `BassWma.BASS_WMA_EncodeOpenFile()` you could use `BassWma.EncodeOpenFile();`

* **Plugin Add-Ons** are wrapped in a light-weight way as instances of the Plugin class.  
  Most Plugins are supported.  
  Their Specific functions are not wrapped due to lack of productivity.

* ManagedBass provides completely managed types for your use along with the Native wrapper types.  
  Namespaces are grouped by AddOns or Features.

* Intended to be a single Cross-Platform library (Works on **Windows**, also runs on **Xamarin.Android**).  
  `<DllMap>` in **ManagedBass.dll.config** may be used with **Mono** to map to custom Dll Names/Paths.  
  **iOS** is **not** currently supported due to the requirement of **static _internal** linking.

Bass and other Add-Ons are trademarks of their respective owners: **Un4Seen Bass - (c) Ian Luck**
