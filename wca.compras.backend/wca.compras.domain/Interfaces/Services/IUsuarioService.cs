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
        public Task<UsuarioDto> Update(int sistemaId, int filialId, UpdateUsuarioDto usuario);
        public Task<bool> Remove(int filialId, int id, int sistemaId = 0);
        public Task<UsuarioDto> GetById(int filialId, int id, int sistemaId);
        public Pagination<UsuarioDto> Paginate(int filialId, int sistemaId, int page = 1, int pageSize = 10, string termo = "");
        public Task<IList<ListItem>> GetToList(int filialId, int sistemaId);
        
        Task<IList<ListItem>> GetToListByPerfil(int perfilId);
    }
}
