using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configuration;
using DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Paciak
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase db;

        private const string DbName = "forume";
        private const string ObjectsCollectionName = "objects";

        public UserRepository(IDbProvider dbProvider)
        {
            db = dbProvider.GetDatabase(DbName);
        }

        public async Task<IEnumerable<User>> GetUsersByUids(int[] uids)
        {
            var objectsCollection = db.GetCollection<BsonDocument>(ObjectsCollectionName);

            var usersByUidFilter = Builders<BsonDocument>.Filter.In("_key", uids.Select(x => $"user:{x}").ToArray());
            using var usersCollection = await objectsCollection.FindAsync<User>(usersByUidFilter);

            return usersCollection.ToList();
        }
    }
}