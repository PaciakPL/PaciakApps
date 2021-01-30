using System;
using System.Configuration;
using System.Threading.Tasks;
using App.MusicHole.Services;

namespace App.MusicHole
{
    public class Startup : IStartup
    {
        private readonly IMusicPostService musicPostService;

        public Startup(IMusicPostService musicPostService)
        {
            this.musicPostService = musicPostService;
        }

        public async Task Run()
        {
            var topicId = ConfigurationManager.AppSettings["musicTopicId"];
            var urls = await musicPostService.GetMusicUrlsFromTopic(topicId);
            Console.WriteLine(urls);
        }
    }
}