using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Dtos;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class RecorrenciaService : IRecorrenciaService
    {
        public Task<RecorrenciaDto> Create(CreateRecorrenciaDto createRecorrenciaDto, string urlOrigin)
        {
            throw new NotImplementedException();
        }

        public Task<RecorrenciaDto> GetById(int filialId, int id)
        {
            throw new NotImplementedException();
        }

        public Pagination<RecorrenciaDto> Paginate(int filialId, int page = 1, int pageSize = 10, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(int filialId, int id)
        {
            throw new NotImplementedException();
        }

        public Task<RequisicaoDto> Update(int filialId, UpdateRecorrenciaDto updateRecorrenciaDto)
        {
            throw new NotImplementedException();
        }
    }
}
