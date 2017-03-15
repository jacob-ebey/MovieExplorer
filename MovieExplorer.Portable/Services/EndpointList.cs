using System;
using System.Net;

namespace MovieExplorer.Services
{
    class EndpointList : IEndpointList
    {
        private const string _apiKey = "ab41356b33d100ec61e6c098ecc92140";

        private const string _nowPlayingUrl = "http://api.themoviedb.org/3/movie/now_playing?api_key={0}&sort_by=popularity.des";
        private const string _topRatedUrl = "http://api.themoviedb.org/3/movie/top_rated?api_key={0}&sort_by=popularity.des";
        private const string _popularUrl = "http://api.themoviedb.org/3/movie/popular?api_key={0}&sort_by=popularity.des";
        private const string _similarUrl = "http://api.themoviedb.org/3/movie/{1}/similar?api_key={0}";
        private const string _videosUrl = "http://api.themoviedb.org/3/movie/{1}/videos?api_key={0}";
        private const string _searchUrl = "https://api.themoviedb.org/3/search/movie?language=en-US&query={1}&include_adult=false&api_key={0}";

        public Uri NowPlayingUri
        {
            get
            {
                return new Uri(string.Format(_nowPlayingUrl, _apiKey));
            }
        }

        public Uri TopRatedUri
        {
            get
            {
                return new Uri(string.Format(_topRatedUrl, _apiKey));
            }
        }

        public Uri PopularUri
        {
            get
            {
                return new Uri(string.Format(_popularUrl, _apiKey));
            }
        }

        public Uri GetSimilarUri(string movieId)
        {
            return new Uri(string.Format(_similarUrl, _apiKey, movieId));
        }

        public Uri GetVideosUri(string movieId)
        {
            return new Uri(string.Format(_videosUrl, _apiKey, movieId));
        }

        public Uri GetSearchUri(string query)
        {
            return new Uri(string.Format(_searchUrl, _apiKey, WebUtility.UrlEncode(query)));
        }
    }
}