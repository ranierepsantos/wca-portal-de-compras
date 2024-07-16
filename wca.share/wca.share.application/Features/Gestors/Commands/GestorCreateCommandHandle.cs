using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Gestors.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Gestors.Commands
{
    public sealed record GestorCreateCommand (string Nome): IRequest<ErrorOr<GestorResponse>>;
    internal class GestorCreateCommandHandle : IRequestHandler<GestorCreateCommand, ErrorOr<GestorResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GestorCreateCommandHandle> _logger;

        public GestorCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<GestorCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<GestorResponse>> Handle(GestorCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do Gestor é obrigatório");

                Gestor data = _mapper.Map<Gestor>(request);

                _repository.GetDbSet<Gestor>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<GestorResponse>(data);

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
