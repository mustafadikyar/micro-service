using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Micro.Catalog.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Image { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime EditedTime { get; set; }
        public string UserId { get; set; }

        public Feature Feature { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonIgnore] //Bu property'i göz ardı et.
        public Category Category { get; set; }
    }
}
