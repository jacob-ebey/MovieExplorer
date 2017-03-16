using MovieExplorer.Models;
using System;
using System.Collections.ObjectModel;

namespace MovieExplorer.Services
{
    public interface IWatchlistService
    {
        ObservableCollection<MovieListResult> Favorites { get; }

        void Add(MovieListResult movie);
        bool Contains(int id);
        void Load();
        void Remove(int id);

        event EventHandler Modified;
    }
}
