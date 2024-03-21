using AutoMapper;
using ErrorOr;
using MediatR;
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
        DateTime DataAdmissao,
        int? CodigoFuncionario = null,
        DateTime? DataDemissao = null,
        string? Email = null,
        string? Cep = null,
        string? Endereco = null,
        string? Complemento = null,
        string? Bairro = null,
        string? Cidade = null,
        string? UF = null,
        string? DDDCelular = null,
        string? NumeroCelular = null
    ) : IRequest<ErrorOr<FuncionarioResponse>>;
    internal class FuncionarioCreateCommandHandle : IRequestHandler<FuncionarioCreateCommand, ErrorOr<FuncionarioResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncionarioCreateCommandHandle> _logger;

        public FuncionarioCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncionarioCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<FuncionarioResponse>> Handle(FuncionarioCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

            //1. validar dados
            FuncionarioCreateCommandBehavior validator = new();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            var data = _mapper.Map<Funcionario>(request);

            _repository.GetDbSet<Funcionario>().Add(data);

            await _repository.SaveAsync();

            return _mapper.Map<FuncionarioResponse>(data);

        }
    }
}
