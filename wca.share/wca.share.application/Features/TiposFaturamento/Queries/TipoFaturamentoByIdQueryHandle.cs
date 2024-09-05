using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TiposFaturamento.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TiposFaturamento.Queries
{
    public record TipoFaturamentoByIdQuery(int Id) : IRequest<ErrorOr<TipoFaturamentoResponse>>;
    internal sealed class TipoFaturamentoByIdQueryHandle : IRequestHandler<TipoFaturamentoByIdQuery, ErrorOr<TipoFaturamentoResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoFaturamentoByIdQueryHandle> _logger;

        public TipoFaturamentoByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoFaturamentoByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<TipoFaturamentoResponse>> Handle(TipoFaturamentoByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            TipoFaturamento? data = await _repository.GetDbSet<TipoFaturamento>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"TipoFaturamento não localizado!");
                return Error.NotFound(description: $"TipoFaturamento não localizado!");
            }

            return _mapper.Map<TipoFaturamentoResponse>(data);


        }
    }
}
