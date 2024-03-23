using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Notificacoes.Commands;
using wca.share.application.Features.SolicitacaoHistoricos.Commands;
using wca.share.application.Features.Solicitacoes.Common;
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

        public SolicitacaoChangeStatusCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoChangeStatusCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
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
            for (var ii = 0; ii < request.Notificar?.Length; ii++)
            {
                if (request.Status.TemplateNotificacao is not null)
                {
                    string mensagem = request.Status.TemplateNotificacao.Replace("{TipoSolicitacao}", GetDescricaoTipoSolicitacao(dado.SolicitacaoTipoId)).Replace("{id}", dado.Id.ToString());

                    var notificacao = new NotificacaoCreateCommand(request.Notificar[ii], mensagem, GetEntidadeTipoSolicitacao(dado.SolicitacaoTipoId), dado.Id);

                    await _mediator.Send(notificacao, cancellationToken);
                }
            }

            // return 
            return true;
        }

        private string GetDescricaoTipoSolicitacao(int Tipo)
        {
            return Tipo switch
            {
                (int)EnumTipoSolicitacao.Comunicado => "Comunicado",
                (int)EnumTipoSolicitacao.Desligamento => "Desligamento",
                (int)EnumTipoSolicitacao.Ferias => "Férias",
                (int)EnumTipoSolicitacao.MudancaBase => "Mudança de Base",
            };
        }

        private string GetEntidadeTipoSolicitacao(int Tipo)
        {
            return Tipo switch
            {
                (int)EnumTipoSolicitacao.Comunicado => EnumTipoSolicitacao.Comunicado.ToString(),
                (int)EnumTipoSolicitacao.Desligamento => EnumTipoSolicitacao.Desligamento.ToString(),
                (int)EnumTipoSolicitacao.Ferias => EnumTipoSolicitacao.Ferias.ToString(),
                (int)EnumTipoSolicitacao.MudancaBase => EnumTipoSolicitacao.MudancaBase.ToString(),
            };
        }
    }
}
