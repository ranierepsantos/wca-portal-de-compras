using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Supervisors.Common;
using wca.share.application.Features.Supervisors.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Supervisors.Commands
{
    public record SupervisorUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<SupervisorResponse>>;
    internal class SupervisorUpdateCommandHandle : IRequestHandler<SupervisorUpdateCommand, ErrorOr<SupervisorResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupervisorUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public SupervisorUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<SupervisorUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<SupervisorResponse>> Handle(SupervisorUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do Supervisor é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new SupervisorByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                Supervisor data = _mapper.Map<Supervisor>(request);

                _repository.GetDbSet<Supervisor>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<SupervisorResponse>(data);

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
