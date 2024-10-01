using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.application.Features.TiposDespesa.Common
{
    public record TipoDespesaResponse(
        int Id,
        string Nome,
        EnumTipoDespesaTipo Tipo,
        bool Ativo,
        decimal Valor,
        bool FaturarCliente,
        bool ReembolsarColaborador,
        bool ExibirParaColaborador
    );
}
