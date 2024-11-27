using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TiposFaturamento.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TiposFaturamento.Commands
{
    public sealed record TipoFaturamentoCreateCommand (string Nome): IRequest<ErrorOr<TipoFaturamentoResponse>>;
    internal class TipoFaturamentoCreateCommandHandle : IRequestHandler<TipoFaturamentoCreateCommand, ErrorOr<TipoFaturamentoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoFaturamentoCreateCommandHandle> _logger;

        public TipoFaturamentoCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoFaturamentoCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<TipoFaturamentoResponse>> Handle(TipoFaturamentoCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do TipoFaturamento é obrigatório");

                TipoFaturamento data = _mapper.Map<TipoFaturamento>(request);

                _repository.GetDbSet<TipoFaturamento>().Add(data);

                await _repository.SaveAsync();

                return _mapper.Map<TipoFaturamentoResponse>(data);

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
