using wca.compras.domain.Dtos;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface ITipoFornecimentoService
    {
        public Task<TipoFornecimentoDto> Create(CreateTipoFornecimentoDto tipo);

        public Task<TipoFornecimentoDto> Update(TipoFornecimentoDto updateTipo);

        public Task<IList<ListItem>> GetToList();

        Pagination<TipoFornecimentoDto> Paginate(int page, int pageSize, string termo = "");

    }
}
