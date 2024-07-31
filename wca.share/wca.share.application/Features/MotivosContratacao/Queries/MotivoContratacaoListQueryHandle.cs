using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.MotivosContratacao.Queries
{
    public record MotivoContratacaoListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class MotivoContratacaoListQueryHandle : IRequestHandler<MotivoContratacaoListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<MotivoContratacaoListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public MotivoContratacaoListQueryHandle(IRepositoryManager repository, ILogger<MotivoContratacaoListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(MotivoContratacaoListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<MotivoContratacao>? items = await _repository.GetDbSet<MotivoContratacao>()
                                            .AsNoTracking()
                                            .Where(q => q.Ativo)
                                            .OrderBy(x => x.Nome)
                                            .ToListAsync(cancellationToken: cancellationToken);

                return _mapper.Map<List<ListItem>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Error.Failure("", ex.Message);
            }
        }
    }
}
