using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlexConnector.Net.JsonObjects
{
    public class PlexShows
    {

        [JsonProperty("_children")]
        public List<PlexShow> Shows { get; set; }

    }

    // type show
    public class PlexShow
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
