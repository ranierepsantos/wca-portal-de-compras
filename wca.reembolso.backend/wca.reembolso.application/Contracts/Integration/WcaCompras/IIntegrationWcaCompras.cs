namespace wca.reembolso.application.Contracts.Integration.WcaCompras
{
    public interface IIntegrationWcaCompras
    {
        Task<IEnumerable<UsuarioResponse>> UsuariosListByPermissao(string permissao);
    }
}
