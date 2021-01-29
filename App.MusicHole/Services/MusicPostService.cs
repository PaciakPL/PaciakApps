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

        public MusicPostService(IPostsRepository postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<IEnumerable<string>> GetMusicUrlsFromTopic(string topicId)
        {
            var posts = await postsRepository.GetTopicPosts(topicId);
            var urls = posts
                .Select(p => p)
                .OrderBy(p => p.Timestamp)
                .SelectMany(p => ExtractUrlsFromPost(p.Content))
                .Where(p => p != null)
                .ToList();

            return urls;
        }

        private List<string> ExtractUrlsFromPost(string content)
        {
            var matches = youtubeRegex.Matches(content);

            if (matches.Any())
            {
                Console.WriteLine(matches);
                return matches.Select(p => p.Value).ToList();
            }

            return new List<string>();
        }
    }
}