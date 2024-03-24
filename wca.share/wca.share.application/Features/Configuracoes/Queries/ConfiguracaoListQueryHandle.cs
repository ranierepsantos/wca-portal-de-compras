using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Configuracoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Configuracoes.Queries
{
    public record ConfiguracaoListQuery() : IRequest<ErrorOr<IList<ConfiguracaoResponse>>>;
    internal sealed class ConfiguracaoListQueryHandle : IRequestHandler<ConfiguracaoListQuery, ErrorOr<IList<ConfiguracaoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ConfiguracaoListQueryHandle> _logger;
        private readonly IMapper _mapper;
        public ConfiguracaoListQueryHandle(IRepositoryManager repository, ILogger<ConfiguracaoListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ConfiguracaoResponse>>> Handle(ConfiguracaoListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listando configurações");

            var query = _repository.GetDbSet<Configuracao>()
                        .AsNoTracking()
                        .AsQueryable()
                        .OrderBy(o => o.Descricao);
            IList<Configuracao>? list = await query.ToListAsync(cancellationToken: cancellationToken);
            
             return _mapper.Map<List<ConfiguracaoResponse>>(list); 

        }
    }
}
