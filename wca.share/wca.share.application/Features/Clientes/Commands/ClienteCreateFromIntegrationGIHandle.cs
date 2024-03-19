using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Persistence;


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
            
            Console.WriteLine("Clientes.total: " + clientes.Count() );

            foreach(var oCli in  clientes)
            {
                var cliente = await _repository.ClienteRepository.ToQuery()
                                    .Where(q => q.CNPJ.Trim().Equals(oCli.Cgc.Trim().ToString()) && q.CodigoCliente.Equals(oCli.CodigoCliente))
                                    .Include(i => i.CentroCusto)
                                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                
                if (cliente != null)
                {
                    ClienteUpdateCommand clienteUpdateCommand = new ClienteUpdateCommand(cliente.Id, oCli.CodigoCliente, cliente.FilialId, oCli.RazaoSocial, oCli.Cgc, oCli.Ie, oCli.Endereco, cliente.Numero, oCli.Cep, oCli.Cidade, oCli.UF, oCli.ClienteAtivo, cliente.CentroCusto);
                    _ = await _mediator.Send(clienteUpdateCommand, cancellationToken);
                }else
                {
                    ClienteCreateCommand clienteCommand = new ClienteCreateCommand(oCli.CodigoCliente, oCli.RazaoSocial, oCli.Cgc, oCli.Ie, oCli.Endereco,
                                                               "", oCli.Cep, oCli.Cidade, oCli.UF, 0, oCli.ClienteAtivo);
                    _ = await _mediator.Send(clienteCommand, cancellationToken);
                }

            }
            Console.WriteLine("Clientes.fim: " + DateTime.Now.ToString());

            return true;
        }
    }
}
