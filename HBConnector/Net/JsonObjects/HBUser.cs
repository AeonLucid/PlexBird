using Newtonsoft.Json;

namespace HBConnector.Net.JsonObjects
{
    public class HBUser
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("life_spent_on_anime")]
        public int LifeSpentOnAnime { get; set; }

        [JsonProperty("last_library_update")]
        public string LastLibraryUpdate { get; set; }

    }
}
