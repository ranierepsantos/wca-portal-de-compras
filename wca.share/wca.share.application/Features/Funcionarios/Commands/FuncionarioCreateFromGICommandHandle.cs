using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Commands
{
    public record FuncionarioCreateFromGICommand() : IRequest<ErrorOr<bool>>;
    internal sealed class FuncionarioCreateFromGICommandHandle : IRequestHandler<FuncionarioCreateFromGICommand, ErrorOr<bool>>
    {
        private readonly IIntegrationGI _gi;
        private readonly IMediator _mediator;
        private readonly IRepositoryManager _repository;

        public FuncionarioCreateFromGICommandHandle(IIntegrationGI gi, IMediator mediator, IRepositoryManager repository)
        {
            _gi = gi;
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ErrorOr<bool>> Handle(FuncionarioCreateFromGICommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Funcionarios.FromGI.início: " + DateTime.Now.ToString());
            var funcionarios = await _gi.FuncionarioGetAllAsync();

            foreach(var ofunc in funcionarios)
            {
                Funcionario? _func = await _repository.GetDbSet<Funcionario>()
                    .AsNoTracking()
                    .Where(q => q.CodigoFuncionario == ofunc.CodigoFuncionario)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (_func is null)
                {
                    //localizar cliente
                    Cliente? cliente = await _repository.ClienteRepository
                                                        .ToQuery()
                                                        .Where(q => q.CodigoCliente == ofunc.CodigoCliente)
                                                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                    if (cliente is not null)
                    {
                        //localizar centro de custo
                        CentroCusto? centroCusto = await _repository.GetDbSet<CentroCusto>()
                                                                    .Where(q => q.ClienteId == cliente.Id && q.Codigo == ofunc.CodigoCentroCusto)
                                                                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                        if (centroCusto is not null)
                        {
                            FuncionarioCreateCommand command = new(ofunc.Nome, cliente.Id, centroCusto.Id, ofunc.DataAdmissao, ofunc.CodigoFuncionario,
                                ofunc.DataDemissao, ofunc.Email, ofunc.SmsdddCel, ofunc.SmsNroCel, ofunc.Pis?.ToString());

                            _ = await _mediator.Send(command, cancellationToken);
                        }
                    }
                } else
                {
                    FuncionarioUpdateCommand command = new(_func.Id, ofunc.Nome, _func.ClienteId, _func.CentroCustoId, ofunc.DataAdmissao, ofunc.CodigoFuncionario,
                                                           ofunc.DataDemissao, ofunc.Email, ofunc.SmsdddCel, ofunc.SmsNroCel, ofunc.Pis?.ToString());
                    _ = await _mediator.Send(command, cancellationToken);
                }
                
            }
            Console.WriteLine("Funcionarios.FromGI.termíno: " + DateTime.Now.ToString());

            return true;
        }
    }
}
