using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using App.MusicHole.Services;
using Common.Services;
using DAL.Entities;
using DAL.Paciak;
using Google;

namespace App.MusicHole
{
    public class Startup : IStartup
    {
        private readonly IMusicPostService musicPostService;
        private readonly ISongService songService;
        private readonly ISettingsService settingsService;
        private readonly IYoutubePlaylistService youtubePlaylistService;

        private const string LastRunSettingName = "lastRun";

        public Startup(
            IMusicPostService musicPostService,
            ISongService songService,
            ISettingsService settingsService,
            IYoutubePlaylistService youtubePlaylistService)
        {
            this.musicPostService = musicPostService;
            this.songService = songService;
            this.settingsService = settingsService;
            this.youtubePlaylistService = youtubePlaylistService;
        }

        public async Task Run()
        {
            var lastRun = await GetLastRun();
            var topicId = ConfigurationManager.AppSettings["musicTopicId"];
            var videoIdsFromTopic = await musicPostService.GetVideoIdsFromTopicByDateOffset(topicId, lastRun);
            var uniqueVideoIds = videoIdsFromTopic.ToList().Select(v => v).Distinct().ToList();
            
            Console.WriteLine($"Found {uniqueVideoIds.Count} unique music videos from last run at {lastRun.ToString(CultureInfo.InvariantCulture)}");
            
            foreach (var videoId in uniqueVideoIds)
            {
                Console.WriteLine($"Processing {videoId}");
                await songService.UpsertSong(new Song()
                {
                    VideoId = videoId
                });
            }

            Console.WriteLine("Songs added, searching for songs not assigned to playlist");

            var orphanSongs = (await songService.GetOrphanedSongs()).ToList();
            int.TryParse(ConfigurationManager.AppSettings["maxBatchSize"], out var maxBatchSize);
            
            Console.WriteLine($"Songs without playlist {orphanSongs.Count}\nProcessing batch of max {maxBatchSize} songs");

            
            if (orphanSongs.Count > 0)
            {
                var playlistId = ConfigurationManager.AppSettings["youtubePlaylistId"];
                var songs = orphanSongs.Select(s => s).Take(maxBatchSize).ToList();
                foreach (var song in songs)
                {
                    Console.Write($"Adding ${song.VideoId} to {playlistId}....");
                    var result = false;
                    try
                    {
                        result = await youtubePlaylistService.InsertVideoToPlaylist(song.VideoId, playlistId);
                        if (result)
                        {
                            Console.Write("OK, updating db\n");
                            await songService.UpsertSong(new Song()
                            {
                                VideoId = song.VideoId,
                                PlaylistId = playlistId
                            });
                        }
                        else
                        {
                            Console.Write("ERR Unknown\n");
                        }
                    }
                    catch (GoogleApiException exception)
                    {
                        Console.WriteLine($"ERR {exception.Error.Message}");
                        if (exception.Error.Code == 404)
                        {
                            Console.Write("Deleting missing song...");
                            var deleteResult = await songService.Delete(song);
                            Console.WriteLine($"{(deleteResult ? "OK" : "ERR")}\n");
                        }
                    }
                    
                    
                }
            }

            Console.Write("Updating last run date ");
            var lastRunSaved = await UpdateLastRun();
            Console.WriteLine($"{lastRunSaved}\nFinish");                
        }

        private async Task<DateTime> GetLastRun()
        {
            var initial = new DateTime(2000, 1, 1);
            var lastRunValue = await settingsService.GetOrDefault(LastRunSettingName, new Option()
            {
                Name = LastRunSettingName,
                Value = initial.ToString(CultureInfo.InvariantCulture)
            });

            return DateTime.TryParse(lastRunValue.Value, out var lastRunDate) ? lastRunDate : initial;
        }
        
        private async Task<string> UpdateLastRun()
        {
            var date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            await settingsService.Upsert(new Option()
            {
                Name = LastRunSettingName,
                Value = date
            });

            return date;
        }
    }
}