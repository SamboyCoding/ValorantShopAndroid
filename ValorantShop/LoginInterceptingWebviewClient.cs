using Android;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Webkit;

namespace ValorantShop
{
    internal class LoginInterceptingWebviewClient : WebViewClient
    {
        private MainActivity mainActivity;

        public LoginInterceptingWebviewClient(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }


        public override void DoUpdateVisitedHistory(WebView view, string url, bool isReload)
        {
            base.DoUpdateVisitedHistory(view, url, isReload);
            if (url.ToLowerInvariant().StartsWith("https://playvalorant.com/opt_in/#"))
            {
                //Login complete, grab cookies
                var cookies = CookieManager.Instance.GetCookie("https://auth.riotgames.com");

                mainActivity.OnLoginComplete(cookies);
            }
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }
    }
}