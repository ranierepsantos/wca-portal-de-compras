using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.MotivosContratacao.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.MotivosContratacao.Commands
{
    public sealed record MotivoContratacaoCreateCommand (string Nome): IRequest<ErrorOr<MotivoContratacaoResponse>>;
    internal class MotivoContratacaoCreateCommandHandle : IRequestHandler<MotivoContratacaoCreateCommand, ErrorOr<MotivoContratacaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<MotivoContratacaoCreateCommandHandle> _logger;

        public MotivoContratacaoCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<MotivoContratacaoCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<MotivoContratacaoResponse>> Handle(MotivoContratacaoCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do MotivoContratacao é obrigatório");

                MotivoContratacao data = _mapper.Map<MotivoContratacao>(request);

                _repository.GetDbSet<MotivoContratacao>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<MotivoContratacaoResponse>(data);

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
