using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Contracts.Persistence;

namespace wca.reembolso.application.Features.Despesas.Queries
{
    public record DespesaChecarByNotaAndCnpjQuery(
        string CNPJ,
        string NotaFiscal,
        int DespesaId = 0
    ) : IRequest<ErrorOr<bool>>;

    public sealed class DespesaChecarByNotaAndCnpjQueryHandle : IRequestHandler<DespesaChecarByNotaAndCnpjQuery, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<DespesaChecarByNotaAndCnpjQuery> _logger;

        public DespesaChecarByNotaAndCnpjQueryHandle(IRepositoryManager repository, ILogger<DespesaChecarByNotaAndCnpjQuery> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(DespesaChecarByNotaAndCnpjQuery request, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, $"Despesa Checando se duplicado {request.CNPJ}, nf: {request.NotaFiscal}");

            var despesa = await _repository.DespesaRepository.ToQuery()
                .Where(q => q.CNPJ == request.CNPJ &&
                            q.NumeroFiscal.ToLower().Equals(request.NotaFiscal.ToLower()) &&
                           !q.Id.Equals(request.DespesaId))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return despesa is not null;
        }
    }
}
