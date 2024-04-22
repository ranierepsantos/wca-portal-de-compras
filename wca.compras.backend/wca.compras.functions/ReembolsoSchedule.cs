using System;
using System.Net.Http.Headers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimerTriggerAttribute = Microsoft.Azure.Functions.Worker.TimerTriggerAttribute;

namespace wca.compras.functions
{
    public class ReembolsoSchedule
    {
        private readonly ILogger _logger;

        public ReembolsoSchedule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ReembolsoSchedule>();
        }

        [Function("ReembolsoSchedule")]
        public async Task Run([TimerTrigger("0 24 4 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"ReembolsoSchedule executed at: {DateTime.Now}");

            HttpClient client = new();

            string baseUrl = "https://api-reembolso-wca.azurewebsites.net/"; //producao
            //string baseUrl = "https://wca-api-reembolso-hml.azurewebsites.net/"; //hml
            //string baseUrl = "https://localhost:7235/"; //desenvolvimento

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            _logger.LogInformation($"{DateTime.Now} Executando schedule di�ria de solicita��es vencidas");
            var path = "api/Solicitacao/ChecarVencidos";

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{DateTime.Now} - Checar Solicita��es vencidas - executeSchedule success!");
            }
            else
            {
                _logger.LogInformation($"{DateTime.Now} - Checar Solicita��es vencidas - executeSchedule problem!");
            }

            _logger.LogInformation($"{DateTime.Now} Executando schedule di�rio de envio de mensagem via chatbot");

            path = "api/Faturamento/SendChatBotAfterDays/7";

            response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{DateTime.Now} - Faturamento chatbot 7 dias - executeSchedule success!\");\r\n            }}\r\n            else\r\n            {{\r\n                _logger.LogInformation($\"{{DateTime.Now}} - Checar Solicita��es vencidas - executeSchedule problem!\");\r\n            }}\r\n - executeSchedule success!");
            }
            else
            {
                _logger.LogInformation($"{DateTime.Now} - Faturamento chatbot 7 dias - executeSchedule problem!");
            }


            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }

    
}
