using System.Net.Http.Headers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace wca.compras.functions
{
    public class WcaFunctions
    {
        private readonly ILogger _logger;

        public WcaFunctions(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WcaFunctions>();
        }

        [Function("WcaComprasSchedule")]
        public async Task Run([TimerTrigger("0 30 6 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            HttpClient client = new HttpClient();
            
            string baseUrl = "https://wca-backend.azurewebsites.net/"; //produção
            //string baseUrl = "https://localhost:7211/"; //desenvolvimento

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var path = "api/Recorrencia/ExecuteSchedule";

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode) {
                 _logger.LogInformation($"{DateTime.Now} ExecuteSchedule success!");
            }else {
                _logger.LogInformation($"{DateTime.Now} ExecuteSchedule problem!");
            }
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
