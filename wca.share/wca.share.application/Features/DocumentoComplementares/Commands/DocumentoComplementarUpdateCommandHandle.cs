using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.DocumentoComplementares.Common;
using wca.share.application.Features.DocumentoComplementares.Queries;
using wca.share.domain.Entities;

namespace wca.share.application.Features.DocumentoComplementares.Commands
{
    public record DocumentoComplementarUpdateCommand(
        int Id,
        string Nome,
        bool Ativo
    ) : IRequest<ErrorOr<DocumentoComplementarResponse>>;
    internal class DocumentoComplementarUpdateCommandHandle : IRequestHandler<DocumentoComplementarUpdateCommand, ErrorOr<DocumentoComplementarResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentoComplementarUpdateCommandHandle> _logger;
        private readonly IMediator _mediator;

        public DocumentoComplementarUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<DocumentoComplementarUpdateCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<DocumentoComplementarResponse>> Handle(DocumentoComplementarUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");
                
                List<Error>? errors = new();

                if (request.Id <=0)
                    errors.Add(Error.Validation("Id", description: "O campo Id é obrigatório"));

                if (string.IsNullOrEmpty(request.Nome))
                    errors.Add(Error.Validation("Nome", description: "O nome do DocumentoComplementar é obrigatório"));

                if (errors.Count > 0)
                    return errors;

                var findResult = await _mediator.Send(new DocumentoComplementarByIdQuery(request.Id), cancellationToken);
                if (findResult.IsError) return findResult;

                DocumentoComplementar data = _mapper.Map<DocumentoComplementar>(request);

                _repository.GetDbSet<DocumentoComplementar>().Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _repository.SaveAsync();

                return _mapper.Map<DocumentoComplementarResponse>(data);

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
