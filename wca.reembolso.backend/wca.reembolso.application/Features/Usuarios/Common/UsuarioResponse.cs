namespace wca.reembolso.application.Features.Usuarios.Common
{
    public record UsuarioToListResponse (
        int Id,
        string Nome,
        string Cargo,
        int CentroCustoId
    );
}
