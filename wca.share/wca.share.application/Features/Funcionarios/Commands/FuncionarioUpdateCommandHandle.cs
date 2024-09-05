using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcionarios.Behaviors;
using wca.share.application.Features.Funcionarios.Common;
using wca.share.application.Features.Funcionarios.Queries;
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
    internal class FuncionarioUpdateCommandHandle : IRequestHandler<FuncionarioUpdateCommand, ErrorOr<FuncionarioResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncionarioUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public FuncionarioUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncionarioUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<FuncionarioResponse>> Handle(FuncionarioUpdateCommand request, CancellationToken cancellationToken)
        {

            //_logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

            //1. validar dados
            FuncionarioUpdateCommandBehavior validator = new();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            //localizar o funcionário
            var findResult = await _mediator.Send(new FuncionarioByIdQuery(request.Id));
            if (findResult.IsError) return findResult;

            Funcionario data = _mapper.Map<Funcionario>(request);

            _repository.GetDbSet<Funcionario>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            await _repository.SaveAsync();

            return _mapper.Map<FuncionarioResponse>(data);
        }
    }
}
