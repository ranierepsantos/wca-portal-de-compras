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
        Task<UsuarioDto> Create(int sistemaId, CreateUsuarioDto usuario);
        public Task<UsuarioDto> Update(int sistemaId,  UpdateUsuarioDto usuario);
        public Task<bool> Remove( int id, int sistemaId = 0);
        public Task<UsuarioDto> GetById( int id, int sistemaId);
        public Pagination<UsuarioDto> Paginate( int sistemaId, int page = 1, int pageSize = 10, string termo = "", int[]? filial= null);
        public Task<IList<ListItem>> GetToList( int sistemaId, int[]? filial);
        
        Task<IList<ListItem>> GetToListByPerfil(int perfilId);
        
    }
}
