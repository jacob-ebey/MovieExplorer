using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieExplorer.Models
{
    public class VideoResults
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("results")]
        public IEnumerable<VideoResult> Results { get; set; }
    }
}