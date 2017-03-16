using MovieExplorer.Models;
using MovieExplorer.Services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmCross.Core.ViewModels;
using System.Windows.Input;
using MvvmCross.Platform.Core;

namespace MovieExplorer.ViewModels
{
    public class DetailViewModel : BaseViewModel
    {
        private const string YoutubeUrl = "https://www.youtube.com/watch?v={0}";

        private const string AddToWatchlistLabel = "Add to favorites";
        private const string RemoveFromWatchlistLabel = "Remove from favorites";

        private IMovieService _movieService;
        private IWatchlistService _watchlistService;
        private IUriService _uriService;
        private IToastService _toastService;

        public DetailViewModel(IMovieService movieService, IWatchlistService watchlistService, IUriService uriService, IToastService toastService)
        {
            _movieService = movieService;
            _watchlistService = watchlistService;
            _uriService = uriService;
            _toastService = toastService;

            MovieSelectedCommand = new MvxCommand<MovieListResult>(r =>
            {
                if (r != null)
                {
                    ShowViewModel<DetailViewModel>(r);
                }
            });

            PlayVideoCommand = new MvxAsyncCommand(async () =>
            {
                await ShowLoaderAsync(async () =>
                {
                    var videoResult = await _movieService.GetVideosAsync(Movie.Id.ToString());

                    if (videoResult.Succeeded && (videoResult.Data?.Results?.Any() ?? false))
                    {
                        var video = videoResult.Data.Results.First();
                        uriService.OpenUrl(string.Format(YoutubeUrl, video.Key));
                    }
                    else
                    {
                        _toastService.ShowToast("No videos found :(", ToastDuration.Short);
                    }
                });
            });

            AddToWatchlistCommand = new MvxCommand(() =>
            {
                if (_watchlistService.Contains(Movie.Id))
                {
                    _watchlistService.Remove(Movie.Id);
                    WatchlistButtonText = AddToWatchlistLabel;
                }
                else
                {
                    _watchlistService.Add(Movie);
                    WatchlistButtonText = RemoveFromWatchlistLabel;
                }
            });
        }

        /// <summary>
        /// A command that expects a <see cref="MovieListResult"/> passed as the parameter.
        /// </summary>
        public ICommand MovieSelectedCommand { get; }

        public ICommand PlayVideoCommand { get; }

        public ICommand AddToWatchlistCommand { get; }

        private string _watchlistButtonText = AddToWatchlistLabel;
        public string WatchlistButtonText
        {
            get { return _watchlistButtonText; }
            set { SetProperty(ref _watchlistButtonText, value); }
        }

        public ObservableCollection<MovieListResult> Similar { get; } = new ObservableCollection<MovieListResult>();

        public void Init(MovieListResult movie)
        {
            Movie = movie;
            
            WatchlistButtonText = _watchlistService.Contains(movie.Id) ? RemoveFromWatchlistLabel : AddToWatchlistLabel;
            
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
