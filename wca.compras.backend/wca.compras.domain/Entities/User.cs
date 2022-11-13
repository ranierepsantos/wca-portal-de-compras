using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class User : IEntity
    {
        [BsonId, BsonElement("id"),BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Name { get; set; }

        [BsonElement("email"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Email { get; set; }

        [BsonElement("active"), BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
        public string Active { get; set; }

        [BsonElement("password"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Password { get; set; }

        [BsonElement("profile_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ProfileId { get; set; }

        [BsonElement("created_at"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;


    }
}
