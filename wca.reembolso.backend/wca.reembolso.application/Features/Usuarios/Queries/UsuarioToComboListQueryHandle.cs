using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Usuarios.Common;

namespace wca.reembolso.application.Features.Usuarios.Queries
{
    public record UsuarioToComboListQuery (int clienteId, int perfilId = 0) : IRequest<ErrorOr<IList<UsuarioToListResponse>>>;

    internal class UsuarioToComboListQueryHandle : IRequestHandler<UsuarioToComboListQuery, ErrorOr<IList<UsuarioToListResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioToComboListQueryHandle> _logger;

        public UsuarioToComboListQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<UsuarioToComboListQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<UsuarioToListResponse>>> Handle(UsuarioToComboListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _repository.UsuarioRepository.ToQuery().AsNoTracking();
                query = query.Where(q => q.UsuarioClientes.Where (q1 =>  q1.ClienteId == request.clienteId).Any());
                if (request.perfilId > 0) 
                   query = query.Where(q => q.PerfilId == request.perfilId);

                query = query.Include(ic => ic.UsuarioCentrodeCustos);

                List<UsuarioToListResponse> list = await query.Select(d =>
                    new UsuarioToListResponse(
                            d.Id,
                            d.Nome,
                            d.Cargo,
                            d.UsuarioCentrodeCustos.Count > 0 ? d.UsuarioCentrodeCustos[0].CentroCustoId : 0
                        )).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Error.Failure("", ex.Message);
            }
        }
    }
}
