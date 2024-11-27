using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TiposFaturamento.Common;
using wca.share.application.Features.TiposFaturamento.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TiposFaturamento.Commands
{
    public record TipoFaturamentoUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<TipoFaturamentoResponse>>;
    internal class TipoFaturamentoUpdateCommandHandle : IRequestHandler<TipoFaturamentoUpdateCommand, ErrorOr<TipoFaturamentoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoFaturamentoUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public TipoFaturamentoUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoFaturamentoUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<TipoFaturamentoResponse>> Handle(TipoFaturamentoUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do TipoFaturamento é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new TipoFaturamentoByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                TipoFaturamento data = _mapper.Map<TipoFaturamento>(request);

                _repository.GetDbSet<TipoFaturamento>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
