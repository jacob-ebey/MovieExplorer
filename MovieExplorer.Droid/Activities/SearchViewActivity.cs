// <copyright file="SearchViewActivity.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using Android.App;
using Android.Content.PM;
using Android.Widget;
using MovieExplorer.Droid.Adapters;
using MovieExplorer.Droid.Extensions;
using MovieExplorer.ViewModels;

namespace MovieExplorer.Droid.Activities
{
    [Activity(Label = "Movie Explorer", Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class SearchViewActivity : BaseActivity<SearchViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Search);
            this.SetupToolbar(true);

            var grid = FindViewById<GridView>(Resource.Id.results_grid);
            grid.Adapter = new MovieAdapter(this, ViewModel.SearchResults) { ClickedCommand = ViewModel.NavigateToMovieDetailCommand };
        }
    }
}

