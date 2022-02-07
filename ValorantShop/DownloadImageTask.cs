using System;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Widget;
using Object = Java.Lang.Object;
using Void = Java.Lang.Void;

namespace ValorantShop;

public class DownloadImageTask : AsyncTask<string, Void, Bitmap?>
{
    ImageView bmImage;

    public DownloadImageTask(ImageView bmImage) {
        this.bmImage = bmImage;
    }
    
    protected override Bitmap? RunInBackground(params string[] urls)
    {
        Bitmap? mIcon11 = null;
        try {
            var input = new Java.Net.URL(urls[0]).OpenStream();
            mIcon11 = BitmapFactory.DecodeStream(input);
        } catch (Exception e) {
            Log.Error("Error", e.ToString());
        }
        return mIcon11;
    }

    protected override void OnPostExecute(Object? result)
    {
        bmImage.SetImageBitmap(result as Bitmap);
    }
}