using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class Profile : IEntity
    {
        [BsonId, BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Name { get; set; }

        [BsonElement("description"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Description { get; set; }

        [BsonElement("active"), BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
        public bool Active { get; set; }
    }

    public class ProfilePermissions : Profile
    {
        public List<Permission>? Permissions { get; set; }
    }
}
