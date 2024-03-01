using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{
    [Table("CentrosDeCustos")]
    public sealed class CentroCusto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(150)")]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        [NotMapped]
        public Cliente? Cliente { get; set; }

        [JsonIgnore]
        public List<UsuarioCentrodeCustos>? Usuarios { get; set; }

    }
}
