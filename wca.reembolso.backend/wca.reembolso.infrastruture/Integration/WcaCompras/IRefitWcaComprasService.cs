using Refit;
using wca.reembolso.application.Contracts.Integration.WcaCompras;

namespace wca.reembolso.infrastruture.Integration.WcaCompras
{
    [Headers("accept: application/json")]
    internal interface IRefitWcaComprasService
    {
        [Post("/Authentication/Autenticar")]
        Task<LoginResponse> Login([Body] LoginRequest request);
        
        [Get("/Usuario/ToListByPermissao/{sistema}/{permissao}")]
        Task<IEnumerable<UsuarioResponse>> ListarUsuariosBySistemaPermissao([AliasAs("sistema")]int sistema, [AliasAs("permissao")] string permissao, [Authorize("Bearer")] string token);
    }
}
