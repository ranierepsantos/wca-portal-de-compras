using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Contracts.Persistence
{
    public interface IClienteRepository: IRepository<Cliente>
    {
        Task<Cliente?> GetByIdAsync(int id);
        Task<List<Cliente>> GetByNamePartialAsync(string name);

    }
}
