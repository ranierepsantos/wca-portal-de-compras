using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wca.reembolso.domain.Common.Enum;

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

        [NotMapped]
        [JsonIgnore]
        public ContaCorrente? ContaCorrente { get; set; }
    }
}
