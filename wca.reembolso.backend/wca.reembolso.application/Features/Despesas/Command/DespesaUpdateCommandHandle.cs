using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Commands;
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
        decimal? KmPercorrido,
        int? Aprovada,
        string? Observacao
    ) : IRequest<ErrorOr<Despesa>>;

    internal sealed class DespesaUpdateCommandHandle : IRequestHandler<DespesaUpdateCommand, ErrorOr<Despesa>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _rm;
        private ILogger<DespesaUpdateCommandHandle> _logger;

        public DespesaUpdateCommandHandle(IMapper mapper, IRepositoryManager rm, ILogger<DespesaUpdateCommandHandle> logger)
        {
            _mapper = mapper;
            _rm = rm;
            _logger = logger;
        }

        public async Task<ErrorOr<Despesa>> Handle(DespesaUpdateCommand request, CancellationToken cancellationToken)
        {
            try
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

                string sqlCommand = "update s set valor_despesa = dd.valor_despesa from solicitacoes s " +
                                    "inner join " +
                                    "(  select solicitacao_id, sum(valor) valor_despesa " +
                                    "   from despesas group by solicitacao_id " +
                                    ") dd on dd.solicitacao_id = s.id " +
                                   $"where id = {request.SolicitacaoId}";

                await _rm.ExecuteCommandAsync(sqlCommand);
                return despesa;

            }
            catch (Exception e)
            {   
                _logger.LogError($"Erro ao atualizar despesa - {e.Message}", e);
                throw new Exception($"Erro ao atualizar despesa - {e.Message}");
            }
        }
    }
}
