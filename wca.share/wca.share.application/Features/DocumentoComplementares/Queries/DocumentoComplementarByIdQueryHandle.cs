using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.DocumentoComplementares.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.DocumentoComplementares.Queries
{
    public record DocumentoComplementarByIdQuery(int Id) : IRequest<ErrorOr<DocumentoComplementarResponse>>;
    internal sealed class DocumentoComplementarByIdQueryHandle : IRequestHandler<DocumentoComplementarByIdQuery, ErrorOr<DocumentoComplementarResponse>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentoComplementarByIdQueryHandle> _logger;

        public DocumentoComplementarByIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<DocumentoComplementarByIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<DocumentoComplementarResponse>> Handle(DocumentoComplementarByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Buscar pelo id: {request.Id}");

            DocumentoComplementar? data = await _repository.GetDbSet<DocumentoComplementar>()
                            .Where(q => q.Id.Equals(request.Id))
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (data == null)
            {
                _logger.LogWarning($"DocumentoComplementar não localizado!");
                return Error.NotFound(description: $"DocumentoComplementar não localizado!");
            }

            return _mapper.Map<DocumentoComplementarResponse>(data);


        }
    }
}
