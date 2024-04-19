using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Notificacoes.Commands;
using wca.reembolso.application.Features.SolicitacaoHistoricos.Commands;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Commands
{

    public record SolicitacaoChangeStatusCommand(
        int SolicitacaoId,
        string Evento,
        StatusSolicitacao Status,
        int[]? Notificar
    ):IRequest<ErrorOr<bool>>;

    internal sealed class SolicitacaoChangeStatusCommandHandler : IRequestHandler<SolicitacaoChangeStatusCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoChangeStatusCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IChatBotMessageHandle _chatbot;

        public SolicitacaoChangeStatusCommandHandler(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoChangeStatusCommandHandler> logger, IMediator mediator, IChatBotMessageHandle chatbot)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _chatbot = chatbot;
        }

        public async Task<ErrorOr<bool>> Handle(SolicitacaoChangeStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Solicitação - alteração de status");

            var querie = new SolicitacaoByIdQuerie(request.SolicitacaoId);

            var findResult = await _mediator.Send(querie, cancellationToken);

            if (findResult.IsError) { return findResult.FirstError; }

            // atualizar o status
            var dado = _mapper.Map<Solicitacao>(findResult.Value);

            //armazenar o status anterior
            dado.StatusAnterior = dado.Status;

            dado.Status = request.Status.Id;
            dado.DataStatus = DateTime.Now;

            _repository.SolicitacaoRepository.Update(dado);
            
            await _repository.SaveAsync();

            // cadastrar evento
            var eventoCommand = new SolicitacaoHistorioCreateCommand(dado.Id, request.Evento);
            await _mediator.Send(eventoCommand);

            // gerar notificação
            for (var ii =0; ii < request.Notificar?.Length; ii++)
            {
                string mensagem = request.Status.TemplateNotificacao.Replace("{id}", request.SolicitacaoId.ToString());

                var notificacao = new NotificacaoCreateCommand(request.Notificar[ii], mensagem,dado.GetType().Name, dado.Id);

                await _mediator.Send(notificacao, cancellationToken);
            }

            findResult = await _mediator.Send(querie, cancellationToken);

            if (!findResult.IsError)
                await _chatbot.SolicitacaoSendMessageAsync(request.Notificar, findResult.Value, cancellationToken);

            // return 
            return true;
        }
    }
}
