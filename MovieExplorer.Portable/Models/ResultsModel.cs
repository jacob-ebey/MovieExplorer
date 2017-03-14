using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace MovieExplorer.Models
{
    public class ResultsModel<T>
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("results")]
        public IEnumerable<T> Results { get; set; }

        internal bool Any()
        {
            throw new NotImplementedException();
        }
    }
}