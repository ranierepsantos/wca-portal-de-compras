using application.Contracts.Persistence;
using Data.Persistence.Common;
using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Persistence
{
    public class TipoDespesaRepository : BaseRepository<TipoDespesa>, ITipoDespesaRepository
    {
        public Task<TipoDespesa?> GetByIdAsync(int id)
        {
            var data = ToQuery().Where(c => c.Id.Equals(id)).FirstOrDefault();
            return Task.FromResult(data);
        }
    }
}
