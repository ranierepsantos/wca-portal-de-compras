using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Despesas.Command
{
    public sealed record DespesaUpdateCommand(
        int Id,
        int SolicitacaoId,
        int TipoDespesaId,
        DateTime DataEvento,
        decimal Valor,
        string? NumeroFiscal,
        string? ImagePath,
        string? RazaoSocial,
        string? CNPJ,
        string? InscricaoEstadual,
        string? Motivo,
        string? Origem,
        string? Destino,
        decimal? KmPercorrido
    ) : IRequest<ErrorOr<Despesa>>;

    internal sealed class DespesaUpdateCommandHandle : IRequestHandler<DespesaUpdateCommand, ErrorOr<Despesa>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _rm;

        public DespesaUpdateCommandHandle(IMapper mapper, IRepositoryManager rm)
        {
            _mapper = mapper;
            _rm = rm;
        }

        public async Task<ErrorOr<Despesa>> Handle(DespesaUpdateCommand request, CancellationToken cancellationToken)
        {
            Despesa? despesa = await _rm.DespesaRepository.ToQuery()
                            .FirstOrDefaultAsync(q => q.Id.Equals(request.Id), cancellationToken: cancellationToken);

            if (despesa == null)
            {
                return Error.NotFound(description: $"Despesa #{request.Id}, não localizada!");
            }

            string imagePath = despesa.ImagePath;

            if (HandleFile.IsBase64(request.ImagePath))
            {
                HandleFile.DeleteFile(despesa.ImagePath);

                imagePath = HandleFile.SaveFile(request.ImagePath);
            }

            despesa = _mapper.Map<Despesa>(request);
            despesa.ImagePath = imagePath;

            _rm.DespesaRepository.Update(despesa);

            await _rm.SaveAsync();

            return despesa;
        }
    }
}
