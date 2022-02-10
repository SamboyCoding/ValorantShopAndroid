using Android.App;
using Android.OS;
using Android.Webkit;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Android.Util;
using System.Collections.Generic;
using ValorantShop.Models;
using Android.Widget;

namespace ValorantShop
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const string authUrl = "https://auth.riotgames.com/authorize?redirect_uri=https%3A%2F%2Fplayvalorant.com%2Fopt_in&client_id=play-valorant-web-prod&response_type=token%20id_token&nonce=1";

        public static MainActivity Instance;

        private WebView _webview;
        private ProgressBar _progressBar;
        private ShopItemRecyclerAdapter _adapter = new();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Instance = this;
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FindViewById<RecyclerView>(Resource.Id.shop_items_recycler).SetAdapter(_adapter);

            _webview = FindViewById<WebView>(Resource.Id.riot_auth_webview) ?? throw new("Could not FindViewById webview");
            _progressBar = FindViewById<ProgressBar>(Resource.Id.shop_progress_spinner);

            Log.Debug("MainActivity", "Checking auth state...");
            if (RiotUserManager.HasAuthData)
            {
                Log.Debug("MainActivity", "Already logged in");
                _webview.Visibility = ViewStates.Gone;
                if (await RiotUserManager.LoginFromSavedCredentials())
                {
                    PopulateShop();
                    return;
                }

                _webview.Visibility = ViewStates.Visible;
            }

            Log.Debug("MainActivity", "Loading Auth URL");
            _webview.Settings.JavaScriptEnabled = true;
            _webview.SetWebViewClient(new LoginInterceptingWebviewClient(this));
            _webview.LoadUrl(authUrl);
        }

        internal async void OnLoginComplete(string cookies)
        {
            _webview.Visibility = ViewStates.Gone;
            await RiotUserManager.LoginFromCookies(cookies);

            PopulateShop();
        }

        internal async void PopulateShop()
        {
            Log.Debug("MainActivity", "Populating Shop");

            var storeItems = await RiotUserManager.CurrentUser.Store.GetPlayerStore();

            List<ShopItem> items = new();
            foreach (var skinId in storeItems.SkinsPanelLayout.SingleItemOffers)
            {
                var (name, url) = await SkinInfoManager.GetNameAndImageForSkin(skinId);
                items.Add(new()
                {
                    Name = name,
                    ImageUrl = url,
                    Price = await SkinInfoManager.GetPriceForSkin(skinId),
                });
            }

            _adapter.SubmitList(items);
            _progressBar.Visibility = ViewStates.Gone;
        }
    }
}