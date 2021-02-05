using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using App.MusicHole.Services;
using DAL.Entities;

namespace App.MusicHole
{
    public class Startup : IStartup
    {
        private readonly IMusicPostService musicPostService;
        private readonly ISongService songService;

        public Startup(IMusicPostService musicPostService, ISongService songService)
        {
            this.musicPostService = musicPostService;
            this.songService = songService;
        }

        public async Task Run()
        {
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

            Console.WriteLine("Songs added");
        }
    }
}