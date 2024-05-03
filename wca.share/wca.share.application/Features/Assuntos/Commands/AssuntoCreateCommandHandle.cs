using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Assuntos.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Assuntos.Commands
{
    public sealed record AssuntoCreateCommand (string Nome): IRequest<ErrorOr<AssuntoResponse>>;
    internal class AssuntoCreateCommandHandle : IRequestHandler<AssuntoCreateCommand, ErrorOr<AssuntoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssuntoCreateCommandHandle> _logger;

        public AssuntoCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<AssuntoCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<AssuntoResponse>> Handle(AssuntoCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do assunto é obrigatório");

                Assunto data = _mapper.Map<Assunto>(request);

                _repository.GetDbSet<Assunto>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<AssuntoResponse>(data);

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
