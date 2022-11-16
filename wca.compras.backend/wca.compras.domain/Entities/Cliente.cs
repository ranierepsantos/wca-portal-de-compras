using MongoDB.Bson.Serialization.Attributes;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Serializable]
    public class Cliente : IEntity
    {
        [BsonId, BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nome"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Nome { get; set; }

        [BsonElement("cnpj"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string CNPJ { get; set; }
        
        [BsonElement("inscricao_estadual")]
        public string InscricaoEstadual { get; set; }

        [BsonElement("ativo"), BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
        public string Ativo { get; set; }

        [BsonElement("filial_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Filial { get; set; }

        [BsonElement("endereco")]
        public ClienteEndereco Enderecao { get; set; }
    }

    [Serializable]
    public class ClienteEndereco
    {
        [BsonElement("logradouro")]
        public string Logradouro { get; set; }

        [BsonElement("numero")]
        public string Numero { get; set; }

        [BsonElement("bairro")]
        public string bairro { get; set; }

        [BsonElement("cidade")]
        public string cidade { get; set; }

        [BsonElement("uf")]
        public string UF { get; set; }
    }
}
