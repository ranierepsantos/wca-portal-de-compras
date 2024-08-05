using MediatR;
using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts;
using wca.share.application.Contracts.Integration.Email;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Notificacoes.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Common
{
    internal class NotificacaoHandle : INotificacaoHandle
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;
        private readonly IRepositoryManager _repository;

        public static string GetDescricaoTipoSolicitacao(int Tipo)
        {
            return Tipo switch
            {
                (int)EnumTipoSolicitacao.Comunicado => "Comunicado",
                (int)EnumTipoSolicitacao.Desligamento => "Desligamento",
                (int)EnumTipoSolicitacao.Ferias => "Férias",
                (int)EnumTipoSolicitacao.MudancaBase => "Mudança de Base",
                (int)EnumTipoSolicitacao.Vaga => "Vaga",
            };
        }

        public static string GetEntidadeTipoSolicitacao(int Tipo)
        {
            return Tipo switch
            {
                (int)EnumTipoSolicitacao.Comunicado => EnumTipoSolicitacao.Comunicado.ToString(),
                (int)EnumTipoSolicitacao.Desligamento => EnumTipoSolicitacao.Desligamento.ToString(),
                (int)EnumTipoSolicitacao.Ferias => EnumTipoSolicitacao.Ferias.ToString(),
                (int)EnumTipoSolicitacao.MudancaBase => EnumTipoSolicitacao.MudancaBase.ToString(),
                (int)EnumTipoSolicitacao.Vaga => EnumTipoSolicitacao.Vaga.ToString(),
            };
        }


        public NotificacaoHandle(IMediator mediator, IEmailService emailService, IRepositoryManager repository)
        {
            _mediator = mediator;
            _emailService = emailService;
            _repository = repository;
        }

        public async Task SolicitacaoEnviarNotificacaoAsync(int[] usersId, string template, Solicitacao solicitacao, CancellationToken cancellationToken = default)
        {
            List<Usuario> users = await _repository.GetDbSet<Usuario>()
                                  .Include("UsuarioConfiguracoes")
                                  .Where(q => usersId.Contains(q.Id)).ToListAsync(cancellationToken: cancellationToken);

            foreach (Usuario user in users)
            {
                string mensagem = template.Replace("{TipoSolicitacao}", GetDescricaoTipoSolicitacao(solicitacao.SolicitacaoTipoId)).Replace("{id}", solicitacao.Id.ToString());

                var notificacao = new NotificacaoCreateCommand(user.Id, mensagem, GetEntidadeTipoSolicitacao(solicitacao.SolicitacaoTipoId), solicitacao.Id);

                await _mediator.Send(notificacao, cancellationToken);

                if (user.UsuarioConfiguracoes?.NotificarPorEmail ?? false)
                {
                    string assunto = GetEntidadeTipoSolicitacao(solicitacao.SolicitacaoTipoId) + $" - código: {solicitacao.Id}";

                    string body = $"Olá {user.Nome}, <br/> <br/>";
                    body += mensagem;

                    _emailService.SendNotificacao(new string[] { user.Email }, assunto, body);
                }
            }
        }

        public async Task VagaEnviarNotificacaoAsync(int[] usersId, string template, Vaga vaga, CancellationToken cancellationToken = default)
        {
            List<Usuario> users = await _repository.GetDbSet<Usuario>()
                                  .Include("UsuarioConfiguracoes")
                                  .Where(q => usersId.Contains(q.Id)).ToListAsync(cancellationToken: cancellationToken);

            foreach (Usuario user in users)
            {
                string mensagem = template.Replace("{TipoSolicitacao}", GetDescricaoTipoSolicitacao((int) EnumTipoSolicitacao.Vaga )).Replace("{id}", vaga.Id.ToString());

                var notificacao = new NotificacaoCreateCommand(user.Id, mensagem, GetEntidadeTipoSolicitacao((int)EnumTipoSolicitacao.Vaga), vaga.Id);

                await _mediator.Send(notificacao, cancellationToken);

                if (user.UsuarioConfiguracoes?.NotificarPorEmail ?? false)
                {
                    string assunto = GetEntidadeTipoSolicitacao((int)EnumTipoSolicitacao.Vaga) + $" - código: {vaga.Id}";

                    string body = $"Olá {user.Nome}, <br/> <br/>";
                    body += mensagem;

                    _emailService.SendNotificacao(new string[] { user.Email }, assunto, body);
                }
            }
        }
    }
}
