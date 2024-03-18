using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Conta.Common;
using wca.reembolso.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace wca.reembolso.application.Features.Conta.Queries
{
    public record ContaCorrenteToPaginateQuery(string UsuarioNome = "",int[]? ClienteIds = null, int[]? CentroCustoIds = null) : PaginationQuery, IRequest<ErrorOr<Pagination<ContaCorrenteResponse>>>;
    public class ContaCorrenteToPaginateHandle : IRequestHandler<ContaCorrenteToPaginateQuery, ErrorOr<Pagination<ContaCorrenteResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContaCorrenteToPaginateHandle> _logger;

        public ContaCorrenteToPaginateHandle(IRepositoryManager repository, IMapper mapper, ILogger<ContaCorrenteToPaginateHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<ContaCorrenteResponse>>> Handle(ContaCorrenteToPaginateQuery request, CancellationToken cancellationToken)
        {
            _logger.Log(logLevel: LogLevel.Information, "Buscando Dados de Conta corrente");

            var query = _repository.GetDbSet<ContaCorrente>()
                .Include(inc => inc.Transacoes)
                .Include(inc => inc.Usuario)
                .ThenInclude(inc => inc.UsuarioClientes).AsQueryable();

            if (request.FilialId > 0)
            {
                query = query.Where(q => q.Usuario.UsuarioClientes
                .Where(x => x.Cliente.FilialId == request.FilialId).Count() > 1);
            }


            if (request.ClienteIds?.Count() > 0)
            {
                query = query.Where(
                        q => q.Usuario.UsuarioClientes.Where(q => request.ClienteIds.Contains(q.ClienteId)).Count() > 0
                    );
            }
            
            if (request.CentroCustoIds?.Count() > 0)
            {
                query = query.Where(
                        q => q.Usuario.UsuarioCentrodeCustos.Where(q => request.CentroCustoIds.Contains(q.CentroCustoId)).Count() > 0
                    );
            }

            if (!string.IsNullOrEmpty(request.UsuarioNome))
                query = query.Where(q => q.Usuario.Nome.Contains(request.UsuarioNome));


            query = query.OrderBy(c => c.Usuario.Nome);


            var pagination = Pagination<ContaCorrenteResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);
        }
    }
}
