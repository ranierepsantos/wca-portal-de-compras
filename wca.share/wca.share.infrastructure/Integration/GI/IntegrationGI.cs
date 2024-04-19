using Microsoft.Extensions.Configuration;
using Refit;
using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Integration.GI.Models;

namespace wca.share.infrastructure.Integration.GI
{
    public class IntegrationGI : IIntegrationGI
    {
        private readonly IGIRefitService _client;
        private readonly IConfiguration _config;
        private HttpClient _httpClient;
        private VerificarConexao.Response token;

 
        public IntegrationGI(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_config["IntegracaoGI:Url"].ToString().TrimEnd('/'))
            };
            _client = RestService.For<IGIRefitService>(_httpClient);
            GetToken();
        }


        private void GetToken()
        {
            var request = new VerificarConexao.Request()
            {
                IdClienteWeb = _config["IntegracaoGI:IdClienteWeb"].ToString(),
                ChaveAcesso = _config["IntegracaoGI:ChaveAcesso"].ToString()
            };

            VerificarConexao.Response response =  _client.VerificarConexao(request).Result;

            var loginRequest = new LoginRequest() { 
                Usuario= _config["IntegracaoGI:Login"],
                Senha = _config["IntegracaoGI:Senha"],
            };

            token = _client.Login(loginRequest, response.Token).Result;
        }

        public async Task<IEnumerable<ClienteResponse>> ClienteGetAllAsync()
        {
            return await _client.ClienteGetAll(token.Token);
        }

        public async Task<IEnumerable<CentroCustoResponse>> CentroCustoGetAllAsync()
        {
            return await _client.CentroCustoGetAll(token.Token);
        }

        public async Task<IEnumerable<FuncionarioResponse>> FuncionarioGetAllAsync()
        {
            return await _client.FuncionarioGetAll(token.Token);
        }
    }
}
