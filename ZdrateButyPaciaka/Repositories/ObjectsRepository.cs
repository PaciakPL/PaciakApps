using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ZdarteButyPaciaka.Entities;
using ZdarteButyPaciaka.Helpers;
using ZdrateButyPaciaka.DB;
using ZdrateButyPaciaka.Repositories;

namespace ZdarteButyPaciaka.Repositories
{
    public class ObjectsRepository : IObjectsRepository
    {
        private readonly IDbProvider _dbProvider;

        public ObjectsRepository(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task<IEnumerable<User>> GetUsersByUids(int[] uids)
        {
            var objectsCollection = GetObjectsCollection();
            using var usersCollection = await objectsCollection.FindAsync<User>(ObjectCollectionHelpers.FilterUsersByUids(uids));

            return usersCollection.ToList();
        }
        
        public async Task<IEnumerable<Post>> GetPostsByTopicId(string topicId)
        {
            var objectsCollection = GetObjectsCollection();
            using var postIds = await objectsCollection.FindAsync<PostBare>(ObjectCollectionHelpers.FilterPostsByTopicId(topicId));
            using var postsCollection = await objectsCollection.FindAsync<Post>(ObjectCollectionHelpers.FilterByPostIds(postIds.ToList().Select(x => x.Value).ToArray()));

            return postsCollection.ToList();
        }

        private IMongoCollection<BsonDocument> GetObjectsCollection()
        {
            return _dbProvider.GetDatabase().GetCollection<BsonDocument>("objects");
        }
    }
}