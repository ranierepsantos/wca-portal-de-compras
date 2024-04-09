using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
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


            var query = _repository.ContaCorrenteRepository.ToQuery();
            query = query.Where(q => q.UsuarioId.Equals(request.UsuarioId));
            query = query.Include(q => q.Transacoes);

            ContaCorrente? conta = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            bool hasConta = false;
            if (conta is null)
                conta = _mapper.Map<ContaCorrente>(request);
            else
                hasConta = true;

            foreach (var transacao in request.Transacoes)
            {
                transacao.SaldoAnterior = conta.Saldo;
                
                if (transacao.Operador == "+")
                    conta.Saldo += transacao.Valor;
                else
                    conta.Saldo -= transacao.Valor;
                
                transacao.Saldo = conta.Saldo;

                if (hasConta)
                {
                    transacao.ContaCorrenteUsuarioId = conta.UsuarioId;
                    _repository.GetDbSet<Transacao>().Add(transacao);
                }
                
            }

            if (hasConta)
                _repository.GetDbSet<ContaCorrente>().Entry(conta).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            else
                _repository.ContaCorrenteRepository.Create(conta);


            await _repository.SaveAsync();   
                
            return _mapper.Map<ContaCorrenteResponse>(conta);
        }
    }
}
