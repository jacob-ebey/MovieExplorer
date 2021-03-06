// <copyright file="DetailViewActivity.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using Android.App;
using MovieExplorer.ViewModels;
using MvvmCross.Droid.Views;
using MovieExplorer.Droid.Extensions;
using MvvmCross.Platform;
using MovieExplorer.Services;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Widget;
using System;
using Cheesebaron.HorizontalListView;
using MovieExplorer.Droid.Adapters;
using MovieExplorer.Droid.Services;
using Android.Content.PM;

namespace MovieExplorer.Droid.Activities
{
    [Activity(Label = "Movie Explorer", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    class DetailViewActivity : BaseActivity<DetailViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Detail);

            var image = FindViewById<ImageView>(Resource.Id.detail_poster_image);

            this.SetupToolbar(true);

            if (ViewModel?.Movie != null)
            {
                // Fetch the move service to fetch the image stream. We use this so
                // we can retrieve cached versions if they already have been loaded.
                var movieService = Mvx.Resolve<IMovieService>();

                // Setup the similar list
                var similarList = FindViewById<HorizontalListView>(Resource.Id.similar_list);
                similarList.Adapter = new MovieAdapter(this, ViewModel.Similar, Resource.Layout.SmallMovieView, "w92")
                {
                    ClickedCommand = ViewModel.NavigateToMovieDetailCommand
                };

                // Kick off a task to fetch the poster image
                Task.Run(async () =>
                {
                    try
                    {
                        var stream = await movieService.GetMoviePosterAsync(ViewModel.Movie.PosterPath, "w154");
                        var bitmap = await Drawable.CreateFromStreamAsync(stream, ViewModel.Movie.PosterPath);

                        RunOnUiThread(() =>
                        {
                            image.SetImageDrawable(bitmap);
                        });
                    }
                    catch { } // Don't care about this one. 
                });
            }
        }
    }
}