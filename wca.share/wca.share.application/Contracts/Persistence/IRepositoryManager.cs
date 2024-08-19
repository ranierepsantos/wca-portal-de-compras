using Microsoft.EntityFrameworkCore;
using wca.share.domain.Entities;

namespace wca.share.application.Contracts.Persistence
{
    public interface IRepositoryManager
    {

        IRepository<Cliente> ClienteRepository { get; }
        IRepository<Usuario> UsuarioRepository { get; }
        IRepository<UsuarioClientes> UsuarioClientesRepository { get; }
        IRepository<Notificacao> NotificacaoRepository { get; }
        IRepository<Solicitacao> SolicitacaoRepository { get; }
        IRepository<ItemMudanca> ItemMudancaRepository { get; }
        IRepository<SolicitacaoHistorico> SolicitacaoHistoricoRepository { get; }

        IRepository<SolicitacaoMudancaBase> MudancaBaseRepository { get; }
        Task SaveAsync();
        Task<int> ExecuteCommandAsync(string command);
        DbSet<T> GetDbSet<T>() where T : class;

        IQueryable<T> FromQuery<T>(string query) where T : class;
    }
}
