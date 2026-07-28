using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IRequisicaoService
    {
        Task<RequisicaoDto> Create(CreateRequisicaoDto createRequisicaoDto, string urlOrigin = "");
        Task<RequisicaoDto> Update(UpdateRequisicaoDto updateRequisicaoDto, string urlOrigin = "");
        Task<bool> Remove(int id, string nomeUsuario);
        Task<RequisicaoDto> GetById(int id);
        Pagination<RequisicaoDto> Paginate(RequisicaoPaginateFilters filters, int page = 1, int pageSize = 10);
        Task<RequisicaoAprovacaoDto> GetByAprovacaoToken(string tokenAprovacao);
        Task<bool> aprovarRequisicao(AprovarRequisicaoDto aprovarRequisicaoDto, string urlOrigin);
        Task<Stream> ExportToExcel(int requisicaoId);
        Task<Stream> ExportToExcel(RequisicaoPaginateFilters filters);
        Task<RequisicaoDuplicarResponse> Duplicate(int requisicaoId, int usuarioId, string usuarioNome, string urlOrigin);
        Task<bool> FinalizarPedido(FinalizarRequisicaoDto finalizarRequisicaoDto, string usuarioNome);
        Task<bool> EnviarRequisicao(int requisicaoId, EnumRequisicaoDestinoEmail destino, string urlOrigin);
        Pagination<RequisicaoDto> PaginateByContextUser(RequisicaoPaginateFilters filters, int page = 1, int pageSize = 10);
    }
}
