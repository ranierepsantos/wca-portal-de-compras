using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.application.Features.Solicitacoes.Behaviors;
using wca.reembolso.domain.Entities;
using wca.reembolso.application.Features.SolicitacaoHistoricos.Commands;
using wca.reembolso.application.Common;
using wca.reembolso.application.Features.Notificacoes.Commands;
using Microsoft.EntityFrameworkCore;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.application.Contracts.NorgeChatBot;
using wca.reembolso.application.Contracts;

namespace wca.reembolso.application.Features.Solicitacoes.Commands
{
    public record SolicitacaoCreateCommand(
        int ClienteId,
        DateTime DataSolicitacao,
        int ColaboradorId,
        int? CentroCustoId,
        string? ColaboradorCargo,
        string Projeto,
        string Objetivo,
        DateTime? PeriodoInicial,
        DateTime? PeriodoFinal,
        decimal? ValorAdiantamento,
        decimal? ValorDespesa,
        int Status,
        int TipoSolicitacao,
        IList<Despesa> Despesa, 
        int[] Notificar
    ) : IRequest<ErrorOr<SolicitacaoResponse>>;

    internal class SolicitacaoCreateCommandHandler : IRequestHandler<SolicitacaoCreateCommand, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoCreateCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IChatBotMessageHandle _chatbot;
        public SolicitacaoCreateCommandHandler(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoCreateCommandHandler> logger, IMediator mediator, IChatBotMessageHandle chatbot)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _chatbot = chatbot;
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

            //2.salvar as imagens das despesas
            for (int idx = 0; idx < request.Despesa.Count; idx++)
            {
                if (HandleFile.IsBase64(request.Despesa[idx].ImagePath))
                {
                    request.Despesa[idx].ImagePath = HandleFile.SaveFile(request.Despesa[idx].ImagePath);
                }
            }

            //3. mapear para cliente e adicionar
            Solicitacao dado = _mapper.Map<Solicitacao>(request);

            _repository.SolicitacaoRepository.Create(dado);

            await _repository.SaveAsync();

            //Criar evento
            var querie = new SolicitacaoHistorioCreateCommand(dado.Id, $"Solicitação criada!");
            await _mediator.Send(querie, cancellationToken);


            var status = await  _repository.StatusSolicitacaoRepository.ToQuery().FirstOrDefaultAsync(q => q.Id.Equals(dado.Status), cancellationToken: cancellationToken);

            // gerar notificação
            for (var ii = 0; ii < request.Notificar.Length; ii++)
            {
                string mensagem = status.TemplateNotificacao.Replace("{id}", dado.Id.ToString());

                var notificacao = new NotificacaoCreateCommand(request.Notificar[ii], mensagem, dado.GetType().Name, dado.Id);

                await _mediator.Send(notificacao, cancellationToken);
            }

            var findResult = await _mediator.Send(new SolicitacaoByIdQuerie(dado.Id), cancellationToken); 

            //chatbot - enviar mensagem para o colaborador
            if (!findResult.IsError)
                await _chatbot.SolicitacaoSendMessageAsync(Array.Empty<int>(), findResult.Value, cancellationToken);

            //3. mapear para SolicitacaoResponse
            return findResult;
        }
    }
}
