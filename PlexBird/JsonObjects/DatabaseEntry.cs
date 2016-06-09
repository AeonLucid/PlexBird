using HBConnector.Net.JsonObjects;
using Newtonsoft.Json;

namespace PlexBird.JsonObjects
{
    internal class DatabaseEntry
    {

        [JsonProperty("episodes_watched")]
        public int EpisodesWatched { get; set; }

        [JsonProperty("last_update_episodes_watched")]
        public int LastUpdateEpisodesWatched { get; set; }

        [JsonProperty("anime")]
        public HBAnime Anime { get; set; }

    }
}
