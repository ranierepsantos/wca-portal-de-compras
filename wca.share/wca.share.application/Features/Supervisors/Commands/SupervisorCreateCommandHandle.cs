using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Supervisors.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Supervisors.Commands
{
    public sealed record SupervisorCreateCommand (string Nome): IRequest<ErrorOr<SupervisorResponse>>;
    internal class SupervisorCreateCommandHandle : IRequestHandler<SupervisorCreateCommand, ErrorOr<SupervisorResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupervisorCreateCommandHandle> _logger;

        public SupervisorCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<SupervisorCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<SupervisorResponse>> Handle(SupervisorCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do Supervisor é obrigatório");

                Supervisor data = _mapper.Map<Supervisor>(request);

                _repository.GetDbSet<Supervisor>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<SupervisorResponse>(data);

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
