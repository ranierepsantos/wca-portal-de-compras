using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Behaviors;
using wca.reembolso.application.Features.Faturamentos.Common;
using wca.reembolso.application.Features.Notificacoes.Commands;
using wca.reembolso.application.Features.Solicitacoes.Commands;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Commands
{
    public record FaturamentoItemCreate(
        int SolicitacaoId
    );

    public record FaturamentoCreateCommand(
        int UsuarioId,
        int ClienteId,
        int CentroCustoId,
        decimal Valor,
        IList<FaturamentoItemCreate> FaturamentoItem,
        StatusSolicitacao Status, 
        int[] Notificar

    ): IRequest<ErrorOr<FaturamentoResponse>>;


    public class FaturamentoCreateCommandHandle : IRequestHandler<FaturamentoCreateCommand, ErrorOr<FaturamentoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoCreateCommandHandle> _logger;
        private readonly IMediator _mediator;
        public FaturamentoCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoCreateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<FaturamentoResponse>> Handle(FaturamentoCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando validação");
            //1. validar dados
            FaturamentoCreateCommandBehavior validator = new FaturamentoCreateCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                _logger.LogWarning("Não passou na validação" + errors.ToString());
                return errors;
            }

            var faturamento = _mapper.Map<Faturamento>(request);

            faturamento.Status = 1; 

            _repository.FaturamentoRepository.Create(faturamento);

            await _repository.SaveAsync();

            var historico = new FaturamentoHistoricoCreateCommand(faturamento.Id, "Faturamento Criado!");

            await _mediator.Send(historico, cancellationToken);

            // buscar o status - faturado
            var status = await _repository.StatusSolicitacaoRepository.ToQuery().Where(q => q.Id == 8).FirstAsync(cancellationToken: cancellationToken);

            // buscar o nome do usuario
            var usuario = await _repository.UsuarioRepository.ToQuery().Where(q => q.Id == faturamento.UsuarioId ).FirstAsync(cancellationToken: cancellationToken);

            string evento = $"Status alterado para {status.Status.ToUpper()} por {usuario.Nome}";

            foreach(var item in faturamento.FaturamentoItem)
            {
                var changeStatus = new SolicitacaoChangeStatusCommand(item.SolicitacaoId, evento, status, null);
                await _mediator.Send(changeStatus, cancellationToken);
            }

            //notificar usuario
            for (var ii = 0; ii < request.Notificar.Length; ii++)
            {
                string mensagem = request.Status.TemplateNotificacao.Replace("{id}", faturamento.Id.ToString());

                var notificacao = new NotificacaoCreateCommand(request.Notificar[ii], mensagem, faturamento.GetType().Name, faturamento.Id );

                await _mediator.Send(notificacao, cancellationToken);
            }

            return _mapper.Map<FaturamentoResponse>(faturamento);
        }
    }
}
