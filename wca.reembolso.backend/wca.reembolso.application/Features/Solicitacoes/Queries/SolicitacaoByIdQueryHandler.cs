using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacoes.Common;

namespace wca.reembolso.application.Features.Solicitacaos.Queries
{
    public record SolicitacaoByIdQuerie(int Id): IRequest<ErrorOr<SolicitacaoResponse>>;
    public class SolicitacaoByIdQueryHandler : IRequestHandler<SolicitacaoByIdQuerie, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoByIdQueryHandler> _logger;
        private readonly HandleFile _handleFile;
        private readonly IConfiguration _configuration;

        public SolicitacaoByIdQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoByIdQueryHandler> logger, HandleFile handleFile, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _handleFile = handleFile;
            _configuration = configuration;
        }
        public async Task<ErrorOr<SolicitacaoResponse>> Handle(SolicitacaoByIdQuerie request, CancellationToken cancellationToken)
        {

            var dado = await _repository.SolicitacaoRepository.ToQuery()
                .Include("Cliente")
                .Include(ic => ic.Despesa)
                    .ThenInclude(ic => ic.TipoDespesa)
                .Include(q => q.Colaborador)
                .Include(q => q.CentroCusto)
                .Include(q => q.SolicitacaoHistorico.OrderByDescending(f => f.DataHora))
                .Where(q => q.Id.Equals(request.Id)).AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (dado == null)
            {
                _logger.LogError($"Solicitacao não localizado!");
                return Error.NotFound(description: $"Solicitacao não localizado!");
            }

            // Se o arquivo da despesa estiver armazenado no storage, tem que gerar um link temporário
            //percorre as despesa e gera o link temporário
            // foreach (var item in dado.Despesa)
            // {
            //     if (item.ImagePath.Contains(value: _configuration["AzureStorage:urldomain"] ?? "notfoundconfigurldomain"))
            //         item.ImagePath = _handleFile.GetTemporyLink(item.ImagePath);
            // }    

            return _mapper.Map<SolicitacaoResponse>(dado);
            
        }
    }
}
