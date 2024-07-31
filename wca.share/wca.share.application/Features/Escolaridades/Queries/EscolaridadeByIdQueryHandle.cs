using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escolaridades.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escolaridades.Queries
{
    public record EscolaridadeByIdQuery(int Id) : IRequest<ErrorOr<EscolaridadeResponse>>;
    internal sealed class EscolaridadeByIdQueryHandle : IRequestHandler<EscolaridadeByIdQuery, ErrorOr<EscolaridadeResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscolaridadeByIdQueryHandle> _logger;

        public EscolaridadeByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscolaridadeByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<EscolaridadeResponse>> Handle(EscolaridadeByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Escolaridade? data = await _repository.GetDbSet<Escolaridade>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Escolaridade não localizado!");
                return Error.NotFound(description: $"Escolaridade não localizado!");
            }

            return _mapper.Map<EscolaridadeResponse>(data);


        }
    }
}
