using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcionarios.Behaviors;
using wca.share.application.Features.Funcionarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Commands
{

    public record FuncionarioCreateCommand(
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
    internal class FuncionarioCreateCommandHandle : IRequestHandler<FuncionarioCreateCommand, ErrorOr<FuncionarioResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncionarioCreateCommandHandle> _logger;
        private readonly FuncionarioCreateCommandBehavior _validator = new();


        public FuncionarioCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncionarioCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<FuncionarioResponse>> Handle(FuncionarioCreateCommand request, CancellationToken cancellationToken)
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

            // 2. Mapear para entidade
            var funcionario = _mapper.Map<Funcionario>(request);

            // 3. Verificar existência
            var dbSet = _repository.GetDbSet<Funcionario>();
            bool exists = await dbSet
                .AsNoTracking()
                .AnyAsync(f => f.eSocialMatricula == funcionario.eSocialMatricula, cancellationToken);

            if (exists)
            {
                _logger.LogWarning(
                    "Tentativa de criar funcionário já existente. eSocialMatricula={Matricula}",
                    funcionario.eSocialMatricula);

                return Error.Conflict(
                    code: "Funcionario.Duplicado",
                    description: $"Funcionário com matrícula {funcionario.eSocialMatricula} já existe.");
            }

            // 4. Persistir
            dbSet.Add(funcionario);
            await _repository.SaveAsync();

            // 5. Retornar DTO
            return _mapper.Map<FuncionarioResponse>(funcionario);
        }

    }
}
