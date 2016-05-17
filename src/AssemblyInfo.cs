using System.Reflection;

[assembly: AssemblyTitle ("ManagedBass")]
[assembly: AssemblyCompany("Mathew Sachin")]
[assembly: AssemblyProduct("ManagedBass")]
[assembly: AssemblyCopyright("(c) 2016 Mathew Sachin")]

[assembly: AssemblyVersion("0.22.0")]

[assembly: AssemblyDescription ("Free Open-Source " +

#if WINDOWS
    "Windows"
#elif LINUX
    "Linux"
#elif __MAC__
    "Max"
#elif __ANDROID__
    "Xamarin.Android"
#elif __IOS__
    "Xamarin.iOS"
#else
    "-"
#endif

    + " .Net Wrapper for Un4seen Bass and its AddOns")]


