using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieExplorer.Models
{
    public class MovieListResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("adult")]
        public bool AdultContent { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("genre_ids")]
        public IEnumerable<int> GenreIds { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("video")]
        public bool OnVideo { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        public double VoteAverageClamped
        {
            get
            {
                return 5.0 * VoteAverage / 10.0;
            }
        }
    }
}