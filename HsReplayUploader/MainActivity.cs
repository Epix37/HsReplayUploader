#region

using Android.App;
using Android.Content;
using Android.OS;

#endregion

namespace HsReplayUploader
{
	[Activity(Label = "HsReplayUploader", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			LogConfig.Setup();

			StartService(new Intent(this, typeof(LogWatcher)));
		}
	}
}