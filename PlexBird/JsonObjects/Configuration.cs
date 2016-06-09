using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlexBird.JsonObjects
{
    internal class Configuration
    {

        [JsonProperty("plex_installations")]
        public List<PlexConfiguration> PlexInstallations { get; set; }

    }

    internal class PlexConfiguration
    {

        [JsonProperty("plex_url")]
        public string PlexUrl { get; set; }

        [JsonProperty("anime_library")]
        public string AnimeLibrary { get; set; }

    }
}
