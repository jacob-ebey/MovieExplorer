using ModernHttpClient;
using MovieExplorer.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace MovieExplorer.Services
{
    /// <summary>
    /// A cross platform implementation of <see cref="IMovieService"/>.
    /// </summary>
    class MovieService : IMovieService
    {
        private const string ImageUrl = "http://image.tmdb.org/t/p/w342/{0}";

        private readonly Dictionary<string, Task<Stream>> _posterCache = new Dictionary<string, Task<Stream>>();
        private object _posterSyncRoot = new object();

        private IEndpointList _endpoints;
        private HttpClient _client;

        public MovieService(IEndpointList endpoints)
        {
            _endpoints = endpoints;
            _client = new HttpClient(new NativeMessageHandler());
        }

        public Task<ServiceResult<ResultsModel<MovieListResult>>> GetNowPlayingAsync()
        {
            return DoRequestAsync<ResultsModel<MovieListResult>>(_endpoints.NowPlayingUri);
        }

        public Task<ServiceResult<ResultsModel<MovieListResult>>> GetPopularAsync()
        {
            return DoRequestAsync<ResultsModel<MovieListResult>>(_endpoints.PopularUri);
        }

        public Task<ServiceResult<ResultsModel<MovieListResult>>> GetSimilarAsync(string movieId)
        {
            return DoRequestAsync<ResultsModel<MovieListResult>>(_endpoints.GetSimilarUri(movieId));
        }

        public Task<ServiceResult<ResultsModel<MovieListResult>>> GetTopRatedAsync()
        {
            return DoRequestAsync<ResultsModel<MovieListResult>>(_endpoints.TopRatedUri);
        }

        public Task<ServiceResult<VideoResults>> GetVideosAsync(string movieId)
        {
            return DoRequestAsync<VideoResults>(_endpoints.GetVideosUri(movieId));
        }

        public Task<ServiceResult<ResultsModel<MovieListResult>>> SearchAsync(string query)
        {
            return DoRequestAsync<ResultsModel<MovieListResult>>(_endpoints.GetSearchUri(query));
        }

        private async Task<ServiceResult<T>> DoRequestAsync<T>(Uri uri)
        {
            ServiceResult<T> result = new ServiceResult<T>();

            try
            {
                var json = await _client.GetStringAsync(uri);
                var dataModel = JsonConvert.DeserializeObject<T>(json);
                result.Data = dataModel;
                result.Succeeded = true;
                return result;
            }
            catch (Exception e)
            {
                result.InnerException = e;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<Stream> GetMoviePosterAsync(string posterPath, string imageSize)
        {
            string src = string.Format(ImageUrl, posterPath, imageSize ?? "w92");

            Task<Stream> t = null;
            lock (_posterSyncRoot)
            {
                // Check the cache
                if (_posterCache.ContainsKey(src))
                {
                    t = _posterCache[src];
                }

                // If nothing in the cache or the image grab failed last time, get a new image.
                if (t == null)
                {
                    _posterCache[src] = t = Task.Run(async () =>
                    {
                        Stream newStream = new MemoryStream();

                        try
                        {
                            using (var s = await _client.GetStreamAsync(src))
                            {
                                s.CopyTo(newStream);
                            }
                        }
                        catch { }

                        return newStream;
                    });
                }
            }

            // Copy the result to a new stream so android doesn't loose it's shit
            MemoryStream result = null;

            Stream stream = await t;
            if (stream != null)
            {
                result = new MemoryStream();
                lock (stream)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(result);
                }
            }

            result.Seek(0, SeekOrigin.Begin);
            return result;
        }
    }
}