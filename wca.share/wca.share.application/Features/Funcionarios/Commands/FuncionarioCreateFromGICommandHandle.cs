using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Integration.GI.Models;
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

            var clientes = await _repository.GetDbSet<Cliente>().AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

            foreach (var cliente in clientes)
            {
                try
                {
                    WhereCondition whereCondition = new();
                    whereCondition.Conditions.Add(new Condition()
                    {
                        Campo = "codigocliente",
                        Valor = cliente.CodigoCliente?.ToString()
                    });

                    var funcionarios = await _gi.FuncionarioGetAllJsonAsync(whereCondition);

                    foreach (var ofunc in funcionarios)
                    {
                        try
                        {
                            Funcionario? _func = await _repository.GetDbSet<Funcionario>()
                            .AsNoTracking()
                            .Where(q => q.eSocialMatricula == ofunc.eSocialMatricula)
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                            if (_func is null)
                            {
                                //localizar centro de custo
                                CentroCusto? centroCusto = await _repository.GetDbSet<CentroCusto>()
                                                                            .Where(q => q.ClienteId == cliente.Id && q.Codigo == ofunc.CodigoCentroCusto)
                                                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                                if (centroCusto is not null)
                                {
                                    FuncionarioCreateCommand command = new(ofunc.Nome, cliente.Id, centroCusto.Id, ofunc.DataAdmissao, ofunc.CodigoFuncionario,
                                        ofunc.DataDemissao, ofunc.Email, ofunc.SmsdddCel, ofunc.SmsNroCel, ofunc.eSocialMatricula);

                                    _ = await _mediator.Send(command, cancellationToken);
                                }
                                else
                                {
                                    _repository.GetDbSet<EventLogGi>().Add(new EventLogGi()
                                    {
                                        Log = $"{ofunc.CodigoFuncionario} - {ofunc.Nome}, , código cliente: {ofunc.CodigoCliente}, código centro de custo: {ofunc.CodigoCentroCusto}, centro de custo não localizado!",
                                        Entidade = "Funcionário"

                                    });
                                   await _repository.SaveAsync();
                                }
                            }
                            else
                            {
                                FuncionarioUpdateCommand command = new(_func.Id, ofunc.Nome, _func.ClienteId, _func.CentroCustoId, ofunc.DataAdmissao, ofunc.CodigoFuncionario,
                                                                        ofunc.DataDemissao, ofunc.Email, ofunc.SmsdddCel, ofunc.SmsNroCel, ofunc.eSocialMatricula);
                                _ = await _mediator.Send(command, cancellationToken);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error.message: " + ex.Message);
                            Console.WriteLine("Error.InnerException: " + ex.InnerException?.Message);
                            Console.WriteLine($"Funcionário com erro - {ofunc.CodigoFuncionario} - {ofunc.Nome}, código cliente: {ofunc.CodigoCliente}, centro de custo: {ofunc.CodigoCentroCusto}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error.cliente: {cliente.CodigoCliente} - {cliente.Nome}");
                    Console.WriteLine("Error.message: " + ex.Message);
                    Console.WriteLine("Error.InnerException: " + ex.InnerException?.Message);
                }
            }
            Console.WriteLine("Funcionarios.FromGI.termíno: " + DateTime.Now.ToString());
            return true;

            
        }
    }
}
