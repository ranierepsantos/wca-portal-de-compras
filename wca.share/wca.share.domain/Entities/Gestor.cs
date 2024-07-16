using System.ComponentModel.DataAnnotations.Schema;

namespace wca.share.domain.Entities
{
    public sealed class Gestor
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome", TypeName = "varchar(200)")]
        public string Nome { get; set; } = string.Empty;

        [Column("ativo")]
        public bool Ativo { get; set; } = true;
    }
}
