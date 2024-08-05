using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace wca.share.domain.Entities
{
    [Table("VagaHistorico")]
    public class VagaHistorico
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("vaga_id")]
        public int VagaId { get; set; }

        [Required]
        [Column("data_hora", TypeName = "smalldatetime")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Required]
        [Column("evento", TypeName = "varchar(500)")]
        public string Evento { get; set; } = "";
    }
}
