using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class Filial: IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Column("nome",TypeName = "varchar(150)")]
        public string Nome { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;
    }
}
