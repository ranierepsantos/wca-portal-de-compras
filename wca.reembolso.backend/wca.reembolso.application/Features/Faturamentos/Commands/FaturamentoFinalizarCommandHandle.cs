using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Queries;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Commands
{
    public sealed record FaturamentoFinalizarCommand(
        int Id,
        string NotaFiscal,
        DateTime DataFinalizacao,
        string NomeUsuario
    ) : IRequest<ErrorOr<bool>>;

    public sealed class FaturamentoFinalizarCommandHandle : IRequestHandler<FaturamentoFinalizarCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoCreateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public FaturamentoFinalizarCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoCreateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(FaturamentoFinalizarCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando finalização faturamento");

            var queryById = new FaturamentoByIdQuery(request.Id);

            var findResutl = await _mediator.Send(queryById, cancellationToken);

            if (findResutl.IsError)
            {
                return findResutl.FirstError;
            }

            Faturamento faturamento = _mapper.Map<Faturamento>(findResutl.Value);

            faturamento.NotaFiscal = request.NotaFiscal;
            faturamento.DataFinalizacao = request.DataFinalizacao;
            faturamento.Status = 3;

            _repository.FaturamentoRepository.Update(faturamento);

            await _repository.SaveAsync();

            // gerar evento
            var historico = new FaturamentoHistoricoCreateCommand(faturamento.Id, $"Faturamento <b>Finalizado</b> por {request.NomeUsuario}, data finalização { request.DataFinalizacao } - nota fiscal: {faturamento.NotaFiscal}");

            await _mediator.Send(historico, cancellationToken);

            return true;
        }
    }
}
