using wca.compras.domain.Dtos;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IFornecedorService
    {
        public Task<FornecedorDto> Create(CreateFornecedorDto createFornecedorDto);
        public Task<FornecedorDto> Update(UpdateFornecedorDto updateFornecedorDto);
        public Task<bool> Remove(int id);
        public Task<FornecedorDto> GetById(int id);
        public Pagination<FornecedorDto> Paginate(int[]? filialId, int page = 1, int pageSize = 10, string termo = "");
        public Task<IList<ListItem>> GetToList(int[] filialId);
        public Task<ProdutoDto> CreateProduto(CreateProdutoDto createProdutoDto);
        public Task<ProdutoDto> GetProdutoById(int fornecedorId, int id);
        public Task<ProdutoDto> UpdateProduto(UpdateProdutoDto updateProdutoDto);
        public Task<bool> RemoveProduto(int fornecedorId, int id);
        public Task<bool> ImportProdutoFromExcel(int fornecedorId, ImportProdutoDto importProdutoDto);
        public Pagination<ProdutoDto> Paginate(int fornecedorId, int page = 1, int pageSize = 10, string termo = "");
        Task<Stream?> ProdutosExportToExcel(int fornecedorId);
        Task<IList<ProdutoWithIcmsDto>> ListProdutoByFornecedorWithIcms(int fornecedorId, string uf, int usuarioId, string? termo = "");
    }
}
