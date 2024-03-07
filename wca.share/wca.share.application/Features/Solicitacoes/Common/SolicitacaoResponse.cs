using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Common
{
    public sealed class SolicitacaoResponse
    {
        
        public int Id { get; set; }
        public int SolicitacaoTipoId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public int FuncionarioId { get;  set; }

        public string FuncionarioNome { get; init; }
        public DateTime DataSolicitacao { get;  set; }
        public string Descricao { get;  set; }
        public int StatusSolicitacaoId { get;  set; }
        public int? ResponsavelId { get;  set; }
        public int? CentroCustoId { get;  set; }
        public string? CentroCustoNome { get; set; }

        public SolicitacaoComunicado? Comunicado { get;  set; }
        public SolicitacaoDesligamento? Desligamento { get;  set; }
        public SolicitacaoMudancaBase? MudancaBase { get;  set; }
        public List<SolicitacaoArquivo>? Anexos { get;  set; }
        
        public List<SolicitacaoHistorico> Historico { get; set; }
    }

    public sealed class SolicitacaoToPaginateResponse {
        public int Id { get; set; }
        public int FilialId { get; set; }
        public string Tipo { get; set; }
        public int Status { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string ClienteNome { get; set; }
        public string FuncionarioNome { get; set; }
        public string? ResponsavelNome { get; set; }
        public string? CentroCustoNome { get; set; }

        public StatusSolicitacaoResponse StatusSolicitacao { get; set; }
        public List<SolicitacaoHistorico> Historico { get; set; }
    }

    public sealed class  StatusSolicitacaoResponse
    {
        public int Id { get; set; }

        public string Status { get; set; } 

        public string StatusIntermediario { get; set; }

        public string Color { get; set; }

    }

}