using MovieExplorer.Models;

namespace MovieExplorer.ViewModels
{
    class DetailViewModel : BaseViewModel
    {
        public void Init(MovieListResult movie)
        {
            Movie = movie;
        }

        private MovieListResult _movie;
        public MovieListResult Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }
    }
}
