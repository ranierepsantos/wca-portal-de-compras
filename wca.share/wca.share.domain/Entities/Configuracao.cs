using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using wca.share.domain.Common.Interfaces;

namespace wca.share.domain.Entities
{
    [Table("Configuracoes")]
    public sealed class Configuracao: IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Column("chave", TypeName = "varchar(150)")]
        public string Chave { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        [Column("descricao", TypeName = "varchar(500)")]
        public string Descricao { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        [Column("tipo_campo")]
        public TipoCampoConfiguracao TipoCampo { get; set; }

        [Required, MaxLength(500)]
        [Column("valor", TypeName = "varchar(1000)")]
        public string Valor { get; set; } = string.Empty;

        [Required, MaxLength(8000)]
        [Column("combo_valores", TypeName = "varchar(8000)")]
        public string ComboValores { get; set; } = string.Empty;
    }

    public enum TipoCampoConfiguracao
    {
        Bool = 1,
        Text = 2,
        Number = 3,
        Decimal = 4,
        Combo = 5
    }
}
