using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlexConnector.Net.JsonObjects
{
    public class PlexSeasons
    {

        [JsonProperty("_children")]
        public List<PlexSeason> Seasons { get; set; }

    }

    // type season
    public class PlexSeason
    {

        [JsonProperty("ratingKey")]
        public string RatingKey { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("addedAt")]
        public long AddedAt { get; set; }

        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

    }
}
