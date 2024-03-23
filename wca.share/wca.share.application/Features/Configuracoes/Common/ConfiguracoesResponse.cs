namespace wca.share.application.Features.Configuracoes.Common
{
    public record ConfiguracaoResponse (
        int Id,
        string Descricao,
        int TipoCampo,
        string? Valor,
        string? ComboValores
    );
}
