using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Vagas.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Vagas.Queries
{
    public record VagaPaginateQuery(
        DateTime? DataIni,
        DateTime? DataFim,
        int? ResponsavelId,
        int[]? ClienteIds,
        int? Status) : PaginationQuery, IRequest<ErrorOr<Pagination<VagaToPaginateResponse>>>;
    internal sealed class VagaToPaginateQueryHandle :
        IRequestHandler<VagaPaginateQuery, ErrorOr<Pagination<VagaToPaginateResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VagaToPaginateQueryHandle> _logger;
        public VagaToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<VagaToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<VagaToPaginateResponse>>> Handle(
            VagaPaginateQuery request, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Parâmetros {JsonSerializer.Serialize(request)}");

            if (request.DataIni > request.DataFim || request.DataIni != null && request.DataFim is null || request.DataFim != null && request.DataIni is null)
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _repository.GetDbSet<Vaga>().AsQueryable().AsNoTracking();
            query = query.Include(i => i.Cliente)
                         .Include(q => q.Responsavel)
                         .Include(q => q.StatusSolicitacao)
                         .Include(q => q.Funcao)
                         .Include(q => q.VagaHistorico.OrderByDescending(f => f.DataHora));

            if (request.FilialId > 1)
                query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.ResponsavelId > 0)
                query = query.Where(q => q.ResponsavelId.Equals(request.ResponsavelId));

            if (request.ClienteIds?.Length > 0)
                query = query.Where(q => request.ClienteIds.Contains(q.ClienteId));

            if (request.Status > 0)
                query = query.Where(q => q.StatusSolicitacaoId.Equals(request.Status));


            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                query = query.Where(c => c.DataSolicitacao >= request.DataIni && c.DataSolicitacao <= dataFim);
            }

            query = query.OrderByDescending(q => q.DataSolicitacao);

            var pagination = Pagination<VagaToPaginateResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);
        }
    }
}
