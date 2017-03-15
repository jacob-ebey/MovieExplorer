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

namespace MovieExplorer.Droid.Activities
{
    [Activity(Label = "Movie Explorer", Theme = "@style/MyTheme")]
    class DetailViewActivity : MvxActivity<DetailViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Detail);

            var image = FindViewById<ImageView>(Resource.Id.detail_poster_image);

            this.SetupToolbar(true);

            if (ViewModel?.Movie != null)
            {
                var movieService = Mvx.Resolve<IMovieService>();

                var similarList = FindViewById<HorizontalListView>(Resource.Id.similar_list);
                similarList.Adapter = new MovieAdapter(this, ViewModel.Similar, Resource.Layout.SmallMovieView, "w92")
                {
                    ClickedCommand = ViewModel.MovieSelectedCommand
                };

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
                    catch (Exception e)
                    {
                        // TODO: Log exception
                    }
                });
            }
        }
    }
}