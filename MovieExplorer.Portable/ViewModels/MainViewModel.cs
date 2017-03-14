using MovieExplorer.Exceptions;
using MovieExplorer.Models;
using MovieExplorer.Services;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieExplorer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private const string _errorMessageTemplate = "No {0} results found... Try reloading...";

        private IMovieService _movieService;
        private bool _isLoading = false;

        // Backing fields
        private IEnumerable<MovieListResult> _topRated, _popular, _nowPlaying;
        private string _topRatedErrorMessage, _popularErrorMessage, _nowPlayingErrorMessage;

        public MainViewModel(IMovieService movieService)
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
        /// A command that reloads the entire pages content.
        /// </summary>
        public ICommand ReloadCommand { get; }

        /// <summary>
        /// A command that expects a <see cref="MovieListResult"/> passed as the parameter.
        /// </summary>
        public ICommand MovieSelectedCommand { get; }

        /// <summary>
        /// The top rated movies.
        /// </summary>
        public IEnumerable<MovieListResult> TopRated
        {
            get { return _topRated; }
            set { SetProperty(ref _topRated, value); }
        }

        /// <summary>
        /// The error message for the top rated context if an issue arises loading the data.
        /// </summary>
        public string TopRatedErrorMessage
        {
            get { return _topRatedErrorMessage; }
            set { SetProperty(ref _topRatedErrorMessage, value); }
        }

        /// <summary>
        /// The popular movies.
        /// </summary>
        public IEnumerable<MovieListResult> Popular
        {
            get { return _popular; }
            set { SetProperty(ref _popular, value); }
        }

        /// <summary>
        /// The error message for the popular context if an issue arises loading the data.
        /// </summary>
        public string PopularErrorMessage
        {
            get { return _popularErrorMessage; }
            set { SetProperty(ref _popularErrorMessage, value); }
        }

        /// <summary>
        /// Movies that are now playing in theaters.
        /// </summary>
        public IEnumerable<MovieListResult> NowPlaying
        {
            get { return _nowPlaying; }
            set { SetProperty(ref _nowPlaying, value); }
        }

        /// <summary>
        /// The error message for the now playing context if an issue arises loading the data.
        /// </summary>
        public string NowPlayingErrorMessage
        {
            get { return _nowPlayingErrorMessage; }
            set { SetProperty(ref _nowPlayingErrorMessage, value); }
        }

        /// <summary>
        /// Call this when the page has been navigated to.
        /// </summary>
        /// <returns></returns>
        public Task OnNavigatedToAsync()
        {
            return LoadAllCategoriesAsync();
        }

        private Task LoadAllCategoriesAsync()
        {
            // Return if we are already loading
            if (_isLoading) return Task.FromResult(false);
            _isLoading = true;

            // Clear messages
            ErrorMessage = null;
            TopRatedErrorMessage = null;
            PopularErrorMessage = null;
            NowPlayingErrorMessage = null;
            
            return ShowLoaderAsync(async () =>
            {
                // Kick off all tasks async
                var topRatedTask = _movieService.GetTopRatedAsync();
                var popularTask = _movieService.GetPopularAsync();
                var nowPlayingTask = _movieService.GetNowPlayingAsync();

                // Await for all results
                var topRatedResult = await topRatedTask;
                var popularResult = await popularTask;
                var nowPlayingResult = await nowPlayingTask;

                // Check top rated for data
                if (topRatedResult.Succeeded && (topRatedResult.Data?.Any() ?? false))
                    TopRated = topRatedResult.Data.Results;
                else
                    TopRatedErrorMessage = string.Format(_errorMessageTemplate, "top rated");

                // Check popular for data
                if (popularResult.Succeeded && (popularResult.Data?.Any() ?? false))
                    Popular = popularResult.Data.Results;
                else
                    PopularErrorMessage = string.Format(_errorMessageTemplate, "popular");
                
                // Check now playing for data
                if (nowPlayingResult.Succeeded && (nowPlayingResult.Data?.Any() ?? false))
                    NowPlaying = nowPlayingResult.Data.Results;
                else
                    NowPlayingErrorMessage = string.Format(_errorMessageTemplate, "now playing");

                // If there was no data for any category show a generic error message because something went seriouslly wrong...
                if (TopRatedErrorMessage != null && PopularErrorMessage != null && NowPlayingErrorMessage != null)
                {
                    throw new MessageException();
                }
            });
        }
    }
}
