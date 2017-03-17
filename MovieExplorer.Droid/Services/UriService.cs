using Android.Content;
using MovieExplorer.Services;
using System;
using static MovieExplorer.Droid.Services.ActivityTracker;

namespace MovieExplorer.Droid.Services
{
    class UriService : IUriService
    {
        public void OpenUrl(string url)
        {
            Uri uri = null;
            if (CurrentActivity != null && Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                CurrentActivity.StartActivity(browserIntent);
            }
        }
    }
}