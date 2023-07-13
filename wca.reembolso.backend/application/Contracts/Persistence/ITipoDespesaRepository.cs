using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Contracts.Persistence
{
    public interface ITipoDespesaRepository: IBaseRepository<TipoDespesa>
    {
        Task<TipoDespesa?> GetByIdAsync(int id);
    }
}
