using Refit;
using wca.reembolso.application.Contracts.Integration.NorgeChatBot;

namespace wca.reembolso.infrastruture.Integration.NorgeChatBot
{
    [Headers("accept: application/json, text/plain, */*",
             "accept-language: pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7")]
    internal interface IRefitNorgeChatBotService
    {
        [Post("/send")]
        Task<Rootobject> SendMessage([Body] ChatBotMessage message);
    }
}
