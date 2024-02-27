using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacoes.Common;

namespace wca.reembolso.application.Features.Solicitacaos.Queries
{

    public record SolicitacaoPaginateQuery(DateTime? DataIni, DateTime? DataFim, int UsuarioId =0, int ClienteId = 0, int Status = 0) : PaginationQuery, IRequest<ErrorOr<Pagination<SolicitacaoResponse>>>;
    public sealed class SolicitacaoToPaginateQueryHandle : 
        IRequestHandler<SolicitacaoPaginateQuery, ErrorOr<Pagination<SolicitacaoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoToPaginateQueryHandle> _logger;
        public SolicitacaoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<SolicitacaoResponse>>> Handle(
            SolicitacaoPaginateQuery request, CancellationToken cancellationToken)
        {

            if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _repository.SolicitacaoRepository.ToQuery();
            query = query.Include(i => i.Cliente)
                         .Include(q => q.Colaborador)
                         .Include(q => q.CentroCusto)
                         .Include(q => q.SolicitacaoHistorico.OrderByDescending(f => f.DataHora));

            if (request.FilialId > 1)
             query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.UsuarioId > 0)
                query = query.Where(q => q.ColaboradorId.Equals(request.UsuarioId));

            if (request.ClienteId > 0)
                query = query.Where(q => q.ClienteId.Equals(request.ClienteId));

            if (request.Status > 0)
                query = query.Where(q => q.Status.Equals( request.Status));

            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                query = query.Where(c => c.DataSolicitacao >= request.DataIni && c.DataSolicitacao <= dataFim);
            }
                

            query = query.OrderBy(q => q.Id);

            var pagination = Pagination<SolicitacaoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination) ;
        }
    }
}
