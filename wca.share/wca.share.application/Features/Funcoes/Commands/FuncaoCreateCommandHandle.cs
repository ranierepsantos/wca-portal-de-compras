using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcoes.Commands
{
    public sealed record FuncaoCreateCommand (string Nome): IRequest<ErrorOr<FuncaoResponse>>;
    internal class FuncaoCreateCommandHandle : IRequestHandler<FuncaoCreateCommand, ErrorOr<FuncaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncaoCreateCommandHandle> _logger;

        public FuncaoCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncaoCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<FuncaoResponse>> Handle(FuncaoCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do Funcao é obrigatório");

                Funcao data = _mapper.Map<Funcao>(request);

                _repository.GetDbSet<Funcao>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<FuncaoResponse>(data);

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
