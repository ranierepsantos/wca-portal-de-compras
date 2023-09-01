using Microsoft.EntityFrameworkCore;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Contracts.Persistence
{
    public interface IRepositoryManager
    {

        IRepository<Cliente> ClienteRepository { get; }
        IRepository<ContaCorrente> ContaCorrenteRepository { get; }
        IRepository<Despesa> DespesaRepository { get; }
        IRepository<Faturamento> FaturamentoRepository { get; }
        IRepository<FaturamentoItem> FaturamentoItemRepository { get; }
        IRepository<Notificacao> NotificacaoRepository { get; }
        IRepository<Solicitacao> SolicitacaoRepository { get; }
        IRepository<SolicitacaoHistorico> SolicitacaoHistoricoRepository { get; }
        IRepository<StatusSolicitacao> StatusSolicitacaoRepository { get; }
        IRepository<TipoDespesa> TipoDespesaRepository { get; }
        IRepository<Transacao> TransacaoRepository { get; }
        IRepository<UsuarioClientes> UsuarioClientesRepository { get; }

        Task SaveAsync();
        Task<int> ExecuteCommandAsync(string command);
        DbSet<T> GetDbSet<T>() where T : class;
    }
}
