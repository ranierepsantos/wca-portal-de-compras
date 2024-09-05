using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escolaridades.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escolaridades.Commands
{
    public sealed record EscolaridadeCreateCommand (string Nome): IRequest<ErrorOr<EscolaridadeResponse>>;
    internal class EscolaridadeCreateCommandHandle : IRequestHandler<EscolaridadeCreateCommand, ErrorOr<EscolaridadeResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscolaridadeCreateCommandHandle> _logger;

        public EscolaridadeCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscolaridadeCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<EscolaridadeResponse>> Handle(EscolaridadeCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do Escolaridade é obrigatório");

                Escolaridade data = _mapper.Map<Escolaridade>(request);

                _repository.GetDbSet<Escolaridade>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<EscolaridadeResponse>(data);

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
