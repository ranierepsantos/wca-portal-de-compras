using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application6.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Clientes.Queries
{
    public record CentrodeCustoByClienteQuery(int ClienteId): IRequest<ErrorOr<IList<ListItem>>>;
    internal class CentrodeCustoByClienteQueryHandle : IRequestHandler<CentrodeCustoByClienteQuery, ErrorOr<IList<ListItem>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;

        public CentrodeCustoByClienteQueryHandle(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(CentrodeCustoByClienteQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.CentroCustoRepository.ToQuery()
                .Where(q => q.ClienteId == request.ClienteId)
                .OrderBy(o => o.Nome);

            List<CentroCusto>? lista = await query.ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<ListItem>>(lista);


        }
    }
}
