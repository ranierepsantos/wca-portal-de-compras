using Org.BouncyCastle.Bcpg;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.compras.domain.Entities
{
    [Table("Usuario_Sistema_Perfil")]
    public sealed class UsuarioSistemaPerfil
    {

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }


        [Column("sistema_id")]
        public int SistemaId { get; set; }
        public Sistema? Sistema { get; set; }


        [Column("perfil_id")]
        public int PerfilId { get; set; }
        public Perfil? Perfil { get; set; }

        
        
        
    }
}
