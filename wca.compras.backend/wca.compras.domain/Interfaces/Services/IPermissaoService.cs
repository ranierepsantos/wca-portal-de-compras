using wca.compras.domain.Util;
using wca.compras.domain.Dtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPermissaoService
    {
        public Task<IList<PermissaoDto>> GetAll(int sistemaId);
        public Task<IList<ListItem>> GetToList(int sistemaId);
    }
}
