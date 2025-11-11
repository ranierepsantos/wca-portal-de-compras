using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;
using wca.share.application.Contracts.Integration.GI.Models;
using Microsoft.Extensions.Logging;

namespace wca.share.application.Features.Clientes.Commands
{

    public record ClienteCreateFromIntegration(): IRequest<ErrorOr<bool>>;
    internal class ClienteCreateFromIntegrationGIHandle : IRequestHandler<ClienteCreateFromIntegration, ErrorOr<bool>>
    {
        private readonly IIntegrationGI _gi;
        private readonly IMediator _mediator;
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ClienteCreateFromIntegrationGIHandle> _logger;

        public ClienteCreateFromIntegrationGIHandle(IIntegrationGI gi, IMediator mediator, IRepositoryManager repository, ILogger<ClienteCreateFromIntegrationGIHandle> logger)
        {
            _gi = gi;
            _mediator = mediator;
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(ClienteCreateFromIntegration request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Clientes.início: {Time}", DateTime.Now);

                var (clientesIntegracao, centrosDeCustosIntegracao) = await FetchIntegrationData(cancellationToken);

                foreach (var oCli in clientesIntegracao)
                {
                    var cliente = await BuscarClienteExistente(oCli, cancellationToken);
                    var centros = MapearCentrosDeCusto(centrosDeCustosIntegracao, oCli.CodigoCliente);

                    if (cliente != null)
                    {
                        var novosCentros = centros.Where(c => !cliente.CentroCusto.Any(x => x.Codigo == c.Codigo)).ToList();
                        
                        foreach (var centro in novosCentros)
                            cliente.CentroCusto.Add(centro);
                        

                        var updateCommand = new ClienteUpdateCommand(
                            cliente.Id, oCli.CodigoCliente, cliente.FilialId, oCli.RazaoSocial, oCli.Cgc, oCli.Ie,
                            oCli.Endereco, cliente.Numero, oCli.Cep, oCli.Cidade, oCli.UF, oCli.ClienteAtivo, cliente.CentroCusto);

                        await _mediator.Send(updateCommand, cancellationToken);
                    }
                    else
                    {
                        var createCommand = new ClienteCreateCommand(
                            oCli.CodigoCliente, oCli.RazaoSocial, oCli.Cgc, oCli.Ie, oCli.Endereco,
                            "", oCli.Cep, oCli.Cidade, oCli.UF, 0, oCli.ClienteAtivo, centros);

                        await _mediator.Send(createCommand, cancellationToken);
                    }
                }
                _logger.LogInformation("Clientes.fim: {Time}", DateTime.Now);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao integrar clientes");

                _repository.GetDbSet<EventLogGi>().Add(new EventLogGi()
                {
                    Log = $"ClienteCreateFromIntegration - Erro: {ex.Message}",
                    Entidade = "Cliente.Error"
                });

                return Error.Failure(description: $"Message: {ex.Message}\nInner Exception: {ex.InnerException?.Message}");
            }
        }

        private async Task<(IEnumerable<ClienteResponse>, IEnumerable<CentroCustoResponse>)> FetchIntegrationData(CancellationToken cancellationToken)
        {
            // Execução paralela para buscar dados de integração
            var clientesTask = _gi.ClienteGetAllAsync();
            var centrosCustoTask = _gi.CentroCustoGetAllAsync();

            await Task.WhenAll(clientesTask, centrosCustoTask);

            return (await clientesTask, await centrosCustoTask);
        }

        private async Task<Cliente?> BuscarClienteExistente(ClienteResponse oCli, CancellationToken cancellationToken)
        {
            return await _repository.ClienteRepository.ToQuery()
                .Where(q => q.CNPJ.Trim() == oCli.Cgc.Trim() && q.CodigoCliente == oCli.CodigoCliente)
                .Include(i => i.CentroCusto)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        private List<CentroCusto> MapearCentrosDeCusto(IEnumerable<CentroCustoResponse> centros, int codigoCliente)
        {
            return centros
                .Where(q => q.CodigoCliente == codigoCliente)
                .Select(f => new CentroCusto
                {
                    Codigo = f.CodigoCentroCusto,
                    Nome = f.Nome
                })
                .ToList();
        }
    }
}
