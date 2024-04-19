using Refit;
using wca.reembolso.application.Contracts.NorgeChatBot;

namespace wca.reembolso.infrastruture.Integration.NorgeChatBot
{
    internal interface IRefitNorgeChatBotService
    {
        [Post("/messages/send")]
        Task<Response> SendMessage([Body] Message message);
    }
}
