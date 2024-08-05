
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
using wca.share.application.Features.Vagas.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Vagas.Commands
{
    public record VagaUpdateCommand(
        int Id,
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
        string? ValorValeTransporte, 
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
        List<ListItem>? DocumentoComplementar,
        StatusSolicitacao? Status,
        int[]? NotificarUsuarioIds,
        string? UsuarioAtualizador
    ) : IRequest<ErrorOr<bool>>;

    internal class VagaUpdateCommandHandle : IRequestHandler<VagaUpdateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<VagaUpdateCommandHandle> _logger;
        private readonly INotificacaoHandle _nofiticacaoHandle;

        public VagaUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<VagaUpdateCommandHandle> logger, IMediator mediator, INotificacaoHandle nofiticacaoHandle)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _nofiticacaoHandle = nofiticacaoHandle;
        }

        public async Task<ErrorOr<bool>> Handle(VagaUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                VagaUpdateCommandBehavior validator = new ();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                    _logger.LogError($"Validação falhou {JsonSerializer.Serialize(errors)}");
                    return errors;
                }

                var findResult = await _mediator.Send(new VagaFindByIdQuery(request.Id), cancellationToken);

                if (findResult.IsError) return findResult.Errors;


                Vaga data = _mapper.Map<Vaga>(request);

                //excluir o documentos complementares e atualizar com o que veio
                await _repository.ExecuteCommandAsync($"delete from DocumentoComplementarVaga where vagaid ={request.Id}");
                
                data.DocumentoComplementares = new List<DocumentoComplementar>();
                
                List<int>? docsId = request.DocumentoComplementar?.Select(f => f.Value).ToList();
                if (docsId is not null && docsId.Any())
                {
                    List<DocumentoComplementar> items = _repository.GetDbSet<DocumentoComplementar>()
                        .Where(q => docsId.Contains(q.Id))
                        .ToList();

                    data.DocumentoComplementares.AddRange(items);
                }

                _repository.GetDbSet<Vaga>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();


                //lancar no histórico
                var querie = new VagaHistorioCreateCommand(data.Id, $"Vaga alterada por {request.UsuarioAtualizador}!");
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
