using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Horarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Horarios.Queries
{
    public record HorarioByIdQuery(int Id) : IRequest<ErrorOr<HorarioResponse>>;
    internal sealed class HorarioByIdQueryHandle : IRequestHandler<HorarioByIdQuery, ErrorOr<HorarioResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<HorarioByIdQueryHandle> _logger;

        public HorarioByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<HorarioByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<HorarioResponse>> Handle(HorarioByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Horario? data = await _repository.GetDbSet<Horario>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Horario não localizado!");
                return Error.NotFound(description: $"Horario não localizado!");
            }

            return _mapper.Map<HorarioResponse>(data);


        }
    }
}
