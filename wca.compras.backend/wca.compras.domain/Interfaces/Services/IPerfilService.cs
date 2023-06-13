using wca.compras.domain.Util;
using wca.compras.domain.Dtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPerfilService
    {
        public Task<PerfilDto> Create(CreatePerfilDto perfil);

        public Task<PerfilDto> Update(UpdatePerfilDto perfil);

        public Task<PerfilPermissoesDto> GetWithPermissoes(int id);

        Task<PerfilPermissoesDto> GetByUserAndSistemaWithPermissoes(int usuarioId, int sistemaId);

        public Task<IList<ListItem>> GetToList(int sistemaId);

        public Task<Pagination<PerfilDto>> Paginate(int sistemaId, int page = 1, int pageSize = 10, string termo = "");

    }
}
