using wca.compras.domain.Dtos;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IFornecedorService
    {
        public Task<FornecedorDto> Create(CreateFornecedorDto createFornecedorDto);
        public Task<FornecedorDto> Update(int filialId, UpdateFornecedorDto updateFornecedorDto);
        public Task<bool> Remove(int filialId, int id);
        public Task<FornecedorDto> GetById(int filialId, int id);
        public Pagination<FornecedorDto> Paginate(int filialId, int page = 1, int pageSize = 10, string termo = "");
        public Task<IList<FornecedorListDto>> GetToList(int filialId);

        public Task<ProdutoDto> CreateProduto(CreateProdutoDto createProdutoDto);
        public Task<ProdutoDto> GetProdutoById(int filialId, int fornecedorId, int id);
        public Task<ProdutoDto> UpdateProduto(int filialId, UpdateProdutoDto updateProdutoDto);
        public Task<bool> RemoveProduto(int filialId, int fornecedorId, int id);
        public Task<bool> ImportProdutoFromExcel(int filialId, int fornecedorId, ImportProdutoDto importProdutoDto);
        public Pagination<ProdutoDto> Paginate(int filialId, int fornecedorId, int page = 1, int pageSize = 10, string termo = "", int usuarioId = 0);
        Task<Stream?> ProdutosExportToExcel(int fornecedorId);
    }
}
