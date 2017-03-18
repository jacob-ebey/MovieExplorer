// <copyright file="ActivityTracker.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using Android.App;

namespace MovieExplorer.Droid.Services
{
    static internal class ActivityTracker
    {
        static internal Activity CurrentActivity { get; set; }
    }
}