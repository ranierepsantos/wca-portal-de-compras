using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.domain.Entities
{
    [Table("StatusSolicitacao")]
    public sealed class StatusSolicitacao
    {
        [Column("id"), Key]
        public int Id { get; set; }

        [Column("texto", TypeName ="varchar(150)")]
        public string Status { get; set; }

        [Column("color", TypeName = "varchar(30)")]
        public string Color { get; set; }

        [Column("notifica")]
        public EnumNotificaQuem Notifica { get; set; }

        [Column("autorizar")]
        public bool Autorizar { get; set; } = false;

        [Column("template_notificacao")]
        public string? TemplateNotificacao { get; set; } = string.Empty;
    }


}
