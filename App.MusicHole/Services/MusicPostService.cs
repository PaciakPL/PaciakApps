using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using DAL.Entities;
using DAL.Paciak;
using Microsoft.Extensions.Logging;

namespace App.MusicHole.Services
{
    public class MusicPostService : IMusicPostService
    {
        private readonly ILogger<MusicPostService> logger;
        private readonly IPostsRepository postsRepository;
        private readonly Regex youtubeRegex = new Regex(@"(https?.+youtu\.?be[^\s\n\t$]+)", RegexOptions.Multiline);

        public MusicPostService(ILogger<MusicPostService> logger, IPostsRepository postsRepository)
        {
            this.logger = logger;
            this.postsRepository = postsRepository;
        }

        public async Task<IEnumerable<string>> GetVideoIdsFromTopic(string topicId)
        {
            logger.LogTrace($"Fetching posts from topic {topicId}");
            var posts = await postsRepository.GetTopicPosts(topicId);
            var result = posts.ToList();
            
            logger.LogTrace($"Found {result.Count} posts");

            return GetUrlsFromPosts(result);
        }

        public async Task<IEnumerable<string>> GetVideoIdsFromTopicByDateOffset(string topicId, DateTime offset)
        {
            logger.LogTrace($"Fetching posts from topic {topicId} with offset {offset}");
            var posts = await postsRepository.GetTopicPostsWithDateOffset(topicId, offset);
            var result = posts.ToList();
            
            logger.LogTrace($"Found {result.Count} posts");

            return GetUrlsFromPosts(result);
        }
        
        private IEnumerable<string> GetUrlsFromPosts(IEnumerable<Post> list)
        {
            var result = list.Select(p => p)
                .OrderBy(p => p.Timestamp)
                .SelectMany(p => ExtractUrlsFromPost(p.Content))
                .Select(ExtractVideoId)
                .Where(p => p != null)
                .ToList();
            
            logger.LogTrace($"Extracted {result.Count} urls from posts");

            return result;
        }

        private IEnumerable<string> ExtractUrlsFromPost(string content)
        {
            var matches = youtubeRegex.Matches(content);
            var result = matches.Any() ? matches.Select(p => p.Value).ToList() : new List<string>();
            
            logger.LogTrace($"Found {result.Count} matches in data \"{content}\"");

            return result;
        }

        private string ExtractVideoId(string url)
        {
            var uri = new Uri(url);
            var queryDictionary = HttpUtility.ParseQueryString(uri.Query);
            var result = queryDictionary.Get("v") ?? uri.Segments.LastOrDefault(); // assume v=videoId
            
            logger.LogTrace($"Video id found {result} in {url}");
            
            return result;
        }
    }
}