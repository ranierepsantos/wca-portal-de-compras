using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.SolicitacaoHistoricos.Commands;
using wca.share.application.Features.Solicitacoes.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Commands
{
    public record SolicitacaoChangeStatusCommand(
    int SolicitacaoId,
    string Evento,
    StatusSolicitacao Status,
    int[]? Notificar
) : IRequest<ErrorOr<bool>>;

    public sealed class SolicitacaoChangeStatusCommandHandle : IRequestHandler<SolicitacaoChangeStatusCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoChangeStatusCommandHandle> _logger;
        private readonly IMediator _mediator;
        private readonly INotificacaoHandle _nofiticacaoHandle;
        public SolicitacaoChangeStatusCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoChangeStatusCommandHandle> logger, IMediator mediator, INotificacaoHandle nofiticacaoHandle)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _nofiticacaoHandle = nofiticacaoHandle;
        }

        public async Task<ErrorOr<bool>> Handle(SolicitacaoChangeStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Solicitação - alteração de status, request {JsonSerializer.Serialize(request)}");

            var querie = new SolicitacaoByIdQuerie(request.SolicitacaoId);

            var findResult = await _mediator.Send(querie, cancellationToken);

            if (findResult.IsError) { return findResult.FirstError; }

            // atualizar o status
            var dado = _mapper.Map<Solicitacao>(findResult.Value);

            dado.StatusSolicitacaoId = request.Status.Id;

            _repository.GetDbSet<Solicitacao>().Entry(dado).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            await _repository.SaveAsync();

            // cadastrar evento
            var eventoCommand = new SolicitacaoHistorioCreateCommand(dado.Id, request.Evento);
            await _mediator.Send(eventoCommand);

            // gerar notificação
            if (request.Status?.TemplateNotificacao is not null && request.Notificar?.Length > 0)
                await _nofiticacaoHandle.SolicitacaoEnviarNotificacaoAsync(request.Notificar, request.Status.TemplateNotificacao, dado, cancellationToken);

            // return 
            return true;
        }
    }
}
