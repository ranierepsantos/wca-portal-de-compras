using wca.compras.domain.Dtos;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IRecorrenciaService
    {
        Task<RecorrenciaDto> Create(CreateRecorrenciaDto createRecorrenciaDto, string urlOrigin);
        Task<RecorrenciaDto> Update(UpdateRecorrenciaDto updateRecorrenciaDto);
        Task<bool> Remove(int id);
        Task<RecorrenciaDto> GetById(int id);
        Pagination<RecorrenciaDto> Paginate(int[]? filialId, int page = 1, int pageSize = 10, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0);
        Task<bool> ExecuteScheduleAsync();
        Pagination<RecorrenciaLogDto> PaginationLog(int recorrenciaId, int page = 1, int pageSize = 10);

        Task<RecorrenciaLogDto> Logs(int recorrenciaId);
        Task<bool> EnabledDisabled(EnabledRecorrenciaDto enabledRecorrenciaDto);

    }
}
