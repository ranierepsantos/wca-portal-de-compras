using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;
using wca.reembolso.infrastruture.Context;

namespace wca.reembolso.infrastruture.Persistence
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(WcaReembolsoContext context) : base(context)
        {
        }

        public Task<Cliente?> GetByIdAsync(int id)
        {
            var query = ToQuery();
            query = query.Where(q => q.Id == id);

            return query.FirstOrDefaultAsync();
        }

        public Task<List<Cliente>> GetByNamePartialAsync(string name)
        {
            var query = ToQuery();
            query = query.Where(q => q.Nome.ToLower().Contains(name.ToLower()))
                    .OrderBy(o =>  o.Nome);
            
            return query.ToListAsync();
        }
    }
}
