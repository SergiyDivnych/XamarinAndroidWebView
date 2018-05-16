using System;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace WebView
{
    [Activity(Label = "Video Tube", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Android.Webkit.WebView _webView;
        bool _doubleBackToExitPressedOnce = false;
        readonly Toast _toast = Toast.MakeText(Application.Context, "Press again to exit", ToastLength.Short);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            ActionBar.Hide();
            var webView = FindViewById<Android.Webkit.WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            webView.Settings.SetPluginState(WebSettings.PluginState.On);
            webView.Settings.MediaPlaybackRequiresUserGesture = false;
            webView.SetWebChromeClient(new WebChromeClient());
            webView.SetWebViewClient(new HybridWebViewClient());
            webView.LoadUrl("your web site");
            webView.Settings.BuiltInZoomControls = true;
            _webView = webView;
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && _webView != null)
            {
                try
                {
                    if (_webView.CanGoBack())
                    {
                        //Toast.MakeText(this, "Allow go back", ToastLength.Short).Show();
                        _webView.GoBack();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Video tube", ex.Message);
                }
            }
            else
            {
                Log.Error("Video tube", "Null webview...");
            }

            if (_doubleBackToExitPressedOnce)
            {
                _toast.Cancel();
                base.OnBackPressed();
               }

            _toast.SetMargin(0, 0.20f);
            _toast.Show();

            this._doubleBackToExitPressedOnce = true;

            new Handler().PostDelayed(() =>
            {
                _doubleBackToExitPressedOnce = false;
            }, 2000);
            return false;
        }

        private class HybridWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView webView, string url)
            {
                webView.LoadUrl(url);
                return false;
            }
        }
    }
}

