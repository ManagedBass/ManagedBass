# ManagedBass Android Sample
The current sample demonstrates using ManagedBass to feed recorded audio to Speaker output.

>The same ManagedBass.dll which is used for .Net development can be used with Xamarin.Android.

## Set-Up for Android Studio (for those new to Xamarin.Android)
* Permissions Required: `RecordAudio`
* Put the specific versions of `libbass.so` (available on Bass website) in the project folders as per your requirement.
* Right click the `libbass.so` file, Select Properties.
  * Set `Copy to Output Directory` = **Always**
  * Set `Build Action` = **AndroidNativeLibrary**

>I recommend using [Xamarin Android Player](https://xamarin.com/android-player) which is much better in speed and features than Google provided emulators.
