using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.VagaHistoricos.Commands;
using wca.share.domain.Entities;
using wca.share.application.Features.Vagas.Queries;

namespace wca.share.application.Features.Vagas.Commands
{
    public record VagaChangeStatusCommand(
    int VagaId,
    string Evento,
    StatusSolicitacao Status,
    int[]? Notificar
) : IRequest<ErrorOr<bool>>;

    public sealed class VagaChangeStatusCommandHandle : IRequestHandler<VagaChangeStatusCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VagaChangeStatusCommandHandle> _logger;
        private readonly IMediator _mediator;
        private readonly INotificacaoHandle _nofiticacaoHandle;
        public VagaChangeStatusCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<VagaChangeStatusCommandHandle> logger, IMediator mediator, INotificacaoHandle nofiticacaoHandle)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _nofiticacaoHandle = nofiticacaoHandle;
        }

        public async Task<ErrorOr<bool>> Handle(VagaChangeStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var querie = new VagaFindByIdQuery(request.VagaId);

                var findResult = await _mediator.Send(querie, cancellationToken);

                if (findResult.IsError) { return findResult.FirstError; }

                // atualizar o status
                var dado = _mapper.Map<Vaga>(findResult.Value);

                dado.StatusSolicitacaoId = request.Status.Id;

                _repository.GetDbSet<Vaga>().Entry(dado).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                // cadastrar evento
                var eventoCommand = new VagaHistorioCreateCommand(dado.Id, request.Evento);
                await _mediator.Send(eventoCommand);

                // gerar notificação
                if (request.Status?.TemplateNotificacao is not null && request.Notificar?.Length > 0)
                    await _nofiticacaoHandle.VagaEnviarNotificacaoAsync(request.Notificar, request.Status.TemplateNotificacao, dado, cancellationToken);

                // return 
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error.Request: {JsonSerializer.Serialize(request)}");
                _logger.LogError($"Error.Message: {ex.Message}");
                _logger.LogError($"Error.InnerException: {ex.InnerException?.Message}");
                return Error.Failure(ex.Message);
            }
        }
    }
}
