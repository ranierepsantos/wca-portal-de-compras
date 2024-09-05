using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TipoContratos.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TipoContratos.Queries
{
    public record TipoContratoByIdQuery(int Id) : IRequest<ErrorOr<TipoContratoResponse>>;
    internal sealed class TipoContratoByIdQueryHandle : IRequestHandler<TipoContratoByIdQuery, ErrorOr<TipoContratoResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoContratoByIdQueryHandle> _logger;

        public TipoContratoByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoContratoByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<TipoContratoResponse>> Handle(TipoContratoByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            TipoContrato? data = await _repository.GetDbSet<TipoContrato>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"TipoContrato não localizado!");
                return Error.NotFound(description: $"TipoContrato não localizado!");
            }

            return _mapper.Map<TipoContratoResponse>(data);


        }
    }
}
