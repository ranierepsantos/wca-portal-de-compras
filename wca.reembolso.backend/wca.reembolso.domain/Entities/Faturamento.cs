using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{
    [Table("Faturamento")]
    public sealed class Faturamento
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("DataCriacao", TypeName ="smalldatetime")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set;}

        [Column("status")]
        public int Status { get; set; }

        [Column("valor", TypeName = "money")]
        public decimal Valor { get; set; }

        [Column("numero_po", TypeName ="varchar(20)")]
        public string? NumeroPO { get; set; }

        [Column("documento_po", TypeName = "varchar(200)")]
        public string? DocumentoPO { get; set; }

        public IList<FaturamentoItem> FaturamentoItem { get; set; } = new List<FaturamentoItem>();

    }
}
