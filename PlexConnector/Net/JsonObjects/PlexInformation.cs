using Newtonsoft.Json;

namespace PlexConnector.Net.JsonObjects
{
    public class PlexInformation
    {

        [JsonProperty("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("myPlexUsername")]
        public string MyPlexUsername { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("platformVersion")]
        public string PlatformVersion { get; set; }

    }
}
