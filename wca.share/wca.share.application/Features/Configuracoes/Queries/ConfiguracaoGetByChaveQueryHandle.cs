using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Configuracoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Configuracoes.Queries
{
    public record ConfiguracaoGetByChaveQuery(string Chave) : IRequest<ErrorOr<ConfiguracaoResponse>>;
    internal sealed class ConfiguracaoGetByChaveQueryHandle : IRequestHandler<ConfiguracaoGetByChaveQuery, ErrorOr<ConfiguracaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ConfiguracaoGetByChaveQueryHandle> _logger;
        private readonly IMapper _mapper;
        public ConfiguracaoGetByChaveQueryHandle(IRepositoryManager repository, ILogger<ConfiguracaoGetByChaveQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<ConfiguracaoResponse>> Handle(ConfiguracaoGetByChaveQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

            var query = _repository.GetDbSet<Configuracao>()
                        .AsQueryable()
                        .AsNoTracking()
                        .Where(o => o.Chave.Equals(request.Chave));

            Configuracao? item = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return _mapper.Map<ConfiguracaoResponse>(item);

        }
    }
}
