using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace SongAdventureAndroid
{
	[Activity(Label = "Sound Boy"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		//, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = false
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.SensorLandscape
		, ConfigurationChanges = ConfigChanges.Orientation | 
		ConfigChanges.Keyboard | 
		ConfigChanges.KeyboardHidden)]
	public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			var g = new Game1 ();

			HideSystemUI ();
			SetContentView((View)g.Services.GetService(typeof(View)));
			g.Run ();
		}

		private void HideSystemUI()
		{
			if (Android.OS.Build.VERSION.SdkInt >= (Android.OS.BuildVersionCodes)19)
			{
				Android.Views.View decorView = this.Window.DecorView;
				var uiOptions = (int)decorView.SystemUiVisibility;
				var newUiOptions = (int)uiOptions;

				newUiOptions |= (int)Android.Views.SystemUiFlags.LowProfile;
				newUiOptions |= (int)Android.Views.SystemUiFlags.Fullscreen;
				newUiOptions |= (int)Android.Views.SystemUiFlags.HideNavigation;
				newUiOptions |= (int)Android.Views.SystemUiFlags.ImmersiveSticky;

				decorView.SystemUiVisibility = (Android.Views.StatusBarVisibility)newUiOptions;
			}
		}

		protected override void OnPause() {
			ScreenManager.Instance.ChangeScreens("TitleScreen");
			base.OnPause ();
		}

		public bool isApplicationSentToBackground(Context context) {

			ActivityManager am = (ActivityManager) context.GetSystemService(Context.ActivityService);
			System.Collections.Generic.IList<ActivityManager.RunningTaskInfo> tasks = am.GetRunningTasks(1);

			if (tasks.Count > 0) {
				ComponentName topActivity = tasks [0].TopActivity;

				if (!topActivity.PackageName.Equals(context.PackageName)) {
					return true;
				}
			}
			return false;
		}
	

	}
}



