using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Paciak
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetTopicPosts(string topicId);
        Task<IEnumerable<Post>> GetTopicPostsWithDateOffset(string topicId, DateTime offset);
        Task<IEnumerable<PostBare>> GetPostsIdsFromTopic(string topicId);
    }
}