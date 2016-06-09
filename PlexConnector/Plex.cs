using System.Collections.Generic;
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

        public PlexShowData GetShowData(string key)
        {
            return _web.RequestData<PlexShowData>($"library/metadata/{key}/children");
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
