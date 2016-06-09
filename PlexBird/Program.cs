using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using HBConnector;
using HBConnector.Net.JsonObjects;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using PlexBird.JsonObjects;
using PlexBird.Properties;
using PlexBird.Util;
using PlexConnector;
using PlexConnector.Net.JsonObjects;
using Formatting = Newtonsoft.Json.Formatting;

namespace PlexBird
{

    internal class ThreadData
    {

        public Plex Plex { get; set; }
        public PlexLibrary PlexLibrary { get; set; }

    }

    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static HBUser User { get; set; }

        public static Configuration Configuration { get; private set; }
        public static Dictionary<string, DatabaseEntry> Database { get; private set; }

        private static void Main(string[] args)
        {
            // Configure the console
            Console.Title = "PlexBird";

            // Configure the logger
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(Resources.log4net);

            XmlConfigurator.Configure(xmlDocument.DocumentElement);

            Logger.Info("Launching PlexBird..");

            // Check config
            var configPath = Path.Combine(Environment.CurrentDirectory, "config.json");
            if (!File.Exists(configPath))
            {
                var defaultConfiguration = new Configuration
                {
                    Username = string.Empty,
                    AuthToken = string.Empty,
                    PlexUrl = "http://localhost:32400/",
                    AnimeLibrary = "Anime TV Series",
                    SyncTime = 60
                };

                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfiguration, Formatting.Indented));

                Logger.Error("Configuration file not found!");
                Logger.Warn("A configuration file has been generated, please edit config.json!");

