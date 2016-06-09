using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace HBConnector.Net
{
    internal class Web
    {

        public string RequestRawData(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hummingbird.me/api/v1" + url);
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

        public string PostRawData(string url, string data)
        {
            var postData = Encoding.UTF8.GetBytes(data);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://hummingbird.me/api/v1{url}");
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "PlexBird";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = postData.Length;

            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(postData, 0, postData.Length);
                requestStream.Close();
            }

            try
            {
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
            catch (WebException webException)
            {
                Console.WriteLine("Exception.. " + webException.Response.Headers["Status"]);

                return null;
            }
        }

        public T PostData<T>(string url, string data)
        {
            var responseText = PostRawData(url, data);

            if (responseText != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(responseText);
                }
                catch (JsonSerializationException)
                {
                    return default(T);
                }
            }

            return default(T);
        }

    }
}
