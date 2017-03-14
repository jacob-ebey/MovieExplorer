using Android.App;
using Android.Views;
using MovieExplorer.Views;
using MovieExplorer.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using ModernHttpClient;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Widget;
using System;
using System.Collections.Generic;
using Android.Views.Animations;
using System.Windows.Input;

namespace MovieExplorer.Droid.Adapters
{
    class MovieAdapter : ObservableCollectionAdapter<MovieListResult>, IDisposable
    {
        private const int TouchTolerance = 4;
        private const string ImageUrl = "http://image.tmdb.org/t/p/w342/{0}";

        private readonly Dictionary<string, Drawable> _posterCache = new Dictionary<string, Drawable>();
        private object _posterSyncRoot = new object();

        private readonly Dictionary<View, MovieListResult> _itemCache = new Dictionary<View, MovieListResult>();

        private HttpClient _client;

        public MovieAdapter(Activity context, ObservableCollection<MovieListResult> items)
            : base(context, Resource.Layout.MovieView, items)
        {
            _client = new HttpClient(new NativeMessageHandler());
        }

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
                        (s as View).StartAnimation(AnimationUtils.LoadAnimation(Context, Resource.Animation.PosterClick));

                        if (_itemCache.ContainsKey(s as View))
                        {
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

            string src = string.Format(ImageUrl, item.PosterPath);
            bool contains = false;

            lock (_posterSyncRoot)
            {
                if (contains = _posterCache.ContainsKey(src))
                {
                    image.SetImageDrawable(_posterCache[src]);
                }
            }

            if (!contains)
            {
                image.SetImageResource(Resource.Drawable.Icon);
                Task.Run(async () =>
                {
                    try
                    {
                        using (var stream = await _client.GetStreamAsync(src))
                        {
                            var bitmap = await BitmapDrawable.CreateFromStreamAsync(stream, src);
                            Context.RunOnUiThread(() =>
                            {
                                image.SetImageDrawable(bitmap);
                            });

                            lock (_posterSyncRoot)
                            {
                                _posterCache[src] = bitmap;
                            }
                        }
                    }
                    catch { }
                });
            }
        }

        void IDisposable.Dispose()
        {

        }
    }
}