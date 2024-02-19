﻿using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Solicitacoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Queries
{
    public record SolicitacaoPaginateQuery(DateTime? DataIni, DateTime? DataFim, int UsuarioId = 0, int ClienteId = 0, int Status = 0) : PaginationQuery, IRequest<ErrorOr<Pagination<SolicitacaoToPaginateResponse>>>;
    internal sealed class SolicitacaoToPaginateQueryHandle :
        IRequestHandler<SolicitacaoPaginateQuery, ErrorOr<Pagination<SolicitacaoToPaginateResponse>>>
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

        public async Task<ErrorOr<Pagination<SolicitacaoToPaginateResponse>>> Handle(
            SolicitacaoPaginateQuery request, CancellationToken cancellationToken)
        {

            if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _repository.SolicitacaoRepository.ToQuery();
            query = query.Include(i => i.Cliente)
                         .Include(q => q.Responsavel)
                         .Include(q => q.Gestor)
                         .Include(q => q.SolicitacaoTipo)
                         .Include(q => q.Funcionario)
                         .Include(q => q.StatusSolicitacao)
                         .Include(q => q.Historico.OrderByDescending(f => f.DataHora));

            if (request.FilialId > 1)
                query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.UsuarioId > 0)
                query = query.Where(q => q.ResponsavelId.Equals(request.UsuarioId) || q.GestorId.Equals(request.UsuarioId));

            if (request.ClienteId > 0)
                query = query.Where(q => q.ClienteId.Equals(request.ClienteId));

            if (request.Status > 0)
                query = query.Where(q => q.StatusSolicitacaoId.Equals(request.Status));

            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                query = query.Where(c => c.DataSolicitacao >= request.DataIni && c.DataSolicitacao <= dataFim);
            }

            query = query.OrderByDescending(q => q.DataSolicitacao);

            var pagination = Pagination<SolicitacaoToPaginateResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);
        }
    }
}
