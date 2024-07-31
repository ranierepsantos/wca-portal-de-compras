
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
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
        List<ListItem>? DocumentoComplementar
    ) : IRequest<ErrorOr<bool>>;

    internal class VagaCreateCommandHandle : IRequestHandler<VagaCreateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VagaCreateCommandHandle> _logger;

        public VagaCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<VagaCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
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

                _repository.GetDbSet<Vaga>().Add(data);

                await _repository.SaveAsync();

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
