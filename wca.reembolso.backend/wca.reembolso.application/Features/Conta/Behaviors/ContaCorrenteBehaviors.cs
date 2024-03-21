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
                t.RuleFor(f => f.Valor).NotNull().WithMessage("O valor da transacão não pode ser nulo!");
                t.RuleFor(f => f.Operador).NotNull().NotEmpty().WithMessage("O operador deve ser informado!");
                t.RuleFor(f => f.Descricao).NotNull().NotEmpty().WithMessage("A descriçaõ deve ser informada!");
            });
        }
    }

    
}