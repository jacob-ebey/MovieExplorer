using Newtonsoft.Json;

namespace MovieExplorer.Models
{
    public class VideoResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("iso_639_1")]
        public string ISO6391 { get; set; }

        [JsonProperty("iso_3166_1")]
        public string ISO31661 { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}