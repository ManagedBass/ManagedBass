<img width="128" height="128" alt="mb-logo-rounded-128" src="https://github.com/user-attachments/assets/834c571c-6fba-4d6c-a203-9774a456424e" />

# ManagedBass
[![NuGet](https://img.shields.io/nuget/v/ManagedBass.svg)](https://www.nuget.org/packages/ManagedBass/) [![NuGet Downloads](https://img.shields.io/nuget/dt/ManagedBass.svg)](https://www.nuget.org/packages/ManagedBass/) [![Build](https://github.com/ManagedBass/ManagedBass/actions/workflows/build-test.yml/badge.svg)](https://github.com/ManagedBass/ManagedBass/actions/workflows/build-test.yml)

Free Open-Source Cross-Platform .Net Wrapper for [Un4seen Bass audio library](https://www.un4seen.com)   and its AddOns.

## Bass Audio Library
Summary from Un4Seen Bass website:
> BASS is an audio library for use in software on several platforms. Its purpose is to provide developers with powerful and efficient sample, stream (MP3, MP2, MP1, OGG, WAV, AIFF, custom generated, and more via OS codecs and add-ons), MOD music (XM, IT, S3M, MOD, MTM, UMX), MO3 music (MP3/OGG compressed MODs), and recording functions. All in a compact DLL that won't bloat your distribution.
> C/C++, Delphi, and Visual Basic APIs are provided, with several examples to get you started. .NET and other APIs are also available.


**IMPORTANT**: Bass and its Add-Ons can be downloaded at http://un4seen.com/. This project provides a wrapper for the C-based library.    

ManagedBass is targeted for **Any CPU**, but bass libraries (.dll/.so/.dylib/.framework) are separate for x86, x64, ARM, etc.  
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

## Supported Frameworks
The NuGet packages ship `net45`, `netstandard2.0`, `net8.0`, `net9.0` and `net10.0`. 

There are no platform-specific target frameworks (no `net10.0-ios` and so on) and none are needed: a .NET for iOS or Mac Catalyst app picks up the plain `net9.0`/`net10.0` asset through NuGet's normal compatibility rules, and the assembly contains no compile-time platform branches that would need a separate build.

## Platform support

**Minimum native BASS version: 2.4.17** (12 October 2022). That is the release where un4seen stopped shipping static iOS libraries and moved to a dynamic `bass.xcframework`, so everything ManagedBass loads is a dynamic library on every platform.

| Platform | How to consume | What you need to do |
|---|---|---|
| Windows, Linux, macOS, Android | NuGet | Nothing beyond putting the native `bass` library where the loader finds it (usually the build output directory). |
| iOS, tvOS, Mac Catalyst (.NET) | NuGet | Embed the native frameworks in the app bundle (`<NativeReference Include="path/to/bass.xcframework" Kind="Framework" />`). The package registers a `DllImportResolver` for you via a `buildTransitive` target, so no code is required. tvOS uses the same path as iOS but has not been tested. |
| Unity on iOS | Internal-DllImport build from the release zip | See [Unity / iOS](#unity--ios). IL2CPP binds P/Invoke at link time, so the runtime resolver cannot help and a differently built set of assemblies is needed. |

Why iOS-family platforms need a resolver: a plain `[DllImport("bass")]` does not find a framework embedded in an app bundle. Default probing looks for `bass.dylib`, `libbass.dylib`, `bass` and `libbass` as files, none of which exist inside `bass.framework`. `ManagedBass.NativeLibraryResolver` probes the plain name first, then `bass.framework/bass`, then the app executable itself (which sees every framework linked into it), so an app that ships BASS as a bare dylib keeps resolving exactly as before. On Windows, Linux, Android and desktop macOS it returns immediately and changes nothing.

The automatic registration lives in the consuming **application** because .NET for iOS does not run a class library's module initialiser. It applies to C# projects targeting `ios`, `tvos` or `maccatalyst`; F# and VB apps, or anyone building from source, should call `ManagedBass.NativeLibraryResolver.RegisterAll()` before the first BASS call.

Opting out:

| Opt-out | Where | Effect |
|---|---|---|
| `<ManagedBassRegisterNativeResolver>false</ManagedBassRegisterNativeResolver>` | Your app's csproj | The target stops injecting the automatic registration. |
| `ManagedBass.DisableNativeLibraryResolver` | `AppContext` switch, set before the first BASS call | `RegisterAll()` returns without registering anything. |

Platform questions at runtime go through `ManagedBass.CrossPlatformHelper` (`IsWindows`, `IsLinux`, `IsMacOS`, `IsIOS`, `IsTvOS`, `IsMacCatalyst`, `IsAndroid`, `IsApple`). `IsIOS` is deliberately `false` on Mac Catalyst, unlike `OperatingSystem.IsIOS()`, so the four Apple predicates are mutually exclusive. On `netstandard2.0` only Windows, Linux and macOS can be told apart (Android reports as Linux); on `net45` everything but Windows is `false`.

### Native downloads

The natives are not redistributed. Download them from un4seen: `bass24.zip` (Windows), `bass24-linux.zip`, `bass24-osx.zip`, `bass24-ios.zip`, `bass24-catalyst.zip`, `bass24-android.zip`; add-ons follow the same scheme (`bassmix24-ios.zip` and so on), except Mac Catalyst, where `bass24-catalyst.zip` carries every framework. macOS, iOS and Mac Catalyst are three distinct Mach-O platforms and need three different downloads; dyld will not load the wrong one.

### Unity / iOS

Unity on desktop and Android needs nothing special: use the standard assemblies. iOS is different because IL2CPP resolves every `DllImport` at link time, so the only workable native name is `"__Internal"` (internal-DllImport symbol lookup, exactly as Unity's own iOS plugin documentation requires). This is not static linking: the embedded `bass.framework` is still the ordinary dynamic one.

Each [GitHub release](https://github.com/ManagedBass/ManagedBass/releases) attaches two zips:

| Asset | Contents | Use it for |
|---|---|---|
| `ManagedBass-<version>-dotnet.zip` | Standard assemblies, one folder per target framework, with XML docs | Unity Editor, standalone players, Android, or anyone wanting plain DLLs |
| `ManagedBass-<version>-unity-ios.zip` | The same 33 assemblies as `netstandard2.0`, built for internal-DllImport lookup, plus a `README.txt` | Unity iOS only |

To install both:

1. Put each set in its own folder under `Assets/Plugins/`. From the dotnet zip take the `netstandard2.0/` folder only.
2. Select the internal-DllImport DLLs and, in the Plugin Inspector, tick **iOS** only.
3. Select the standard DLLs and tick Editor, the standalone platforms and Android, with **iOS unticked**.

The two filters **must be mutually exclusive**. Both sets share an assembly identity, so any target with both enabled (the Editor included) is a duplicate-assembly error. If Unity reports the same assembly twice, one of the filters is wrong.

Put `bass.xcframework` from `bass24-ios.zip` under `Assets/Plugins/iOS` and confirm in the generated Xcode project that it is embedded and signed, not merely copied.

Alternatively, vendor `src/` into `Assets/` and add `MANAGEDBASS_INTERNAL_DLLIMPORT` to Player Settings → iOS → Other Settings → Scripting Define Symbols. Define symbols are per build target, so Unity compiles the right variant for each platform and there are no filters to get wrong.

# AddOns
AddOns add more functionality to BASS.

AddOns may need to be loaded into memory before using with BASS. This happens automatically if you call any method on the AddOn class. But, in cases where you use the features of an AddOn directly through BASS, you may need to load it explicitly. This can be done in the best way by calling the `Version` member which is least resource intensive, or you can call any method like `CreateStream` with invalid parameters.

## Plugin AddOns
A Plugin plugs into standard BASS functions like sample or stream creation to provide support for more audio formats.
BASS has built in support for various audio codecs like MPEG, OGG, WAV, AIFF, etc.
A Plugin is loaded using `Bass.PluginLoad` method and unloaded using `Bass.PluginFree`.

## Platform Availability
AddOn        | Is Plugin          | Windows            | Linux              | Mac                | Android            | iOS                | WindowsStore
-------------|--------------------|--------------------|--------------------|--------------------|--------------------|--------------------|------------------
BassAac      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |                    | :heavy_check_mark: |                    |
BassAc3      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |                    |                    |
BassAdx      | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    |                    |
BassAix      | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    |                    |
BassAlac     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |                    | :heavy_check_mark: |                    |
BassApe      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassAsio     |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassCd       | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    |
BassDsd      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassDShow    |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassEnc      |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassEnc_Ogg  |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassEnc_Opus |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassFlac     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassFx       |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark:
BassHls      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassMidi     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark:
BassMix      |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark:
BassMpc      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassOfr      | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    |                    |
BassOpus     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassSfx      |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassSpx      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |                    |                    |
BassTags     |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassTta      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassVst      |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassWA       |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassWaDsp    |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassWasapi   |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassWinamp   |                    | :heavy_check_mark: |                    |                    |                    |                    |
BassWma      | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    |                    |
BassWv       | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
BassZXTune   | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |                    |

## Changelog
See our [releases page](https://github.com/ManagedBass/ManagedBass/releases)

## 
