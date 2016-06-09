using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlexConnector.Net.JsonObjects
{
    public class PlexSections
    {

        [JsonProperty("_children")]
        public List<PlexLibrary> Libraries { get; set; }

    }

    public class PlexLibrary
    {

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

    }
}
