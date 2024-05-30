using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Integration;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Notificacoes.Commands;
using wca.reembolso.application.Features.SolicitacaoHistoricos.Commands;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.application.Features.Solicitacoes.Behaviors;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Commands
{
    public record SolicitacaoUpdateCommand(
        int Id,
        int ClienteId,
        string Projeto,
        string Objetivo,
        DateTime? PeriodoInicial,
        DateTime? PeriodoFinal,
        decimal ValorAdiantamento,
        decimal ValorDespesa,
        int Status,
        IList<Despesa> Despesa, 
        int[] Notificar
    ) : IRequest<ErrorOr<SolicitacaoResponse>>;

    internal class SolicitacaoUpdateCommandHandler : IRequestHandler<SolicitacaoUpdateCommand, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoUpdateCommandHandler> _logger;
        private readonly IChatBotMessageHandle _chatbot;
        public SolicitacaoUpdateCommandHandler(IMediator mediator, IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoUpdateCommandHandler> logger, IChatBotMessageHandle chatbot)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _chatbot = chatbot;
        }

        async Task<ErrorOr<SolicitacaoResponse>> IRequestHandler<SolicitacaoUpdateCommand, ErrorOr<SolicitacaoResponse>>.Handle(SolicitacaoUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SolicitacaoUpdateCommandHandler - validation");
            
            // validar dados
            var validator = new SolicitacaoUpdateCommandBehavior();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.PropertyName, x.ErrorMessage));
                return errors;
            }
            // localizar cliente
            var querie = new SolicitacaoByIdQuerie(request.Id);

            var findResult = await _mediator.Send(querie, cancellationToken);

            if (findResult.IsError) return findResult;

            var dado = _mapper.Map<Solicitacao>(findResult.Value);

            // remover despesas que foram excluídas
            _logger.LogInformation("SolicitacaoUpdateCommandHandler - removendo despesas que foram excluídas");
            List<Despesa> despesasRemover = dado.Despesa
                .Where(x => !request.Despesa.Where(q => q.Id == x.Id).Any())
                .Where(x => x.Id != 0)
                .ToList();

            foreach (var item in despesasRemover)
            {
                var despesa = _repository.DespesaRepository.ToQuery().Where(q => q.Id.Equals(item.Id)).FirstOrDefault();
                if (despesa != null)
                {
                    HandleFile.DeleteFile(despesa.ImagePath);
                    _repository.DespesaRepository.Delete(despesa);
                }
            }

            // excluir imagem de despesa que trocou de imagem
            _logger.LogInformation("SolicitacaoUpdateCommandHandler - excluindo imagens que foram trocadas");
            List<string> removerImagens = dado.Despesa
                .Where(x => request.Despesa.Where(q => q.Id == x.Id && q.ImagePath != x.ImagePath).Any())
                .Select(f => f.ImagePath)
                .ToList();

            for (int idx = 0; idx < removerImagens.Count; idx++)
            {
                HandleFile.DeleteFile(removerImagens[idx]);
            }

            // salvar imagens de despesas que trocaram imagem ou são novas
            _logger.LogInformation("SolicitacaoUpdateCommandHandler - salvando imagens");
            for (int idx = 0; idx < request.Despesa.Count; idx++)
            {
                if (HandleFile.IsBase64(request.Despesa[idx].ImagePath))
                {
                    request.Despesa[idx].ImagePath = HandleFile.SaveFile(request.Despesa[idx].ImagePath);
                }
            }

            // mapear para cliente e adicionar
            _mapper.Map(request, dado);

            if (findResult.Value.Status == 4)
                dado.Status = findResult.Value.StatusAnterior;

            //registra a data da alteração de status
            dado.DataStatus = DateTime.Now;

            _repository.SolicitacaoRepository.Update(dado);

            await _repository.SaveAsync();

            //Criar evento
            var evento = new SolicitacaoHistorioCreateCommand(dado.Id, $"Solicitação atualizada!");
            await _mediator.Send(evento, cancellationToken);


            // gerar notificação
            var status = await _repository.StatusSolicitacaoRepository.ToQuery().FirstOrDefaultAsync(q => q.Id.Equals(dado.Status), cancellationToken: cancellationToken);

            for (var ii = 0; ii < request.Notificar.Length; ii++)
            {
                string mensagem = status.TemplateNotificacao.Replace("{id}", dado.Id.ToString());

                var notificacao = new NotificacaoCreateCommand(request.Notificar[ii], mensagem, dado.GetType().Name, dado.Id);

                await _mediator.Send(notificacao, cancellationToken);
            }

            findResult = await _mediator.Send(querie, cancellationToken);

            if (!findResult.IsError)
                await _chatbot.SolicitacaoSendMessageAsync(request.Notificar, findResult.Value, cancellationToken);

            //3. mapear para SolicitacaoResponse
            return findResult;
        }
    }
}
