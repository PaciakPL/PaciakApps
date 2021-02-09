using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using DAL.Entities;
using DAL.Paciak;

namespace App.MusicHole.Services
{
    public class MusicPostService : IMusicPostService
    {
        private readonly IPostsRepository postsRepository;
        private readonly Regex youtubeRegex = new Regex(@"(https?.+youtu\.?be[^\s\n\t$]+)", RegexOptions.Multiline);

        public MusicPostService(IPostsRepository postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<IEnumerable<string>> GetVideoIdsFromTopic(string topicId)
        {
            var posts = await postsRepository.GetTopicPosts(topicId);

            return GetUrlsFromPosts(posts.ToList());
        }

        public async Task<IEnumerable<string>> GetVideoIdsFromTopicByDateOffset(string topicId, DateTime offset)
        {
            var posts = await postsRepository.GetTopicPostsWithDateOffset(topicId, offset);

            return GetUrlsFromPosts(posts.ToList());
        }
        
        private IEnumerable<string> GetUrlsFromPosts(IEnumerable<Post> list)
        {
            return list.Select(p => p)
                .OrderBy(p => p.Timestamp)
                .SelectMany(p => ExtractUrlsFromPost(p.Content))
                .Select(ExtractVideoId)
                .Where(p => p != null)
                .ToList();
        }

        private IEnumerable<string> ExtractUrlsFromPost(string content)
        {
            var matches = youtubeRegex.Matches(content);

            return matches.Any() ? matches.Select(p => p.Value).ToList() : new List<string>();
        }

        private string ExtractVideoId(string url)
        {
            var uri = new Uri(url);
            var queryDictionary = HttpUtility.ParseQueryString(uri.Query);
            return queryDictionary.Get("v") ?? uri.Segments.LastOrDefault(); // assume v=videoId
        }
    }
}