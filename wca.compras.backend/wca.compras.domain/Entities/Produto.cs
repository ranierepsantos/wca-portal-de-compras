using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Interfaces;
using System.Text.Json.Serialization;

namespace wca.compras.domain.Entities
{
    public class Produto : IEntity
    {
        [Column("id")]
        public int Id { get; set; }

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

        [Column("percentual_ipi", TypeName = "decimal(4,2)")]
        public decimal PercentualIPI { get; set; }

        [Column("fornecedor_id")]
        public int? FornecedorId { get; set; }

        [JsonIgnore]
        public Fornecedor? Fornecedor { get ; set ; }

        [JsonIgnore]
        public TipoFornecimento TipoFornecimento { get; set; }

        [Column("tipofornecimento_id")]
        public int TipoFornecimentoId { get; set; }

        [JsonIgnore]
        public IList<ProdutoIcmsEstado> ProdutoIcmsEstado { get; set; } = new List<ProdutoIcmsEstado>();
    }
}
