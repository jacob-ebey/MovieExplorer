// <copyright file="MainViewModel.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using MovieExplorer.Models;
using MovieExplorer.Services;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieExplorer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private const string _errorMessageTemplate = "No {0} results found... Try reloading...";

        private IMovieService _movieService;
        private IFavoritesService _favoritesService;
        private ILogger _logger;
        
        private bool _reloadFavorites = true;

        // Backing fields
        private string _topRatedErrorMessage, _popularErrorMessage, _nowPlayingErrorMessage;

        public MainViewModel(IMovieService movieService, IFavoritesService favoritesService, ILogger logger)
        {
            _movieService = movieService;
            _favoritesService = favoritesService;
            _logger = logger;
            
            _favoritesService.Modified += (s, e) => _reloadFavorites = true;

            SearchCommand = new MvxCommand(() => ShowViewModel<SearchViewModel>());

            ReloadCommand = new MvxAsyncCommand(async () =>
            {
                await LoadAllCategoriesAsync();
            });
        }

        public ICommand SearchCommand { get; }

        /// <summary>
        /// A command that reloads the entire pages content.
        /// </summary>
        public ICommand ReloadCommand { get; }
                
        /// <summary>
        /// If the user has any favorites.
        /// </summary>
        public bool HasFavorites
        {
            get { return Favorites.Any(); }
        }

        /// <summary>
        /// Inverse of <see cref="HasFavorites"/>.
        /// </summary>
        public bool InverseHasFavorites { get { return !HasFavorites; } }

        /// <summary>
        /// The favorited movies.
        /// </summary>
        public ObservableCollection<MovieListResult> Favorites { get; } = new ObservableCollection<MovieListResult>();

        /// <summary>
        /// The top rated movies.
        /// </summary>
        public ObservableCollection<MovieListResult> TopRated { get; } = new ObservableCollection<MovieListResult>();

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
        public ObservableCollection<MovieListResult> Popular { get; } = new ObservableCollection<MovieListResult>();

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
        public ObservableCollection<MovieListResult> NowPlaying { get; } = new ObservableCollection<MovieListResult>();

        /// <summary>
        /// The error message for the now playing context if an issue arises loading the data.
        /// </summary>
        public string NowPlayingErrorMessage
        {
            get { return _nowPlayingErrorMessage; }
            set { SetProperty(ref _nowPlayingErrorMessage, value); }
        }

        public override void OnResume()
        {
            ReloadFavorites();
        }

        /// <summary>
        /// Call this when the page has been navigated to.
        /// </summary>
        /// <returns></returns>
        public Task OnNavigatedToAsync()
        {
            _favoritesService.Load();

            return LoadAllCategoriesAsync();
        }

        private void ReloadFavorites()
        {
            if (_reloadFavorites)
            {
                Favorites.Clear();
                // Show the newest first
                foreach (var item in _favoritesService.Favorites.Reverse())
                {
                    Favorites.Add(item);
                }

                RaisePropertyChanged(nameof(HasFavorites));
                RaisePropertyChanged(nameof(InverseHasFavorites));

                _reloadFavorites = false;
            }
        }

        private async Task LoadAllCategoriesAsync()
        {
            // Return if we are already loading
            if (ShowLoader) return;

            try
            {
                // Clear messages
                ErrorMessage = null;
                TopRatedErrorMessage = null;
                PopularErrorMessage = null;
                NowPlayingErrorMessage = null;

                TopRated.Clear();
                Popular.Clear();
                NowPlaying.Clear();

                await ShowLoaderAsync(async () =>
                {
                    // Kick off all tasks async
                    var topRatedTask = _movieService.GetTopRatedAsync();
                    var popularTask = _movieService.GetPopularAsync();
                    var nowPlayingTask = _movieService.GetNowPlayingAsync();

                    ReloadFavorites();

                    // Await for all results
                    var topRatedResult = await topRatedTask;
                    var popularResult = await popularTask;
                    var nowPlayingResult = await nowPlayingTask;


                    PrepTopRatedResult(topRatedResult);
                    PrepPopularResult(popularResult);
                    PrepNowPlayingResult(nowPlayingResult);
                });
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        private void PrepTopRatedResult(ServiceResult<ResultsModel<MovieListResult>> topRatedResult)
        {
            // Check top rated for data
            if (topRatedResult.Succeeded && (topRatedResult.Data?.Results?.Any() ?? false))
            {
                foreach (var item in topRatedResult.Data.Results)
                {
                    TopRated.Add(item);
                }
            }
            else
            {
                TopRatedErrorMessage = string.Format(_errorMessageTemplate, "top rated");
            }
        }

        private void PrepPopularResult(ServiceResult<ResultsModel<MovieListResult>> popularResult)
        {
            // Check popular for data
            if (popularResult.Succeeded && (popularResult.Data?.Results?.Any() ?? false))
            {
                foreach (var item in popularResult.Data.Results)
                {
                    Popular.Add(item);
                }
            }
            else
            {
                PopularErrorMessage = string.Format(_errorMessageTemplate, "popular");
            }
        }

        private void PrepNowPlayingResult(ServiceResult<ResultsModel<MovieListResult>> nowPlayingResult)
        {
            // Check now playing for data
            if (nowPlayingResult.Succeeded && (nowPlayingResult.Data?.Results?.Any() ?? false))
            {
                foreach (var item in nowPlayingResult.Data.Results)
                {
                    NowPlaying.Add(item);
                }
            }
            else
            {
                NowPlayingErrorMessage = string.Format(_errorMessageTemplate, "now playing");
            }
        }
    }
}
