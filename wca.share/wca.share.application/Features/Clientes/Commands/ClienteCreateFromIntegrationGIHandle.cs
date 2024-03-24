using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;


namespace wca.share.application.Features.Clientes.Commands
{

    public record ClienteCreateFromIntegration(): IRequest<ErrorOr<bool>>;
    internal class ClienteCreateFromIntegrationGIHandle : IRequestHandler<ClienteCreateFromIntegration, ErrorOr<bool>>
    {
        private readonly IIntegrationGI _gi;
        private readonly IMediator _mediator;
        private readonly IRepositoryManager _repository;

        public ClienteCreateFromIntegrationGIHandle(IIntegrationGI gi, IMediator mediator, IRepositoryManager repository)
        {
            _gi = gi;
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ErrorOr<bool>> Handle(ClienteCreateFromIntegration request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Clientes.início: " + DateTime.Now.ToString());
            var clientes = await _gi.ClienteGetAllAsync();
            var centroDeCustos = await _gi.CentroCustoGetAllAsync();


            Console.WriteLine("Clientes.total: " + clientes.Count() );

            foreach(var oCli in  clientes)
            {
                var cliente = await _repository.ClienteRepository.ToQuery()
                                    .Where(q => q.CNPJ.Trim().Equals(oCli.Cgc.Trim().ToString()) && q.CodigoCliente.Equals(oCli.CodigoCliente))
                                    .Include(i => i.CentroCusto)
                                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                
                List<CentroCusto>? centros = centroDeCustos.Where(q => q.CodigoCliente.Equals(oCli.CodigoCliente))
                                            .Select(f =>  new CentroCusto()
                                                            {
                                                                ClienteId = 0,
                                                                Codigo = f.CodigoCentroCusto,
                                                                Nome = f.Nome
                                                            }
                                            )
                                            .ToList();

                if (cliente != null)
                {
                    //checar da lista de centros de custo, o que já esta na base e o que não esta, o que não está incluir
                    var centrosAdicionar = centros.Where(c =>  !cliente.CentroCusto.Where(x => x.Codigo == c.Codigo).Any()).ToList();

                    if (centrosAdicionar != null)
                    {
                        foreach(var item in centrosAdicionar)
                        {
                            cliente.CentroCusto.Add(item);
                        }
                    }

                    ClienteUpdateCommand clienteUpdateCommand = new ClienteUpdateCommand(cliente.Id, oCli.CodigoCliente, cliente.FilialId, oCli.RazaoSocial, oCli.Cgc, oCli.Ie, oCli.Endereco, cliente.Numero, oCli.Cep, oCli.Cidade, oCli.UF, oCli.ClienteAtivo, cliente.CentroCusto);
                    _ = await _mediator.Send(clienteUpdateCommand, cancellationToken);
                }else
                {
                    ClienteCreateCommand clienteCommand = new ClienteCreateCommand(oCli.CodigoCliente, oCli.RazaoSocial, oCli.Cgc, oCli.Ie, oCli.Endereco,
                                                               "", oCli.Cep, oCli.Cidade, oCli.UF, 0, oCli.ClienteAtivo, centros);
                    _ = await _mediator.Send(clienteCommand, cancellationToken);
                }

            }
            Console.WriteLine("Clientes.fim: " + DateTime.Now.ToString());

            return true;
        }
    }
}
