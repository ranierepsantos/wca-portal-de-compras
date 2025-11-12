using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcionarios.Behaviors;
using wca.share.application.Features.Funcionarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Commands
{
    public record FuncionarioUpdateCommand(
        int Id,
        string Nome,
        int ClienteId,
        int CentroCustoId,
        DateTime? DataAdmissao,
        int? CodigoFuncionario = null,
        DateTime? DataDemissao = null,
        string? Email = null,
        int? DDDCelular = null,
        double? NumeroCelular = null,
        string? eSocialMatricula = null
    ) : IRequest<ErrorOr<FuncionarioResponse>>;

    internal class FuncionarioUpdateCommandHandle 
        : IRequestHandler<FuncionarioUpdateCommand, ErrorOr<FuncionarioResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncionarioUpdateCommandHandle> _logger;
        private readonly FuncionarioUpdateCommandBehavior _validator = new();

        public FuncionarioUpdateCommandHandle(
            IRepositoryManager repository,
            IMapper mapper,
            ILogger<FuncionarioUpdateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<FuncionarioResponse>> Handle(FuncionarioUpdateCommand request, CancellationToken cancellationToken)
        {
            // 1. Validar dados
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => Error.Validation(x.PropertyName, x.ErrorMessage))
                    .ToList();
                return errors;
            }

            var dbSet = _repository.GetDbSet<Funcionario>();
            // 2. Localizar funcionário
            var funcionario = await dbSet.FindAsync(new object[] { request.Id }, cancellationToken);

            if (funcionario is null)
            {
                _logger.LogWarning("Funcionário não encontrado. Id={Id}", request.Id);
                return Error.NotFound("Funcionario.NotFound", $"Funcionário {request.Id} não encontrado.");
            }


            // 3. Verificar duplicidade de matrícula (usando valor do request)
            if (!string.IsNullOrWhiteSpace(request.eSocialMatricula))
            {
                bool exists = await dbSet
                    .AsNoTracking()
                    .AnyAsync(f => f.eSocialMatricula == request.eSocialMatricula && f.Id != request.Id, cancellationToken);

                if (exists)
                {
                    _logger.LogWarning(
                        "Tentativa de atualizar funcionário duplicado. Id={Id}, eSocialMatricula={Matricula}",
                        request.Id, request.eSocialMatricula);

                    return Error.Conflict(
                        code: "Funcionario.Duplicado",
                        description: $"Funcionário com matrícula {request.eSocialMatricula} já existe.");
                }
            }

            // 4. Atualizar dados
            _mapper.Map(request, funcionario);

            await _repository.SaveAsync();

            // 4. Retornar DTO
            return _mapper.Map<FuncionarioResponse>(funcionario);
        }
    }
}
