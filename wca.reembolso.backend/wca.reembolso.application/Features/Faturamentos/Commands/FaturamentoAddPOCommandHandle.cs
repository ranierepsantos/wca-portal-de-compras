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
        StatusSolicitacao Status,
        int[] Notificar,
        string? NumeroPO,
        string? DocumentoPO
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

            string nomeArquivo = $"faturamento_{faturamento.Id}_po_" + (String.IsNullOrEmpty(request.NumeroPO)? "naoinfo": request.NumeroPO);

            faturamento.NumeroPO = request.NumeroPO;
            faturamento.DocumentoPO = String.IsNullOrEmpty(request.DocumentoPO) == false ? HandleFile.SaveFile(request.DocumentoPO, nomeArquivo) : "";
            faturamento.Status = 2;

            _repository.FaturamentoRepository.Update(faturamento);

            await _repository.SaveAsync();

            // gerar evento
            string evento = "<b>P.O Emitido</b> - numero: " + (String.IsNullOrEmpty(faturamento.NumeroPO) ? " não informado" : faturamento.NumeroPO);
            evento += "<br/> Documento: " + (String.IsNullOrEmpty(faturamento.DocumentoPO) ? " não informado" : nomeArquivo);

            var historico = new FaturamentoHistoricoCreateCommand(faturamento.Id, evento);

            await _mediator.Send(historico, cancellationToken);

            //notificar usuario
            if (!String.IsNullOrEmpty(request.Status.TemplateNotificacao))
            {
                for (var ii = 0; ii < request.Notificar.Length; ii++)
                {
                    string mensagem = request.Status.TemplateNotificacao.Replace("{id}", faturamento.Id.ToString());

                    var notificacao = new NotificacaoCreateCommand(request.Notificar[ii], mensagem, faturamento.GetType().Name, faturamento.Id);

                    await _mediator.Send(notificacao, cancellationToken);
                }
            }

            return true;
        }
    }
}
