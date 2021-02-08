using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class Song
    {
        public string VideoId { get; set; }

        public string PlaylistId { get; set; }
    }
}