using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Despesas.Command
{
    public record DespesaDeleteCommand(int Id): IRequest<ErrorOr<bool>>;

    internal sealed class DespesaDeleteCommandHandle : IRequestHandler<DespesaDeleteCommand, ErrorOr<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _rm;

        public DespesaDeleteCommandHandle(IMapper mapper, IRepositoryManager rm)
        {
            _mapper = mapper;
            _rm = rm;
        }

        public async Task<ErrorOr<bool>> Handle(DespesaDeleteCommand request, CancellationToken cancellationToken)
        {

            Despesa? despesa = await _rm.DespesaRepository.ToQuery()
                            .FirstOrDefaultAsync(q => q.Id.Equals(request.Id), cancellationToken: cancellationToken);

            if (despesa == null)
            {
                return Error.NotFound(description: $"Despesa #{request.Id}, não localizada!");
            }

            _rm.DespesaRepository.Delete(despesa);
            await _rm.SaveAsync();

            string sqlCommand = "update s set valor_despesa = dd.valor_despesa from solicitacoes s " +
                                "inner join " +
                                "(  select solicitacao_id, sum(valor) valor_despesa " +
                                "   from despesas group by solicitacao_id "+
                                ") dd on dd.solicitacao_id = s.id " +
                               $"where id = {despesa.SolicitacaoId}";

            await _rm.ExecuteCommandAsync(sqlCommand);

            return true;
        }
    }
}
