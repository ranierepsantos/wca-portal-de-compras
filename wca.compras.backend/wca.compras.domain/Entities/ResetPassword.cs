using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class ResetPassword: IEntity
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Required, MaxLength(100)]
        [Column("token", TypeName = "varchar(100)")]
        public string Token { get; set; }

        [Required]
        [Column("data_criacao", TypeName = "smalldatetime")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("data_expiracao", TypeName = "smalldatetime")]
        public DateTime DataExpiracao { get; set; } = DateTime.UtcNow.AddDays(1);

        [Column("data_revogacao", TypeName = "smalldatetime")]
        public DateTime? DataRevogacao { get; set; }

        public Usuario Usuarios { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [JsonIgnore, NotMapped]
        public bool Expirado => DateTimeOffset.UtcNow > DataExpiracao;

        [JsonIgnore, NotMapped]
        public bool Ativo => !Expirado && DataRevogacao == null;
    }
}
