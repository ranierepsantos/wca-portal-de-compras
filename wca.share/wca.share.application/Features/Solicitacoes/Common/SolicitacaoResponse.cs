using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;
using wca.share.application.Common;
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

        public DateTime FuncionarioDataAdmissao { get; init; }
        public DateTime DataSolicitacao { get;  set; }
        public string Descricao { get;  set; }
        public int StatusSolicitacaoId { get;  set; }
        public int? ResponsavelId { get;  set; }
        public int? CentroCustoId { get;  set; }
        public string? CentroCustoNome { get; set; }
        public string? eSocialMatricula { get; set; }

        public SolicitacaoComunicado? Comunicado { get;  set; }
        public SolicitacaoDesligamento? Desligamento { get;  set; }
        public SolicitacaoFerias? Ferias { get;  set; }
        public SolicitacaoMudancaBaseResponse? MudancaBase { get;  set; }
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

    public sealed class SolicitacaoMudancaBaseResponse
    {
        public int SolicitacaoId { get; init; }
        
        public DateTime? DataAlteracao { get; init; }
        
        public string? Observacao { get; init; }

        public int? ClienteDestinoId { get; init; }

        public string? ClienteDestinoNome { get; init; }

        public List<ListItem>? ItensMudanca { get; init; } = new List<ListItem>();
    }


}