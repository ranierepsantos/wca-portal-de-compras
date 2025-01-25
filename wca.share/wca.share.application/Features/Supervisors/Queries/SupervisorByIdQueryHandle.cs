using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Supervisors.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Supervisors.Queries
{
    public record SupervisorByIdQuery(int Id) : IRequest<ErrorOr<SupervisorResponse>>;
    internal sealed class SupervisorByIdQueryHandle : IRequestHandler<SupervisorByIdQuery, ErrorOr<SupervisorResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupervisorByIdQueryHandle> _logger;

        public SupervisorByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<SupervisorByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<SupervisorResponse>> Handle(SupervisorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Supervisor? data = await _repository.GetDbSet<Supervisor>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Supervisor não localizado!");
                return Error.NotFound(description: $"Supervisor não localizado!");
            }

            return _mapper.Map<SupervisorResponse>(data);


        }
    }
}
