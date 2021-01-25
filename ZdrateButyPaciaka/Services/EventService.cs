using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZdarteButyPaciaka.Entities;
using ZdrateButyPaciaka.Extensions;
using ZdrateButyPaciaka.Helpers;
using ZdrateButyPaciaka.Repositories;

namespace ZdrateButyPaciaka.Services
{
    public class EventService : IEventsService
    {
        private readonly IObjectsRepository _objectsRepository;

        public EventService(IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;
        }

        public async Task<IEnumerable<Event>> GetEventsFromTopic(string topicId)
        {
            var posts = await _objectsRepository.GetPostsByTopicId(topicId);
            var postsArray = posts.ToArray();
            
            var users = await _objectsRepository.GetUsersByUids(GetUidsFromPosts(postsArray));
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