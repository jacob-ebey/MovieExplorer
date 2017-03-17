﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MovieExplorer.Models;
using Newtonsoft.Json;

namespace MovieExplorer.Services
{
    public class FavoritesService : IFavoritesService
    {
        private const string WatchlistFile = "watchlist.json";
        private IFileService _fileService;

        public FavoritesService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public ObservableCollection<MovieListResult> Favorites { get; } = new ObservableCollection<MovieListResult>();

        public void Add(MovieListResult movie)
        {
            Favorites.Add(movie);
            Save();
            OnModified();
        }

        public bool Contains(int id)
        {
            return Favorites.Any(f => f.Id == id);
        }

        public void Load()
        {
            Favorites.Clear();

            var json = _fileService.LoadText(WatchlistFile);

            if (json != null)
            {
                try
                {
                    IEnumerable<MovieListResult> loadedList = JsonConvert.DeserializeObject<IEnumerable<MovieListResult>>(json);

                    if (loadedList?.Any() ?? false)
                    {
                        foreach (var item in loadedList)
                        {
                            Favorites.Add(item);
                            OnModified();
                        }
                    }
                }
                catch (Exception e)
                {
                    // TODO: Log exception
                }
            }
        }

        public void Remove(int id)
        {
            var toRemove = Favorites.FirstOrDefault(m => m.Id == id);
            if (toRemove != null)
            {
                Favorites.Remove(toRemove);
                Save();
                OnModified();
            }
        }

        private void Save()
        {
            _fileService.SaveText(WatchlistFile, JsonConvert.SerializeObject(Favorites));
        }

        protected virtual void OnModified() => Modified?.Invoke(this, new EventArgs());

        public event EventHandler Modified;
    }
}