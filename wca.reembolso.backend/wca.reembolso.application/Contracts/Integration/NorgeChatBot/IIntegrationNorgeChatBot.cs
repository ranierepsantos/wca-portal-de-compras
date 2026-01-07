namespace wca.reembolso.application.Contracts.Integration.NorgeChatBot
{
    public interface IIntegrationNorgeChatBot
    {
        Task<Rootobject> Send(string number, string message);
    }
}
