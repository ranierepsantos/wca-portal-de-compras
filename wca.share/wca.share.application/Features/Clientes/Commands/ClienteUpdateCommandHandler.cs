using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Clientes.Behaviors;
using wca.share.application.Features.Clientes.Common;
using wca.share.application.Features.Clientes.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Clientes.Commands
{
    public record ClienteUpdateCommand(
        int id,
        int CodigoCliente,
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
        IList<CentroCusto> CentroCusto
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
            _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
            //1. validar dados
            UpdateClienteCommandBehavior validator = new UpdateClienteCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }
            //1. localizar cliente
            ClienteByIdQuerie querie = new(request.id);

            var findResult = await _mediator.Send(querie, cancellationToken);

            if (findResult.IsError) return findResult;

            var dado = findResult.Value;
            //3. remover centro de custo que foi excluído!
            List<CentroCusto> centroCustoRemover = dado.CentroCusto
                .Where(x => !request.CentroCusto.Where(q => q.Id == x.Id).Any())
                .Where(x => x.Id != 0)
                .ToList();

            foreach (var item in centroCustoRemover)
            {
                var centro = _reposistory.GetDbSet<CentroCusto>()
                            .Where(q => q.Id.Equals(item.Id)
                                     && q.ClienteId.Equals(item.ClienteId))
                            .FirstOrDefault();
                if (centro != null)
                {
                    _reposistory.GetDbSet<CentroCusto>().Remove(centro);
                }
            }

            //2. mapear para cliente e adicionar
            Cliente cliente = _mapper.Map<Cliente>(request);

            _reposistory.ClienteRepository.Update(cliente);

            await _reposistory.SaveAsync();

            //3. mapear para clienteresponse
            return _mapper.Map<ClienteResponse>(cliente);
        }
    }
}
