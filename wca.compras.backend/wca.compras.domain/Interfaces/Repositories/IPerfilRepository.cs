using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces.Repositories
{
    public interface IPerfilRepository: IRepository<Perfil>
    {
        void attachPermissao(Permissao permissao);
    }
}
