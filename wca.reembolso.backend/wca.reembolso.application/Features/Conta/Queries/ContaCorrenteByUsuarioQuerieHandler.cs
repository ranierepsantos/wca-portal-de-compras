using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Conta.Commands;
using wca.reembolso.application.Features.Conta.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Conta.Queries
{
    public record ContaCorrenteByUsuarioQuery(int UsuarioId):IRequest<ErrorOr<ContaCorrenteResponse>>;
    public class ContaCorrenteByUsuarioQuerieHandler : IRequestHandler<ContaCorrenteByUsuarioQuery, ErrorOr<ContaCorrenteResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContaCorrenteByUsuarioQuerieHandler> _logger;
        private readonly IMediator _mediator;

        public ContaCorrenteByUsuarioQuerieHandler(IRepositoryManager repository, IMapper mapper, ILogger<ContaCorrenteByUsuarioQuerieHandler> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<ContaCorrenteResponse>> Handle(ContaCorrenteByUsuarioQuery request, CancellationToken cancellationToken)
        {
            _logger.Log(logLevel: LogLevel.Information, "Buscando Conta corrente do usuário {0}", request.UsuarioId);

            var query = _repository.ContaCorrenteRepository.ToQuery();
            query = query.Where(q => q.UsuarioId.Equals(request.UsuarioId));
            query = query.Include(q => q.Transacoes);

            ContaCorrente? conta = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (conta is null) {
                _logger.LogInformation("Conta do usuario {0}, não localizado - criando nova conta", request.UsuarioId);
                ContaCorrenteCreateUpdateCommand command = new(
                    UsuarioId: request.UsuarioId,
                    Transacoes: new List<Transacao>()
                );
                return await _mediator.Send(command, cancellationToken);
            }

            return _mapper.Map<ContaCorrenteResponse>(conta);
        }
    }
}
