using Android.App;
using Cheesebaron.HorizontalListView;
using MovieExplorer.Droid.Adapters;
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

            var topRatedList = FindViewById<HorizontalListView>(Resource.Id.top_rated_list);
            topRatedList.Adapter = new MovieAdapter(this, ViewModel.TopRated);

            await ViewModel.OnNavigatedToAsync();
        }
    }
}

