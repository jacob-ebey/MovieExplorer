using ModernHttpClient;
using MovieExplorer.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace MovieExplorer.Services
{
    /// <summary>
    /// A cross platform implementation of <see cref="IMovieService"/>.
    /// </summary>
    public class MovieService : IMovieService
    {
        private const string ImageUrl = "http://image.tmdb.org/t/p/{1}{0}";

        private readonly Dictionary<string, Task<Stream>> _posterCache = new Dictionary<string, Task<Stream>>();
        private readonly Dictionary<string, Mutex> _posterMutexCache = new Dictionary<string, Mutex>();
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

        // Sorry this looks a bit complex, but I wanted to only fetch the images once per app session and reuse them on views without
        // having to do anything fancy on the view side (Android doesn't like reusing the same stream for multiple image sources)
        // while keeping the service implementation completely platform agnostic.
        public async Task<Stream> GetMoviePosterAsync(string posterPath, string imageSize)
        {
            string src = string.Format(ImageUrl, posterPath, imageSize ?? "w92");

            Task<Stream> task = null;
            lock (_posterSyncRoot)
            {
                bool kickOffNew = true;
                // Check to see if we have already tried to get the image before and it succeeded,
                // otherwise kick off a new task to fetch the image.
                if (_posterCache.TryGetValue(src, out task))
                {
                    kickOffNew = task.IsFaulted;
                }

                if (kickOffNew)
                {
                    _posterCache[src] = task = Task.Run<Stream>(async () =>
                    {
                        // Let the http client throw exceptions so we can check above if
                        // we should attempt to get the image again. The client will throw
                        // if it 404's or something platform specific happens like changing
                        // from wifi to celular wich would interrupt the communication.
                        MemoryStream newStream = null;
                        using (var s = await _client.GetStreamAsync(src))
                        {
                            newStream = new MemoryStream();
                            s.CopyTo(newStream);
                        }
                        return newStream;
                    });
                }
            }

            Stream stream = null;
            try
            {
                stream = await task;
            }
            catch (Exception e)
            {
                // TODO: Log exception
            }

            // Copy the result to a new stream so android doesn't loose it's shit
            MemoryStream result = null;
            if (stream != null)
            {
                lock (stream)
                {

                    try
                    {
                        result = new MemoryStream();

                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(result);
                        result.Seek(0, SeekOrigin.Begin);
                    }
                    catch (Exception e)
                    {
                        // TODO: Log exception
                    }
                }
            }

            return result;
        }
    }
}