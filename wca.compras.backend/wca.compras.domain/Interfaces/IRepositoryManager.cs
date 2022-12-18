using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces
{
    public interface IRepositoryManager
    {
        public IRepository<Perfil> PerfilRepository { get; }
        public IRepository<Permissao> PermissaoRepository { get; }
        public IRepository<ResetPassword> ResetPasswordRepository { get; }
        public IRepository<Usuario> UsuarioRepository { get; }
        public IRepository<Cliente> ClienteRepository { get; }
        public IRepository<Filial> FilialRepository { get; }
        public IRepository<TipoFornecimento> TipoFornecimentoRepository { get; }
        public IRepository<ClienteContato> ClienteContatoRepository { get; }
        public IRepository<Fornecedor> FornecedorRepository { get; }
        public IRepository<Produto> ProdutoRepository { get; }
        public IRepository<Requisicao> RequisicaoRepository { get; }
        public IRepository<RequisicaoHistorico> RequisicaoHistoricoRepository { get; }
        public IRepository<RequisicaoItem> RequisicaoItemRepository { get; }

        Task SaveAsync();
    }
}
