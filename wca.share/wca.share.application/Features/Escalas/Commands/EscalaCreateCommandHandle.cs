using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escalas.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escalas.Commands
{
    public sealed record EscalaCreateCommand (string Nome): IRequest<ErrorOr<EscalaResponse>>;
    internal class EscalaCreateCommandHandle : IRequestHandler<EscalaCreateCommand, ErrorOr<EscalaResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscalaCreateCommandHandle> _logger;

        public EscalaCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscalaCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<EscalaResponse>> Handle(EscalaCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do Escala é obrigatório");

                Escala data = _mapper.Map<Escala>(request);

                _repository.GetDbSet<Escala>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<EscalaResponse>(data);

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
