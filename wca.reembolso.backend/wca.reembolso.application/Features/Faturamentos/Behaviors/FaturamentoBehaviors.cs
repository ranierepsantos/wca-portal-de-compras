using FluentValidation;
using wca.reembolso.application.Features.Faturamentos.Commands;

namespace wca.reembolso.application.Features.Faturamentos.Behaviors
{
    public sealed class FaturamentoCreateCommandBehavior : AbstractValidator<FaturamentoCreateCommand>
    {
        public FaturamentoCreateCommandBehavior()
        {
            RuleFor(v => v.ClienteId).NotNull().GreaterThan(0).WithMessage("O cliente é obrigatório");
            RuleFor(v => v.CentroCustoId).NotNull().GreaterThan(0).WithMessage("O Centro de Custo é obrigatório");
            RuleFor(v => v.Valor).NotNull().GreaterThan(0).WithMessage("Valor do faturamento não informado");
            RuleFor(v => v.FaturamentoItem).Must(x => x.Count() > 0).WithMessage("Adicione ao menos 1 solicitação");
        }
    }


}
