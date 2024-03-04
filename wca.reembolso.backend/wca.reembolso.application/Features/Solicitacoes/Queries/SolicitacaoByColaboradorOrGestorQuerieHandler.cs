﻿using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacoes.Common;

namespace wca.reembolso.application.Features.Solicitacoes.Queries
{
    public record SolicitacaoByColaboradorOrGestorQuerie(int ColaboradorId = 0, int GestorId = 0, int[]? Status = null) : IRequest<ErrorOr<IList<SolicitacaoResponse>>>;
    public class SolicitacaoByColaboradorOrGestorQueryHandler : IRequestHandler<SolicitacaoByColaboradorOrGestorQuerie, ErrorOr<IList<SolicitacaoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoByColaboradorOrGestorQueryHandler> _logger;

        public SolicitacaoByColaboradorOrGestorQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoByColaboradorOrGestorQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<IList<SolicitacaoResponse>>> Handle(SolicitacaoByColaboradorOrGestorQuerie request, CancellationToken cancellationToken)
        {

            var query = _repository.SolicitacaoRepository.ToQuery();

            if (request.ColaboradorId > 0)
                query = query.Where(q => q.ColaboradorId.Equals(request.ColaboradorId));

            if (request.GestorId > 0)
                query = query.Where(q => q.GestorId.Equals(request.ColaboradorId));

            var status = request.Status ?? Array.Empty<int>();

            if (status.Length > 0)
            {
                query = query.Where(q => status.Contains(q.Status));
            }

            var list = await query.ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<SolicitacaoResponse>>(list);

        }
    }
}