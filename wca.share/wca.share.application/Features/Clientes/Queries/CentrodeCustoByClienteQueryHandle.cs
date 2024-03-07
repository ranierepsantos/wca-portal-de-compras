using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Clientes.Queries
{
    public record CentrodeCustoByClienteQuery(int[] ClienteId) : IRequest<ErrorOr<IList<CentroCusto>>>;
    internal class CentrodeCustoByClienteQueryHandle : IRequestHandler<CentrodeCustoByClienteQuery, ErrorOr<IList<CentroCusto>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;

        public CentrodeCustoByClienteQueryHandle(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<CentroCusto>>> Handle(CentrodeCustoByClienteQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.GetDbSet<CentroCusto>()
                .Where(q => request.ClienteId.Contains(q.ClienteId))
                .OrderBy(o => o.Nome);

            List<CentroCusto>? lista = await query.ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<CentroCusto>>(lista);


        }
    }
}
