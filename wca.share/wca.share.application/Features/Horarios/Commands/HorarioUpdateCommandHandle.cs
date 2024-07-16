using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Horarios.Common;
using wca.share.application.Features.Horarios.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Horarios.Commands
{
    public record HorarioUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<HorarioResponse>>;
    internal class HorarioUpdateCommandHandle : IRequestHandler<HorarioUpdateCommand, ErrorOr<HorarioResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<HorarioUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public HorarioUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<HorarioUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<HorarioResponse>> Handle(HorarioUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do Horario é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new HorarioByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                Horario data = _mapper.Map<Horario>(request);

                _repository.GetDbSet<Horario>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<HorarioResponse>(data);

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
