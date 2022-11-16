using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class PerfilRelPermissoes: IEntity
    {
        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("perfil_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PerfilId { get; set; }

        [BsonElement("permissao_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PermissaoId { get; set; }
        
    }
}