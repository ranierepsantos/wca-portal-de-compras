using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace wca.compras.domain.Entities
{
    public class Requisicao
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("data_criacao", TypeName = "smalldatetime")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Required]
        [Column("valor_total", TypeName = "money")]
        public decimal ValorTotal { get; set; } = 0;

        [Column("taxa_gestao", TypeName = "money")]
        public decimal TaxaGestao { get; set; } = 0;

        [Column("status", TypeName = "tinyint")]
        public EnumStatusRequisicao Status { get; set; } = 0;

        [Column("destino", TypeName = "tinyint")]
        public EnumDestinoRequisicao Destino { get; set; } = 0;

        [Column("usuario_id")]
        public int? UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Column("filial_id")]
        public int? FilialId { get; set; }

        [JsonIgnore]
        public Filial? Filial { get; set; }

        [Column("cliente_id")]
        public int? ClienteId { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [Column("fornecedor_id")]
        public int? FornecedorId { get; set; }

        [JsonIgnore]
        public Fornecedor? Fornecedor { get; set; }

        public IList<RequisicaoItem> RequisicaoItens { get; set; } = new List<RequisicaoItem>();
        
        public IList<RequisicaoHistorico> RequisicaoHistorico { get; set; } = new List<RequisicaoHistorico>();
    }

    public enum EnumStatusRequisicao
    {
        TODOS = -1,
        AGUARDANDO,
        APROVADO,
        REJEITADO
    }

    public enum EnumDestinoRequisicao
    {
        OUTROS,
        DIRETORIA
    }
}
