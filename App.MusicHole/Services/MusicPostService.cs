using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL.Paciak;

namespace App.MusicHole.Services
{
    public class MusicPostService : IMusicPostService
    {
        private readonly IPostsRepository postsRepository;
        private readonly Regex youtubeRegex = new Regex(@"(http.+youtube[^\s\n\t$]+)", RegexOptions.Multiline);
        private readonly Regex videoIdRegex = new Regex(@"v=([^$&]+)", RegexOptions.Multiline);

        public MusicPostService(IPostsRepository postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<IEnumerable<string>> GetVideoIdsFromTopic(string topicId)
        {
            var posts = await postsRepository.GetTopicPosts(topicId);
            var urls = posts
                .Select(p => p)
                .OrderBy(p => p.Timestamp)
                .SelectMany(p => ExtractUrlsFromPost(p.Content))
                .Select(ExtractVideoId)
                .Where(p => p != null)
                .ToList();

            return urls;
        }

        private IEnumerable<string> ExtractUrlsFromPost(string content)
        {
            var matches = youtubeRegex.Matches(content);

            return matches.Any() ? matches.Select(p => p.Value).ToList() : new List<string>();
        }

        private string ExtractVideoId(string url)
        {
            var matches = videoIdRegex.Matches(url);
            return matches.Any() ? matches.FirstOrDefault()?.Groups[1].Value : null;
        }
    }
}