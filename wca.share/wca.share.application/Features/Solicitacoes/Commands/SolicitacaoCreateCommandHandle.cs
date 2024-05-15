using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.SolicitacaoHistoricos.Commands;
using wca.share.application.Features.Solicitacoes.Behaviors;
using wca.share.application.Features.Solicitacoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Commands
{
    public record SolicitacaoCreateCommand (
        int SolicitacaoTipoId,
        int ClienteId,
        int FuncionarioId,
        StatusSolicitacao Status,
        string UsuarioCriador,
        int? CentroCustoId,
        int? ResponsavelId,
        string? Descricao,
        SolicitacaoComunicado? Comunicado,
        SolicitacaoDesligamento? Desligamento,
        SolicitacaoMudancaBaseResponse? MudancaBase,
        SolicitacaoFerias? Ferias,
        List<SolicitacaoArquivo>? Anexos,
        int[]? NotificarUsuarioIds
    ) :IRequest<ErrorOr<SolicitacaoResponse>>;
    internal class SolicitacaoCreateCommandHandle : IRequestHandler<SolicitacaoCreateCommand, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoCreateCommandHandle> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMediator _mediator;
        private readonly INotificacaoHandle _nofiticacaoHandle;
        public SolicitacaoCreateCommandHandle(IMapper mapper, ILogger<SolicitacaoCreateCommandHandle> logger, IRepositoryManager repository, IMediator mediator, INotificacaoHandle nofiticacaoHandle)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
            _mediator = mediator;
            _nofiticacaoHandle = nofiticacaoHandle;
        }

        async Task<ErrorOr<SolicitacaoResponse>> IRequestHandler<SolicitacaoCreateCommand, ErrorOr<SolicitacaoResponse>>.Handle(SolicitacaoCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando validações");

            SolicitacaoCreateCommandBehavior validator = new();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                _logger.LogError($"Validação falhou {JsonSerializer.Serialize(errors)}");
                return errors;
            }

            //salvar as anexos
            for (int idx = 0; idx < request.Anexos?.Count; idx++)
            {
                if (HandleFile.IsBase64(request.Anexos[idx].CaminhoArquivo))
                {
                    request.Anexos[idx].CaminhoArquivo = HandleFile.SaveFile(request.Anexos[idx].CaminhoArquivo);
                }
            }

            //mapear para Solicitacao e adicionar
            Solicitacao dado = new();

            _mapper.Map(request,dado);

            dado.StatusSolicitacaoId = request.Status.Id;

            if (dado.SolicitacaoTipoId == (int) EnumTipoSolicitacao.MudancaBase)
            {
                dado.MudancaBase.ClienteDestino = null;
                List<int> itensMudancaIds = dado.MudancaBase.ItensMudanca
                                            .Select(x => x.Id)
                                            .ToList();
                dado.MudancaBase.ItensMudanca = new();

                var items = _repository.ItemMudancaRepository.ToQuery().Where(c => itensMudancaIds.Contains(c.Id)).ToList();
                if (items.Any())
                {
                    dado.MudancaBase.ItensMudanca.AddRange(items);
                }
            }

            _repository.SolicitacaoRepository.Attach(dado);

            await _repository.SaveAsync();

            //lancar no histórico
            var querie = new SolicitacaoHistorioCreateCommand(dado.Id, $"Solicitação criada por {request.UsuarioCriador}!");
            await _mediator.Send(querie, cancellationToken);

            //notificar responsáveis
            if (request.Status?.TemplateNotificacao is not null && request.NotificarUsuarioIds?.Length > 0)
                await _nofiticacaoHandle.SolicitacaoEnviarNotificacaoAsync(request.NotificarUsuarioIds, request.Status.TemplateNotificacao, dado, cancellationToken);

            // mapear para SolicitacaoResponse e retornar
            return _mapper.Map<SolicitacaoResponse>(dado);

        }
    }
}
