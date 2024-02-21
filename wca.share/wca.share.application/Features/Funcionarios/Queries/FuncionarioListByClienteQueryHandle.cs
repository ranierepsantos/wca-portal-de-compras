using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Solicitacoes.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Queries
{
    public record FuncionarioListByClienteQuery (int ClienteId):IRequest<ErrorOr<IList<ListItem>>>;
    internal class FuncionarioListByClienteQueryHandle : IRequestHandler<FuncionarioListByClienteQuery, ErrorOr<IList<ListItem>>>
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

        public async Task<ErrorOr<IList<ListItem>>> Handle(FuncionarioListByClienteQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Funcionario - ListByCliente");

            var query = _repository.GetDbSet<Funcionario>()
                        .AsQueryable()
                        .AsNoTracking()
                        .Where(q =>  q.ClienteId.Equals(request.ClienteId))
                        .OrderBy(o => o.Nome);
            List<Funcionario> items = await query.ToListAsync();

            return _mapper.Map<List<ListItem>>(items);
        }
    }
}
