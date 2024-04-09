using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Usuarios.Queries
{
    public record UsuarioListarPorCentrodeCustoQuery(int CentroCustoId) : IRequest<ErrorOr<int[]>>;
    internal class UsuarioListarPorCentrodeCustoQueryHandle : IRequestHandler<UsuarioListarPorCentrodeCustoQuery, ErrorOr<int[]>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UsuarioListarPorCentrodeCustoQueryHandle(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<int[]>> Handle(UsuarioListarPorCentrodeCustoQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetDbSet<UsuarioCentrodeCustos>().Where(q => q.CentroCustoId == request.CentroCustoId).ToListAsync(cancellationToken: cancellationToken);

            return list.Select(q => q.UsuarioId).ToArray();

        }
    }
}
