using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class Usuario: IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Column("nome", TypeName = "varchar(100)")]
        public string Nome { get; set; }

        [Required, MaxLength(150)]
        [Column("email", TypeName = "varchar(150)")]
        public string Email { get; set; }

        [MaxLength(200)]
        [Column("password", TypeName = "varchar(200)")]
        public string? Password { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        [Column("celular", TypeName = "varchar(30)")]
        public string? Celular { get; set; }

        public IList<Filial> Filial { get; set; } = new List<Filial>();
        public IList<Cliente> Cliente { get; set; } = new List<Cliente>();
        public IList<TipoFornecimento> TipoFornecimento { get; set; }  = new List<TipoFornecimento>();

        public IList<UsuarioSistemaPerfil> UsuarioSistemaPerfil { get; set; } = new List<UsuarioSistemaPerfil>();

        public UsuarioReembolsoComplemento? UsuarioReembolsoComplemento { get;set; }
        
        public IList<UsuarioConfiguracoes> UsuarioConfiguracoes { get; set; } = new List<UsuarioConfiguracoes>();
    }

    [Table("UsuarioConfiguracoes")]
    public sealed class UsuarioConfiguracoes
    {
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Column("sistema_id")]
        public int SistemaId { get; set; }
        
        [JsonIgnore]
        public Sistema? Sistema { get; set; }

        [Column("notificar_por_email")]
        public bool NotificarPorEmail { get; set; } = false;

        [Column("notificar_por_chatbot")]
        public bool NotificarPorChatBot { get; set; } = false;
    }
}
