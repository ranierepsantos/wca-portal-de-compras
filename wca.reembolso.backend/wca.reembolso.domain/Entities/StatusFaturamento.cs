using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.domain.Entities
{
    public class StatusFaturamento
    {
        [Column("id"), Key]
        public int Id { get; set; }

        [Column("texto", TypeName = "varchar(150)")]
        public string Status { get; set; }

        [Column("color", TypeName = "varchar(30)")]
        public string Color { get; set; }

        [Column("notifica")]
        public EnumNotificaQuem Notifica { get; set; }

    }
}
