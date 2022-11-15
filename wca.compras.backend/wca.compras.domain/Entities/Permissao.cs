using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class Permissao : IEntity
    {
        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; }

        [BsonElement("regra")]
        public string Regra { get; set; }

        [BsonElement("descricao")]
        public string Descricao { get; set; }

    }
}