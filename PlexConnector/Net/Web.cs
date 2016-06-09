using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace PlexConnector.Net
{
    internal class Web
    {
        private readonly string _plexUrl;

        public Web(string plexUrl)
        {
            _plexUrl = plexUrl;
        }

        public string RequestRawData(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_plexUrl + url);
            httpWebRequest.UserAgent = "PlexBird";
            httpWebRequest.Accept = "application/json";

            string responseText;

            using (var response = httpWebRequest.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new Exception("ResponseStream was null..");
                }
            }

            return responseText;
        }

        public T RequestData<T>(string url = "")
        {
            var responseText = RequestRawData(url);

            return JsonConvert.DeserializeObject<T>(responseText);
        }

    }
}
