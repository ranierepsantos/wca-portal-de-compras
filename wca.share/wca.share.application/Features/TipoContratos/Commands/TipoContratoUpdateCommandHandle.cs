using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TipoContratos.Common;
using wca.share.application.Features.TipoContratos.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TipoContratos.Commands
{
    public record TipoContratoUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<TipoContratoResponse>>;
    internal class TipoContratoUpdateCommandHandle : IRequestHandler<TipoContratoUpdateCommand, ErrorOr<TipoContratoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoContratoUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public TipoContratoUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoContratoUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<TipoContratoResponse>> Handle(TipoContratoUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do TipoContrato é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new TipoContratoByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                TipoContrato data = _mapper.Map<TipoContrato>(request);

                _repository.GetDbSet<TipoContrato>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
