using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Behaviors;
using wca.reembolso.application.Features.Clientes.Common;
using wca.reembolso.application.Features.Clientes.Queries;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Clientes.Commands
{
    public record ClienteUpdateCommand(
        int id,
        int FilialId,
        string Nome,
        string CNPJ,
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        decimal ValorLimite
    ) : IRequest<ErrorOr<ClienteResponse>>;

    public class ClienteUpdateCommandHandler : IRequestHandler<ClienteUpdateCommand, ErrorOr<ClienteResponse>>
    {
        private readonly IRepositoryManager _reposistory;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private ILogger<ClienteUpdateCommandHandler> _logger;

        public ClienteUpdateCommandHandler(IMediator mediator, IRepositoryManager reposistory, IMapper mapper, ILogger<ClienteUpdateCommandHandler> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        async Task<ErrorOr<ClienteResponse>> IRequestHandler<ClienteUpdateCommand, ErrorOr<ClienteResponse>>.Handle(ClienteUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateClienteCommandHandler - validation");
            //1. validar dados
            UpdateClienteCommandBehavior validator = new UpdateClienteCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }
            //1. localizar cliente
            ClienteByIdQuerie querie = new ClienteByIdQuerie(request.id);
            
            var findResult = await _mediator.Send(querie);

            if (findResult.IsError) return findResult;

            //2. mapear para cliente e adicionar
            Cliente cliente = _mapper.Map<Cliente>(request);

            _reposistory.ClienteRepository.Update(cliente);

            await _reposistory.SaveAsync();

            //3. mapear para clienteresponse
            return _mapper.Map<ClienteResponse>(cliente);
        }
    }
}
