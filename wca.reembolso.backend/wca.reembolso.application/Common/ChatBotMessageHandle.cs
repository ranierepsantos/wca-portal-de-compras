using MediatR;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using wca.reembolso.application.Contracts;
using wca.reembolso.application.Contracts.NorgeChatBot;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.domain.Common.Enum;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Common
{
    internal class ChatBotMessageHandle : IChatBotMessageHandle
    {
        private readonly IRepositoryManager _repository;
        private readonly IIntegrationNorgeChatBot _norgeBot;
        private readonly IMediator _mediator;
        private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public ChatBotMessageHandle(IRepositoryManager repository, IIntegrationNorgeChatBot norgeBot, IMediator mediator)
        {
            _repository = repository;
            _norgeBot = norgeBot;
            _mediator = mediator;
        }

        public Task FaturamentoSendMessageAsync(int[] usersId, Faturamento faturamento, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task SolicitacaoSendMessageAsync(int[] usersId, SolicitacaoResponse solicitacao, CancellationToken cancellationToken = default)
        {
            
            List<StatusChatBotMensagem>? messages = await _repository
                                                       .GetDbSet<StatusChatBotMensagem>()
                                                       .Where(q => q.StatusSolicitacaoId.Equals(solicitacao.Status) &&
                                                       (q.TipoSolicitacao == solicitacao.TipoSolicitacao || q.TipoSolicitacao == 0))
                                                       .ToListAsync(cancellationToken: cancellationToken);
            if (messages?.Count > 0)
            {
                List<Usuario>? usuarios = await _repository.GetDbSet<Usuario>()
                                            .Where(q => (q.Celular != null || q.Celular != "") && usersId.Contains(q.Id))
                                            .ToListAsync(cancellationToken: cancellationToken);
                
                foreach (StatusChatBotMensagem message in messages)
                {
                    string mensagem = SolicitacaoMontaMensagem(message.Mensagem, solicitacao);

                    if (message.EnviarPara == (int)EnumNotificaQuem.Usuario && !string.IsNullOrEmpty(solicitacao.ColaboradorCelular))
                        await _norgeBot.Send("55" + solicitacao.ColaboradorCelular, mensagem);
                    else
                    {
                        foreach(var usuario in usuarios)
                        {
                            await _norgeBot.Send("55" + usuario?.Celular.ToString(), mensagem);
                        }
                    }
                }
            
            }
        }

        private string SolicitacaoMontaMensagem(string mensagem, SolicitacaoResponse solicitacao)
        {

            /*
            {saudacao.dia} 
            {colaborador.nome}
            {solicitacao.valor}
            {solitacao.datahora}
            {conta.saldo}
            {func.valordeposito}
            {solicitacao.tipo}
            {link}
            {solicitacao.centrocusto}
            {func.dataUltimaSolicitacao}
            */
            decimal contaSaldo = 0M;
            decimal solicitacaoValor = GetSolicitacaoValor(solicitacao);
            decimal valorDeposito = 0M;
            DateTime? dataUltimaSolicitacao = null;
            if (mensagem.IndexOf("{conta.saldo}") > -1 || mensagem.IndexOf("{func.valordeposito}") > -1)
                contaSaldo = GetContaSaldo(solicitacao.ColaboradorId);
            
            if (mensagem.IndexOf("{func.valordeposito}") > -1)
                valorDeposito = solicitacao.ValorDespesa - contaSaldo;

            if (mensagem.IndexOf("{func.dataUltimaSolicitacao}") > -1)
                dataUltimaSolicitacao = GetDataUltimaSolicitacao(solicitacao.ColaboradorId, solicitacao.Id);


            mensagem = mensagem.Replace("{saudacao.dia}", GetSaudacaoDia());
            mensagem = mensagem.Replace("{colaborador.nome}", solicitacao.ColaboradorNome);
            mensagem = mensagem.Replace("{solicitacao.centrocusto}", solicitacao.CentroCustoNome);
            mensagem = mensagem.Replace("{solicitacao.datahora}", solicitacao.DataSolicitacao.ToString("dd/MM/yyyy"));
            mensagem = mensagem.Replace("{solicitacao.valor}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", solicitacaoValor));
            mensagem = mensagem.Replace("{conta.saldo}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", contaSaldo));
            mensagem = mensagem.Replace("{func.valordeposito}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", valorDeposito < 0 ? 0 : valorDeposito));
            mensagem = mensagem.Replace("{solicitacao.datahora}", dataUltimaSolicitacao?.ToString("dd/MM/yyyy")?? "Não há");
            mensagem = mensagem.Replace("{solicitacao.tipo}", solicitacao.TipoSolicitacao ==1 ? "reembolso":  "adiantamento");

            return mensagem;
        }

        private string GetSaudacaoDia()
        {
            int hora = int.Parse(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone).ToString("HH"));

            if (hora >= 4 && hora < 12)
                return "Bom Dia";
            else if (hora >= 12 && hora < 18)
                return "Boa Tarde";
            else 
                return "Boa Noite";
        }

        private static decimal GetSolicitacaoValor(SolicitacaoResponse solicitacao)
        {
            return solicitacao.TipoSolicitacao == (int)EnumTipoSolicitacao.Adiantamento ? 
                                   solicitacao.ValorAdiantamento : 
                                   solicitacao.ValorDespesa;

        }

        private decimal GetContaSaldo(int usuarioId)
        {
            var conta = _repository.GetDbSet<ContaCorrente>().Where(q => q.UsuarioId  == usuarioId).FirstOrDefault();
            
            return conta?.Saldo ?? 0;
        }

        private DateTime? GetDataUltimaSolicitacao(int colaboradorId, int solicitacaoId)
        {
            var ultima = _repository.GetDbSet<Solicitacao>()
                                    .Where(q => q.ColaboradorId.Equals(colaboradorId) && 
                                                q.Id != solicitacaoId)
                                    .OrderByDescending(o => o.DataSolicitacao)
                                    .FirstOrDefault();
            return ultima?.DataSolicitacao;

        }
    }
}
