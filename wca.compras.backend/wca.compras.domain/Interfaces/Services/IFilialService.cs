using wca.compras.domain.Dtos;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IFilialService
    {
        public Task<FilialDto> Create(CreateFilialDto createFilialDto);
        public Task<FilialDto> Update(FilialDto filialDto);
        public Task<IList<FilialDto>> GetAll();
        public Task<FilialDto> GetById(int id);
        public Task<IList<ListItem>> GetToList(int filialId);

        Task<IList<ListItem>> GetToListByUser(int usuarioId);
        public Pagination<FilialDto> Paginate(int page, int pageSize, string termo = "");
    }
}
