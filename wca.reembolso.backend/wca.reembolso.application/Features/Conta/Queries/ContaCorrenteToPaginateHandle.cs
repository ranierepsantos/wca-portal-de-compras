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
    public record ContaCorrenteToPaginateQuery( int UsuarioId, string UsuarioNome = "") : PaginationQuery, IRequest<ErrorOr<Pagination<ContaCorrenteResponse>>>;
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
            try
            {
                _logger.Log(logLevel: LogLevel.Information, "Buscando Dados de Conta corrente");

                string sqlQuery = @$"select distinct c.usuario_id, c.saldo from ContaCorrente c
                    inner join UsuarioClientes uc on uc.usuario_id  = c.usuario_id
                    inner join UsuarioCentrodeCustos ucc on ucc.UsuarioId = c.usuario_id
                    inner join UsuarioClientes cc on cc.usuario_id = {request.UsuarioId} and uc.cliente_id= cc.cliente_id
                    inner join UsuarioCentrodeCustos ccc on ccc.UsuarioId = {request.UsuarioId} and ccc.CentroCustoId=ucc.CentroCustoId
                ";


                var query = _repository.FromQuery<ContaCorrente>(sqlQuery)
                    .Include(inc => inc.Transacoes)
                    .Include(inc => inc.Usuario).AsQueryable();

                if (!string.IsNullOrEmpty(request.UsuarioNome))
                    query = query.Where(q => q.Usuario.Nome.Contains(request.UsuarioNome));

                query = query.OrderBy(c => c.Usuario.Nome);


                var pagination = Pagination<ContaCorrenteResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

                return await Task.FromResult(pagination);    
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            
        }
    }
}
