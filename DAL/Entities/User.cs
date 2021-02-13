using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("_key")]
        public string Key { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }
        public int Uid
        {
            get
            {
                return Convert.ToInt32(Key.Split(':')[1]);
            }
        }
    }
}