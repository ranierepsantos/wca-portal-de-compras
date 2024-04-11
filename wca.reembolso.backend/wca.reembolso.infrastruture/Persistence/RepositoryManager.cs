using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;
using wca.reembolso.infrastruture.Context;

namespace wca.reembolso.infrastruture.Persistence
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly WcaReembolsoContext _context;
        private IRepository<ContaCorrente>? _contaCorrenteRepo;
        private IRepository<Cliente>? _clienteRepo;
        private IRepository<Despesa>? _despesaRepo;
        private IRepository<Faturamento>? _faturamentoRepo;
        private IRepository<FaturamentoItem>? _faturamentoItemRepo;
        private IRepository<StatusSolicitacao>? _statusSolicitacaoRepo;
        private IRepository<TipoDespesa>? _tipoDespesaRepo;
        private IRepository<SolicitacaoHistorico>? _solicitacaoHistoricoRepo;
        private IRepository<Solicitacao>? _solicitacaoRepo;
        private IRepository<Notificacao>? _notificacaoRepo;
        private IRepository<Transacao>? _transacaoRepo;
        private IRepository<UsuarioClientes>? _usuarioClienteRepo;
        private IRepository<Usuario>? _usuarioRepo;
        private IRepository<FaturamentoHistorico>? _faturamentoHistoricoRepo;
        private IRepository<CentroCusto>? _centroCustoRepo;
        //private IRepository<FilialUsuario>? _filialUsuarioRepo;

        public RepositoryManager(WcaReembolsoContext context) => _context = context;

        public IRepository<Cliente> ClienteRepository => _clienteRepo ??= new BaseRepository<Cliente>(_context);

        public IRepository<ContaCorrente> ContaCorrenteRepository => _contaCorrenteRepo ??= new BaseRepository<ContaCorrente>(_context);

        public IRepository<Despesa> DespesaRepository => _despesaRepo ??= new BaseRepository<Despesa>(_context);

        public IRepository<Faturamento> FaturamentoRepository => _faturamentoRepo ??= new BaseRepository<Faturamento>(_context);

        public IRepository<FaturamentoItem> FaturamentoItemRepository => _faturamentoItemRepo ??= new BaseRepository<FaturamentoItem>(_context);

        public IRepository<Notificacao> NotificacaoRepository => _notificacaoRepo ??= new BaseRepository<Notificacao>(_context);

        public IRepository<Solicitacao> SolicitacaoRepository => _solicitacaoRepo ??= new BaseRepository<Solicitacao>(_context);

        public IRepository<SolicitacaoHistorico> SolicitacaoHistoricoRepository => _solicitacaoHistoricoRepo ??= new BaseRepository<SolicitacaoHistorico>(_context);

        public IRepository<StatusSolicitacao> StatusSolicitacaoRepository => _statusSolicitacaoRepo ??= new BaseRepository<StatusSolicitacao>(_context);

        public IRepository<TipoDespesa> TipoDespesaRepository => _tipoDespesaRepo ??= new BaseRepository<TipoDespesa>(_context);

        public IRepository<Transacao> TransacaoRepository => _transacaoRepo ??= new BaseRepository<Transacao>(_context);

        public IRepository<UsuarioClientes> UsuarioClientesRepository => _usuarioClienteRepo ??= new BaseRepository<UsuarioClientes>(_context);
        public IRepository<Usuario> UsuarioRepository => _usuarioRepo ??= new BaseRepository<Usuario>(_context);
        public IRepository<FaturamentoHistorico> FaturamentoHistoricoRepository => _faturamentoHistoricoRepo ??= new BaseRepository<FaturamentoHistorico>(_context);
        public IRepository<CentroCusto> CentroCustoRepository => _centroCustoRepo ??= new BaseRepository<CentroCusto>(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> ExecuteCommandAsync(string command)
        {
            int result = await _context.Database.ExecuteSqlRawAsync(command);
            return result;
        }
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public async Task<List<T>> GetFromSQL<T>(string query) where T: class
        {
            return await _context.Set<T>().FromSqlRaw(query)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
