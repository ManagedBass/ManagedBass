using Android.App;
using Android.Widget;
using Android.OS;
using ManagedBass;

namespace MBassAndroid
{
    /// <summary>
    /// Demonstrates feeding Recorded audio to Speaker output.
    /// </summary>
	[Activity (Label = "MBass Android", MainLauncher = true)]
	public class MainActivity : Activity
    {
        SineWave _sineWave;

		protected override void OnCreate (Bundle SavedInstanceState)
		{
			base.OnCreate (SavedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

            var controller = FindViewById<Button>(Resource.Id.controller);

            controller.Click += (sender, e) =>
            {
                if (_sineWave == null)
                {
                    _sineWave = new SineWave(20, 20, 40, 100);
                    _sineWave.Start();

                    controller.Text = "Stop";
                }
                else
                {
                    _sineWave.Dispose();
                    _sineWave = null;
                    
                    controller.Text = "Start SineWave";
                }
            };
		}
	}
}
