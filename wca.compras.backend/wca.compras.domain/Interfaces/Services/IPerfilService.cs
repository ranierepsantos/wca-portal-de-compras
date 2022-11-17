using wca.compras.domain.Util;
using wca.compras.domain.Dtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPerfilService
    {
        public Task<PerfilDto> Create(CreatePerfilDto perfil);

        public Task<PerfilDto> Update(UpdatePerfilDto perfil);

        public Task<PerfilPermissoesDto> GetWithPermissoes(string id);

        public Task<IList<ListItem>> GetToList();

        public Task<Pagination<PerfilDto>> Paginate(int page =1, int pageSize = 10, string termo ="");


    }
}
