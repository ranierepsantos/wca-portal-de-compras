using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
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
        IList<Despesa> Despesa
    ) : IRequest<ErrorOr<SolicitacaoResponse>>;

    public class SolicitacaoUpdateCommandHandler : IRequestHandler<SolicitacaoUpdateCommand, ErrorOr<SolicitacaoResponse>>
    {
        private readonly IRepository<Solicitacao> _reposistory;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoUpdateCommandHandler> _logger;
        private readonly IRepository<Despesa> _despesaRepository;

        public SolicitacaoUpdateCommandHandler(IMediator mediator, IRepository<Solicitacao> reposistory, IMapper mapper, ILogger<SolicitacaoUpdateCommandHandler> logger, IRepository<Despesa> despesaRepository)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _despesaRepository = despesaRepository;
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

            var findResult = await _mediator.Send(querie);

            if (findResult.IsError) return findResult;

            var dado = _mapper.Map<Solicitacao>(findResult.Value);

            // remover despesas que foram excluídas
            _logger.LogInformation("SolicitacaoUpdateCommandHandler - removendo despesas que foram excluídas");
            List<Despesa> despesasRemover = dado.Despesa
                .Where(x => request.Despesa.Where(q => q.Id == x.Id).Count() == 0)
                .Where(x => x.Id != 0)
                .ToList();

            foreach (var item in despesasRemover)
            {
                var despesa = _despesaRepository.ToQuery().Where(q => q.Id.Equals(item.Id)).FirstOrDefault();
                if (despesa != null)
                {
                    _despesaRepository.Delete(despesa);
                }
            }

            // excluir imagem de despesa que trocou de imagem
            _logger.LogInformation("SolicitacaoUpdateCommandHandler - excluindo imagens que foram trocadas");
            List<string> removerImagens = dado.Despesa
                .Where(x => request.Despesa.Where(q => q.Id == x.Id && q.ImagePath != x.ImagePath).Count() > 0)
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
                if (HandleFile.IsBase64Image(request.Despesa[idx].ImagePath))
                {
                    request.Despesa[idx].ImagePath = HandleFile.SaveImage(request.Despesa[idx].ImagePath);
                }
            }

            // mapear para cliente e adicionar
            _mapper.Map(request, dado);
            
            _reposistory.Update(dado);

            await _reposistory.SaveChangesAsync();

            //3. mapear para SolicitacaoResponse
            return _mapper.Map<SolicitacaoResponse>(dado);
        }
    }
}
