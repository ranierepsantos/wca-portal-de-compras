using Microsoft.EntityFrameworkCore;
using wca.compras.data.DataAccess;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;

namespace wca.compras.data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly WcaContext _context;
        private IRepository<Perfil> _perfilRepo;
        private IRepository<Permissao> _permissaoRepo;
        private IRepository<Usuario> _usuarioRepo;
        private IRepository<ResetPassword> _resetPassRepo;
        private IRepository<Cliente> _clienteRepo;
        private IRepository<Filial> _filialRepo;
        private IRepository<TipoFornecimento> _tipoFornecimentoRepo;
        private IRepository<ClienteContato> _clienteContatoRepo;
        private IRepository<Fornecedor> _fornecedorRepo;
        private IRepository<Produto> _produtoRepo;
        private IRepository<Requisicao> _requisicaoRepo;
        private IRepository<RequisicaoItem> _requisicaoItemRepo;
        private IRepository<RequisicaoHistorico> _requisicaoHistoricoRepo;
        private IRepository<FornecedorContato> _fornecedorContatoRepo;
        private IRepository<RequisicaoAprovacao> _requisicaoAprovacaoRepo;
        private IRepository<Recorrencia> _recorrenciaRepo;
        private IRepository<RecorrenciaProduto> _recorrenciaProdutoRepo;
        private IRepository<RecorrenciaLog> _recorrenciaLogRepo;
        private IRepository<Configuracao> _configuracaoRepo;

        public RepositoryManager(WcaContext context)
        {
            _context = context;
        }

        public IRepository<Perfil> PerfilRepository { 
            get
            {
                if (_perfilRepo == null)
                {
                    _perfilRepo = new BaseRepository<Perfil>(_context);
                }
                return _perfilRepo;
            }
        }

        public IRepository<Permissao> PermissaoRepository
        {
            get
            {
                if (_permissaoRepo == null)
                {
                    _permissaoRepo = new BaseRepository<Permissao>(_context);
                }
                return _permissaoRepo;
            }
        }

        public IRepository<Usuario> UsuarioRepository
        {
            get
            {
                if (_usuarioRepo == null)
                {
                    _usuarioRepo = new BaseRepository<Usuario>(_context);
                }
                return _usuarioRepo;
            }
        }

        public IRepository<ResetPassword> ResetPasswordRepository
        {
            get
            {
                if (_resetPassRepo == null)
                {
                    _resetPassRepo = new BaseRepository<ResetPassword>(_context);
                }
                return _resetPassRepo;
            }
        }

        public IRepository<Cliente> ClienteRepository
        {
            get
            {
                if (_clienteRepo == null)
                {
                    _clienteRepo = new BaseRepository<Cliente>(_context);
                }
                return _clienteRepo;
            }
        }

        public IRepository<Filial> FilialRepository
        {
            get
            {
                if (_filialRepo == null)
                {
                    _filialRepo = new BaseRepository<Filial>(_context);
                }
                return _filialRepo;
            }
        }

        public IRepository<TipoFornecimento> TipoFornecimentoRepository
        {
            get
            {
                if (_tipoFornecimentoRepo == null)
                {
                    _tipoFornecimentoRepo = new BaseRepository<TipoFornecimento>(_context);
                }
                return _tipoFornecimentoRepo;
            }
        }

        public IRepository<ClienteContato> ClienteContatoRepository
        {
            get
            {
                if (_clienteContatoRepo == null)
                {
                    _clienteContatoRepo = new BaseRepository<ClienteContato>(_context);
                }
                return _clienteContatoRepo;
            }
            
        }

        public IRepository<Fornecedor> FornecedorRepository
        {
            get
            {
                if (_fornecedorRepo == null)
                {
                    _fornecedorRepo = new BaseRepository<Fornecedor>(_context);
                }
                return _fornecedorRepo;
            }
        }

        public IRepository<Produto> ProdutoRepository
        {
            get
            {
                if (_produtoRepo == null)
                {
                    _produtoRepo = new BaseRepository<Produto>(_context);
                }
                return _produtoRepo;
            }
        }

        public IRepository<Requisicao> RequisicaoRepository
        {
            get
            {
                if (_requisicaoRepo == null) 
                    _requisicaoRepo = new BaseRepository<Requisicao>(_context);
                return _requisicaoRepo;
            }
        }

        public IRepository<RequisicaoHistorico> RequisicaoHistoricoRepository
        {
            get
            {
                if (_requisicaoHistoricoRepo == null)
                    _requisicaoHistoricoRepo = new BaseRepository<RequisicaoHistorico>(_context);
                return _requisicaoHistoricoRepo;
            }
        }

        public IRepository<RequisicaoItem> RequisicaoItemRepository
        {
            get
            {
                if (_requisicaoItemRepo == null)
                    _requisicaoItemRepo = new BaseRepository<RequisicaoItem>(_context);
                return _requisicaoItemRepo;
            }
        }

        public IRepository<FornecedorContato> FornecedorContatoRepository
        {
            get
            {
                if (_fornecedorContatoRepo == null)
                    _fornecedorContatoRepo = new BaseRepository<FornecedorContato>(_context);

                return _fornecedorContatoRepo;
            }
        }

        public IRepository<RequisicaoAprovacao> RequisicaoAprovacaoRepository
        {
            get
            {
                if (_requisicaoAprovacaoRepo == null)
                    _requisicaoAprovacaoRepo = new BaseRepository<RequisicaoAprovacao>(_context);

                return _requisicaoAprovacaoRepo;
            }
        }

        public IRepository<Recorrencia> RecorrenciaRepository
        {
            get
            {
                if (_recorrenciaRepo == null)
                    _recorrenciaRepo = new BaseRepository<Recorrencia>(_context);

                return _recorrenciaRepo;
            }
        }

        public IRepository<RecorrenciaProduto> RecorrenciaProdutoRepository
        {
            get
            {
                if (_recorrenciaProdutoRepo == null)
                    _recorrenciaProdutoRepo = new BaseRepository<RecorrenciaProduto>(_context);

                return _recorrenciaProdutoRepo;
            }
        }
        public IRepository<RecorrenciaLog> RecorrenciaLogRepository
        {
            get
            {
                if (_recorrenciaLogRepo == null)
                    _recorrenciaLogRepo = new BaseRepository<RecorrenciaLog>(_context);

                return _recorrenciaLogRepo;
            }
        }

        public IRepository<Configuracao> ConfiguracaoRepository
        {
            get
            {
                if (_configuracaoRepo == null)
                    _configuracaoRepo = new BaseRepository<Configuracao>(_context);

                return _configuracaoRepo;
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> ExecuteCommandAsync(string command)
        {
            int result = await _context.Database.ExecuteSqlRawAsync(command);
            return result;
        }
    }
}
