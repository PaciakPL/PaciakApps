using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class Option
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}