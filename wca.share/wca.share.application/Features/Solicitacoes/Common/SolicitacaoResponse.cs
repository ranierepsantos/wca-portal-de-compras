using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Common
{
    public sealed class SolicitacaoResponse
    {
        public SolicitacaoResponse(int id, int solicitacaoTipoId, int clienteId, int funcionarioId, DateTime dataSolicitacao, string descricao, int statusSolicitacaoId, int? responsavelId, int? centroCustoId, SolicitacaoComunicado? comunicado, SolicitacaoDesligamento? desligamento, SolicitacaoMudancaBase? mudancaBase, List<SolicitacaoArquivo>? anexos)
        {
            Id = id;
            SolicitacaoTipoId = solicitacaoTipoId;
            ClienteId = clienteId;
            FuncionarioId = funcionarioId;
            DataSolicitacao = dataSolicitacao;
            Descricao = descricao;
            StatusSolicitacaoId = statusSolicitacaoId;
            ResponsavelId = responsavelId;
            CentroCustoId = centroCustoId;
            Comunicado = comunicado;
            Desligamento = desligamento;
            MudancaBase = mudancaBase;
            Anexos = anexos;
        }

        public int Id { get; private set; }
        public int SolicitacaoTipoId { get; private set; }
        public int ClienteId { get; private set; }
        public int FuncionarioId { get; private set; }
        public DateTime DataSolicitacao { get; private set; }
        public string Descricao { get; private set; }
        public int StatusSolicitacaoId { get; private set; }
        public int? ResponsavelId { get; private set; }
        public int? CentroCustoId { get; private set; }
        public SolicitacaoComunicado? Comunicado { get; private set; }
        public SolicitacaoDesligamento? Desligamento { get; private set; }
        public SolicitacaoMudancaBase? MudancaBase { get; private set; }
        public List<SolicitacaoArquivo>? Anexos { get; private set; }
        
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