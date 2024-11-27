using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Solicitacoes.Common;
using wca.share.application.Features.Solicitacoes.Common.Data.Extensions;

namespace wca.share.application.Features.Solicitacoes.Queries
{
    public record SolicitacaoByIdQuerie(int Id) : IRequest<ErrorOr<SolicitacaoResponse>>;
    internal class SolicitacaoByIdQueryHandler : IRequestHandler<SolicitacaoByIdQuerie, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoByIdQueryHandler> _logger;

        public SolicitacaoByIdQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<SolicitacaoResponse>> Handle(SolicitacaoByIdQuerie request, CancellationToken cancellationToken)
        {

            var dado = await _repository.SolicitacaoRepository.ToQuery().IncludeAll()
                .Where(q => q.Id.Equals(request.Id))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken: cancellationToken); 


            if (dado == null)
            {
                _logger.LogError($"Solicitacao #{request.Id} não localizada!");
                return Error.NotFound(description: $"Não localizamos a solicitação #{request.Id}!");
            }

            return _mapper.Map<SolicitacaoResponse>(dado);

        }
    }
}
