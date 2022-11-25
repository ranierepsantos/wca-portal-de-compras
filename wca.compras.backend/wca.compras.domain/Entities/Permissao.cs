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
    [Table("Permissao")]
    public class Permissao: IEntity
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Required, MaxLength(50)]
        [Column("nome", TypeName = "varchar(50)")]
        public string Nome { get; set; }
        
        
        [Required, MaxLength(50)]
        [Column("regra", TypeName = "varchar(50)")]
        public string Regra { get; set; }
        
        [MaxLength(250)]
        [Column("descricao", TypeName = "varchar(250)")]
        public string Descricao { get; set; } = string.Empty;

        [JsonIgnore]
        public IList<Perfil> Perfil { get; set; }

    }
}
