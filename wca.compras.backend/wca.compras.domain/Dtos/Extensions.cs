using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Entities;
using static wca.compras.domain.Dtos.PermissaoDtos;
using static wca.compras.domain.Dtos.PerfilDtos;

namespace wca.compras.domain.Dtos
{
    public static class Extensions
    {

        public static PerfilDto asDto(this Perfil perfil)
        {
            return new PerfilDto(
                perfil.Id,
                perfil.Nome,
                perfil.Descricao,
                perfil.Ativo
                );
        }

        public static PerfilPermissoesDto asDto (this PerfilPermissoes perfilPermissoes)
        {
            return new PerfilPermissoesDto(perfilPermissoes.Id,
                perfilPermissoes.Nome,
                perfilPermissoes.Descricao,
                perfilPermissoes.Ativo,
                perfilPermissoes.Permissoes.Select(p => p.asDtoToPerfil()).ToList()
                ); 
        }

        public static PermissaoDto asDto(this Permissao permissao)
        {
            return new PermissaoDto(
                permissao.Id,
                permissao.Nome,
                permissao.Regra,
                permissao.Descricao
            );
        }

        public static PermissoesDto asDtoToPerfil(this Permissao permissao)
        {
            return new PermissoesDto(
                permissao.Id,
                permissao.Nome
            );
        }
    }
}
