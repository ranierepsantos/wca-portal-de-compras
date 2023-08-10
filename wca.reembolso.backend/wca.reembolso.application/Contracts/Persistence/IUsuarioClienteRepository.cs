using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Contracts.Persistence
{
    public interface IUsuarioClienteRepository: IRepository<UsuarioClientes>
    {
    }
}
