using System.Text.Json;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Queries
{

    public record FuncionarioListQuery(int UsuarioId) : IRequest<ErrorOr<IList<ListItem>>>;
    internal class FuncionarioListQueryHandle : IRequestHandler<FuncionarioListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FuncionarioListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public FuncionarioListQueryHandle(IRepositoryManager repository, ILogger<FuncionarioListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(FuncionarioListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Funcionario - Listando funcionários associados aos centros de custo do usuário");

                var query = _repository.GetDbSet<Funcionario>()
                            .AsQueryable()
                            .AsNoTracking()
                            .Include(i => i.CentroCusto)
                            .ThenInclude(i => i.UsuarioCentrodeCustos)
                            .Where(x => x.CentroCusto.UsuarioCentrodeCustos.Count(x => x.UsuarioId.Equals(request.UsuarioId)) > 0)
                            //.Where(x => x.DataDemissao == null)
                            .OrderBy(o => o.Nome);
                List<Funcionario> items = await query.ToListAsync();

                return _mapper.Map<List<ListItem>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Parâmetros: {JsonSerializer.Serialize(request)}\n Error.Message: {ex.Message}\n Error.InnerException: {ex.InnerException?.Message}");
                return Error.Failure(ex.Message);
            }
            
        }
    }
}