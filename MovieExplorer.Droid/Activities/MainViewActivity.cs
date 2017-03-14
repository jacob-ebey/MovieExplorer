using Android.App;
using MovieExplorer.ViewModels;
using MvvmCross.Droid.Views;

namespace MovieExplorer.Droid.Activities
{
    [Activity(Label = "Movie Explorer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainViewActivity : MvxActivity<MainViewModel>
    {
        public MainViewActivity() { }

        protected override async void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Main);

            await ViewModel.OnNavigatedToAsync();
        }
    }
}

