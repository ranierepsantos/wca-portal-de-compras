using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Features.Clientes.Commands;
using wca.reembolso.application.Features.TiposDespesa.Commands;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.application.Features.TiposDespesa.Behaviors
{
    public class TipoDespesaCreateCommandBehavior: AbstractValidator<TipoDespesaCreateCommand>
    {
        public TipoDespesaCreateCommandBehavior()
        {
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
            RuleFor(v => v.Tipo).IsInEnum().WithMessage("O tipo não é valido, use 1 - Consumo e 2 - Distância");
            RuleFor(v => v.Valor).GreaterThan(0).When(c => c.Tipo.Equals(EnumTipoDespesaTipo.Distancia))
                .WithMessage("O valor deve ser maior que 0 (zero)");
        }
    }

    public class TipoDespesaUpdateCommandBehavior : AbstractValidator<TipoDespesaUpdateCommand>
    {
        public TipoDespesaUpdateCommandBehavior()
        {
            RuleFor(v => v.Id).GreaterThan(0).NotNull().WithMessage("O id é obrigatório!");
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
            RuleFor(v => v.Tipo).IsInEnum().WithMessage("O tipo não é valido, use 1 - Consumo e 2 - Distância");
            RuleFor(v => v.Valor).GreaterThan(0).When(c => c.Tipo.Equals(EnumTipoDespesaTipo.Distancia))
                .WithMessage("O valor deve ser maior que 0 (zero)");
        }
    }
}
