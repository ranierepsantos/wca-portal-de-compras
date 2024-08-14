using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        [Column("valor_frete", TypeName = "money")]
        public decimal ValorFrete { get; set; } = 0;

        [Column("status", TypeName = "tinyint")]
        public int Status { get; set; } = 0;

        [Column("destino", TypeName = "tinyint")]
        public EnumDestinoRequisicao Destino { get; set; } = 0;

        [Column("usuario_id")]
        public int? UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Column("filial_id")]
        public int FilialId { get; set; }

        [JsonIgnore]
        public Filial? Filial { get; set; }

        [Column("cliente_id")]
        public int? ClienteId { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [Column("fornecedor_id")]
        public int? FornecedorId { get; set; }

        [Column("data_entrega", TypeName = "smalldatetime")]
        public DateTime? DataEntrega { get; set; }

        [Column("nota_fiscal", TypeName = "varchar(50)")]
        public string? NotaFiscal { get; set; }

        [Column("requer_autorizacao_cliente")]
        public bool RequerAutorizacaoCliente { get; set; }

        [Column("requer_autorizacao_wca")]
        public bool RequerAutorizacaoWCA { get; set;  }

        [Column("valor_icms", TypeName = "money")]
        public decimal ValorIcms { get; set; }

        [MaxLength(1500)]
        [Column("periodo_entrega", TypeName = "varchar(1500)")]
        public string? PeriodoEntrega { get; set; }

        //DADOS DO ENDERECO DE ENTREGA
        [MaxLength(150)]
        [Column("endereco", TypeName = "varchar(150)")]
        public string? Endereco { get; set; }

        [MaxLength(9)]
        [Column("cep", TypeName = "varchar(9)")]
        public string? Cep { get; set; }

        [MaxLength(10)]
        [Column("numero", TypeName = "varchar(10)")]
        public string? Numero { get; set; }

        [MaxLength(100)]
        [Column("cidade", TypeName = "varchar(100)")]
        public string? Cidade { get; set; }

        [MaxLength(2)]
        [Column("uf", TypeName = "varchar(2)")]
        public string? UF { get; set; }



        [JsonIgnore]
        public Fornecedor? Fornecedor { get; set; }

        public IList<RequisicaoItem> RequisicaoItens { get; set; } = new List<RequisicaoItem>();
        
        public IList<RequisicaoHistorico> RequisicaoHistorico { get; set; } = new List<RequisicaoHistorico>();
    }

    public enum EnumStatusRequisicao
    {
        RETORNARPARAAPROVACAO =-3,
        ITENSALTERADOS = -2,
        TODOS = -1,
        AGUARDANDO,
        APROVADO,
        REJEITADO,
        FINALIZADO,
        CANCELADO
    }

    public enum EnumDestinoRequisicao
    {
        OUTROS,
        DIRETORIA
    }

    public enum EnumRequisicaoDestinoEmail
    {
        CLIENTE =1,
        FORNECEDOR =2
    }
}
