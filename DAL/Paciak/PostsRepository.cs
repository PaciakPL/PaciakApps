using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configuration;
using DAL.Entities;
using DAL.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Paciak
{
    public class PostsRepository : IPostsRepository
    {
        private readonly IMongoDatabase db;
        private const string DbName = "forume";
        private const string ObjectsCollectionName = "objects";

        public PostsRepository(IDbProvider dbProvider)
        {
            db = dbProvider.GetDatabase(DbName);
        }

        public async Task<IEnumerable<Post>> GetTopicPosts(string topicId)
        {
            var objectsCollection = GetObjectsCollection();

            var postIds = await GetPostsIdsFromTopic(topicId);

            var postsByIdsFilter = Builders<BsonDocument>.Filter.In("_key", postIds.Select(x => $"post:{x.Value}").ToArray());
            using var postsCollection = await objectsCollection.FindAsync<Post>(postsByIdsFilter);

            return postsCollection.ToList();
        }

        public async Task<IEnumerable<Post>> GetTopicPostsWithDateOffset(string topicId, DateTime offset)
        {
            var objectsCollection = GetObjectsCollection();

            var postIds = await GetPostsIdsFromTopic(topicId);
            
            var filters = Builders<BsonDocument>.Filter.And(new[]
            {
                Builders<BsonDocument>.Filter.In("_key", postIds.Select(x => $"post:{x.Value}").ToArray()),
                Builders<BsonDocument>.Filter.Gte("timestamp", offset.ToJsTimestamp())
            });
            using var postsCollection = await objectsCollection.FindAsync<Post>(filters);

            return postsCollection.ToList();
        }

        public async Task<IEnumerable<PostBare>> GetPostsIdsFromTopic(string topicId)
        {
            var objectsCollection = GetObjectsCollection();
            
            var postIdsFilter = Builders<BsonDocument>.Filter.Eq("_key", $"tid:{topicId}:posts");
            using var postIds = await objectsCollection.FindAsync<PostBare>(postIdsFilter);

            return postIds.ToList();
        }

        private IMongoCollection<BsonDocument> GetObjectsCollection()
        {
            return db.GetCollection<BsonDocument>(ObjectsCollectionName);
        }
    }
}