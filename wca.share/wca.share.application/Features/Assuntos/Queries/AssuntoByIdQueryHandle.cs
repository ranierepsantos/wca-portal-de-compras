using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Assuntos.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Assuntos.Queries
{
    public record AssuntoByIdQuery(int Id) : IRequest<ErrorOr<AssuntoResponse>>;
    internal sealed class AssuntoByIdQueryHandle : IRequestHandler<AssuntoByIdQuery, ErrorOr<AssuntoResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssuntoByIdQueryHandle> _logger;

        public AssuntoByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<AssuntoByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<AssuntoResponse>> Handle(AssuntoByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Assunto? data = await _repository.GetDbSet<Assunto>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Assunto não localizado!");
                return Error.NotFound(description: $"Assunto não localizado!");
            }

            return _mapper.Map<AssuntoResponse>(data);
                

        }
    }
}
