using System.ComponentModel.DataAnnotations.Schema;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.domain.Entities
{
    [Table("TiposDespesa")]
    public class TipoDespesa
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("nome", TypeName ="varchar(50)")]
        public string Nome { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        [Column("tipo", TypeName ="int")]
        public EnumTipoDespesaTipo Tipo { get; set; } = EnumTipoDespesaTipo.Consumo;

        [Column("valor", TypeName ="money")]
        public decimal Valor { get; set; } = decimal.Zero;
        
    }
}
