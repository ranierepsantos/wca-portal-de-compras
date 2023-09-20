using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Notificacoes.Common;

namespace wca.reembolso.application.Features.Notificacoes.Queries
{
    public sealed record NotificacaoListByUserQuery(int UsuarioId): IRequest<ErrorOr<IList<NotificacaoResponse>>>;
    public sealed class NotificacaoListByUserQueryHandle : IRequestHandler<NotificacaoListByUserQuery, ErrorOr<IList<NotificacaoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificacaoListByUserQueryHandle> _logger;

        public NotificacaoListByUserQueryHandle(IRepositoryManager repository, ILogger<NotificacaoListByUserQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<NotificacaoResponse>>> Handle(NotificacaoListByUserQuery request, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, $"Listando notificações por usuário {request.UsuarioId}");

            var query = _repository.NotificacaoRepository.ToQuery();
            
            query = query.Where(q => q.UsuarioId.Equals(request.UsuarioId)).OrderByDescending(o => o.DataHora);

            var list = await query.ToListAsync();

            return _mapper.Map<List<NotificacaoResponse>>(list);
        }
    }
}
