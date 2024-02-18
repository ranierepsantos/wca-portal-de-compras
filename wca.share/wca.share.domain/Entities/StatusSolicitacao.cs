using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wca.share.domain.Common.Enum;

namespace wca.share.domain.Entities
{
    [Table("StatusSolicitacao")]
    public sealed class StatusSolicitacao
    {
        [Column("id"), Key]
        public int Id { get; set; }

        [Column("status", TypeName = "varchar(150)")]
        public string Status { get; set; } = string.Empty;

        [Column("status_intermediario", TypeName = "varchar(150)")]
        public string StatusIntermediario { get; set; } = string.Empty;

        [Column("color", TypeName = "varchar(30)")]
        public string Color { get; set; } = string.Empty;

        [Column("notifica")]
        public EnumNotificaQuem Notifica { get; set; }

        [Column("autorizar")]
        public bool Autorizar { get; set; } = false;

        [Column("template_notificacao")]
        public string? TemplateNotificacao { get; set; } = string.Empty;
    }


}
