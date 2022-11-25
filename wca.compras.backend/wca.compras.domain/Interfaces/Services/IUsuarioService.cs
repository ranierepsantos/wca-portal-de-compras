using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Dtos;
using wca.compras.domain.Util;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        public Task<UsuarioDto> Create(CreateUsuarioDto usuario);
        public Task<UsuarioDto> Update(UpdateUsuarioDto usuario);
        public Task<bool> Remove(int id);
        public Task<IList<UsuarioDto>> GetAll();
        public Task<UsuarioDto> GetById(int id);
        public Task<Pagination<UsuarioDto>> Paginate(int page = 1, int pageSize = 10, string termo = "");
    }
}
