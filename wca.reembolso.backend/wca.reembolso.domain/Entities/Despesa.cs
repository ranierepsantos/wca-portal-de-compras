using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{
    [Table("Despesas")]
    public sealed class Despesa
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("solicitacao_id")]
        public int SolicitacaoId { get; set; }

        [Column("data_evento", TypeName = "smalldatetime")]
        public DateTime? DataEvento { get; set; } = DateTime.Today;

        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set;}

        [Column("tipodespesa_id")]
        public int TipoDespesaId { get; set; }

        [JsonIgnore]
        public TipoDespesa? TipoDespesa { get; set;}

        [Column("numero_fiscal", TypeName="varchar(100)")]
        public string NumeroFiscal { get; set; } = string.Empty;
        
        [Column("valor", TypeName = "money")]
        public decimal Valor { get; set; } = decimal.Zero;

        [Column("image_path", TypeName = "varchar(200)")]
        public string ImagePath { get; set; } = string.Empty;

        [Column("razao_social", TypeName = "varchar(150)")]
        public string RazaoSocial { get; set; } = string.Empty;

        [Column("cnpj", TypeName = "varchar(20)")]
        public string CNPJ { get; set; } = string.Empty;

        [Column("inscricao_estadual", TypeName = "varchar(20)")]
        public string InscricaoEstadual { get; set; } = string.Empty;

        [Column("motivo", TypeName = "varchar(1000)")]
        public string Motivo { get; set; } = string.Empty;

        [Column("origem", TypeName = "varchar(1000)")]
        public string Origem { get; set; } = string.Empty;

        [Column("destino", TypeName = "varchar(1000)")]
        public string Destino { get; set; } = string.Empty;

        [Column("km_percorrido", TypeName = "decimal(10,2)")]
        public decimal KmPercorrido { get; set; } = decimal.Zero;

    }
}
