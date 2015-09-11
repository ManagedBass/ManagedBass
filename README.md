# ManagedBass

[![Join the chat at https://gitter.im/MathewSachin/ManagedBass](https://img.shields.io/gitter/room/MathewSachin/ManagedBass.svg?style=flat-square)](https://gitter.im/MathewSachin/ManagedBass?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Build Status](https://img.shields.io/appveyor/ci/MathewSachin/ManagedBass/master.svg?style=flat-square)](https://ci.appveyor.com/project/MathewSachin/ManagedBass)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ManagedBass.svg?style=flat-square)](https://www.nuget.org/Packages/ManagedBass)

(c) 2015 Mathew Sachin. All Rights Reserved.  
An Alternative to the Bass.Net Managed Wrapper around Un4seen Bass.dll

Bass and its Add-Ons can be downloaded at http://un4seen.com/  
ManagedBass is targeted for **Any CPU**, but bass dll(s) are separate for x86 and x64.  
Download the versions you need

Features
-----------------------------------------
* **Open Source**
* **FREE**: You get freedom from the licensing of Bass.Net.  
  BASS and other ADDONS STILL NEED TO BE LICENSED.
* You don't have to suppress Popups by providing a Registration Key.
* **Method Names** have been **simplified** in Dynamics using `EntryPoint` Parameter of `DllImport`.  
  e.g. Instead of `BassWma.BASS_WMA_EncodeOpenFile()` you could use `BassWma.EncodeOpenFile();`
* **Plugin Add-Ons** are wrapped in a light-weight way as instances of the Plugin class.  
  Most Plugins are supported.  
  They all were wrapped in just 15 minutes using Plugin class.
* ManagedBass provides completely managed types for your use.  
  It does the Native wrapping in **ManagedBass.Dynamics**.  
  You could use these types in this namespace directly or use Managed Alternatives.

Bass and other Add-Ons are trademarks of their respective owners
**Un4Seen Bass - (c) Ian Luck**

Examples
-------------------------------
**ManagedBass.ShowDown** included in the project demonstrates some samples of ManagedBass working with a WPF UI.  
It is also useful for the purpose of testing.
