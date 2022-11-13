using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class ProfileHasPermission: IEntity
    {
        [BsonId, BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("profile_id"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ProfileId { get; set; }

        [BsonElement("permission_id"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PermissionId { get; set; }
        
    }
}