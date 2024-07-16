using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TipoContratos.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TipoContratos.Commands
{
    public sealed record TipoContratoCreateCommand (string Nome): IRequest<ErrorOr<TipoContratoResponse>>;
    internal class TipoContratoCreateCommandHandle : IRequestHandler<TipoContratoCreateCommand, ErrorOr<TipoContratoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoContratoCreateCommandHandle> _logger;

        public TipoContratoCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoContratoCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<TipoContratoResponse>> Handle(TipoContratoCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do TipoContrato é obrigatório");

                TipoContrato data = _mapper.Map<TipoContrato>(request);

                _repository.GetDbSet<TipoContrato>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<TipoContratoResponse>(data);

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
