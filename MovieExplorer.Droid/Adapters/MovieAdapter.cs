using Android.App;
using Android.Views;
using MovieExplorer.Views;
using MovieExplorer.Models;
using System.Collections.ObjectModel;

namespace MovieExplorer.Droid.Adapters
{
    class MovieAdapter : ObservableCollectionAdapter<MovieListResult>
    {
        public MovieAdapter(Activity context, ObservableCollection<MovieListResult> items) 
            : base(context, Resource.Layout.MovieView, items)
        {
        }

        protected override long GetItemId(MovieListResult item, int position)
        {
            return position;
        }

        protected override void PrepareView(MovieListResult item, View view)
        {
            
        }
    }
}