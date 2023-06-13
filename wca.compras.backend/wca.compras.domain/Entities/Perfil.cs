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
    [Table("Perfil")]
    public class Perfil: IEntity
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Required, MaxLength(50)]
        [Column("nome", TypeName = "varchar(50)")]
        public string Nome { get; set; }

        [MaxLength(250)]
        [Column("descricao", TypeName = "varchar(250)")]
        public string Descricao { get; set; } = string.Empty;

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        [Column("sistema_id")]
        public int? SistemaId { get; set; }

        public IList<Permissao> Permissao { get; set; } = new List<Permissao>();

        [JsonIgnore]
        public IList<UsuarioSistemaPerfil> UsuarioSistemaPerfil { get; set; } = new List<UsuarioSistemaPerfil>();

        [JsonIgnore]
        public Sistema Sistema { get; set; }

    }
}
