using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class Usuario : IEntity
    {
        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nome"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Nome { get; set; }

        [BsonElement("email"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Email { get; set; }

        [BsonElement("ativo"), BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
        public string Ativo { get; set; }

        [BsonElement("password"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Password { get; set; }

        [BsonElement("perfil_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PerfilId { get; set; }

        [BsonElement("cliente_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ClienteId { get; set; }

        [BsonElement("filial_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string FilialId { get; set; }
    }
}
