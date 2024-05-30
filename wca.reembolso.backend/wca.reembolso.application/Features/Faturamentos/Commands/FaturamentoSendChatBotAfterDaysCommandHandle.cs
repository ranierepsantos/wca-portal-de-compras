using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Integration;
using wca.reembolso.application.Contracts.Integration.WcaCompras;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Commands
{

    public sealed record FaturamentoSendChatBotAfterDaysCommand(int days = 7) : IRequest<ErrorOr<bool>>;
    internal class FaturamentoSendChatBotAfterDaysCommandHandle : IRequestHandler<FaturamentoSendChatBotAfterDaysCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoSendChatBotAfterDaysCommandHandle> _logger;
        private readonly IIntegrationWcaCompras _apiCompras;
        private readonly IChatBotMessageHandle _chatbotHandle;

        public FaturamentoSendChatBotAfterDaysCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoSendChatBotAfterDaysCommandHandle> logger, IIntegrationWcaCompras apiCompras, IChatBotMessageHandle chatbotHandle)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _apiCompras = apiCompras;
            _chatbotHandle = chatbotHandle;
        }

        public async Task<ErrorOr<bool>> Handle(FaturamentoSendChatBotAfterDaysCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Checando faturamento elegíveis de envio");

                Dictionary<int, int[]> CentroGestor = new Dictionary<int, int[]>();
                DateTime _date = DateTime.Now.AddDays(-1 * request.days);

                var query = _repository.FaturamentoRepository.ToQuery()
                            .Where(q => q.Status == 1 &&
                                        q.DataChatBotMessage <= _date)
                            .Include(n => n.Cliente)
                            .Include(n => n.CentroCusto)
                            .Include(n => n.FaturamentoItem)
                            .ThenInclude(n => n.Solicitacao);

                var lista = await query.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

                if (lista.Count > 0)
                {
                    //traz os usuário que tem permissao de aprovar e informar p.o
                    var comprasGestores = await _apiCompras.UsuariosListByPermissao("cliente_faturamento");

                    int[] _ids = comprasGestores.Select(p => p.Id).ToArray();

                    foreach (var item in lista)
                    {
                        //traz usuarios do centros de custo que são gestores clientes
                        if (!CentroGestor.ContainsKey(item.CentroCustoId))
                        {
                            var gestores = await _repository.GetDbSet<UsuarioCentrodeCustos>()
                            .Where(q => q.CentroCustoId == item.CentroCustoId &&
                                        _ids.Contains(q.UsuarioId))
                            .ToListAsync(cancellationToken: cancellationToken);

                            CentroGestor.Add(item.CentroCustoId, gestores.Select(p => p.UsuarioId).ToArray());
                        }

                        int[] notificar = CentroGestor[item.CentroCustoId];

                        await _chatbotHandle.FaturamentoSendMessageAsync(notificar, _mapper.Map<FaturamentoChatBot>(item), cancellationToken: cancellationToken);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error.Message: {ex.Message}");
                _logger.LogError($"Error.InnerException.Message: {ex.InnerException?.Message}");

                return Error.Failure (description: ex.Message);
            }



        }
    }
}
