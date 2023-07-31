using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.infrastruture.Context
{
    public class WcaReembolsoContext : DbContext
    {
        public WcaReembolsoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
