using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    public sealed class Funcionario
    {
        [Column("id")]
        public int Id { get;  set; }

        [Column("nome", TypeName = "varchar(200)")]
        public string Nome { get; set; } = string.Empty;

        [Column("cliente_id")]
        public int ClienteId {  get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; } = null;

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }

        [JsonIgnore]
        public CentroCusto? CentroCusto { get; set; } = null;
                
        [Column("codigo_funcionario")]
        public int? CodigoFuncionario { get; set; }

        [Column("data_admissao")]
        public DateTime? DataAdmissao { get; set; }

        [Column("data_demissao")]
        public DateTime? DataDemissao { get; set; }

        [Column("email", TypeName ="varchar(200)")]
        public string? Email { get; set; }

        [Column("cep", TypeName = "varchar(10)")]
        public string? Cep { get;set; }
        
        [Column("bairro", TypeName = "varchar(150)")]
        public string? Bairro { get;set; }

        [Column("cidade", TypeName = "varchar(150)")]
        public string? Cidade { get;set; }
        
        [Column("uf", TypeName = "varchar(2)")]
        public string? UF { get;set; }
        
        [Column("endereco", TypeName = "varchar(200)")]
        public string? Endereco { get;set; }

        [Column("complemento", TypeName = "varchar(200)")]
        public string? Complemento { get; set; }

        [Column("ddd_celular")]
        public int? DDDCelular { get;set; }
        
        [Column("numero_celular")]
        public int? NumeroCelular { get;set; }
    }
}
