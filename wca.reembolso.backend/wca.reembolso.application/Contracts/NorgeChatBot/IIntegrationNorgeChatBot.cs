namespace wca.reembolso.application.Contracts.NorgeChatBot
{
    public interface IIntegrationNorgeChatBot
    {
        Task<Response> Send(string number, string message);
    }
}
