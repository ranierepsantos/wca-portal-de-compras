using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcoes.Queries
{
    public record FuncaoByIdQuery(int Id) : IRequest<ErrorOr<FuncaoResponse>>;
    internal sealed class FuncaoByIdQueryHandle : IRequestHandler<FuncaoByIdQuery, ErrorOr<FuncaoResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncaoByIdQueryHandle> _logger;

        public FuncaoByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncaoByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<FuncaoResponse>> Handle(FuncaoByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            Funcao? data = await _repository.GetDbSet<Funcao>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"Funcao não localizado!");
                return Error.NotFound(description: $"Funcao não localizado!");
            }

            return _mapper.Map<FuncaoResponse>(data);


        }
    }
}
