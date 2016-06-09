using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlexConnector.Net.JsonObjects
{
    public class PlexEpisodes
    {

        [JsonProperty("_children")]
        public List<PlexEpisode> Episodes { get; set; }

    }

    // type episode
    public class PlexEpisode
    {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("viewCount")]
        public int ViewCount { get; set; }

        [JsonProperty("addedAt")]
        public long AddedAt { get; set; }

        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

    }
}
