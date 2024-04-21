using Refit;
using wca.reembolso.application.Contracts.Integration.NorgeChatBot;

namespace wca.reembolso.infrastruture.Integration.NorgeChatBot
{
    [Headers("accept: application/json, text/plain, */*",
             "authority: api.norgebots.com",
             "accept-language: pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7")]
    internal interface IRefitNorgeChatBotService
    {
        [Post("/messages/send")]
        Task<Response> SendMessage([Body] Message message);
    }
}
