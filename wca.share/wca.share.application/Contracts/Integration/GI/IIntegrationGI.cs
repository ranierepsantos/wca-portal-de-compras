using wca.share.application.Contracts.Integration.GI.Models;

namespace wca.share.application.Contracts.Integration.GI
{
    public interface IIntegrationGI
    {
        Task<IEnumerable<ClienteResponse>> ClienteGetAllAsync();
    }
}
