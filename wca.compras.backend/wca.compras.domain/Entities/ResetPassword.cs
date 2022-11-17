using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class ResetPassword: IEntity
    {
        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("usuario_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UsuarioId { get; set; }

        [BsonElement("token")]
        public string Token { get; set; }

        [BsonElement("data_criacao")]
        public DateTimeOffset DataCriacao { get; set; } = DateTimeOffset.UtcNow;
        
        [BsonElement("data_expiracao")]
        public DateTimeOffset DataExpiracao { get; set; } = DateTimeOffset.UtcNow.AddDays(1);

        [BsonElement("data_revogacao")]
        public DateTimeOffset? DataRevogacao { get; set; }

        [BsonIgnore]
        public bool Expirado => DateTimeOffset.UtcNow > DataExpiracao;
        
        [BsonIgnore]
        public bool Ativo => !Expirado && DataRevogacao == null;

    }
}
