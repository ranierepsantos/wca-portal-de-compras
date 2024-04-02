namespace wca.share.application.Features.Notificacoes.Common
{
    public record NotificacaoResponse(
        int Id,
        DateTime DataHora,
        string Nota,
        string Entidade,
        int EntidadeId, 
        bool Lido
    );
}
