using Irony.Parsing;
using Microsoft.Extensions.Configuration;
using Refit;
using System.Net;
using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Integration.GI.Models;

namespace wca.share.infrastructure.Integration.GI
{
    public class IntegrationGI : IIntegrationGI
    {
        private readonly IGIRefitService _client;
        private readonly IntegracaoGIConfig _settings;
        private VerificarConexao.Response _tokenResponse;

        public IntegrationGI(IConfiguration config)
        {
            _settings = config.GetSection("IntegracaoGI").Get<IntegracaoGIConfig>() ?? throw new ArgumentNullException(nameof(config));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_settings.Url.TrimEnd('/')),
                Timeout = TimeSpan.FromMinutes(10)
            };

            _client = RestService.For<IGIRefitService>(httpClient);
        }

        
        private async Task<string> GetTokenAsync(bool forceGenerate = false)
        {
            if (forceGenerate || _tokenResponse == null || (_tokenResponse.ValidTo - DateTime.Now).TotalMinutes <= 30)
            {
                var conexaoRequest = new VerificarConexao.Request
                {
                    IdClienteWeb = _settings.IdClienteWeb,
                    ChaveAcesso = _settings.ChaveAcesso
                };

                VerificarConexao.Response conexaoResponse;
                try
                {
                    conexaoResponse = await _client.VerificarConexao(conexaoRequest);
                }
                catch (ApiException ex)
                {
                    throw new InvalidOperationException($"Falha ao verificar conexão com a API GI. Status: {ex.StatusCode}", ex);
                }

                var loginRequest = new LoginRequest
                {
                    Usuario = _settings.Login,
                    Senha = _settings.Senha
                };

                _tokenResponse = await _client.Login(loginRequest, conexaoResponse.Token);
            }

            if (_tokenResponse == null || string.IsNullOrEmpty(_tokenResponse.Token))
            {
                throw new InvalidOperationException("Falha ao obter o token de acesso.");
            }

            return _tokenResponse.Token;
        }

        public async Task<IEnumerable<ClienteResponse>> ClienteGetAllAsync()
        {
            return await ExecuteWithTokenRetryAsync(async (token) => await _client.ClienteGetAll(token));
            //try
            //{
            //    var token = await GetTokenAsync();
            //    return await _client.ClienteGetAll(token);
            //}
            //catch (ApiException ex)
            //{
            //    if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
            //        ex.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
            //    {
            //        var token = await GetTokenAsync(true);
            //        var response = await _client.ClienteGetAll(token);
            //        return response;
            //    }
            //    throw;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }

        public async Task<IEnumerable<CentroCustoResponse>> CentroCustoGetAllAsync()
        {

            return await ExecuteWithTokenRetryAsync(async (token) => await _client.CentroCustoGetAll(token));
            //try
            //{
            //    var token = await GetTokenAsync();
            //    return await _client.CentroCustoGetAll(token);
            //}
            //catch (ApiException ex)
            //{
            //    if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
            //        ex.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
            //    {
            //        var token = await GetTokenAsync(true);
            //        var response = await _client.CentroCustoGetAll(token);
            //        return response;
            //    }
            //    throw;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}


        }

        public async Task<IEnumerable<FuncionarioResponse>> FuncionarioGetAllAsync()
        {
            return await ExecuteWithTokenRetryAsync(async (token) => await _client.FuncionarioGetAll(token));
            //var token = await GetTokenAsync();
            //return await _client.FuncionarioGetAll(token);
        }

        public async Task<IEnumerable<FuncionarioResponse>> FuncionarioGetAllJsonAsync(WhereCondition where)
        {


            return await ExecuteWithTokenRetryAsync(async (token) => await _client.FuncionarioGetAllJson(token, where));

            //try
            //{
            //    var token = await GetTokenAsync();
            //    var response = await _client.FuncionarioGetAllJson(token, where);
            //    return response;
            //}
            //catch (ApiException ex)
            //{
            //    if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
            //        ex.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
            //    {
            //        var token = await GetTokenAsync(true);
            //        var response = await _client.FuncionarioGetAllJson(token, where);
            //        return response;
            //    }
            //    throw;                
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"FuncionarioGetAllJsonAsync.Error: {ex.Message}");
            //    throw;
            //}
            
        }

        /// <summary>
        /// Executa uma chamada à API com lógica de retry em caso de falha de autenticação (401) ou timeout (504).
        /// </summary>
        /// <typeparam name="TResponse">Tipo da resposta esperada.</typeparam>
        /// <param name="apiCall">Função que representa a chamada à API (ex: (token) => _client.ClienteGetAll(token)).</param>
        private async Task<TResponse> ExecuteWithTokenRetryAsync<TResponse>(Func<string, Task<TResponse>> apiCall)
        {
            string token = await GetTokenAsync();
            int tryNumber = 5;
            try
            {
                return await apiCall(token);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized || ex.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                if (tryNumber > 0)
                {
                    tryNumber--;
                    // Tratamento de Retry: Tentativa de renovar o token e repetir a chamada
                    token = await GetTokenAsync(forceGenerate: true);
                    return await apiCall(token); // Repete a chamada com o novo token
                    
                }
                else
                {
                    Console.WriteLine($"Erro ao executar API GI, número de tentativas excedido: Mensagem: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Captura outras exceções da API (ex: 404, 500 que não sejam 504)
                Console.WriteLine($"Erro ao executar API GI. Mensagem: {ex.Message}");
                throw;
            }
        }

    }

    public class IntegracaoGIConfig
    {
        public string Url { get; set; } = string.Empty;
        public string IdClienteWeb { get; set; } = string.Empty;
        public string ChaveAcesso { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}