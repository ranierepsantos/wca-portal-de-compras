using Microsoft.Extensions.Configuration;
using Refit;
using System.Net.Http.Headers;
using wca.reembolso.application.Contracts.Integration.NorgeChatBot;

namespace wca.reembolso.infrastruture.Integration.NorgeChatBot
{
    internal class IntegrationNorgeChatBot : IIntegrationNorgeChatBot
    {
        private readonly IConfiguration _config;
        private readonly IRefitNorgeChatBotService? _client = null;
        

        public IntegrationNorgeChatBot(IConfiguration config)
        {
            _config = config;
            string? chatbotUrl = _config["NorgeChatBot:Url"];
            string? chatbotToken = _config["NorgeChatBot:BearerToken"];
            HttpClient? _httpClient = new();    
            if (!string.IsNullOrEmpty(chatbotUrl) && !string.IsNullOrEmpty(chatbotToken))
            {
                _httpClient.BaseAddress = new Uri(chatbotUrl);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", chatbotToken);
                _client = RestService.For<IRefitNorgeChatBotService>(_httpClient);
            }
        }

        public async Task<Response> Send(string number, string message)
        {
            //trazer somente números
            if (_client is not null){
                number = String.Join("", System.Text.RegularExpressions.Regex.Split(number, @"[^\d]"));
            
                Message _msg = new(number, message);
                return await _client.SendMessage(_msg);
            }
            return new Response(){
                Error = "Chatbot não configurado"
            };
        }
    }
}
