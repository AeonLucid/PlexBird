using System.Collections.Generic;
using System.IO;
using PlexConnector.Net;
using PlexConnector.Net.JsonObjects;

namespace PlexConnector
{
    public class Plex
    {
        private readonly Web _web;

        public Plex(string plexUrl)
        {
            _web = new Web(plexUrl);

            Information = _web.RequestData<PlexInformation>();

            //File.WriteAllText("test.txt", Web.RequestRawData($"{_plexUrl}library/sections/2/recentlyViewed"));
            //File.WriteAllText("test_2.txt", Web.RequestRawData($"{_plexUrl}library/sections"));
        }

        public PlexInformation Information { get; private set; }

        public List<PlexLibrary> GetLibraries()
        {
            return _web.RequestData<PlexSections>("library/sections").Libraries;
        }

        public List<PlexShow> GetAllShows(string key)
        {
            return _web.RequestData<PlexShows>($"library/sections/{key}/all").Shows;
        }

        public List<PlexSeason> GetAllSeasons(string key)
        {
            return _web.RequestData<PlexSeasons>($"library/metadata/{key}/children").Seasons;
        }

        public List<PlexEpisode> GetAllEpisodes(string key)
        {
            return _web.RequestData<PlexEpisodes>($"library/metadata/{key}/children").Episodes;
        }

        public void GetRecentlyWatched()
        {

        }
    }
}
