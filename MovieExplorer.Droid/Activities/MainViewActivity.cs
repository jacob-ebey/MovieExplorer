using Android.App;
using Cheesebaron.HorizontalListView;
using MovieExplorer.Droid.Adapters;
using MovieExplorer.Droid.Extensions;
using MovieExplorer.Droid.Services;
using MovieExplorer.ViewModels;
using MvvmCross.Droid.Views;

namespace MovieExplorer.Droid.Activities
{
    [Activity(Label = "Movie Explorer", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainViewActivity : MvxActivity<MainViewModel>
    {
        protected override void OnResume()
        {
            base.OnResume();
            ActivityTracker.CurrentActivity = this;
        }

        protected override async void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Main);

            this.SetupToolbar();

            var topRatedList = FindViewById<HorizontalListView>(Resource.Id.top_rated_list);
            topRatedList.Adapter = new MovieAdapter(this, ViewModel.TopRated) { ClickedCommand = ViewModel.MovieSelectedCommand };
            
            var popularList = FindViewById<HorizontalListView>(Resource.Id.popular_list);
            popularList.Adapter = new MovieAdapter(this, ViewModel.Popular) { ClickedCommand = ViewModel.MovieSelectedCommand };

            var nowPlayingList = FindViewById<HorizontalListView>(Resource.Id.now_playing_list);
            nowPlayingList.Adapter = new MovieAdapter(this, ViewModel.NowPlaying) { ClickedCommand = ViewModel.MovieSelectedCommand };
            
            await ViewModel.OnNavigatedToAsync();
        }
    }
}

