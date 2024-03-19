﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wca.share.application.Contracts.Integration.GI.Models
{
    public class VerificarConexao
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

    public class LoginRequest
    {

        [JsonPropertyName("login")]
        public string Usuario { get; set; }

        [JsonPropertyName("senha")]
        public string Senha { get; set; }

        [JsonPropertyName("esqueciMinhaSenha")]
        public bool EsqueciMinhaSenha { get; set; } = false;
    }

    public class ClienteResponse
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
}
