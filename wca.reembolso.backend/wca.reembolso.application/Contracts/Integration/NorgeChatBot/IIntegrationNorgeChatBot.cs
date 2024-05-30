namespace wca.reembolso.application.Contracts.Integration.NorgeChatBot
{
    public interface IIntegrationNorgeChatBot
    {
        Task<Response> Send(string number, string message);
    }
}
