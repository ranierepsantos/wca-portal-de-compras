using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcionarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Queries
{
    public record FuncionarioListByClienteQuery (int ClienteId):IRequest<ErrorOr<IList<FuncionarioListItem>>>;
    internal class FuncionarioListByClienteQueryHandle : IRequestHandler<FuncionarioListByClienteQuery, ErrorOr<IList<FuncionarioListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FuncionarioListByClienteQueryHandle> _logger;
        private readonly IMapper _mapper;

        public FuncionarioListByClienteQueryHandle(IRepositoryManager repository, ILogger<FuncionarioListByClienteQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<FuncionarioListItem>>> Handle(FuncionarioListByClienteQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Funcionario - ListByCliente");

            var query = _repository.GetDbSet<Funcionario>()
                        .AsQueryable()
                        .AsNoTracking()
                        .Where(q =>  q.ClienteId.Equals(request.ClienteId))
                        .Include("CentroCusto")
                        .OrderBy(o => o.Nome);
            List<Funcionario> items = await query.ToListAsync();

            return _mapper.Map<List<FuncionarioListItem>>(items);
        }
    }
}
