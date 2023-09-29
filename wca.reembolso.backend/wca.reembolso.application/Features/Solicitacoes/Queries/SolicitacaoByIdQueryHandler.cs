using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Common;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacaos.Queries
{
    public record SolicitacaoByIdQuerie(int Id): IRequest<ErrorOr<SolicitacaoResponse>>;
    public class SolicitacaoByIdQueryHandler : IRequestHandler<SolicitacaoByIdQuerie, ErrorOr<SolicitacaoResponse>>
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

            var dado = await _repository.SolicitacaoRepository.ToQuery()
                .Include("Cliente")
                .Include("Despesa")
                .Include(q =>  q.Colaborador)
                .Include(q => q.Gestor)
                .Include(q => q.SolicitacaoHistorico.OrderByDescending(f => f.DataHora))
                .Where(q => q.Id.Equals(request.Id)).AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (dado == null)
            {
                _logger.LogError($"Solicitacao não localizado!");
                return Error.NotFound(description: $"Solicitacao não localizado!");
            }

            return _mapper.Map<SolicitacaoResponse>(dado);
            
        }
    }
}
