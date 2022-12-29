using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class RequisicaoAprovacao : IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("requisicao_id")]
        public int RequisicaoId { get; set; }
        
        [JsonIgnore]
        public Requisicao? Requisicao { get; set; }

        [Required, MaxLength(100)]
        [Column("token", TypeName = "varchar(100)")]
        public string TokenRequisicao { get; set; }

        [Required, MaxLength(100)]
        [Column("token_aprovador", TypeName = "varchar(100)")]
        public string TokenAprovador { get; set; }

        [Required]
        [Column("data_criacao", TypeName = "smalldatetime")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("data_expiracao", TypeName = "smalldatetime")]
        public DateTime DataExpiracao { get; set; } = DateTime.UtcNow.AddDays(1);

        [Column("data_revogacao", TypeName = "smalldatetime")]
        public DateTime? DataRevogacao { get; set; }

        [Column("nome_aprovador", TypeName = "varchar(150)")]
        public string NomeAprovador { get; set; }

        [Column("altera_status")]
        public bool AlteraStatus { get; set; }

        [JsonIgnore, NotMapped]
        public bool Expirado => DateTimeOffset.UtcNow > DataExpiracao;

        [JsonIgnore, NotMapped]
        public bool Ativo => !Expirado && DataRevogacao == null;
    }
}
