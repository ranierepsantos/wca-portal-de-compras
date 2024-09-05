using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.MotivosContratacao.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.MotivosContratacao.Queries
{
    public record MotivoContratacaoByIdQuery(int Id) : IRequest<ErrorOr<MotivoContratacaoResponse>>;
    internal sealed class MotivoContratacaoByIdQueryHandle : IRequestHandler<MotivoContratacaoByIdQuery, ErrorOr<MotivoContratacaoResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<MotivoContratacaoByIdQueryHandle> _logger;

        public MotivoContratacaoByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<MotivoContratacaoByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<MotivoContratacaoResponse>> Handle(MotivoContratacaoByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            MotivoContratacao? data = await _repository.GetDbSet<MotivoContratacao>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"MotivoContratacao não localizado!");
                return Error.NotFound(description: $"MotivoContratacao não localizado!");
            }

            return _mapper.Map<MotivoContratacaoResponse>(data);


        }
    }
}
