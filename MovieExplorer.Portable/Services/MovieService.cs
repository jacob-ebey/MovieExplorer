using ModernHttpClient;
using MovieExplorer.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieExplorer.Services
{
    class MovieService : IMovieService
    {
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

        private async Task<ServiceResult<T>> DoRequestAsync<T>(Uri uri)
        {
            ServiceResult<T> result = new ServiceResult<T>();

            try
            {
                var json = await _client.GetStringAsync(uri);
                var dataModel =  JsonConvert.DeserializeObject<T>(json);
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
    }
}