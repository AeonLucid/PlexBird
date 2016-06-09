using Newtonsoft.Json;

namespace HBConnector.Net.JsonObjects
{
    public class HBLibraryItem
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("episodes_watched")]
        public int EpisodesWatched { get; set; }

        [JsonProperty("last_watched")]
        public string LastWatched { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("rewatched_times")]
        public int RewatchedTimes { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("notes_present")]
        public bool NotesPresent { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("private")]
        public bool Private { get; set; }

        [JsonProperty("rewatching")]
        public bool Rewatching { get; set; }

        [JsonProperty("anime")]
        public HBAnime Anime { get; set; }

    }
}
