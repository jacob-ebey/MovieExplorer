using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using MovieExplorer.Droid.Services;
using MovieExplorer.ViewModels;
using Android.OS;
using Microsoft.Azure.Mobile.Crashes;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile;

namespace MovieExplorer.Droid.Activities
{
    public class BaseActivity<TViewModel> : MvxActivity<TViewModel> where TViewModel : BaseViewModel, IMvxViewModel
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MobileCenter.Start("e709b5ec-fb3b-434c-a1c9-2e7e18decca1",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnResume()
        {
            base.OnResume();
            ActivityTracker.CurrentActivity = this;
            ViewModel?.OnResume();
        }
    }
}