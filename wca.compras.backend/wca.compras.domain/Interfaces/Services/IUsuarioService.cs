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
        Task<UsuarioDto> Update(int sistemaId,  UpdateUsuarioDto usuario);
        Task<bool> Remove( int id, int sistemaId = 0);
        Task<UsuarioDto> GetById( int id, int sistemaId);
        Pagination<UsuarioDto> Paginate( int sistemaId, int page = 1, int pageSize = 10, string termo = "", int[]? filial= null);
        Task<IList<ListItem>> GetToList( int sistemaId, int[]? filial);
      
        Task<IList<ListItem>> GetToListByPerfil(int perfilId);
        Task<UsuarioDto> GetByEmail(string email);
        Task<IList<ListItem>> GetToListByPermissao(int sistemaId, string permissao);


    }
}
