using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Configuracoes.Commands
{
    public sealed record ConfiguracaoUpdateCommand(
        int Id,
        string Valor
    ): IRequest<ErrorOr<bool>>;
    internal sealed class ConfiguracaoUpdateCommandHandle : IRequestHandler<ConfiguracaoUpdateCommand, ErrorOr<bool>>
    {
        private readonly ILogger<ConfiguracaoUpdateCommandHandle> _logger;
        private readonly IRepositoryManager _repository;

        public ConfiguracaoUpdateCommandHandle(ILogger<ConfiguracaoUpdateCommandHandle> logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ErrorOr<bool>> Handle(ConfiguracaoUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Parâmetro: {JsonSerializer.Serialize(request)}");

            Configuracao? configuracao = await _repository.GetDbSet<Configuracao>()
                                        .Where(q =>  q.Id.Equals(request.Id))
                                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if ( configuracao == null )
            {
                _logger.LogWarning($"Configuração não localizada!");
                return Error.NotFound(description: $"Configuração não localizado!");
            }

            configuracao.Valor = request.Valor;

            _repository.GetDbSet<Configuracao>().Update(configuracao);

            await _repository.SaveAsync();

            return true;
        }
    }
}
