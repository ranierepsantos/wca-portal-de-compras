using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Despesas.Command
{
    public sealed record DespesaCreateCommand (
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
        int? Aprovada
    ): IRequest<ErrorOr<Despesa>>;

    internal sealed class DespesaCreateCommandHandle : IRequestHandler<DespesaCreateCommand, ErrorOr<Despesa>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _rm;
        private readonly ILogger<DespesaCreateCommandHandle> _logger;
        private readonly HandleFile _handleFile;

        public DespesaCreateCommandHandle(IMapper mapper, IRepositoryManager rm, ILogger<DespesaCreateCommandHandle> logger, HandleFile handleFile)
        {
            _mapper = mapper;
            _rm = rm;
            _logger = logger;
            _handleFile = handleFile;
        }

        public async Task<ErrorOr<Despesa>> Handle(DespesaCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Despesa despesa = _mapper.Map<Despesa>(request);

                if (_handleFile.IsBase64(despesa.ImagePath))
                {
                    despesa.ImagePath = await _handleFile.SaveFileAsync(despesa.ImagePath);
                }

                _rm.DespesaRepository.Create(despesa);

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
                _logger.LogError($"Erro ao criar despesa - {e.Message}", e);
                throw new Exception($"Erro ao criar despesa - {e.Message}");
                
            }
        }
    }
}
