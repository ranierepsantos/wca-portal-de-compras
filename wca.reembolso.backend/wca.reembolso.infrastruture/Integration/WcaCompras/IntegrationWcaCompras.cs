using Microsoft.Extensions.Configuration;
using Refit;
using wca.reembolso.application.Contracts.Integration.WcaCompras;

namespace wca.reembolso.infrastruture.Integration.WcaCompras
{
    public class IntegrationWcaCompras : IIntegrationWcaCompras
    {
        private readonly IConfiguration _configuration;
        private string _token;
        private readonly IRefitWcaComprasService _client;

        public IntegrationWcaCompras(IConfiguration configuration)
        {
            _configuration = configuration;
            var wcaurl = new Uri(_configuration["WCACompras:Url"]?.ToString());

            if (wcaurl == null)
            {
                throw new ArgumentNullException(nameof(wcaurl));
            }
            _client = RestService.For<IRefitWcaComprasService>(wcaurl.ToString());
            
            GetTokenAsync();
        }

        private void GetTokenAsync()
        {
            LoginRequest _request = new (_configuration["WCACompras:User"]?.ToString(),
                                         _configuration["WCACompras:Password"]?.ToString());

            var response = (_client.Login(_request)).Result;

            if (response.Authenticated)
                _token = response.AccessToken;
            else
                throw new InvalidOperationException(response.Message);
        }
        public async Task<IEnumerable<UsuarioResponse>> UsuariosListByPermissao(string permissao)
        {
            return await _client.ListarUsuariosBySistemaPermissao(2, permissao, _token);
        }
    }
}
