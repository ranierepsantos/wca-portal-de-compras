using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Behaviors;
using wca.reembolso.application.Features.Clientes.Common;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.application.Features.Solicitacoes.Behaviors;
using wca.reembolso.domain.Entities;
using wca.reembolso.application.Features.SolicitacaoHistoricos.Commands;

namespace wca.reembolso.application.Features.Solicitacoes.Commands
{
    public record SolicitacaoCreateCommand(
        int ClienteId,
        DateTime DataSolicitacao,
        int ColaboradorId,
        int GestorId,
        string ColaboradorCargo,
        string Projeto,
        string Objetivo,
        DateTime? PeriodoInicial,
        DateTime? PeriodoFinal,
        decimal ValorAdiantamento,
        decimal ValorDespesa,
        int Status,
        IList<Despesa> Despesa
    ) : IRequest<ErrorOr<SolicitacaoResponse>>;

    public class SolicitacaoCreateCommandHandler : IRequestHandler<SolicitacaoCreateCommand, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IRepository<Solicitacao> _reposistory;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoCreateCommandHandler> _logger;
        private readonly IMediator _mediator;

        public SolicitacaoCreateCommandHandler(IRepository<Solicitacao> reposistory, IMapper mapper, ILogger<SolicitacaoCreateCommandHandler> logger, IMediator mediator)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        async Task<ErrorOr<SolicitacaoResponse>> IRequestHandler<SolicitacaoCreateCommand, ErrorOr<SolicitacaoResponse>>.Handle(SolicitacaoCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SolicitacaoCreateCommandHandler - validation");
            //1. validar dados
            SolicitacaoCreateCommandBehavior validator = new SolicitacaoCreateCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            //2. mapear para cliente e adicionar
            Solicitacao dado = _mapper.Map<Solicitacao>(request);

            _reposistory.Create(dado);

            await _reposistory.SaveChangesAsync();

            //Criar evento
            var querie = new SolicitacaoHistorioCreateCommand(dado.Id, $"Solicitação criada!");
            await _mediator.Send ( querie );

            //3. mapear para SolicitacaoResponse
            return _mapper.Map<SolicitacaoResponse>(dado);
        }
    }
}
