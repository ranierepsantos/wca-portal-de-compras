using FluentValidation;
using wca.reembolso.application.Features.Conta.Commands;

namespace wca.reembolso.application.Features.Solicitacoes.Behaviors
{
    public sealed class ContaCorrenteCreateUpdateCommandBehavior : AbstractValidator<ContaCorrenteCreateUpdateCommand>
    {
        public ContaCorrenteCreateUpdateCommandBehavior()
        {
            RuleFor(v => v.UsuarioId).NotEmpty().NotNull().WithMessage("O código do usuário é obrigatório");
            RuleForEach(v => v.Transacoes).ChildRules(t =>
            {
                t.RuleFor(f => f.Valor).NotNull().NotEqual(0).WithMessage("O valor da transacão deve ser maior que zero!");
                t.RuleFor(f => f.Operador).NotNull().NotEmpty().WithMessage("O operador deve ser informado!");
                t.RuleFor(f => f.Descricao).NotNull().NotEmpty().WithMessage("A descriçaõ deve ser informada!");
            });
        }
    }

    
}