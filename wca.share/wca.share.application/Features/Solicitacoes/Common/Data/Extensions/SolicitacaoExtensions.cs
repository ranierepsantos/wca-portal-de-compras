using Microsoft.EntityFrameworkCore;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Common.Data.Extensions
{
    public static class SolicitacaoExtensions
    {
        
        public static IQueryable<Solicitacao> IncludeAll(this IQueryable<Solicitacao> query)
        {
            return query
                .IncludeCliente()
                .IncludeResponsavel()
                .IncludeAnexos()
                .IncludeComunicado()
                .IncludeFerias()
                .IncludeDesligamento()
                .IncludeMudancaBase()
                .IncludeVaga()
                .IncludeHistorico()
                .IncludeStatus()
                .IncludeSupervisor();
        }

        public static IQueryable<Solicitacao> IncludeTipo(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.SolicitacaoTipo);
        }
        public static IQueryable<Solicitacao> IncludeStatus(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.StatusSolicitacao);
        }
        public static IQueryable<Solicitacao> IncludeHistorico(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Historico);
        }
        public static IQueryable<Solicitacao> IncludeCliente(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Cliente);
        }
        public static IQueryable<Solicitacao> IncludeResponsavel(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Responsavel);
        }

        public static IQueryable<Solicitacao> IncludeAnexos(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Anexos);
        }

        public static IQueryable<Solicitacao> IncludeComunicado(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Comunicado)
                    .ThenInclude(x => x.CentroCusto)
                .Include(x => x.Comunicado)
                    .ThenInclude(x => x.Funcionario)
                .Include(x => x.Comunicado)
                    .ThenInclude(x => x.Assunto);
        }

        public static IQueryable<Solicitacao> IncludeFerias(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Ferias)
                    .ThenInclude(x => x.TipoFerias)
                .Include(x => x.Ferias)
                    .ThenInclude(x => x.CentroCusto)
                .Include(x => x.Ferias)
                    .ThenInclude(x => x.Funcionario);
        }

        public static IQueryable<Solicitacao> IncludeDesligamento(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Desligamento)
                    .ThenInclude(x => x.CentroCusto)
                .Include(x => x.Desligamento)
                    .ThenInclude(x => x.Funcionario);
        }

        public static IQueryable<Solicitacao> IncludeMudancaBase(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.MudancaBase)
                    .ThenInclude(x => x.ClienteDestino)
                .Include(x => x.MudancaBase)
                    .ThenInclude(x => x.ItensMudanca)
                .Include(x => x.MudancaBase)
                    .ThenInclude(x => x.CentroCusto)
                .Include(x => x.MudancaBase)
                    .ThenInclude(x => x.Funcionario);
        }

        public static IQueryable<Solicitacao> IncludeVaga(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Vaga).ThenInclude(q => q.DocumentoComplementares)
                .Include(x => x.Vaga).ThenInclude(q => q.Escala)
                .Include(x => x.Vaga).ThenInclude(q => q.Escolaridade)
                .Include(x => x.Vaga).ThenInclude(q => q.Funcao)
                .Include(x => x.Vaga).ThenInclude(q => q.Gestor)
                .Include(x => x.Vaga).ThenInclude(q => q.Horario)
                .Include(x => x.Vaga).ThenInclude(q => q.MotivoContratacao)
                .Include(x => x.Vaga).ThenInclude(q => q.Sexo)
                .Include(x => x.Vaga).ThenInclude(q => q.TipoContrato)
                .Include(x => x.Vaga).ThenInclude(q => q.TipoFaturamento);
        }

        public static IQueryable<Solicitacao> IncludeVagaToPaginate(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Vaga).ThenInclude(q => q.Funcao);
        }

        public static IQueryable<Solicitacao> IncludeSupervisor(this IQueryable<Solicitacao> query)
        {
            return query
                .Include(x => x.Supervisor);
        }
    }
}
