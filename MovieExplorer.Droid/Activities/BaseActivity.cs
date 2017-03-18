// <copyright file="BaseActivity.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using MovieExplorer.Droid.Services;
using MovieExplorer.ViewModels;

namespace MovieExplorer.Droid.Activities
{
    public class BaseActivity<TViewModel> : MvxActivity<TViewModel> where TViewModel : BaseViewModel, IMvxViewModel
    {
        protected override void OnResume()
        {
            base.OnResume();
            ActivityTracker.CurrentActivity = this;
            ViewModel?.OnResume();
        }
    }
}