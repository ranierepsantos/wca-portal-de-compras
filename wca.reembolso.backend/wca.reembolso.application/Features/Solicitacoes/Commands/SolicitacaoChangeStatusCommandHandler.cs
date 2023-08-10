using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.SolicitacaoHistoricos.Commands;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Commands
{

    public record SolicitacaoChangeStatusCommand(
        int SolicitacaoId,
        string Evento,
        StatusSolicitacao Status
    ):IRequest<ErrorOr<bool>>;

    public sealed class SolicitacaoChangeStatusCommandHandler : IRequestHandler<SolicitacaoChangeStatusCommand, ErrorOr<bool>>
    {
        private readonly IRepository<Solicitacao> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoChangeStatusCommandHandler> _logger;
        private readonly IMediator _mediator;

        public SolicitacaoChangeStatusCommandHandler(IRepository<Solicitacao> repository, IMapper mapper, ILogger<SolicitacaoChangeStatusCommandHandler> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(SolicitacaoChangeStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Solicitação - alteração de status");

            var querie = new SolicitacaoByIdQuerie(request.SolicitacaoId);

            var findResult = await _mediator.Send(querie, cancellationToken);

            if (findResult.IsError) { return findResult.FirstError; }

            // atualizar o status
            var dado = _mapper.Map<Solicitacao>(findResult.Value);
            
            dado.Status = request.Status.Id;

            _repository.Update(dado);
            
            await _repository.SaveChangesAsync();

            // cadastrar evento
            var eventoCommand = new SolicitacaoHistorioCreateCommand(dado.Id, request.Evento);

            var result = await _mediator.Send(eventoCommand);
            
            if (result.IsError) { return result.FirstError; }

            // lancar notificação





            // return 
            return true;
        }
    }
}
