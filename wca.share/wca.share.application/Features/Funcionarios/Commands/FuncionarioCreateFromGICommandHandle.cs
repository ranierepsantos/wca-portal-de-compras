using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Integration.GI.Models;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Commands
{
    public record FuncionarioCreateFromGICommand(int? CodigoCliente) : IRequest<ErrorOr<bool>>;
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
            Console.WriteLine($"Funcionarios.FromGI.início: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

            try
            {
                // 1. Limpeza de Logs (Mantida, mas com melhor formatação SQL)
                // Nota: Considere usar um método no repositório com parâmetros em vez de string literal para segurança (SQL Injection).
                await _repository.ExecuteCommandAsync("DELETE FROM EventLogGi WHERE data_hora <=convert(varchar, DATEADD(day, -5, GETDATE()),120);");

                var query = _repository.GetDbSet<Cliente>()
                            .AsNoTracking();

                if (request.CodigoCliente != null)
                {
                    query = query.Where(q => q.CodigoCliente == request.CodigoCliente);
                }else
                {
                    query = query.Where(q => q.CodigoCliente != null && q.CodigoCliente > 0);
                }

                var clientes = await query.OrderBy(o => o.Id).ToListAsync(cancellationToken: cancellationToken);

                // 3. Processar por cliente
                foreach (var cliente in clientes)
                {
                    await ProcessarFuncionariosDoCliente(cliente, cancellationToken);
                }

                // 4. Salvar quaisquer logs pendentes que podem ter sido adicionados durante o processo
                await _repository.SaveAsync();

                Console.WriteLine($"Funcionarios.FromGI.termíno: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                return true;
            }
            catch (Exception ex)
            {
                // Tratar erros gerais fora do loop
                Console.WriteLine($"Error geral: {ex.Message}");
                Console.WriteLine($"Error.InnerException: {ex.InnerException?.Message}");

                _repository.GetDbSet<EventLogGi>().Add(new EventLogGi()
                {
                    Log = $"Erro fatal no Handle. Mensagem: {ex.Message}",
                    Entidade = "Fatal"
                });
                await _repository.SaveAsync();

                // Retorna um ErrorOr em caso de falha completa
                return Error.Failure(description: $"Message: {ex.Message}\nInner Exception: {ex.InnerException?.Message}");
            }
        }

        private async Task ProcessarFuncionariosDoCliente(Cliente cliente, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Buscar Funcionários da Integração (Por Cliente)
                WhereCondition whereCondition = new();
                whereCondition.Conditions.Add(new Condition()
                {
                    Campo = "codigocliente",
                    Valor = cliente.CodigoCliente?.ToString()
                });

                var funcionariosIntegracao = await _gi.FuncionarioGetAllJsonAsync(whereCondition);

                if (!funcionariosIntegracao.Any()) return;

                // Coleta todas as matrículas para buscar todos os funcionários existentes de uma só vez (elimina o N+1 de Funcionario)
                var matriculasIntegracao = funcionariosIntegracao.Select(f => f.eSocialMatricula).ToList();

                var funcionariosExistentes = await _repository.GetDbSet<Funcionario>()
                    .AsNoTracking()
                    .Where(q => matriculasIntegracao.Contains(q.eSocialMatricula))
                    .ToListAsync(cancellationToken: cancellationToken);

                var funcExistentesDict = funcionariosExistentes.ToDictionary(f => f.eSocialMatricula, f => f);

                // Coleta todos os códigos de centro de custo para o cliente atual
                var codigosCentroCustoIntegracao = funcionariosIntegracao
                    .Where(f => !string.IsNullOrEmpty(f.CodigoCentroCusto.ToString()))
                    .Select(f => f.CodigoCentroCusto)
                    .Distinct()
                    .ToList();

                var centrosCustoExistentes = await _repository.GetDbSet<CentroCusto>()
                    .AsNoTracking()
                    .Where(q => q.ClienteId == cliente.Id && codigosCentroCustoIntegracao.Contains(q.Codigo))
                    .ToListAsync(cancellationToken: cancellationToken);

                var centrosCustoDict = centrosCustoExistentes.ToDictionary(cc => cc.Codigo, cc => cc);

                foreach (var ofunc in funcionariosIntegracao)
                {
                    try
                    {
                        Funcionario? funcExistente = funcExistentesDict.GetValueOrDefault(ofunc.eSocialMatricula);

                        if (funcExistente is null)
                        {
                            // Processo de Criação (Funcionario não existe)
                            await HandleFuncionarioCreate(cliente, ofunc, centrosCustoDict, cancellationToken);
                        }
                        else
                        {
                            if (funcExistente.ClienteId != cliente.Id)
                            {
                                // Tenta obter o Centro de Custo (O(1))
                                if (!centrosCustoDict.TryGetValue(ofunc.CodigoCentroCusto, out CentroCusto? centroCusto) || centroCusto is null)
                                {
                                    // Centro de custo não encontrado (Logar erro)
                                    _repository.GetDbSet<EventLogGi>().Add(new EventLogGi()
                                    {
                                        Log = $"Cliente Id Anterior: {funcExistente.ClienteId}, cliente ID atual: {cliente.Id}, funcionário: {ofunc.CodigoFuncionario} - {ofunc.Nome}, código cliente: {cliente.CodigoCliente}, código centro de custo: {ofunc.CodigoCentroCusto}, centro de custo não localizado!",
                                        Entidade = "Func.Cliente.Change"
                                    });
                                }
                                else
                                {
                                    funcExistente.CentroCustoId = centroCusto.Id;
                                }
                                funcExistente.ClienteId = cliente.Id;
                            }
                            // Processo de Atualização (Funcionario existe)
                            await HandleFuncionarioUpdate(funcExistente, ofunc, cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Tratar erros individuais do Funcionário e logar
                        await LogFuncionarioError(ofunc, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessarFuncionariosDoCliente > Error.message: {ex.Message}");
                if (ex.InnerException?.Message != null)
                    Console.WriteLine($"ProcessarFuncionariosDoCliente > Error.InnerException: {ex.InnerException?.Message}");
                
                _repository.GetDbSet<EventLogGi>().Add(new EventLogGi()
                {
                    Log = $"ProcessarFuncionariosDoCliente - Erro: {ex.Message}",
                    Entidade = "Func.Fatal"
                });
            }
        }

        private async Task HandleFuncionarioCreate(Cliente cliente, dynamic ofunc, Dictionary<int, CentroCusto> centrosCustoDict, CancellationToken cancellationToken)
        {
            try
            {
                // Tenta obter o Centro de Custo (O(1))
                if (!centrosCustoDict.TryGetValue(ofunc.CodigoCentroCusto, out CentroCusto? centroCusto) || centroCusto is null)
                {
                    // Centro de custo não encontrado (Logar erro)
                    _repository.GetDbSet<EventLogGi>().Add(new EventLogGi()
                    {
                        Log = $"{ofunc.CodigoFuncionario} - {ofunc.Nome}, código cliente: {cliente.CodigoCliente}, código centro de custo: {ofunc.CodigoCentroCusto}, centro de custo não localizado!",
                        Entidade = "Funcionario"
                    });
                    // Não é necessário SaveAsync() aqui, o Save final no Handle será suficiente.
                    return;
                }

                // Criação do comando
                FuncionarioCreateCommand command = new(
                    ofunc.Nome,
                    cliente.Id,
                    centroCusto.Id,
                    ofunc.DataAdmissao,
                    ofunc.CodigoFuncionario,
                    ofunc.DataDemissao,
                    ofunc.Email,
                    ofunc.SmsdddCel,
                    ofunc.SmsNroCel,
                    ofunc.eSocialMatricula
                );

                _ = await _mediator.Send(command, cancellationToken);
            }
            catch (Exception ex)
            {
                LogFuncionarioError(ofunc, ex);
            }
        }

        private async Task HandleFuncionarioUpdate(Funcionario funcExistente, dynamic ofunc, CancellationToken cancellationToken)
        {
            try
            {
                // Atualização do comando (usando os IDs existentes)
                FuncionarioUpdateCommand command = new(
                    funcExistente.Id,
                    ofunc.Nome,
                    funcExistente.ClienteId,
                    funcExistente.CentroCustoId, // Mantendo o CentroCustoId existente (sua lógica original)
                    ofunc.DataAdmissao,
                    ofunc.CodigoFuncionario,
                    ofunc.DataDemissao,
                    ofunc.Email,
                    ofunc.SmsdddCel,
                    ofunc.SmsNroCel,
                    ofunc.eSocialMatricula
                );

                _ = await _mediator.Send(command, cancellationToken);
            }
            catch (Exception ex)
            {
                LogFuncionarioError(ofunc, ex);
            }

        }

        private async Task LogFuncionarioError(dynamic ofunc, Exception ex)
        {
            Console.WriteLine($"Error.message: {ex.Message}");
            if (ex.InnerException?.Message != null)  
                Console.WriteLine($"Error.InnerException: {ex.InnerException?.Message}");
            
            Console.WriteLine($"Funcionário com erro - {ofunc.CodigoFuncionario} - {ofunc.Nome}, código cliente: {ofunc.CodigoCliente}, centro de custo: {ofunc.CodigoCentroCusto}");

            _repository.GetDbSet<EventLogGi>().Add(new EventLogGi()
            {
                Log = $"Funcionário {ofunc.CodigoFuncionario} - Código cliente: {ofunc.CodigoCliente} - Erro: {ex.Message}",
                Entidade = "Func.Error"
            });
            // Não é necessário SaveAsync() aqui, o Save final no Handle será suficiente.
        }
    }
}
