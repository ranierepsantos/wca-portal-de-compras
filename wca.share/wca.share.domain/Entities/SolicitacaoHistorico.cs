using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace wca.share.domain.Entities
{
    [Table("SolicitacaoHistorico")]
    public class SolicitacaoHistorico
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("solicitacao_id")]
        public int SolicitacaoId { get; set; }

        [Required]
        [Column("data_hora", TypeName = "smalldatetime")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Required]
        [Column("evento", TypeName = "varchar(500)")]
        public string Evento { get; set; } = "";
    }
}
