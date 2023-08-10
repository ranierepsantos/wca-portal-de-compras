using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.TiposDespesa.Behaviors;
using wca.reembolso.application.Features.TiposDespesa.Common;
using wca.reembolso.domain.Common.Enum;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.TiposDespesa.Commands
{

    public record TipoDespesaCreateCommand (
        string Nome,
        EnumTipoDespesaTipo Tipo,
        decimal Valor = 0
    ):IRequest<ErrorOr<TipoDespesaResponse>>;

    public class TipoDespesaCreateCommandHandler : IRequestHandler<TipoDespesaCreateCommand, ErrorOr<TipoDespesaResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TipoDespesaCreateCommandHandler> _logger;
        private readonly IRepository<TipoDespesa> _repository;

        public TipoDespesaCreateCommandHandler(IMapper mapper, ILogger<TipoDespesaCreateCommandHandler> logger, IRepository<TipoDespesa> repository)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        public async Task<ErrorOr<TipoDespesaResponse>> Handle(TipoDespesaCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("TipoDespesaCreateCommandHandler - validation");
            
            //1. validar dados
            TipoDespesaCreateCommandBehavior validator = new TipoDespesaCreateCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            //2. mapear para entidade
            TipoDespesa tipoDespesa = _mapper.Map<TipoDespesa>(request);

            //3. criar e salvar
            _repository.Create(tipoDespesa);
            await _repository.SaveChangesAsync();

            return _mapper.Map<TipoDespesaResponse>(tipoDespesa);
        }
    }
}
