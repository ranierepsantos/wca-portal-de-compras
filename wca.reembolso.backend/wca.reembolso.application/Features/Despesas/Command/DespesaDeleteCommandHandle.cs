using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return true;
        }
    }
}
