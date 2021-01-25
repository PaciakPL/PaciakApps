using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ZdarteButyPaciaka.Helpers
{
    public static class ObjectCollectionHelpers
    {
        public static FilterDefinition<BsonDocument> FilterPostsByTopicId(string topicId)
        {
            return Builders<BsonDocument>.Filter.Eq("_key", $"tid:{topicId}:posts");
        }

        public static FilterDefinition<BsonDocument> FilterByPostIds(IEnumerable<string> postIds)
        {
            return Builders<BsonDocument>.Filter.In("_key", postIds.Select(x => $"post:{x}").ToArray());
        }

        public static FilterDefinition<BsonDocument> FilterUsersByUids(IEnumerable<int> uids)
        {
            return Builders<BsonDocument>.Filter.In("_key", uids.Select(x => $"user:{x}").ToArray());
        }
    }
}