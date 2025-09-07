using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace demo_api.Models
{
    public class FormSubmission
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("message")]
        public string Message { get; set; } = null!;

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }

        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }
    }
}
