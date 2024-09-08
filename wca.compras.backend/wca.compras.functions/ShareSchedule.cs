using System;
using System.Net.Http.Headers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimerTriggerAttribute = Microsoft.Azure.Functions.Worker.TimerTriggerAttribute;

namespace wca.compras.functions
{
    public class ShareSchedule
    {
        private readonly ILogger _logger;

        public ShareSchedule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ShareSchedule>();
        }

        [Function("ShareSchedule")]
        public async Task Run([TimerTrigger("0 11 1 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"ShareSchedule executed at: {DateTime.Now}");

            HttpClient client = new();

            //string baseUrl = "https://api-share-wca.azurewebsites.net/"; //producao
            string baseUrl = "https://wca-api-share-hml.azurewebsites.net/"; //hml
            //string baseUrl = "https://localhost:7235/"; //desenvolvimento

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromMinutes(120);

            _logger.LogInformation($"{DateTime.Now} Executando schedule diária - Integração GI - Clientes");
            var path = "api/Cliente/CreateFromGI";

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{DateTime.Now} - Criar/Atualizar Clientes a partir do GI - executeSchedule success!");
            }
            else
            {
                _logger.LogInformation($"{DateTime.Now} - Criar/Atualizar Clientes a partir do GI - executeSchedule problem!");
            }

            _logger.LogInformation($"{DateTime.Now} Executando schedule diária - Integração GI - Funcionários");

            path = "api/Funcionario/CreateFromGI";

            response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{DateTime.Now} - Criar/Atualizar Funcionarios a partir do GI - executeSchedule success!");
            }
            else
            {
                _logger.LogInformation($"{DateTime.Now} -  Criar/Atualizar Funcionarios a partir do GI  - executeSchedule problem!");
            }


            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }

    
}
