using System;
using System.Collections.Generic;
using HBConnector.Net;
using HBConnector.Net.JsonObjects;

namespace HBConnector
{
    public class HummingBird
    {

        private static readonly Web Web;
        private static string _authenticationToken;

        static HummingBird()
        {
            Web = new Web();
        }

        public static void SetAuthenticationToken(string authenticationToken)
        {
            _authenticationToken = authenticationToken;
        }

        public static string Authenticate(string username, string password)
        {
            return Web.PostData<string>("/users/authenticate", $"&username={username}&password={password}");
        }

        public static HBUser GetUser(string username)
        {
            return Web.RequestData<HBUser>($"/users/{username}");
        }

        public static List<HBAnime> SearchAnime(string title)
        {
            return Web.RequestData<List<HBAnime>>($"/search/anime?query={title}");
        }

        public static HBLibraryItem UpdateLibrary(HBAnime anime, int currentEpisodes)
        {
            var notes = $"Synchronized by PlexBird on {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToShortDateString()}.";
            var status = "plan-to-watch";

            if (currentEpisodes == anime.EpisodeCount)
            {
                status = "completed";
            }
            else if(currentEpisodes > 0)
            {
                status = "currently-watching";
            }

            return Web.PostData<HBLibraryItem>($"/libraries/{anime.Id}", $"&id={anime.Id}&auth_token={_authenticationToken}&status={status}&notes={notes}&episodes_watched={currentEpisodes}");
        }
    }
}
