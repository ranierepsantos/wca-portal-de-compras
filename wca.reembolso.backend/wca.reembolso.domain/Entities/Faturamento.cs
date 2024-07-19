using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{
    [Table("Faturamento")]
    public sealed class Faturamento
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("data_criacao", TypeName ="smalldatetime")]
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

        [Column("numero_po", TypeName ="varchar(500)")]
        public string? NumeroPO { get; set; }

        [Column("documento_po", TypeName = "varchar(500)")]
        public string? DocumentoPO { get; set; }

        [Column("data_finalizacao", TypeName = "smalldatetime")]
        public DateTime? DataFinalizacao { get; set; } = null;

        [Column("notaFiscal", TypeName = "varchar(500)")]
        public string? NotaFiscal { get; set; } = null;

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }

        public CentroCusto? CentroCusto { get; set; }

        [Column("data_chatbot_message", TypeName = "smalldatetime")]
        public DateTime? DataChatBotMessage { get; set; }

        [Column("numero_ds", TypeName = "varchar(500)")]
        public string? NumeroDS { get; set; } = null;

        public IList<FaturamentoItem> FaturamentoItem { get; set; } = new List<FaturamentoItem>();

        public IList<FaturamentoHistorico> FaturamentoHistorico { get; set; } = new List<FaturamentoHistorico>();
    }
}
