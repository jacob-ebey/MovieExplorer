using System;
using MovieExplorer.Models;
using MovieExplorer.Services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace MovieExplorer.ViewModels
{
    class DetailViewModel : BaseViewModel
    {
        private IMovieService _movieService;

        public DetailViewModel(IMovieService movieService)
        {
            _movieService = movieService;

            MovieSelectedCommand = new MvxCommand<MovieListResult>(r =>
            {
                if (r != null)
                {
                    ShowViewModel<DetailViewModel>(r);
                }
            });
        }

        /// <summary>
        /// A command that expects a <see cref="MovieListResult"/> passed as the parameter.
        /// </summary>
        public ICommand MovieSelectedCommand { get; }

        public ObservableCollection<MovieListResult> Similar { get; } = new ObservableCollection<MovieListResult>();

        public void Init(MovieListResult movie)
        {
            Movie = movie;

            var _ = LoadSimilarAsync();
        }

        private async Task LoadSimilarAsync()
        {
            Similar.Clear();

            await ShowLoaderAsync(async () =>
            {
                var result = await _movieService.GetSimilarAsync(Movie?.Id.ToString());

                if (result.Succeeded && (result.Data?.Results?.Any() ?? false))
                {
                    foreach (var item in result.Data.Results)
                    {
                        Similar.Add(item);
                    }
                }
            });
        }

        private MovieListResult _movie;
        public MovieListResult Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }
    }
}
