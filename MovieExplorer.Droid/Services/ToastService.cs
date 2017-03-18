// <copyright file="ToastService.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using Android.Widget;
using MovieExplorer.Services;

using static MovieExplorer.Droid.Services.ActivityTracker;

namespace MovieExplorer.Droid.Services
{
    class ToastService : IToastService
    {
        public void ShowToast(string message, ToastDuration duration)
        {
            if (CurrentActivity != null)
            {
                Toast.MakeText(
                    CurrentActivity, 
                    message, 
                    duration == ToastDuration.Short ? ToastLength.Short : ToastLength.Long)
                    .Show();
            }
        }
    }
}