using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Filiais.Common;

namespace wca.reembolso.application.Features.Filiais.Queries
{
    public record FilialByIdQuerie(int Id) : IRequest<ErrorOr<FilialResponse>>;
    public class FilialByIdQueryHandle : IRequestHandler<FilialByIdQuerie, ErrorOr<FilialResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FilialByIdQueryHandle> _logger;

        public FilialByIdQueryHandle(IRepositoryManager reposistory, IMapper mapper, ILogger<FilialByIdQueryHandle> logger)
        {
            _repository = reposistory;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<FilialResponse>> Handle(FilialByIdQuerie request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando tipo de despesa por id");

            var Filial = await _repository.FilialRepository.ToQuery()
                                    .Where(q => q.Id.Equals(request.Id))
                                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (Filial == null)
            {
                _logger.LogError($"Filial não localizada!");
                return Error.NotFound(description: $"Filial não localizado!");
            }

            return _mapper.Map<FilialResponse>(Filial);

        }
    }
}
