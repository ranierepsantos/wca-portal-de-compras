using System.ComponentModel.DataAnnotations.Schema;

namespace wca.share.domain.Entities
{
    public sealed class ItemMudanca
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

    }
}
