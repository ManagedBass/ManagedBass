# ManagedBass
[![Build status](https://ci.appveyor.com/api/projects/status/0xjpk9r156tn6reu?svg=true)](https://ci.appveyor.com/project/MathewSachin/managedbass)  
(c) 2015 Mathew Sachin. All Rights Reserved.  
An Alternative to the Bass.Net Managed Wrapper around Un4seen Bass.dll

Available as a NuGet Package  
Bass and its AddOns can be downloaded at http://un4seen.com/bass  
ManagedBass is targeted for AnyCPU, but bass dlls are separate for x86 and x64. Download the versions you need

AddOns
-----------------------------------------
* Open Source
* You get freedom from the licensing of Bass.Net.  
  BASS and other ADDONS STILL NEED TO BE LICENSED.
* You don't have to supress Popups by providing a Registration Key.

Bass and other addons are trademarks of their respective owners
`Un4Seen Bass - (c) Ian Luck`

ManagedBass does the Native wrapping in `ManagedBass.Dynamics`.  
You could use these types in this namespace directly or use Managed Alternatives

MethodNames have been simplified in Dynamics using EntryPoint Parameter of DllImport.  
for eg: Instead of `BASS_WMA_EncodeOpenFile()` you could use `BassWma.EncodeOpenFile();`

Examples
-------------------------------
These Repositories on My Account may help you:  
* [Avio](http://github.com/Revica/Avio)
* [Revic](http://github.com/Revica/Revic)
* [PitchTracker](http://github.com/Revica/PitchTracker)
