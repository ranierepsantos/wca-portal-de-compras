using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wca.share.application.Contracts.Integration.GI.Models
{
    public sealed class VerificarConexao
    {
        public class Request
        {
            [JsonPropertyName("idClienteWeb")]
            public string IdClienteWeb { get; set; }

            [JsonPropertyName("chaveAcesso")]
            public string ChaveAcesso { get; set; }
        }
        public class Response
        {
            [JsonPropertyName("validTo")]
            public DateTime ValidTo { get; set; }

            [JsonPropertyName("value")]
            public string Token { get; set; }
        }
    }

    public sealed class LoginRequest
    {

        [JsonPropertyName("login")]
        public string Usuario { get; set; }

        [JsonPropertyName("senha")]
        public string Senha { get; set; }

        [JsonPropertyName("esqueciMinhaSenha")]
        public bool EsqueciMinhaSenha { get; set; } = false;
    }

    public sealed class ClienteResponse
    {
        [JsonPropertyName("codigoCliente")]
        public int CodigoCliente { get; set; }

        [JsonPropertyName("razaoSocial")]
        public string RazaoSocial { get; set; }

        [JsonPropertyName("endereco")]
        public string Endereco { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("uf")]
        public string UF { get; set; }

        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("cgc")]
        public string Cgc { get; set; }

        [JsonPropertyName("ie")]
        public string Ie { get; set; }

        [JsonPropertyName("clienteAtivo")]
        public bool ClienteAtivo { get; set; }
    }

    public sealed class CentroCustoResponse
    {
        [JsonPropertyName("codigoCliente")]
        public int CodigoCliente { get; set; }

        [JsonPropertyName("codigoCentroCusto")]
        public int CodigoCentroCusto { get; set; }

        [JsonPropertyName("nomeCentroCusto")]
        public string Nome { get; set; }

        [JsonPropertyName("centroCustoAtivo")]
        public bool centroCustoAtivo { get; set; }
    }

    public sealed class FuncionarioResponse {
        [JsonPropertyName("codigoFuncionario")]
        public int? CodigoFuncionario { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("codigoCliente")]
        public int CodigoCliente { get; set; }

        [JsonPropertyName("codigoCentroCusto")]
        public int CodigoCentroCusto { get; set; }

        [JsonPropertyName("dataAdmissao")]
        public DateTime? DataAdmissao { get; set; }

        [JsonPropertyName("dataDemissao")]
        public DateTime? DataDemissao { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("cepResid")]
        public string? CepResid { get; set; }

        [JsonPropertyName("bairroResid")]
        public string? BairroResid { get; set; }

        [JsonPropertyName("cidadeResid")]
        public string? CidadeResid { get; set; }

        [JsonPropertyName("ufResid")]
        public string? UfResid { get; set; }

        [JsonPropertyName("tipoEndereco")]
        public string? TipoEndereco { get; set; }

        [JsonPropertyName("enderecoResid")]
        public string? EnderecoResid { get; set; }

        [JsonPropertyName("nroEndereco")]
        public int? NroEndereco { get; set; }

        [JsonPropertyName("cplEndereco")]
        public string? CplEndereco { get; set; }

        [JsonPropertyName("smsdddCel")]
        public int? SmsdddCel { get; set; }

        [JsonPropertyName("smsNroCel")]
        public double? SmsNroCel { get; set; }

        [JsonPropertyName("pis")]
        public double? Pis { get; set; }

        [JsonPropertyName("eSocialMatricula")]
        public string eSocialMatricula { get; set; }

    }

    public class WhereCondition
    {
        [JsonPropertyName("where")]
        public List<Condition> Conditions { get; set; } = new List<Condition>();
    }
    public class Condition
    {
        [JsonPropertyName("campo")]
        public string Campo { get; set; }
        [JsonPropertyName("valor")]
        public string Valor { get; set; }  
    }
}
