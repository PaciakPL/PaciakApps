using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZdarteButyPaciaka.Entities
{
    [BsonIgnoreExtraElements]
    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }
        
        [BsonElement("timestamp")]
        public double Timestamp { get; set; }

        public DateTime Date
        {
            get
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                return epoch.AddMilliseconds(Timestamp);
            }
        }

        [BsonElement("uid")]
        public int Uid { get; set; }
    }
}