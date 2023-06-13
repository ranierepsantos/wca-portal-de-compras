using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    [Table("Sistemas")]
    public sealed class Sistema: IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(150)")]
        public string Nome { get; set; } = string.Empty;
        
        [Required, MaxLength(250)]
        [Column("descricao", TypeName = "varchar(250)")]
        public string Descricao { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        [Column("icon", TypeName = "varchar(250)")]
        public string Icon { get; set; } = string.Empty;

        public IList<UsuarioSistemaPerfil> UsuarioSistemaPerfil { get; set; } = new List<UsuarioSistemaPerfil>();
    }
}
