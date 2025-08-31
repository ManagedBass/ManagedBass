<img width="128" height="128" alt="mb-logo-rounded-128" src="https://github.com/user-attachments/assets/834c571c-6fba-4d6c-a203-9774a456424e" />

# ManagedBass
[![NuGet](https://img.shields.io/nuget/v/ManagedBass.svg)](https://www.nuget.org/packages/ManagedBass/) [![NuGet Downloads](https://img.shields.io/nuget/dt/ManagedBass.svg)](https://www.nuget.org/packages/ManagedBass/) [![Build](https://github.com/ManagedBass/ManagedBass/actions/workflows/build-test.yml/badge.svg)](https://github.com/ManagedBass/ManagedBass/actions/workflows/build-test.yml)

Free Open-Source Cross-Platform .Net Wrapper for [Un4seen Bass audio library](https://www.un4seen.com)   and its AddOns.

## Bass Audio Library
Summary from Un4Seen Bass website:
> BASS is an audio library for use in software on several platforms. Its purpose is to provide developers with powerful and efficient sample, stream (MP3, MP2, MP1, OGG, WAV, AIFF, custom generated, and more via OS codecs and add-ons), MOD music (XM, IT, S3M, MOD, MTM, UMX), MO3 music (MP3/OGG compressed MODs), and recording functions. All in a compact DLL that won't bloat your distribution.
> C/C++, Delphi, and Visual Basic APIs are provided, with several examples to get you started. .NET and other APIs are also available.


**IMPORTANT**: Bass and its Add-Ons can be downloaded at http://un4seen.com/. This project provides a wrapper for the C-based library.    

ManagedBass is targeted for **Any CPU**, but bass Libraries(.dll/.so/.dylib/.a) are separate for x86, x64, ARM, etc.  
Download the versions you need directly from Un4Seen's website.

See the [Sample Repositories](https://github.com/ManagedBass) for examples.

See [Un4Seen Bass documentation for full details](https://www.un4seen.com/doc/). This documentation is for their c-based library, which this project provides an almost 1:1 wrapper for.  

ManagedBass is now provided as a set of packages split per AddOn.

----
### Supporting the project

ManagedBass is developed and supported by [@olitee](https://github.com/olitee) and [@DustinBond](https://github.com/DustinBond) in their spare time for free. The project was originally created by [@MathewSachin](https://github.com/MathewSachin). Contributions are very welcome, but please be patient.

----

## Getting Started

* Install the NuGet package
```powershell
Install-Package ManagedBass
```

* Download the BASS libraries from http://un4seen.com and place them in Build Output Directory.

## Supported Frameworks and Platforms
Our goal is to support all platforms and frameworks that Un4seen Bass supports.  
The nuget package contains support for the following targets:
- .NET Framework 4.5+
- .NET 8.0+
- .NET Standard 2.0+
- Xamarin.iOS**

with the following platforms:
- Windows
- Linux
- MacOS
- Android
- iOS**

** NOTE that we create a specific iOS version, as iOS requires that you statically link the native Bass libraries.  
  Other platforms (Linux, Android, Mac, etc) should be able to use the .NET Standard 2.0 or .NET 8 versions of our wrapper.

## Changelog
See our [releases page](https://github.com/ManagedBass/ManagedBass/releases)

## 
