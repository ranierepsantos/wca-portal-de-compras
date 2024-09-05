using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Gestors.Common;
using wca.share.application.Features.Gestors.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Gestors.Commands
{
    public record GestorUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<GestorResponse>>;
    internal class GestorUpdateCommandHandle : IRequestHandler<GestorUpdateCommand, ErrorOr<GestorResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GestorUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public GestorUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<GestorUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<GestorResponse>> Handle(GestorUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do Gestor é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new GestorByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                Gestor data = _mapper.Map<Gestor>(request);

                _repository.GetDbSet<Gestor>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<GestorResponse>(data);

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
