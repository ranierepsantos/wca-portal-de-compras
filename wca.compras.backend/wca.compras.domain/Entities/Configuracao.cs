using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class Configuracao: IEntity
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
        public string combo_valores { get; set; } = string.Empty;

    }

    public enum TipoCampoConfiguracao
    {
        Text,
        Bool,
        Combo
    }
}
