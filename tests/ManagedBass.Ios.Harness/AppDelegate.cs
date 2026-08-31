using System.Runtime.InteropServices;
using Foundation;
using UIKit;

namespace ManagedBass.Ios.Harness;

/// <summary>Boots a bare UIKit app, runs <see cref="HarnessProbe"/> once and exits with its result.</summary>
[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    private UIWindow? _window;

    public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        _window = new UIWindow(UIScreen.MainScreen.Bounds) { RootViewController = new UIViewController() };
        _window.MakeKeyAndVisible();

        // NOTE: exiting from inside FinishedLaunching is reported by the simulator as a launch failure.
        NSTimer.CreateScheduledTimer(0.5, false, _ => RunProbe());
        return true;
    }

    private static void RunProbe()
    {
        var exitCode = 1;

        try
        {
            exitCode = HarnessProbe.Run(Emit);
        }
        catch (Exception ex)
        {
            Emit($"{HarnessProbe.ResultPrefix}exception={ex.GetType().Name}");
            Emit($"{HarnessProbe.ResultPrefix}outcome=FAIL reason=harnesscrash");
        }

        Console.Out.Flush();

        // Exiting is what lets `simctl launch --console-pty` return.
        NSTimer.CreateScheduledTimer(0.5, false, _ => Environment.Exit(exitCode));
    }

    // Both channels on purpose: --console-pty reads stdout, `log stream` reads NSLog, and which one
    // survives a given Xcode version has varied.
    private static void Emit(string line)
    {
        Console.WriteLine(line);

        // NOTE: the text is passed as the format string with % escaped, so no variadic argument is needed;
        // arm64 passes variadics on the stack, which a fixed-arity DllImport would get wrong.
        using var message = new NSString(line.Replace("%", "%%"));
        NSLog(message.Handle);
    }

    [DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
    private static extern void NSLog(IntPtr format);
}
