using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.DocumentoComplementares.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.DocumentoComplementares.Commands
{
    public sealed record DocumentoComplementarCreateCommand (string Nome): IRequest<ErrorOr<DocumentoComplementarResponse>>;
    internal class DocumentoComplementarCreateCommandHandle : IRequestHandler<DocumentoComplementarCreateCommand, ErrorOr<DocumentoComplementarResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentoComplementarCreateCommandHandle> _logger;

        public DocumentoComplementarCreateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<DocumentoComplementarCreateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<DocumentoComplementarResponse>> Handle(DocumentoComplementarCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(request.Nome))
                    return Error.Validation("Nome", description: "O nome do DocumentoComplementar é obrigatório");

                DocumentoComplementar data = _mapper.Map<DocumentoComplementar>(request);

                _repository.GetDbSet<DocumentoComplementar>().Add(data);

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
