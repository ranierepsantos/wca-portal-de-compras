using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.TiposDespesa.Common;

namespace wca.reembolso.application.Features.TiposDespesa.Queries
{
    public record TipoDespesaByPerfilQuery (int PerfilId): IRequest<ErrorOr<List<TipoDespesaResponse>>>;
    public class TipoDespesaByPerfilQueryHandle : IRequestHandler<TipoDespesaByPerfilQuery, ErrorOr<List<TipoDespesaResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoDespesaByPerfilQueryHandle> _logger;

        public TipoDespesaByPerfilQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoDespesaByPerfilQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<List<TipoDespesaResponse>>> Handle(TipoDespesaByPerfilQuery request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Buscando tipo de despesa por perfil do usuário");

            var items = await _repository.TipoDespesaRepository.ToQuery()
                            .Include(x => x.Perfil)
                            .Where(q => q.Perfil.Any(p => p.PerfilId == request.PerfilId))
                            .OrderBy(x => x.Nome)
                            .ToListAsync();

            return _mapper.Map<List<TipoDespesaResponse>>(items);
        }
    }
}