using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    [Table("UsuarioClientes")]
    public class UsuarioClientes
    {
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set; }
    }
}
