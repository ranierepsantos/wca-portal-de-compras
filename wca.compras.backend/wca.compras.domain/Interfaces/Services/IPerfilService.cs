using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PerfilDtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPerfilService
    {
        public Task<PerfilDto> Create(CreatePerfilDto perfil);

        public Task<PerfilDto> Update(UpdatePerfilDto perfil);

        public Task<PerfilPermissoesDto> GetWithPermissoes(string id);

        public Task<IList<ListItem>> GetToList();


    }
}
