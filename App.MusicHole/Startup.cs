using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using App.MusicHole.Services;
using Common.Services;
using DAL.Entities;
using DAL.Paciak;

namespace App.MusicHole
{
    public class Startup : IStartup
    {
        private readonly IMusicPostService musicPostService;
        private readonly ISongService songService;
        private readonly ISettingsService settingsService;
        
        private const string LastRunSettingName = "lastRun";

        public Startup(
            IMusicPostService musicPostService,
            ISongService songService,
            ISettingsService settingsService)
        {
            this.musicPostService = musicPostService;
            this.songService = songService;
            this.settingsService = settingsService;
        }

        public async Task Run()
        {
            var lastRun = await GetLastRun();
            var topicId = ConfigurationManager.AppSettings["musicTopicId"];
            var videoIdsFromTopic = await musicPostService.GetVideoIdsFromTopic(topicId);
            
            foreach (var videoId in videoIdsFromTopic)
            {
                Console.WriteLine($"Processing {videoId}");
                await songService.UpsertSong(new Song()
                {
                    VideoId = videoId
                });
            }

            await UpdateLastRun();
            Console.WriteLine("Songs added");
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
        
        private async Task UpdateLastRun()
        {
            await settingsService.Upsert(new Option()
            {
                Name = LastRunSettingName,
                Value = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            });
        }
    }
}