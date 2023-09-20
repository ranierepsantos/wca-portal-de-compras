using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Queries;
using wca.reembolso.domain.Common.Enum;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Commands
{
    public sealed record FaturamentoChangeStatusCommand(
        int Id, 
        string Evento,
        StatusFaturamento status
    ):IRequest<ErrorOr<bool>>;

    public sealed class FaturamentoChangeStatusCommandHandle : IRequestHandler<FaturamentoChangeStatusCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoChangeStatusCommandHandle> _logger;
        private readonly IMediator _mediator;

        public FaturamentoChangeStatusCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoChangeStatusCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(FaturamentoChangeStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando alteração de status");

            var queryById = new FaturamentoByIdQuery(request.Id);

            var findResutl = await _mediator.Send(queryById, cancellationToken);

            if (findResutl.IsError)
            {
                return findResutl.FirstError;
            }

            Faturamento faturamento = _mapper.Map<Faturamento>(findResutl.Value);

            faturamento.Status = request.status.Id;

            _repository.FaturamentoRepository.Update(faturamento);

            await _repository.SaveAsync();

            // gerar evento
            var historico = new FaturamentoHistoricoCreateCommand(faturamento.Id, request.Evento);

            await _mediator.Send(historico, cancellationToken);


            // gerar notificação
            if (request.status.Notifica == EnumNotificaQuem.WCA)
            {

            }else if (request.status.Notifica == EnumNotificaQuem.Cliente) { 

            }

            return true;
        }
    }
}
