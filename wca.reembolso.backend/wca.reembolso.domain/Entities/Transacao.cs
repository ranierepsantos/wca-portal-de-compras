using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{
    [Table("Transacoes")]
    public sealed class Transacao
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("data_hora", TypeName = "smalldatetime")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Column("descricao", TypeName = "varchar(250)")]
        public string Descricao { get; set; } = string.Empty;

        [Column("operador", TypeName = "varchar(1)")]
        public string Operador { get; set; } = "+";

        [Column("valor", TypeName = "money")]
        public decimal Valor { get; set; } = decimal.Zero;

        [Column("saldo_anterior", TypeName = "money")]
        public decimal SaldoAnterior { get; set; } = decimal.Zero;

        [Column("saldo", TypeName = "money")]
        public decimal Saldo { get; set; } = decimal.Zero;



        [Column("ContaCorrenteUsuarioId")]
        public int ContaCorrenteUsuarioId { get; set; }
        
        [JsonIgnore]
        public ContaCorrente? ContaCorrente { get; set; }
    }
}
