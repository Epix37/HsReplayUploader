#region

using System;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

#endregion

namespace HsReplayUploader
{
	[Activity(Label = "HsReplayUploader", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		public static MainActivity Instance;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			Instance = this;

			SetContentView(Resource.Layout.Main);

			LogConfig.Setup();

			StartService(new Intent(this, typeof(LogWatcher)));
			ShowNotification();
		}

		protected override void OnDestroy()
		{
			HideNotification();
			base.OnDestroy();
		}

		public void Log(string msg)
			=> RunOnUiThread(() => FindViewById<TextView>(Resource.Id.TextViewLog).Text += $"\n[{DateTime.Now.ToString("T")}] " + msg);

		public void ShowNotification() => NotificationManager.FromContext(this).Notify(0, Notification);
		public void HideNotification() => NotificationManager.FromContext(this)?.Cancel(0);

		private Notification Notification => _notification ?? (_notification = BuildNotification());

		private Notification BuildNotification()
		{
			return
				new Notification.Builder(this).SetContentTitle("HSReplay Uploader")
											  .SetContentText("Running!")
											  .SetSmallIcon(Resource.Drawable.Icon)
											  .SetOngoing(true)
											  .Build();
		}

		private Notification _notification;
	}
}