using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Queries;
using wca.reembolso.application.Features.Notificacoes.Commands;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Commands
{
    public sealed record FaturamentoAddPOCommand(
        int Id,
        string NumeroPO,
        string DocumentoPO,
        StatusSolicitacao Status,
        int[] Notificar
    ) : IRequest<ErrorOr<bool>>;

    public sealed class FaturamentoAddPOCommandHandle : IRequestHandler<FaturamentoAddPOCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoCreateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public FaturamentoAddPOCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoCreateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(FaturamentoAddPOCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando adição de PO");

            var queryById = new FaturamentoByIdQuery(request.Id);

            var findResutl = await _mediator.Send(queryById, cancellationToken);

            if (findResutl.IsError)
            {
                return findResutl.FirstError;
            }

            Faturamento faturamento = _mapper.Map<Faturamento>(findResutl.Value);

            string nomeArquivo = $"fat_{faturamento.Id}_po_{request.NumeroPO}";

            faturamento.NumeroPO = request.NumeroPO;
            faturamento.DocumentoPO = HandleFile.SaveFile(request.DocumentoPO, nomeArquivo);
            faturamento.Status = 2;

            _repository.FaturamentoRepository.Update(faturamento);

            await _repository.SaveAsync();

            // gerar evento
            var historico = new FaturamentoHistoricoCreateCommand(faturamento.Id, $"<b>P.O Emitido</b> - numero: {faturamento.NumeroPO}");

            await _mediator.Send(historico, cancellationToken);

            //notificar usuario
            for (var ii = 0; ii < request.Notificar.Length; ii++)
            {
                string mensagem = request.Status.TemplateNotificacao.Replace("{id}", faturamento.Id.ToString());

                var notificacao = new NotificacaoCreateCommand(request.Notificar[ii], mensagem, faturamento.GetType().Name, faturamento.Id);

                await _mediator.Send(notificacao, cancellationToken);
            }

            return true;
        }
    }
}
