using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class Perfil : IEntity
    {
        [BsonId, BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("nome")]
        public string Nome { get; set; }

        [BsonElement("descricao")]
        public string Descricao { get; set; }

        [BsonElement("ativo")]
        public bool Ativo { get; set; }
    }

    public class PerfilPermissoes : Perfil
    {
        public List<Permissao>? Permissoes { get; set; }
    }
}
