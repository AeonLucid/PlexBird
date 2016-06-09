using System.Collections.Generic;
using Newtonsoft.Json;

namespace HBConnector.Net.JsonObjects
{
    public class HBAnime
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("mal_id")]
        public int? IdMal { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("alternate_title")]
        public string TitleAlternate { get; set; }

        [JsonProperty("episode_count")]
        public int? EpisodeCount { get; set; }

        [JsonProperty("episode_length")]
        public int? EpisodeLength { get; set; }

        [JsonProperty("cover_image")]
        public string CoverImage { get; set; }

        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }

        [JsonProperty("show_type")]
        public string ShowType { get; set; }

        [JsonProperty("started_airing")]
        public string StartedAiring { get; set; }

        [JsonProperty("finished_airing")]
        public string FinishedAiring { get; set; }

        [JsonProperty("community_ratring")]
        public float? CommunityRating { get; set; }

        [JsonProperty("age_rating")]
        public string AgeRating { get; set; }

        [JsonProperty("genres")]
        public List<HBAnimeGenre> Genres { get; set; }
    }

    public class HBAnimeGenre
    {
        
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
