using Foundation;
using MKDemoApp.iOS.Services;
using MKDemoApp.Services;
using System;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(StatusBarService))]

namespace MKDemoApp.iOS.Services
{
    public sealed class StatusBarService : IStatusBarService
    {
        public void SetColor(Color color)
        {
            SetStatusBarStyle(color, UIStatusBarStyle.DarkContent);
        }

        private static void SetStatusBarStyle(Color statusBarColor, UIStatusBarStyle statusBarStyle)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) && UIApplication.SharedApplication.KeyWindow
                        .WindowScene?.StatusBarManager?.StatusBarFrame != null)
                    {
                        var statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene
                            .StatusBarManager.StatusBarFrame)
                        {
                            BackgroundColor = statusBarColor.ToUIColor()
                        };

                        UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                    }
                    else if (
                        UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) is UIView statusBar &&
                        statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                    {
                        statusBar.BackgroundColor = statusBarColor.ToUIColor();
                    }

                    UIApplication.SharedApplication.SetStatusBarStyle(statusBarStyle, false);
                    GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
                }
                catch (Exception)
                {
                    // Ignored
                }
            });
        }

        private static UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;

            var vc = window.RootViewController;

            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            return vc;
        }
    }
}