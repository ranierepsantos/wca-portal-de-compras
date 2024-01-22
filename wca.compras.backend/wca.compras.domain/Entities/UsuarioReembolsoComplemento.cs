using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Table("UsuarioReembolsoComplemento")]
    public class UsuarioReembolsoComplemento : IEntity
    {
     
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        
        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Column("gestor_id")]
        public int? GestorId { get; set; }
        
        [JsonIgnore]
        public Usuario? Gestor { get; set; }

        [Column("cargo", TypeName = "varchar(100)")]
        public string? Cargo { get; set; } = string.Empty;

        [Column("centrocusto_id")]
        public int? CentroCustoId { get; set; }

    }
}
