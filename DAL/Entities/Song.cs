using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class Song
    {
        [BsonId]
        [BsonElement("_id")]
        public string VideoId { get; set; }

        [BsonElement("PlaylistId")]
        public string PlaylistId { get; set; }
    }
}