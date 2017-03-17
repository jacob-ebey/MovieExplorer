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

        public const string AddToFavoritesLabel = "Add to favorites";
        public const string RemoveFromFavoritesLabel = "Remove from favorites";

        private IMovieService _movieService;
        private IFavoritesService _favoritesService;
        private IUriService _uriService;
        private IToastService _toastService;

        public DetailViewModel(IMovieService movieService, IFavoritesService favoritesService, IUriService uriService, IToastService toastService)
        {
            _movieService = movieService;
            _favoritesService = favoritesService;
            _uriService = uriService;
            _toastService = toastService;

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

            AddToFavoritesCommand = new MvxCommand(() =>
            {
                if (_favoritesService.Contains(Movie.Id))
                {
                    _favoritesService.Remove(Movie.Id);
                    FavoritesButtonText = AddToFavoritesLabel;
                }
                else
                {
                    _favoritesService.Add(Movie);
                    FavoritesButtonText = RemoveFromFavoritesLabel;
                }
            });
        }

        public ICommand PlayVideoCommand { get; }

        public ICommand AddToFavoritesCommand { get; }

        private string _favoritesButtonText = AddToFavoritesLabel;
        public string FavoritesButtonText
        {
            get { return _favoritesButtonText; }
            set { SetProperty(ref _favoritesButtonText, value); }
        }

        public ObservableCollection<MovieListResult> Similar { get; } = new ObservableCollection<MovieListResult>();

        /// <summary>
        /// If the user has any favorites.
        /// </summary>
        public bool HasSimilar
        {
            get { return Similar.Any(); }
        }

        public void Init(MovieListResult movie)
        {
            Movie = movie;
            
            FavoritesButtonText = _favoritesService.Contains(movie.Id) ? RemoveFromFavoritesLabel : AddToFavoritesLabel;
            
            var _ = LoadSimilarAsync();
        }

        private async Task LoadSimilarAsync()
        {
            Similar.Clear();
            RaisePropertyChanged(nameof(HasSimilar));

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

                RaisePropertyChanged(nameof(HasSimilar));
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
