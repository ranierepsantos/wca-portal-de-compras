using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Conta.Common;
using wca.reembolso.application.Features.Conta.Queries;
using wca.reembolso.application.Features.Solicitacoes.Behaviors;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Conta.Commands
{
    public record ContaCorrenteCreateUpdateCommand (
        int UsuarioId,
        IList<Transacao> Transacoes

    ): IRequest<ErrorOr<ContaCorrenteResponse>>;
    public class CreateUpdateContaCorrenteCommandHandler : IRequestHandler<ContaCorrenteCreateUpdateCommand, ErrorOr<ContaCorrenteResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<CreateUpdateContaCorrenteCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateUpdateContaCorrenteCommandHandler(IRepositoryManager repository, ILogger<CreateUpdateContaCorrenteCommandHandler> logger, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        async Task<ErrorOr<ContaCorrenteResponse>> IRequestHandler<ContaCorrenteCreateUpdateCommand, ErrorOr<ContaCorrenteResponse>>.Handle(ContaCorrenteCreateUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Criando conta corrente");

            ContaCorrenteCreateUpdateCommandBehavior validator = new();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            var findResult = await _mediator.Send(new ContaCorrenteByUsuarioQuery(request.UsuarioId), cancellationToken);
            ContaCorrente conta = new();
            if (findResult.IsError)
                conta = _mapper.Map<ContaCorrente>(request);
            else
                conta = _mapper.Map<ContaCorrente>(findResult.Value);

            foreach (var transacao in request.Transacoes)
            {
                if (!findResult.IsError)  conta.Transacoes.Add(transacao);
                
                if (transacao.Operador == "+")
                    conta.Saldo += transacao.Valor;
                else
                    conta.Saldo -= transacao.Valor;
            }

            if (findResult.IsError)
                _repository.ContaCorrenteRepository.Create(conta);
            else
                _repository.ContaCorrenteRepository.Update(conta);

            await _repository.SaveAsync();   
                
            return _mapper.Map<ContaCorrenteResponse>(conta);
        }
    }
}
