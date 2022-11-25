using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class Cliente: IEntity
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(50)")]
        public string Nome { get; set; }
    }
}
