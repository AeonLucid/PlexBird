using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using PlexBird.JsonObjects;
using PlexBird.Properties;
using PlexConnector;
using Formatting = Newtonsoft.Json.Formatting;

namespace PlexBird
{
    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        public static Configuration Configuration { get; private set; }

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
                    PlexInstallations = new List<PlexConfiguration>()
                    {
                        new PlexConfiguration
                        {
                            PlexUrl = "http://localhost:32400/",
                            AnimeLibrary = "Anime TV Series"
                        }
                    }
                };

                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfiguration, Formatting.Indented));

                Logger.Error("Configuration file not found!");
                Logger.Warn("A configuration file has been generated, please edit config.json!");

                Console.ReadKey();
                return;
            }

            Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configPath));

            // Test
            foreach (var plexConfiguration in Configuration.PlexInstallations)
            {
                var plex = new Plex(plexConfiguration.PlexUrl);

                foreach (var plexLibrary in plex.GetLibraries())
                {
                    if (plexLibrary.Title.Equals(plexConfiguration.AnimeLibrary))
                    {
                        Logger.Info($"Connected to {plex.Information.FriendlyName} running {plex.Information.Platform} {plex.Information.PlatformVersion}.");

                        foreach (var plexShow in plex.GetAllShows(plexLibrary.Key))
                        {
                            Console.WriteLine(plexShow.Title);

                            foreach (var plexSeason in plex.GetAllSeasons(plexShow.RatingKey))
                            {
                                Console.WriteLine($" - {plexSeason.Title}");

                                foreach (var plexEpisode in plex.GetAllEpisodes(plexSeason.RatingKey))
                                {
                                    Console.WriteLine($" -- {plexEpisode.Index} ({plexEpisode.ViewCount}): {plexEpisode.Title}");
                                }
                            }
                        }
                    }
                }
            }

            // End

            Console.ReadKey();
        }
    }
}
