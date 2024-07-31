using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.MotivosContratacao.Common;
using wca.share.application.Features.MotivosContratacao.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.MotivosContratacao.Commands;

public record MotivoContratacaoUpdateCommand(
    int Id,
    string Nome,
    bool Ativo
) : IRequest<ErrorOr<MotivoContratacaoResponse>>;
internal class MotivoContratacaoUpdateCommandHandle : IRequestHandler<MotivoContratacaoUpdateCommand, ErrorOr<MotivoContratacaoResponse>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<MotivoContratacaoUpdateCommandHandle> _logger;
    private readonly IMediator _mediator;

    public MotivoContratacaoUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<MotivoContratacaoUpdateCommandHandle> logger, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<ErrorOr<MotivoContratacaoResponse>> Handle(MotivoContratacaoUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
            
            List<Error>? errors = new();

            if (request.Id <=0)
                errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

            if (string.IsNullOrEmpty(request.Nome))
                errors.Add(Error.Validation("Nome", description: "O nome do MotivoContratacao é obrigatório"));

            if (errors.Count > 0)
                return errors;

            var findResult = await _mediator.Send(new MotivoContratacaoByIdQuery(request.Id), cancellationToken);
            if (findResult.IsError) return findResult;

            MotivoContratacao data = _mapper.Map<MotivoContratacao>(request);

            _repository.GetDbSet<MotivoContratacao>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            await _repository.SaveAsync();

            return _mapper.Map<MotivoContratacaoResponse>(data);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error.Message: {ex.Message}");
            _logger.LogError($"Error.InnerException: {ex.InnerException?.Message}");
            return Error.Failure(ex.Message);
        }
    }
}
