// <copyright file="SearchViewModel.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using MovieExplorer.Models;
using MovieExplorer.Services;
using MvvmCross.Platform.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieExplorer.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private const int SearchDelayMillis = 1000;

        private IMovieService _movieService;
        private IMvxMainThreadDispatcher _dispatcher;

        private object _syncTokenSourceSyncRoot = new object();
        private CancellationTokenSource _syncToken;

        public SearchViewModel(IMovieService movieService, IMvxMainThreadDispatcher dispatcher)
        {
            _movieService = movieService;
            _dispatcher = dispatcher;
        }

        public ObservableCollection<MovieListResult> SearchResults { get; } = new ObservableCollection<MovieListResult>();

        private string _query;
        public string Query
        {
            get { return _query; }
            set
            {
                SetProperty(ref _query, value);

                var _ = StartCounterAsync(value);
            }
        }

        private async Task StartCounterAsync(string query)
        {
            CancellationToken token = CancellationToken.None;
            lock (_syncTokenSourceSyncRoot)
            {
                if (_syncToken != null)
                {
                    _syncToken.Cancel();
                }
                _syncToken = new CancellationTokenSource();
                token = _syncToken.Token;
            }

            await Task.Delay(SearchDelayMillis);

            if (token.IsCancellationRequested) return;

            await DoSearchAsync(query, token);
        }

        private Task DoSearchAsync(string query, CancellationToken token)
        {
            return ShowLoaderAsync(async () =>
            {
                if (token.IsCancellationRequested) return;

                SearchResults.Clear();

                var searchResult = await _movieService.SearchAsync(query);

                if (searchResult.Succeeded && (searchResult?.Data?.Results?.Any() ?? false))
                {
                    _dispatcher.RequestMainThreadAction(() =>
                    {
                        if (token.IsCancellationRequested) return;
                        
                        foreach (var item in searchResult.Data.Results)
                        {
                            SearchResults.Add(item);
                        }
                    });
                }
            });
        }
    }
}
