using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.ZBP.Helpers;
using App.ZBP.Extensions;
using DAL.Entities;
using DAL.Paciak;

namespace App.ZBP.Services
{
    public class EventService : IEventsService
    {
        private readonly IPostsRepository postsRepository;
        private readonly IUserRepository userRepository;

        public EventService(IPostsRepository postsRepository, IUserRepository userRepository)
        {
            this.postsRepository = postsRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<Event>> GetEventsFromTopic(string topicId)
        {
            var posts = await postsRepository.GetTopicPosts(topicId);
            var postsArray = posts.ToArray();
            
            var users = await userRepository.GetUsersByUids(GetUidsFromPosts(postsArray));
            var events = postsArray.ToArray()
                .Select(e => TransformPostToEvent(e, users)).ToList();

            return events;
        }

        private int[] GetUidsFromPosts(IEnumerable<Post> posts)
        {
            return posts.Select(x => x.Uid).ToArray();
        }

        private Event TransformPostToEvent(Post post, IEnumerable<User> users)
        {
            var line = post.Content.TrimNewLines().GetFirstLine();
            var matches = PostEventHelpers.GetStepMatchesFromPostContent(line);
            var isPostOk = false;
            var lastValue = 0;
            var newValue = 0;
            var steps = 0;
            var diff = 0;

            if (matches.Any())
            {
                (lastValue, newValue) = PostEventHelpers.GetGlobalEventValues(line);
                isPostOk = lastValue != 0 && newValue != 0;

                foreach (var match in matches.ToArray())
                {
                    if (int.TryParse(match.Groups[3]?.Value.JustNumbers(), out var subSteps))
                    {
                        steps += subSteps;
                    }
                }

                diff = lastValue - steps - newValue;
                if (diff != 0)
                {
                    isPostOk = false;
                }
            }
            
            return new Event()
            {
                User = users.FirstOrDefault(u => u.Uid == post.Uid)?.Username,
                Date = post.Date,
                Steps = steps,
                LastValue = lastValue,
                NewValue = newValue,
                Diff = diff,
                Ok = isPostOk
            };
        }
    }
}