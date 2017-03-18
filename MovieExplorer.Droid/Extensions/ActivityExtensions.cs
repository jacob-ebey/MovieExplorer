// <copyright file="ActivityExtensions.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using Android.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using System.Windows.Input;

namespace MovieExplorer.Droid.Extensions
{
    static class ActivityExtensions
    {
        public static void SetupToolbar(this Activity activity, bool addExitButton = false, ICommand searchCommand = null)
        {
            var toolbar = activity.FindViewById<Toolbar>(Resource.Id.toolbar);
            
            if (searchCommand != null)
            {
                var image = activity.FindViewById<ImageView>(Resource.Id.search_image);
                image.Visibility = ViewStates.Visible;
                image.Click += (s, e) =>
                {
                    (s as View).StartAnimation(AnimationUtils.LoadAnimation(activity, Resource.Animation.ClickAnimation));
                    
                    if (searchCommand.CanExecute(null))
                    {
                        searchCommand.Execute(null);
                    }
                };
            }

            if (addExitButton)
            {
                var image = activity.FindViewById<ImageView>(Resource.Id.close_image);
                image.Visibility = ViewStates.Visible;
                image.Click += (s, e) =>
                {
                    (s as View).StartAnimation(AnimationUtils.LoadAnimation(activity, Resource.Animation.ClickAnimation));
                    activity.Finish();
                };
            }

            activity.SetActionBar(toolbar);
        }
    }
}