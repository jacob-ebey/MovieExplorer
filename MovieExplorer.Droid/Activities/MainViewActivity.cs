﻿// <copyright file="MainViewActivity.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Cheesebaron.HorizontalListView;
using Microsoft.Azure.Mobile.Crashes;
using MovieExplorer.Droid.Adapters;
using MovieExplorer.Droid.Extensions;
using MovieExplorer.Services;
using MovieExplorer.ViewModels;
using MvvmCross.Platform;
using System.Threading.Tasks;

namespace MovieExplorer.Droid.Activities
{
    [Activity(Label = "Movie Explorer", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainViewActivity : BaseActivity<MainViewModel>
    {
        protected override async void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Main);

            this.SetupToolbar(searchCommand: ViewModel.SearchCommand);

            SwipeRefreshLayout swipe = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe);
            swipe.SetDistanceToTriggerSync(120);
            swipe.Refreshing = ViewModel.ShowLoader;

            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.ShowLoader))
                {
                    swipe.Refreshing = ViewModel.ShowLoader;
                }
            };

            swipe.Refresh += (s, e) =>
            {
                swipe.Refreshing = true;
                if (ViewModel.ReloadCommand.CanExecute(null))
                {
                    ViewModel.ReloadCommand.Execute(null);
                }
            };

            var topRatedList = FindViewById<HorizontalListView>(Resource.Id.top_rated_list);
            topRatedList.Adapter = new MovieAdapter(this, ViewModel.TopRated) { ClickedCommand = ViewModel.NavigateToMovieDetailCommand };
            
            var popularList = FindViewById<HorizontalListView>(Resource.Id.popular_list);
            popularList.Adapter = new MovieAdapter(this, ViewModel.Popular) { ClickedCommand = ViewModel.NavigateToMovieDetailCommand };

            var nowPlayingList = FindViewById<HorizontalListView>(Resource.Id.now_playing_list);
            nowPlayingList.Adapter = new MovieAdapter(this, ViewModel.NowPlaying) { ClickedCommand = ViewModel.NavigateToMovieDetailCommand };

            var favoritesList = FindViewById<HorizontalListView>(Resource.Id.favorites_list);
            favoritesList.Adapter = new MovieAdapter(this, ViewModel.Favorites) { ClickedCommand = ViewModel.NavigateToMovieDetailCommand };

            await ViewModel.OnNavigatedToAsync();
        }
    }
}

