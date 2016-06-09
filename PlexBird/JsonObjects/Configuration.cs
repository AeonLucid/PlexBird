using Newtonsoft.Json;

namespace PlexBird.JsonObjects
{
    internal class Configuration
    {

        [JsonProperty("hummingbird_username")]
        public string Username { get; set; }

        [JsonProperty("hummingbird_auth_token")]
        public string AuthToken { get; set; }

        [JsonProperty("plex_url")]
        public string PlexUrl { get; set; }

        [JsonProperty("anime_library")]
        public string AnimeLibrary { get; set; }

        [JsonProperty("sync_time_in_seconds")]
        public int SyncTime { get; set; }

    }
}
