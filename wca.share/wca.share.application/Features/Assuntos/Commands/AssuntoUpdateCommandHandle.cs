using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Assuntos.Common;
using wca.share.application.Features.Assuntos.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Assuntos.Commands
{
    public record AssuntoUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<AssuntoResponse>>;
    internal class AssuntoUpdateCommandHandle : IRequestHandler<AssuntoUpdateCommand, ErrorOr<AssuntoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssuntoUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public AssuntoUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<AssuntoUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<AssuntoResponse>> Handle(AssuntoUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do assunto é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new AssuntoByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                Assunto data = _mapper.Map<Assunto>(request);

                _repository.GetDbSet<Assunto>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<AssuntoResponse>(data);

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
