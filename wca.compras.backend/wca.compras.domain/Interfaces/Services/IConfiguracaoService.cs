using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IConfiguracaoService
    {
        Task<Configuracao> Update(UpdateConfiguracaoDto configuracao);

        Task<IList<Configuracao>> GetAll();

    }
}
