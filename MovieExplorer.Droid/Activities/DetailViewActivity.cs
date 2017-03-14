using Android.App;
using MovieExplorer.ViewModels;
using MvvmCross.Droid.Views;
using MovieExplorer.Droid.Extensions;

namespace MovieExplorer.Droid.Activities
{
    [Activity(Label = "Movie Explorer", Theme = "@style/MyTheme")]
    class DetailViewActivity : MvxActivity<DetailViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Detail);

            this.SetupToolbar(true);
        }
    }
}