using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escalas.Common;
using wca.share.application.Features.Escalas.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escalas.Commands
{
    public record EscalaUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<EscalaResponse>>;
    internal class EscalaUpdateCommandHandle : IRequestHandler<EscalaUpdateCommand, ErrorOr<EscalaResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscalaUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public EscalaUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscalaUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<EscalaResponse>> Handle(EscalaUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do Escala é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new EscalaByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                Escala data = _mapper.Map<Escala>(request);

                _repository.GetDbSet<Escala>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<EscalaResponse>(data);

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
