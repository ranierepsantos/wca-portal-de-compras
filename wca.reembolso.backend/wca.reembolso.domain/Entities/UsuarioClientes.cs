using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Entities
{
    [Table("UsuarioClientes")]
    public class UsuarioClientes
    {
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        
        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set; }
    }
}
