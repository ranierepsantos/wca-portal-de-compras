using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{

    public sealed class Usuario
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Column("nome", TypeName = "varchar(100)")]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        [Column("email", TypeName = "varchar(150)")]
        public string Email { get; set; } = string.Empty;

        [Column("ativo")]
        public bool Ativo { get; set; } = true;
        
        [Column("celular", TypeName = "varchar(30)")]
        public string? Celular { get; set; }

        [Column("cargo", TypeName = "varchar(100)")]
        public string? Cargo { get; set; }
        
        [Column("perfil_id")]
        public int? PerfilId { get; set; }

        [Column("gestor_id")]
        public int? GestorId { get; set; }

        public ContaCorrente? ContaCorrente { get; set;}
        public List<UsuarioClientes>? UsuarioClientes { get; set;}
        public List<UsuarioCentrodeCustos>? UsuarioCentrodeCustos { get; set; }
        public UsuarioConfiguracoes? UsuarioConfiguracoes { get; set; }
    }

    [Table("UsuarioConfiguracoes")]
    public sealed class UsuarioConfiguracoes
    {
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Column("notificar_por_email")]
        public bool NotificarPorEmail { get; set; } = false;

        [Column("notificar_por_chatbot")]
        public bool NotificarPorChatBot { get; set; } = false;
    }
}
