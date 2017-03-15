using Android.Content;
using Android.Net;
using MovieExplorer.Services;

using static MovieExplorer.Droid.Services.ActivityTracker;

namespace MovieExplorer.Droid.Services
{
    class UriService : IUriService
    {
        public void OpenUrl(string url)
        {
            Intent browserIntent = new Intent(Intent.ActionView, Uri.Parse(url));
            CurrentActivity.StartActivity(browserIntent);
        }
    }
}