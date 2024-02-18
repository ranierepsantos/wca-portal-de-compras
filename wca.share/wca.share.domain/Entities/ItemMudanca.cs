using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    public sealed class ItemMudanca
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }
        
    }
}
