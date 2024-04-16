using System;
using System.Net.Http.Headers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimerTriggerAttribute = Microsoft.Azure.Functions.Worker.TimerTriggerAttribute;

namespace wca.compras.functions
{
    public class ReembolsoChecarSolicitacaoVencida
    {
        private readonly ILogger _logger;

        public ReembolsoChecarSolicitacaoVencida(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ReembolsoChecarSolicitacaoVencida>();
        }

        [Function("ReembolsoChecarSolicitacaoVencida")]
        public async Task Run([TimerTrigger("0 24 4 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"Timer trigger function executed at: {DateTime.Now}");

            HttpClient client = new();

            string baseUrl = "https://api-reembolso-wca.azurewebsites.net/api"; //producao
            //string baseUrl = "https://wca-api-reembolso-hml.azurewebsites.net/"; //hml
            //string baseUrl = "https://localhost:7235/"; //desenvolvimento

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var path = "api/Solicitacao/ChecarVencidos";

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{DateTime.Now} ExecuteSchedule success!");
            }
            else
            {
                _logger.LogInformation($"{DateTime.Now} ExecuteSchedule problem!");
            }

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }

    
}
