using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Dtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        public Task<UsuarioDto> Create(CreateUsuarioDto usuario);
        public Task<UsuarioDto> Update(UpdateUsuarioDto usuario);
        public Task<bool> Remove(string id);
        public Task<IList<UsuarioDto>> GetAll();
        public Task<UsuarioDto> GetById(string id);
    }
}