                Console.ReadKey();
                return;
            }

            Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configPath));

            if (Configuration.Username.Equals(string.Empty) || Configuration.AuthToken.Equals(string.Empty))
            {
                Logger.Info("No AuthToken was found, you have to sign in to connect with HummingBird.");

                while (Configuration.AuthToken.Equals(string.Empty))
                {
                    Console.Write("HummingBird username: ");
                    var username = Console.ReadLine();

                    Console.Write("HummingBird password: ");
                    var password = ConsoleUtil.ReadPassword();

                    var authToken = HummingBird.Authenticate(username, password);
                    if (authToken != null)
                    {
                        Configuration.Username = username;
                        Configuration.AuthToken = authToken;
                        SaveConfiguration();
                    }
                    else
                    {
                        Logger.Error("Wrong username/password combination, try again.");
                    }
                }
            }

            HummingBird.SetAuthenticationToken(Configuration.AuthToken);
            User = HummingBird.GetUser(Configuration.Username);

            Logger.Info($"Connected to HummingBird as the user {User.Name}, you spent {User.LifeSpentOnAnime} minutes watching anime.");

            // Check database
            var databasePath = Path.Combine(Environment.CurrentDirectory, "database.json");
            if (!File.Exists(databasePath))
            {
                Database = new Dictionary<string, DatabaseEntry>();
                SaveDatabase();
            }
            else
            {
                Database = JsonConvert.DeserializeObject<Dictionary<string, DatabaseEntry>>(File.ReadAllText(databasePath));
            }
            
            // Setup plex
            var plex = new Plex(Configuration.PlexUrl);
            PlexLibrary plexLibrary = null;

            // Search plexLibrary
            foreach (var plexLib in plex.GetLibraries())
            {
                if (!plexLib.Title.Equals(Configuration.AnimeLibrary)) continue;

                if(plexLibrary != null)
                    throw new Exception($"The plex installation '{plex.Information.FriendlyName}' seems to have more than one '{plexLib.Title}' library.");

                plexLibrary = plexLib;
            }

            if(plexLibrary == null)
                throw new Exception($"The plex installation '{plex.Information.FriendlyName}' seems to not have '{Configuration.AnimeLibrary}' library.");

            // Connected
            new Thread(SynchronizeLibrary).Start(new ThreadData
            {
                Plex = plex,
                PlexLibrary = plexLibrary
            });

            // End
            SaveDatabase();
        }

        private static void SynchronizeLibrary(object objThreadData)
        {
            var plex = ((ThreadData) objThreadData).Plex;
            var plexLibrary = ((ThreadData) objThreadData).PlexLibrary;

            Logger.Info($"Connected to {plex.Information.FriendlyName} running {plex.Information.Platform} {plex.Information.PlatformVersion}.");

            while (true)
            {
                // Iterate through all (anime) shows
                foreach (var plexShow in plex.GetAllShows(plexLibrary.Key))
                {
                    var plexShowData = plex.GetShowData(plexShow.RatingKey);
                    if (plexShowData.Seasons.Count != 1)
                        throw new Exception($"The show '{plexLibrary.Title}' seems to have more than or less than one season.");

                    var plexSeason = plexShowData.Seasons[0];
                    var lastViewedEpisode = 0;

                    // Iterate through all episodes of a show
                    foreach (var plexEpisode in plex.GetAllEpisodes(plexSeason.RatingKey))
                    {
                        if (plexEpisode.ViewCount > 0 && plexEpisode.Index > lastViewedEpisode)
                            lastViewedEpisode = plexEpisode.Index;
                    }

                    // Find the show on HummingBird
                    if (!Database.ContainsKey(plexShowData.Key))
                    {
                        var animeShows = HummingBird.SearchAnime(plexShow.Title);
                        HBAnime animeShow = null;

                        // If there is just one result, it MUST be the anime we're looking for.
                        if (animeShows.Count == 1)
                        {
                            animeShow = animeShows[0];
                        }

                        if (animeShows.Count > 1)
                        {
                            foreach (var hbAnime in animeShows)
                            {
                                if (hbAnime.Title.Equals(plexShow.Title)) // Check if title equals local title
                                {
                                    animeShow = hbAnime;
                                    break;
                                }

                                if (hbAnime.TitleAlternate != null && hbAnime.TitleAlternate.Equals(plexShow.Title)) // Check if alternate title equals local title
                                {
                                    animeShow = hbAnime;
                                    break;
                                }

                                // Maybe missing a random character? (e.g. https://hummingbird.me/anime/isshuukan-friends)

                                if (hbAnime.Title.Contains(plexShow.Title) &&
                                    hbAnime.Title.Replace(plexShow.Title, string.Empty).Length <= 1)
                                {
                                    animeShow = hbAnime;
                                    break;
                                }

                                if (hbAnime.TitleAlternate != null &&
                                    hbAnime.TitleAlternate.Contains(plexShow.Title) &&
                                    hbAnime.TitleAlternate.Replace(plexShow.Title, string.Empty).Length <= 1)
                                {
                                    animeShow = hbAnime;
                                    break;
                                }
                            }
                        }

                        // Add to the database
                        if (animeShow != null)
                        {
                            if (Database.ContainsKey(plexShowData.Key)) continue;

                            Database.Add(plexShowData.Key, new DatabaseEntry
                            {
                                EpisodesWatched = lastViewedEpisode,
                                LastUpdateEpisodesWatched = 0,
                                Anime = animeShow
                            });
                        }
                        else
                        {
                            Logger.Info($"Couldn't find the show '{StringUtil.RemoveNonASCII(plexShow.Title)}' on HummingBird.");
                        }
                    }

                    // Update!
                    var databaseEntry = Database[plexShowData.Key];
                    if (databaseEntry.EpisodesWatched != databaseEntry.Anime.EpisodeCount) // Not yet completed
                    {
                        if (lastViewedEpisode > databaseEntry.EpisodesWatched)
                            databaseEntry.EpisodesWatched = lastViewedEpisode;

                        if (databaseEntry.EpisodesWatched != databaseEntry.LastUpdateEpisodesWatched)
                        {
                            var episodesWatched = databaseEntry.EpisodesWatched;

                            // Apply manual fixes
                            if (databaseEntry.Anime.Title.Equals("Gakusen Toshi Asterisk 2nd Season"))
                            {
                                episodesWatched = episodesWatched - 12;
                            }

                            var libraryItem = HummingBird.UpdateLibrary(databaseEntry.Anime, episodesWatched);

                            if (libraryItem != null)
                            {
                                Database[plexShowData.Key].LastUpdateEpisodesWatched = databaseEntry.EpisodesWatched;
                                Logger.Info($"Synchronized '{StringUtil.RemoveNonASCII(plexShow.Title)}'.");
                            }
                            else
                                Logger.Info($"Something went wrong with '{StringUtil.RemoveNonASCII(plexShow.Title)}'.");
                        }
                    }

                    SaveDatabase();
                }

                // Sleepwell for 1 minute.
                Thread.Sleep(1000 * Configuration.SyncTime);
            }
        }

        private static void SaveConfiguration()
        {
            var configPath = Path.Combine(Environment.CurrentDirectory, "config.json");

            File.WriteAllText(configPath, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
        }

        private static void SaveDatabase()
        {
            var databasePath = Path.Combine(Environment.CurrentDirectory, "database.json");

            File.WriteAllText(databasePath, JsonConvert.SerializeObject(Database, Formatting.Indented));
        }
    }
}
