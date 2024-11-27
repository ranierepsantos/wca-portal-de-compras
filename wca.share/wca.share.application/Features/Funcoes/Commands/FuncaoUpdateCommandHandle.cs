using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcoes.Common;
using wca.share.application.Features.Funcoes.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcoes.Commands
{
    public record FuncaoUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<FuncaoResponse>>;
    internal class FuncaoUpdateCommandHandle : IRequestHandler<FuncaoUpdateCommand, ErrorOr<FuncaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncaoUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public FuncaoUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncaoUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<FuncaoResponse>> Handle(FuncaoUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do Funcao é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new FuncaoByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                Funcao data = _mapper.Map<Funcao>(request);

                _repository.GetDbSet<Funcao>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<FuncaoResponse>(data);

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
