using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escalas.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escalas.Queries
{
    public record EscalaByIdQuery(int Id) : IRequest<ErrorOr<EscalaResponse>>;
    internal sealed class EscalaByIdQueryHandle : IRequestHandler<EscalaByIdQuery, ErrorOr<EscalaResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscalaByIdQueryHandle> _logger;

        public EscalaByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscalaByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<EscalaResponse>> Handle(EscalaByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Escala? data = await _repository.GetDbSet<Escala>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Escala não localizado!");
                return Error.NotFound(description: $"Escala não localizado!");
            }

            return _mapper.Map<EscalaResponse>(data);


        }
    }
}
