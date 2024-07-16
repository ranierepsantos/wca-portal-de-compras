using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Horarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Horarios.Commands
{
    public sealed record HorarioCreateCommand (string Nome): IRequest<ErrorOr<HorarioResponse>>;
    internal class HorarioCreateCommandHandle : IRequestHandler<HorarioCreateCommand, ErrorOr<HorarioResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<HorarioCreateCommandHandle> _logger;

        public HorarioCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<HorarioCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<HorarioResponse>> Handle(HorarioCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do Horario é obrigatório");

                Horario data = _mapper.Map<Horario>(request);

                _repository.GetDbSet<Horario>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<HorarioResponse>(data);

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
