using Android.App;
using Android.Widget;
using Android.OS;
using ManagedBass;
using ManagedBass.Dynamics;

namespace MBassAndroid
{
    /// <summary>
    /// Demonstrates feeding Recorded audio to Speaker output.
    /// </summary>
	[Activity (Label = "MBass Android", MainLauncher = true)]
	public class MainActivity : Activity
	{
        int pushStream;
        Recording R;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            var controller = FindViewById<Button>(Resource.Id.controller);

            controller.Click += (sender, e) =>
            {
                    if (R == null)
                    {
                        R = new Recording();

                        Bass.Init();

                        pushStream = Bass.CreateStream(44100, 2, BassFlags.Default, StreamProcedureType.Push);
                        Bass.ChannelPlay(pushStream);

                        R.DataAvailable += (obj) => Bass.StreamPutData(pushStream, obj.Pointer, obj.ByteLength);

                        R.Start();

                        controller.Text = "Stop";
                    }
                    else
                    {
                        R.Dispose();
                        R = null;

                        Bass.StreamFree(pushStream);

                        controller.Text = "Start";
                    }
            };
		}
	}
}
