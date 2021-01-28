using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Paciak
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetTopicPosts(string topicId);
    }
}