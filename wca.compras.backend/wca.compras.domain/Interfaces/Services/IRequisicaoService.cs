using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IRequisicaoService
    {
        Task<RequisicaoDto> Create(CreateRequisicaoDto createRequisicaoDto, string urlOrigin = "");
        Task<RequisicaoDto> Update(int filialId, UpdateRequisicaoDto updateRequisicaoDto, string urlOrigin = "");
        Task<bool> Remove(int filialId, int id, string nomeUsuario);
        Task<RequisicaoDto> GetById(int filialId, int id);
        Pagination<RequisicaoDto> Paginate(int filialId, int page = 1, int pageSize = 10, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0, EnumStatusRequisicao status = EnumStatusRequisicao.TODOS);
        Task<RequisicaoAprovacaoDto> GetByAprovacaoToken(string tokenAprovacao);
        Task<bool> aprovarRequisicao(AprovarRequisicaoDto aprovarRequisicaoDto, string urlOrigin);
        Task<Stream> ExportToExcel(int requisicaoId);
        Task<RequisicaoDuplicarResponse> Duplicate(int requisicaoId, int usuarioId, string usuarioNome, string urlOrigin);
        Task<bool> FinalizarPedido(FinalizarRequisicaoDto finalizarRequisicaoDto, string usuarioNome);
        Task<bool> EnviarRequisicao(int requisicaoId, EnumRequisicaoDestinoEmail destino, string urlOrigin);
    }
}
