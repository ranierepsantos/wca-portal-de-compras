using System.ComponentModel.DataAnnotations.Schema;

namespace wca.share.domain.Entities
{
    public sealed class MotivoDemissao
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("motivo", TypeName = "varchar(300)")]
        public string Motivo { get; set; } = string.Empty;
    }
}
