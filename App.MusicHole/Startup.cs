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
using Microsoft.Extensions.Logging;
using Serilog;

namespace App.MusicHole
{
    public class Startup : IStartup
    {
        private readonly ILogger<Startup> logger;
        private readonly IMusicPostService musicPostService;
        private readonly ISongService songService;
        private readonly ISettingsService settingsService;
        private readonly IYoutubePlaylistService youtubePlaylistService;

        private const string LastRunSettingName = "lastRun";

        public Startup(
            ILogger<Startup> logger,
            IMusicPostService musicPostService,
            ISongService songService,
            ISettingsService settingsService,
            IYoutubePlaylistService youtubePlaylistService)
        {
            this.logger = logger;
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
            
            logger.LogInformation($"Found {uniqueVideoIds.Count} unique music videos from last run at {lastRun.ToString(CultureInfo.InvariantCulture)}");
            
            foreach (var videoId in uniqueVideoIds)
            {
                logger.LogDebug($"Processing {videoId}");
                await songService.UpsertSong(new Song()
                {
                    VideoId = videoId
                });
            }

            logger.LogInformation("Searching for songs not assigned to playlist");

            var orphanSongs = (await songService.GetOrphanedSongs()).ToList();
            int.TryParse(ConfigurationManager.AppSettings["maxBatchSize"], out var maxBatchSize);
            
            logger.LogInformation($"Found {orphanSongs.Count} songs without playlist");
            logger.LogDebug($"Processing batch of max {maxBatchSize} songs");
            
            if (orphanSongs.Count > 0)
            {
                var playlistId = ConfigurationManager.AppSettings["youtubePlaylistId"];
                var songs = orphanSongs.Select(s => s).Take(maxBatchSize).ToList();
                foreach (var song in songs)
                {
                    logger.LogInformation($"Adding {song.VideoId} to {playlistId}....");
                    try
                    {
                        var result = await youtubePlaylistService.InsertVideoToPlaylist(song.VideoId, playlistId);
                        if (result)
                        {
                            logger.LogDebug("Added, updating db");
                            await songService.UpsertSong(new Song()
                            {
                                VideoId = song.VideoId,
                                PlaylistId = playlistId
                            });
                        }
                        else
                        {
                            logger.LogError("Unknown error");
                        }
                    }
                    catch (GoogleApiException exception)
                    {
                        logger.LogError($"Error while adding {song.VideoId}, reason {exception.Error.Message}");
                        if (exception.Error.Code == 404)
                        {
                            logger.LogDebug("Deleting missing song");
                            var deleteResult = await songService.Delete(song);
                            logger.LogTrace($"{(deleteResult ? "Deleted" : "Error while deleting song")}");
                        }
                    }
                    
                    
                }
            }

            logger.LogDebug("Updating last run date ");
            var lastRunSaved = await UpdateLastRun();
            logger.LogInformation($"Finished at {lastRunSaved}");                
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