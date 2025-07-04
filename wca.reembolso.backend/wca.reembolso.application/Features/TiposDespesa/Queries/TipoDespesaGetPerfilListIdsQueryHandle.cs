using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.TiposDespesa.Queries
{
    public record TipoDespesaGetPerfilListIdsQuery (int TipoDespesaId): IRequest<ErrorOr<List<int>>>;
    public class TipoDespesaGetPerfilListIdsQueryHandle : IRequestHandler<TipoDespesaGetPerfilListIdsQuery, ErrorOr<List<int>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoDespesaGetPerfilListIdsQueryHandle> _logger;

        public TipoDespesaGetPerfilListIdsQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoDespesaGetPerfilListIdsQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<List<int>>> Handle(TipoDespesaGetPerfilListIdsQuery request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Buscando list de perfil por tipo de despesa");

            var items = await _repository.TipoDespesaRepository.ToQuery()
                            .Include(x => x.Perfil)
                            .Where(q => q.Id == request.TipoDespesaId)
                            .FirstOrDefaultAsync();

            return items?.Perfil.Select(p => p.PerfilId).ToList() ?? new List<int>();
        }
    }
}