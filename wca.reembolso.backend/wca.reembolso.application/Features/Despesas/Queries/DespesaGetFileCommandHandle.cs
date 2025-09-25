using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Common.dtos;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Despesas.Queries
{
    public record DespesaGetFileQuery(int Id) : IRequest<ErrorOr<AzureFile>>;

    internal sealed class DespesaGetFileQueryHandle : IRequestHandler<DespesaGetFileQuery, ErrorOr<AzureFile>>
    {
        private readonly IRepositoryManager _rm;
        private readonly HandleFile _handleFile;

        public DespesaGetFileQueryHandle( IRepositoryManager rm, HandleFile handleFile)
        {
            _rm = rm;
            _handleFile = handleFile;
        }

        public async Task<ErrorOr<AzureFile>> Handle(DespesaGetFileQuery request, CancellationToken cancellationToken)
        {
            Despesa? despesa = await _rm.DespesaRepository.ToQuery()
                                .FirstOrDefaultAsync(q => q.Id.Equals(request.Id), cancellationToken: cancellationToken);
            if (despesa is null)
            {
                return Error.NotFound(description: $"Despesa #{request.Id}, não localizada!");
            }

            if (string.IsNullOrEmpty(despesa.ImagePath))
            {
                return Error.NotFound(description: $"Despesa #{request.Id}, não contém anexo!");
            }
            
            return await _handleFile.GetFile(despesa.ImagePath);
        }
    }
}
