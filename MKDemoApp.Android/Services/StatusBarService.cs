using Android.OS;
using Android.Views;
using MKDemoApp.Droid.Services;
using MKDemoApp.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using ColorExtensions = Xamarin.Forms.Platform.Android.ColorExtensions;

[assembly: Dependency(typeof(StatusBarService))]

namespace MKDemoApp.Droid.Services
{
    public sealed class StatusBarService : IStatusBarService
    {
        public void SetColor(Color color) => SetStatusBarStyle(color, 0);

        private static void SetStatusBarStyle(Color statusBarColor, StatusBarVisibility statusBarVisibility)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                    {
                        return;
                    }

                    var currentWindow = GetCurrentWindow();

                    if (currentWindow == null)
                    {
                        return;
                    }

                    currentWindow.DecorView.SystemUiVisibility = statusBarVisibility;
                    currentWindow.SetStatusBarColor(ColorExtensions.ToAndroid(statusBarColor));
                }
                catch (Exception)
                {
                    // Ignored
                }
            });
        }

        private static Window GetCurrentWindow()
        {
            var window = Platform.CurrentActivity?.Window;

            if (window == null)
            {
                return null;
            }

            // clear FLAG_TRANSLUCENT_STATUS flag:
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);

            // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            return window;
        }
    }
}