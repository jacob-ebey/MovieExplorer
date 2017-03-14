using Android.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace MovieExplorer.Droid.Extensions
{
    static class ActivityExtensions
    {
        public static void SetupToolbar(this Activity activity, bool addExitButton = false)
        {
            var toolbar = activity.FindViewById<Toolbar>(Resource.Id.toolbar);
            
            if (addExitButton)
            {
                var image = activity.FindViewById<ImageView>(Resource.Id.close_image);
                image.Visibility = ViewStates.Visible;
                image.Click += (s, e) =>
                {
                    (s as View).StartAnimation(AnimationUtils.LoadAnimation(activity, Resource.Animation.PosterClick));
                    activity.Finish();
                };
            }

            activity.SetActionBar(toolbar);
        }
    }
}