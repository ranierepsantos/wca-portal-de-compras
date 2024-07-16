using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Gestors.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Gestors.Queries
{
    public record GestorByIdQuery(int Id) : IRequest<ErrorOr<GestorResponse>>;
    internal sealed class GestorByIdQueryHandle : IRequestHandler<GestorByIdQuery, ErrorOr<GestorResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GestorByIdQueryHandle> _logger;

        public GestorByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<GestorByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<GestorResponse>> Handle(GestorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Gestor? data = await _repository.GetDbSet<Gestor>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Gestor não localizado!");
                return Error.NotFound(description: $"Gestor não localizado!");
            }

            return _mapper.Map<GestorResponse>(data);


        }
    }
}
