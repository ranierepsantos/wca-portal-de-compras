using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escolaridades.Common;
using wca.share.application.Features.Escolaridades.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escolaridades.Commands
{
    public record EscolaridadeUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<EscolaridadeResponse>>;
    internal class EscolaridadeUpdateCommandHandle : IRequestHandler<EscolaridadeUpdateCommand, ErrorOr<EscolaridadeResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscolaridadeUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public EscolaridadeUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscolaridadeUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<EscolaridadeResponse>> Handle(EscolaridadeUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do Escolaridade é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new EscolaridadeByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                Escolaridade data = _mapper.Map<Escolaridade>(request);

                _repository.GetDbSet<Escolaridade>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<EscolaridadeResponse>(data);

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
