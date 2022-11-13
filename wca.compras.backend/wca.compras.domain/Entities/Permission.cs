using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class Permission : IEntity
    {
        [BsonId, BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Name { get; set; }

        [BsonElement("internal_name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string InternalName { get; set; }

        [BsonElement("description"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Description { get; set; }

    }
}