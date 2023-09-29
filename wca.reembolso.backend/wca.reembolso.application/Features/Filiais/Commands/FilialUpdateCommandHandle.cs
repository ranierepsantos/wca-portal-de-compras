using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Filiais.Behaviors;
using wca.reembolso.application.Features.Filiais.Common;
using wca.reembolso.application.Features.Filiais.Queries;
using wca.reembolso.application.Features.TiposDespesa.Queries;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Filiais.Commands
{
    public sealed record FilialUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ): IRequest<ErrorOr<FilialResponse>>;

    public sealed class FilialUpdateCommandHandle : IRequestHandler<FilialUpdateCommand, ErrorOr<FilialResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FilialCreateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public FilialUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FilialCreateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<FilialResponse>> Handle(FilialUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Criando Filial - início");

            // 1. validar dados
            FilialUpdateCommandBehavior validator = new FilialUpdateCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            // 2. Localizar registro
            var findResult = await _mediator.Send(new FilialByIdQuerie(request.Id), cancellationToken);

            if (findResult.IsError)
                return findResult;

            // 3. mapear dados
            var filial = _mapper.Map<Filial>(request);

            // 4. atualizar
            _repository.FilialRepository.Update(filial);

            await _repository.SaveAsync();

            return _mapper.Map<FilialResponse>(filial);
        }
    }
}
