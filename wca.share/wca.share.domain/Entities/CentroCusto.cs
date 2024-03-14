using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    [Table("CentrosDeCusto")]
    public sealed class CentroCusto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("codigo")]
        public int Codigo { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(150)")]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

    }
}
