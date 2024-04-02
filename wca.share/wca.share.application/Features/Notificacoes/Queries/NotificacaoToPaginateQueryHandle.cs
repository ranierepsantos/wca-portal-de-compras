using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Notificacoes.Common;

namespace wca.share.application.Features.Notificacoes.Queries
{
    public record NotificacaoToPaginateQuery(
        int UsuarioId, 
        bool NotRead = false,
        DateTime? DataIni = null,
        DateTime? DataFim = null) : PaginationQuery, IRequest<ErrorOr<Pagination<NotificacaoResponse>>>;
    internal class NotificacaoToPaginateQueryHandle : IRequestHandler<NotificacaoToPaginateQuery, ErrorOr<Pagination<NotificacaoResponse>>>
    {

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificacaoToPaginateQueryHandle> _logger;

        public NotificacaoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<NotificacaoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<NotificacaoResponse>>> Handle(NotificacaoToPaginateQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Parâmetros {JsonSerializer.Serialize(request)}");

            var query = _repository.NotificacaoRepository.ToQuery()
                        .Where(q => q.UsuarioId.Equals(request.UsuarioId));

            if (request.NotRead)
                query = query.Where(q => q.Lido == false);

            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                query = query.Where(c => c.DataHora >= request.DataIni && c.DataHora <= dataFim);
            }

            query = query.OrderByDescending(o => o.DataHora);

            var pagination = Pagination<NotificacaoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);
        }
    }
}
