using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Queries;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Commands
{
    public sealed record FaturamentoChangeNumeroDsCommand(
        int Id, 
        string Evento,
        string NumeroDS
    ):IRequest<ErrorOr<bool>>;

    public sealed class FaturamentoChangeNumeroDsCommandHandle : IRequestHandler<FaturamentoChangeNumeroDsCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoChangeNumeroDsCommandHandle> _logger;
        private readonly IMediator _mediator;

        public FaturamentoChangeNumeroDsCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoChangeNumeroDsCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(FaturamentoChangeNumeroDsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando alteração do número DS");

            var queryById = new FaturamentoByIdQuery(request.Id);

            var findResutl = await _mediator.Send(queryById, cancellationToken);

            if (findResutl.IsError)
            {
                return findResutl.FirstError;
            }

            Faturamento faturamento = _mapper.Map<Faturamento>(findResutl.Value);

            faturamento.NumeroDS = request.NumeroDS;

            _repository.FaturamentoRepository.Update(faturamento);

            await _repository.SaveAsync();

            // gerar evento
            var historico = new FaturamentoHistoricoCreateCommand(faturamento.Id, request.Evento);

            await _mediator.Send(historico, cancellationToken);

            return true;
        }
    }
}
