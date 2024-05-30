using Microsoft.Extensions.Configuration;
using Refit;
using System.Net.Http;
using System.Net.Http.Headers;
using wca.reembolso.application.Contracts.Integration.NorgeChatBot;

namespace wca.reembolso.infrastruture.Integration.NorgeChatBot
{
    internal class IntegrationNorgeChatBot : IIntegrationNorgeChatBot
    {
        private readonly IConfiguration _config;
        private readonly IRefitNorgeChatBotService _client;
        private HttpClient _httpClient;

        public IntegrationNorgeChatBot(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_config["NorgeChatBot:Url"].ToString().TrimEnd('/'))
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["NorgeChatBot:BearerToken"]);
            _client = RestService.For<IRefitNorgeChatBotService>(_httpClient);
        }

        public async Task<Response> Send(string number, string message)
        {
            //trazer somente números
            number = String.Join("", System.Text.RegularExpressions.Regex.Split(number, @"[^\d]"));
            
            Message _msg = new(number, message);
            return await _client.SendMessage(_msg);
        }
    }
}
