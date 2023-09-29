using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace wca.reembolso.domain.Entities
{
    [Table("FaturamentoHistorico")]
    public class FaturamentoHistorico
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("faturamento_id")]
        public int FaturamentoId { get; set; }

        [NotMapped]
        public Faturamento Faturamento { get; set; }

        [Required]
        [Column("data_hora", TypeName = "smalldatetime")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Required]
        [Column("evento", TypeName = "varchar(500)")]
        public string Evento { get; set; } = "";
    }
}
