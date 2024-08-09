
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.VagaHistoricos.Commands;
using wca.share.application.Features.Vagas.Behaviors;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Vagas.Commands
{
    public record VagaCreateCommand(
        int ClienteId,
        int TipoContratoId,
        int TipoFaturamentoId,
        int GestorId,
        int FuncaoId,
        int EscalaId,
        int HorarioId,
        int StatusId,
        int QuantidadeVagas,
        int SexoId,
        int? IdadeMinima,
        int? IdadeMaxima,
        string? Caracteristica,
        string? Indicacao,
        string? EnderecoCliente,
        string? Anotacoes,
        int? MotivoContratacaoId,
        string? JustificativaContratacao, 
        bool PermiteFumante,
        int? EscolaridadeId,
        string? LocalResidencia, 
        string? ExperienciaProfissinal,
        string? DescricaoAtividades,
        bool TemCNH,
        string? CategoriaCNH,
        bool TemValeTransporte,
        decimal? ValorValeTransporte, 
        int? DiasValeTransporte,
        string? Refeicao, 
        string? OutrosBeneficios,
        decimal? SalarioBase, 
        bool TemInsalubridade,
        decimal? PercentualInsalubridade,
        bool TemPericulosidade,
        decimal? PercentualPericulosidade,
        DateTime? DataInicioPrevista ,
        bool TemCopiaAdmissaoCliente,
        bool TemIntegracaoCliente,
        string? HorarioIntegracao,
        string? IntegracaoDiasSemana,
        List<ListItem>? DocumentoComplementares,
        int[]? NotificarUsuarioIds,
        StatusSolicitacao Status,
        string? UsuarioCriador
    ) : IRequest<ErrorOr<bool>>;

    internal class VagaCreateCommandHandle : IRequestHandler<VagaCreateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VagaCreateCommandHandle> _logger;
        private readonly IMediator _mediator;
        private readonly INotificacaoHandle _nofiticacaoHandle;

        public VagaCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<VagaCreateCommandHandle> logger, IMediator mediator, INotificacaoHandle nofiticacaoHandle)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _nofiticacaoHandle = nofiticacaoHandle;
        }

        public async Task<ErrorOr<bool>> Handle(VagaCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                VagaCreateCommandBehavior validator = new ();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                    _logger.LogError($"Validação falhou {JsonSerializer.Serialize(errors)}");
                    return errors;
                }

                Vaga data = _mapper.Map<Vaga>(request);
                data.DocumentoComplementares = new List<DocumentoComplementar>();
                List<int>? docsId = request.DocumentoComplementares?.Select(f  => f.Value).ToList();
                if (docsId is not null && docsId.Any())
                {
                    List<DocumentoComplementar> items = _repository.GetDbSet<DocumentoComplementar>()
                        .Where(q => docsId.Contains(q.Id))
                        .ToList();

                    data.DocumentoComplementares.AddRange(items);
                }

                _repository.GetDbSet<Vaga>().Add(data);


                await _repository.SaveAsync();

                //lancar no histórico
                var querie = new VagaHistorioCreateCommand(data.Id, $"Vaga criada por {request.UsuarioCriador}!");
                await _mediator.Send(querie, cancellationToken);

                //notificar responsáveis
                if (request.Status?.TemplateNotificacao is not null && request.NotificarUsuarioIds?.Length > 0)
                    await _nofiticacaoHandle.VagaEnviarNotificacaoAsync(request.NotificarUsuarioIds, request.Status.TemplateNotificacao, data, cancellationToken);

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error.Message: {ex.Message}");
                _logger.LogError($"Error.InnerException: {ex.InnerException?.Message}");
                return Error.Failure(ex.Message);
            }
        }
    }
}
