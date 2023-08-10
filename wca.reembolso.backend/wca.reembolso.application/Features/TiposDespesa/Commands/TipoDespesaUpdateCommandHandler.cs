using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.TiposDespesa.Behaviors;
using wca.reembolso.application.Features.TiposDespesa.Common;
using wca.reembolso.application.Features.TiposDespesa.Queries;
using wca.reembolso.domain.Common.Enum;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.TiposDespesa.Commands
{

    public record TipoDespesaUpdateCommand (
        int Id,
        string Nome,
        bool Ativo,
        EnumTipoDespesaTipo Tipo,
        decimal Valor = 0
    ):IRequest<ErrorOr<TipoDespesaResponse>>;

    public class TipoDespesaUpdateCommandHandler : IRequestHandler<TipoDespesaUpdateCommand, ErrorOr<TipoDespesaResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoDespesaUpdateCommandHandler> _logger;
        private readonly IRepository<TipoDespesa> _repository;

        public TipoDespesaUpdateCommandHandler(IMapper mapper, ILogger<TipoDespesaUpdateCommandHandler> logger, IRepository<TipoDespesa> repository, IMediator mediator)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<ErrorOr<TipoDespesaResponse>> Handle(TipoDespesaUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("TipoDespesaUpdateCommandHandler - validation");
            
            //1. validar dados
            TipoDespesaUpdateCommandBehavior validator = new TipoDespesaUpdateCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            //2. localizar o registro

            var findResult = await _mediator.Send(new TipoDespesaByIdQuerie(request.Id), cancellationToken);

            if (findResult.IsError)
                return findResult;
            
            //3. mapear para entidade
            TipoDespesa tipoDespesa = _mapper.Map<TipoDespesa>(request);

            //4. atualizar e salvar
            _repository.Update(tipoDespesa);
            await _repository.SaveChangesAsync();

            return _mapper.Map<TipoDespesaResponse>(tipoDespesa);
        }
    }
}
