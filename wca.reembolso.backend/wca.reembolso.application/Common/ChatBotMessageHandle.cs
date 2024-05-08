using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;
using wca.reembolso.application.Contracts.Integration;
using wca.reembolso.application.Contracts.Integration.NorgeChatBot;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Common;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.domain.Common.Enum;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Common
{
    internal class ChatBotMessageHandle : IChatBotMessageHandle
    {
        private readonly IRepositoryManager _repository;
        private readonly IIntegrationNorgeChatBot _norgeBot;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatBotMessageHandle> _logger;
        private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        private string faturamentoFirstMessage = @"{saudacao.dia}!\nAqui é o assistente WCA de apoio aos gestores clientes. Há uma relação de despesas aguardando sua análise para faturamento.\nRELAÇÃO DE DESPESAS: {faturamento.codigo}\nVALOR TOTAL: {faturamento.valor}\nCENTRO DE CUSTOS: {faturamento.centrocusto}\nDATA DA PRIMEIRA DESPESA: {faturamento.dataPrimeiraDespesa}\nDATA DA DESPESA MAIS RECENTE: {faturamento.dataUltimaDespesa}\nPara acessar o sistema clique no link: https://app.wcabrasil.com.br";
        private string faturamentoSevenDaysMessage = @"{saudacao.dia}!\nAqui é o assistente WCA de apoio aos gestores clientes. Há faturamentos que aguardam os correspondentes números de ordem de compra ou arquivos equivalentes (P.O.).\nCENTRO DE CUSTOS: {faturamento.centrocusto}\nRELAÇÃO DE DESPESAS: {faturamento.codigo}\nDATA DA PRIMEIRA DESPESA: {faturamento.dataPrimeiraDespesa}\nDATA DA DESPESA MAIS RECENTE: {faturamento.dataUltimaDespesa}\nVALOR A SER FATURADO: {faturamento.valor}\nPara acessar o sistema clique no link: https://app.wcabrasil.com.br";
        private string depositoMessage = "{saudacao.dia}!\nAqui é o assistente WCA de apoio aos colaboradores. O depósito em conta corrente correspondente aos valores da solicitação #{solicitacao.codigo} recebida em {solicitacao.datahora} será feito em {datadeposito}, no valor {valordeposito}.";

        public ChatBotMessageHandle(IRepositoryManager repository, IIntegrationNorgeChatBot norgeBot, IMapper mapper, ILogger<ChatBotMessageHandle> logger)
        {
            _repository = repository;
            _norgeBot = norgeBot;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task FaturamentoSendMessageAsync(int[] usersId, int faturamentoId, CancellationToken cancellationToken = default)
        {
            var query = _repository.FaturamentoRepository.ToQuery()
                        .Where(q => q.Id == faturamentoId)
                        .Include(n => n.Cliente)
                        .Include(n => n.CentroCusto)
                        .Include(n => n.FaturamentoItem)
                        .ThenInclude(n => n.Solicitacao);

            var dado = await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);

            await FaturamentoSendMessageAsync(usersId, _mapper.Map<FaturamentoChatBot>(dado),true, cancellationToken);
        }
        public async Task FaturamentoSendMessageAsync(int[] usersId, FaturamentoChatBot faturamento, bool firstSend = false, CancellationToken cancellationToken = default)
        {
            List<Usuario>? usuarios = await _repository.GetDbSet<Usuario>()
                                           .Where(q => (q.Celular != null && q.Celular != "") && usersId.Contains(q.Id))
                                           .ToListAsync(cancellationToken: cancellationToken);
            
            
            string mensagem = firstSend ? faturamentoFirstMessage : faturamentoSevenDaysMessage;

            mensagem = Regex.Unescape(FaturamentoMontaMensagem(mensagem, faturamento));
            
            if (usuarios?.Count > 0) 
            {
                foreach (var usuario in usuarios)
                {
                   var response =  await _norgeBot.Send("55" + usuario?.Celular.ToString(), mensagem);
                    if (!string.IsNullOrEmpty(response.Error))
                        _logger.LogError($"norgebot.send.error, number: {"55" + usuario?.Celular.ToString()}, error: {response.Error}");
                }
            }

            await _repository.ExecuteCommandAsync($"update faturamento set data_chatbot_message = getdate() where id ={faturamento.Id}");
        }


        public async Task DepositoSendMessageAsync(SolicitacaoResponse solicitacao, DateTime? datadeposito, decimal? valordeposito, CancellationToken cancellationToken) 
        { 
            string mensagem = Regex.Unescape(SolicitacaoMontaMensagem(depositoMessage, solicitacao));
            mensagem = mensagem.Replace("{datadeposito}", datadeposito?.ToString("dd/MM/yyyy"));
            mensagem = mensagem.Replace("{valordeposito}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", valordeposito));
            await _norgeBot.Send("55" + solicitacao.ColaboradorCelular, mensagem);
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
                                            .Where(q => (q.Celular != null && q.Celular != "") && usersId.Contains(q.Id))
                                            .ToListAsync(cancellationToken: cancellationToken);
                
                foreach (StatusChatBotMensagem message in messages)
                {
                    string mensagem = Regex.Unescape (SolicitacaoMontaMensagem(message.Mensagem, solicitacao));

                    if (message.EnviarPara == (int)EnumNotificaQuem.Usuario && !string.IsNullOrEmpty(solicitacao.ColaboradorCelular))
                    {
                        var response = await _norgeBot.Send("55" + solicitacao.ColaboradorCelular, mensagem);
                        if (!string.IsNullOrEmpty(response.Error))
                            _logger.LogError($"norgebot.send.error, number: {"55" + solicitacao.ColaboradorCelular}, error: {response.Error}");
                    }
                    else
                    {
                        foreach(var usuario in usuarios)
                        {
                            var response = await _norgeBot.Send("55" + usuario?.Celular.ToString(), mensagem);
                            if (!string.IsNullOrEmpty(response.Error))
                                _logger.LogError($"norgebot.send.error, number: {"55" + usuario?.Celular.ToString()}, error: {response.Error}");
                        }
                    }
                }
            
            }
        }

        private string FaturamentoMontaMensagem(string mensagem, FaturamentoChatBot faturamento)
        {

            /*
            {saudacao.dia}
            {faturamento.centrocusto}
            {faturamento.codigo}
            {faturamento.dataPrimeiraDespesa}
            {faturamento.dataUltimaDespesa}
            {faturamento.valor}
            */

            mensagem = mensagem.Replace("{saudacao.dia}", GetSaudacaoDia());
            mensagem = mensagem.Replace("{faturamento.codigo}", faturamento.Id.ToString());
            mensagem = mensagem.Replace("{faturamento.centrocusto}", faturamento.CentroCustoNome);
            mensagem = mensagem.Replace("{faturamento.valor}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", faturamento.Valor));
            mensagem = mensagem.Replace("{faturamento.dataPrimeiraDespesa}", faturamento.DataMenorDespesa.ToString("dd/MM/yyyy"));
            mensagem = mensagem.Replace("{faturamento.dataUltimaDespesa}", faturamento.DataMaiorDespesa.ToString("dd/MM/yyyy"));
            
            return mensagem;
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
                valorDeposito = (solicitacao.Status == 1? solicitacao.ValorAdiantamento: solicitacao.ValorDespesa) - contaSaldo;

            if (mensagem.IndexOf("{func.dataUltimaSolicitacao}") > -1)
                dataUltimaSolicitacao = GetDataUltimaSolicitacao(solicitacao.ColaboradorId, solicitacao.Id);


            mensagem = mensagem.Replace("{saudacao.dia}", GetSaudacaoDia());
            mensagem = mensagem.Replace("{solicitacao.codigo}", solicitacao.Id.ToString());
            mensagem = mensagem.Replace("{colaborador.nome}", solicitacao.ColaboradorNome);
            mensagem = mensagem.Replace("{solicitacao.centrocusto}", solicitacao.CentroCustoNome);
            mensagem = mensagem.Replace("{solicitacao.datahora}", solicitacao.DataSolicitacao.ToString("dd/MM/yyyy"));
            mensagem = mensagem.Replace("{solicitacao.valor}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", solicitacaoValor));
            mensagem = mensagem.Replace("{conta.saldo}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", contaSaldo));
            mensagem = mensagem.Replace("{func.valordeposito}", string.Format(new CultureInfo("pt-BR", true), "{0:c2}", valorDeposito < 0 ? 0 : valorDeposito));
            mensagem = mensagem.Replace("{func.dataUltimaSolicitacao}", dataUltimaSolicitacao?.ToString("dd/MM/yyyy")?? "Não há");
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
            var valor = solicitacao.ValorDespesa;

            if (solicitacao.Status == 1)
                valor = solicitacao.ValorAdiantamento;

            return valor;

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
