using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Queries
{
    public record StatusSolicitacaoGetAllQuery():IRequest<ErrorOr<IList<StatusSolicitacao>>>;
    public class StatusSolicitacaoGetAllQuerieHandle : IRequestHandler<StatusSolicitacaoGetAllQuery, ErrorOr<IList<StatusSolicitacao>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<StatusSolicitacaoGetAllQuerieHandle> _logger;

        public StatusSolicitacaoGetAllQuerieHandle(IRepositoryManager repository, ILogger<StatusSolicitacaoGetAllQuerieHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<StatusSolicitacao>>> Handle(StatusSolicitacaoGetAllQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Status Solicitação - GetAll");

            var query = _repository.StatusSolicitacaoRepository.ToQuery().OrderBy(o => o.Status);

            return await query.ToListAsync(cancellationToken: cancellationToken);

        }
    }
}
