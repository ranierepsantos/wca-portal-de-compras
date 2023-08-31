using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{
    [Table("FaturamentoItems")]
    public sealed class FaturamentoItem
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("faturamento_id")]
        public int FaturamentoId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public Faturamento Faturamento { get; set; }

        [Column("solicitacao_id")]
        public int SolicitacaoId { get; set; }

        public Solicitacao Solicitacao { get; set;}
    }

}
