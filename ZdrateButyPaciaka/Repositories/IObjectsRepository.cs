using System.Collections.Generic;
using System.Threading.Tasks;
using ZdarteButyPaciaka.Entities;

namespace ZdrateButyPaciaka.Repositories
{
    public interface IObjectsRepository
    {
        Task<IEnumerable<Post>> GetPostsByTopicId(string topicId);
        Task<IEnumerable<User>> GetUsersByUids(int[] uids);
    }
}