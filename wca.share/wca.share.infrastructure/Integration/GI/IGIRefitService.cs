using Refit;
using wca.share.application.Contracts.Integration.GI.Models;
namespace wca.share.infrastructure.Integration.GI
{
    public interface IGIRefitService
    {
        [Post("/Conexao/VerificaConexao")]
        Task<VerificarConexao.Response> VerificarConexao([Body] VerificarConexao.Request request);

        [Post("/Login/Login")]
        Task<VerificarConexao.Response> Login([Body] LoginRequest request, [Authorize("Bearer")] string token);

        [Get("/Cliente/GetAll")]
        Task<IEnumerable<ClienteResponse>> ClienteGetAll([Authorize("Bearer")] string token);

        [Get("/CentroCusto/GetAll")]
        Task<IEnumerable<CentroCustoResponse>> CentroCustoGetAll([Authorize("Bearer")] string token);

        [Get("/Funcionario/GetAll")]
        Task<IEnumerable<FuncionarioResponse>> FuncionarioGetAll([Authorize("Bearer")] string token);

    }
}
