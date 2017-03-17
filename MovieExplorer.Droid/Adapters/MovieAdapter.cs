using Android.App;
using Android.Views;
using MovieExplorer.Views;
using MovieExplorer.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Widget;
using System;
using System.Collections.Generic;
using Android.Views.Animations;
using System.Windows.Input;
using System.Threading;
using MvvmCross.Platform;
using MovieExplorer.Services;
using System.IO;

namespace MovieExplorer.Droid.Adapters
{
    /// <summary>
    /// An adapter for the movies view.
    /// </summary>
    class MovieAdapter : ObservableCollectionAdapter<MovieListResult>
    {
        private const int TouchTolerance = 3;

        private readonly Dictionary<View, MovieListResult> _itemCache = new Dictionary<View, MovieListResult>();
        private readonly Dictionary<View, CancellationTokenSource> _uiSyncCache = new Dictionary<View, CancellationTokenSource>();
        private object _uiCacheSyncRoot = new object();

        private string _imageSize;
        private HttpClient _client;
        private IMovieService _movieService;

        /// <summary>
        /// Creates a new instance of <see cref="MovieAdapter"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="items">The bindable collection to observe.</param>
        public MovieAdapter(Activity context, ObservableCollection<MovieListResult> items, int resource = Resource.Layout.MovieView, string imageSize = "w154")
            : base(context, resource, items)
        {
            _imageSize = imageSize;
            _client = Mvx.Resolve<HttpClient>();
            _movieService = Mvx.Resolve<IMovieService>();
        }

        /// <summary>
        /// The command to raise with the item as a parameter when a movie is clicked.
        /// </summary>
        public ICommand ClickedCommand { get; set; }

        protected override long GetItemId(MovieListResult item, int position)
        {
            return position;
        }

        protected override void InitializeNewView(View view)
        {
            base.InitializeNewView(view);

            ImageView image = view.FindViewById<ImageView>(Resource.Id.poster_image);

            int oldX = -1;
            int oldY = -1;
            image.Touch += (s, e) =>
            {
                int x = (int)e.Event.GetX();
                int y = (int)e.Event.GetY();

                if (e.Event.Action == MotionEventActions.Down)
                {
                    oldX = x;
                    oldY = y;
                }
                else if (e.Event.Action == MotionEventActions.Up)
                {
                    if (Math.Abs(oldX - x) < TouchTolerance && Math.Abs(oldY - y) < TouchTolerance)
                    {
                        // We get here if we have tapped the image but have not dragged our finger more than the allowed tolerance.
                        // This is to get around a bug in the HorizontalListView, but it also is a place for us to introduce a nice little
                        // animation.
                        (s as View).StartAnimation(AnimationUtils.LoadAnimation(Context, Resource.Animation.ClickAnimation));

                        // Check to see if there is an item associated with the view.
                        if (_itemCache.ContainsKey(s as View))
                        {
                            // If so, raise the clicked command assigned to this adapter.
                            MovieListResult item = _itemCache[s as View];
                            if (ClickedCommand?.CanExecute(item) ?? false)
                            {
                                ClickedCommand.Execute(item);
                            }
                        }
                    }
                }
            };
        }

        protected override void PrepareView(MovieListResult item, View view)
        {
            ImageView image = view.FindViewById<ImageView>(Resource.Id.poster_image);
            _itemCache[image] = item;

            var oldBitmap = image.Drawable as BitmapDrawable;
            if (oldBitmap != null)
            {
                // Dispose of old bitmaps so we don't have a memory leak, no one likes those.
                image.SetImageDrawable(null);
                oldBitmap.Dispose();
                oldBitmap = null;
            }

            // Check to see if an image for the view is currently being loaded, if so cancel it.
            // This will cause the image to be cached but not assigned to a view.
            CancellationToken token = CancellationToken.None;
            lock (_uiCacheSyncRoot)
            {
                if (_uiSyncCache.ContainsKey(image))
                {
                    _uiSyncCache[image].Cancel();
                }

                var tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;
                _uiSyncCache[image] = tokenSource;
            }


            image.SetImageResource(Resource.Drawable.placeholder);
            Task.Run(async () =>
            {
                bool setDefault = false;
                if (!(setDefault = string.IsNullOrWhiteSpace(item.PosterPath)))
                {
                    try
                    {
                        Drawable bitmap = null;
                        Stream stream = await _movieService.GetMoviePosterAsync(item.PosterPath, _imageSize);
                        if (stream != null)
                        {
                            using (stream)
                            {
                                bitmap = await Drawable.CreateFromStreamAsync(stream, null);
                            }

                            Context.RunOnUiThread(() =>
                            {
                                // If the view has not been recycled, set the image.
                                if (!token.IsCancellationRequested)
                                {
                                    image.SetImageDrawable(bitmap);
                                }
                            });
                        }
                        else
                        {
                            setDefault = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Mvx.Resolve<ILogger>().LogException(e);
                        setDefault = true;
                    }
                }

                if (setDefault)
                {
                    Context.RunOnUiThread(() =>
                    {
                        // If the view has not been recycled, set the image.
                        if (!token.IsCancellationRequested)
                        {
                            image.SetImageResource(Resource.Drawable.placeholder);
                        }
                    });
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _itemCache.Clear();
                _uiSyncCache.Clear();
                _client.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}