using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Common;

namespace wca.reembolso.application.Features.Faturamentos.Queries
{
    public record FaturamentoByIdQuery(int Id) : IRequest<ErrorOr<FaturamentoResponse>>;
    public class FaturamentoByIdQueryHandle : IRequestHandler<FaturamentoByIdQuery, ErrorOr<FaturamentoResponse>>
    {
        private readonly ILogger<FaturamentoByIdQueryHandle> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public FaturamentoByIdQueryHandle(ILogger<FaturamentoByIdQueryHandle> logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<FaturamentoResponse>> Handle(FaturamentoByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.FaturamentoRepository.ToQuery()
                .Where(q =>  q.Id == request.Id)
                .Include(n => n.Cliente)
                .Include(n => n.CentroCusto)
                .Include(n => n.FaturamentoHistorico.OrderByDescending(f => f.DataHora))
                .Include(n => n.FaturamentoItem)
                .ThenInclude(n => n.Solicitacao)
                .ThenInclude(n => n.Colaborador);
            
            var dado = await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (dado == null)
            {
                _logger.LogError($"Faturamento não localizado!");
                return Error.NotFound(description: $"Faturamento não localizado!");
            }

            return _mapper.Map<FaturamentoResponse>(dado);
        }
    }
}
