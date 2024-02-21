using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Vml.Office;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.SolicitacaoHistoricos.Commands;
using wca.share.application.Features.Solicitacoes.Behaviors;
using wca.share.application.Features.Solicitacoes.Common;
using wca.share.domain.Common.Interfaces;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Commands
{
    public record SolicitacaoUpdateCommand(
        int Id, 
        int SolicitacaoTipoId,
        int ClienteId,
        int FuncionarioId,
        int StatusSolicitacaoId,
        string UsuarioAtualizador,
        int? GestorId,
        int? ResponsavelId,
        string? Descricao,
        SolicitacaoComunicado? Comunicado,
        SolicitacaoDesligamento? Desligamento,
        SolicitacaoMudancaBase? MudancaBase,
        List<SolicitacaoArquivo>? Anexos,
        int[]? NotificarUsuarioIds
    ) : IRequest<ErrorOr<SolicitacaoResponse>>;
    internal class SolicitacaoUpdateCommandHandle : IRequestHandler<SolicitacaoUpdateCommand, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<SolicitacaoUpdateCommandHandle> _logger;

        public SolicitacaoUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, IMediator mediator, ILogger<SolicitacaoUpdateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ErrorOr<SolicitacaoResponse>> Handle(SolicitacaoUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando validação");

            SolicitacaoUpdateCommandBehavior validator = new();
            FluentValidation.Results.ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }
            //localizar registro
            _logger.LogInformation($"Localizando solicitação #{request.Id}");
            Solicitacao? dado = await _repository.SolicitacaoRepository.ToQuery().AsNoTracking()
                                .Include(x => x.Comunicado)
                                .Include(x => x.Desligamento)
                                .Include(x => x.Anexos)
                                .Include(x => x.MudancaBase).ThenInclude(x => x.ItensMudanca)
                                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
            if (dado == null)
            {
                _logger.LogError($"Solicitacao #{request.Id} não localizada!");
                return Error.NotFound(description: $"Não localizamos a solicitação #{request.Id}!");
            }

            _mapper.Map(request, dado);

            //remover anexos que foram removidos
            _logger.LogInformation("Removendo anexos");
            List<SolicitacaoArquivo> anexosRemover = dado.Anexos
                .Where(x => !request.Anexos.Where(q => q.Id == x.Id).Any())
                .Where(x => x.Id != 0)
                .ToList();

            foreach (var anexo in anexosRemover)
            {
                HandleFile.DeleteFile(anexo.CaminhoArquivo);
                _repository.GetDbSet<SolicitacaoArquivo>().Remove(anexo);
            }

            //adicionar / atualizar anexos
            for (int idx = 0; idx < request.Anexos?.Count; idx++)
            {
                if (HandleFile.IsBase64(request.Anexos[idx].CaminhoArquivo))
                {
                    request.Anexos[idx].CaminhoArquivo = HandleFile.SaveFile(request.Anexos[idx].CaminhoArquivo);
                    request.Anexos[idx].SolicitacaoId = request.Id;
                    _repository.GetDbSet<SolicitacaoArquivo>().Add(request.Anexos[idx]);
                }
            }

            if (request.SolicitacaoTipoId == (int)EnumTipoSolicitacao.MudancaBase)
            {

                //remover itens de mudança q foram retirados
                _logger.LogInformation("Mudança de base - removendo itens da mudança");

                SolicitacaoMudancaBase? _mud = _repository.MudancaBaseRepository.ToQuery().Where(x => x.SolicitacaoId == request.Id).FirstOrDefault();
                if (_mud is not null)
                {
                    _repository.GetDbSet<SolicitacaoMudancaBase>().Entry(_mud).CurrentValues.SetValues(request.MudancaBase);

                    await _repository.ExecuteCommandAsync($"delete from itemmudancaSolicitacaoMudancabase where SolicitacaoMudancaBaseSolicitacaoId ={request.Id}");

                    List<int> itensMudancaId = request.MudancaBase.ItensMudanca
                    .Select(x => x.Id)
                    .ToList();
                    List<ItemMudanca> items = _repository.ItemMudancaRepository.ToQuery().Where(c => itensMudancaId.Contains(c.Id)).ToList();
                    if (items.Any())
                    {
                        _mud.ItensMudanca.AddRange(items);
                    }
                    _repository.GetDbSet<SolicitacaoMudancaBase>().Update(_mud);

                }
            }
            else if (dado.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Comunicado)
                _repository.GetDbSet<SolicitacaoComunicado>().Entry(dado.Comunicado).State = EntityState.Modified;
            else if (dado.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Desligamento)
                _repository.GetDbSet<SolicitacaoDesligamento>().Entry(dado.Desligamento).State = EntityState.Modified;


            _repository.GetDbSet<Solicitacao>().Entry(dado).State = EntityState.Modified;

            await _repository.SaveAsync();

            //lancar no histórico
            var querie = new SolicitacaoHistorioCreateCommand(dado.Id, $"Solicitação alterada por {request.UsuarioAtualizador}!");
            await _mediator.Send(querie, cancellationToken);




            // mapear para SolicitacaoResponse e retornar
            return _mapper.Map<SolicitacaoResponse>(dado);
        }
    }
}
