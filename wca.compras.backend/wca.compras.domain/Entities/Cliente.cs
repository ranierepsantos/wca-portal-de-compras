using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class Cliente : IEntity
    {
        [BsonId, BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Name { get; set; }

        [BsonElement("email"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Email { get; set; }

        [BsonElement("active"), BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
        public string Active { get; set; }

        [BsonElement("branch_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string BranchId { get; set; }

        [BsonElement("created_at"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        [BsonElement("updated_at"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    }
}
