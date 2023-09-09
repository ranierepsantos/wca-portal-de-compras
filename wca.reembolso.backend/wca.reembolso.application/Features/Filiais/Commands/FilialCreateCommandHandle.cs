using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Filiais.Behaviors;
using wca.reembolso.application.Features.Filiais.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Filiais.Commands
{

    public sealed record FilialCreateCommand(
        string Nome
    ):IRequest<ErrorOr<FilialResponse>>;

    public sealed class FilialCreateCommandHandle : IRequestHandler<FilialCreateCommand, ErrorOr<FilialResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FilialCreateCommandHandle> _logger;

        public FilialCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FilialCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<FilialResponse>> Handle(FilialCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Criando Filial - início");

            //1. validar dados
            FilialCreateCommandBehavior validator = new FilialCreateCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }

            var filial = _mapper.Map<Filial>(request);

            _repository.FilialRepository.Create(filial);

            await _repository.SaveAsync();

            return _mapper.Map<FilialResponse>(filial);

        }
    }
}
