using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.compras.domain.Entities
{
    public class RequisicaoItem
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("requisicao_id")]
        public int RequisicaoId { get; set; }

        [Required, MaxLength(50)]
        [Column("codigo", TypeName = "varchar(50)")]
        public string Codigo { get; set; }

        [Required, MaxLength(200)]
        [Column("nome", TypeName = "varchar(200)")]
        public string Nome { get; set; }

        [Required, MaxLength(20)]
        [Column("unidade_medida", TypeName = "varchar(20)")]
        public string UnidadeMedida { get; set; }

        [Column("valor", TypeName = "money")]
        public decimal Valor { get; set; }

        [Column("taxa_gestao", TypeName = "money")]
        public decimal TaxaGestao { get; set; }

        [Column("quantidade", TypeName = "int")]
        public int Quantidade { get; set; }

        [Column("valor_total", TypeName = "money")]
        public decimal ValorTotal { get; set; }

        [Column("tipofornecimento_id")]
        public int TipoFornecimentoId { get; set; }

        [JsonIgnore]
        public TipoFornecimento TipoFornecimento { get; set; }

    }
}
