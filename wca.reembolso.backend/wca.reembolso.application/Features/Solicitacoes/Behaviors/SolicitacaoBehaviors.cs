using FluentValidation;
using wca.reembolso.application.Features.Clientes.Commands;
using wca.reembolso.application.Features.Solicitacoes.Commands;

namespace wca.reembolso.application.Features.Solicitacoes.Behaviors;

public sealed class SolicitacaoCreateCommandBehavior : AbstractValidator<SolicitacaoCreateCommand>
{
    public SolicitacaoCreateCommandBehavior()
    {
        //RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
        //RuleFor(v => v.CNPJ).NotEmpty().NotNull().WithMessage("O CNPJ do cliente é obrigatório");
    }
}

public sealed class SolicitacaoUpdateCommandBehavior : AbstractValidator<SolicitacaoUpdateCommand>
{
    public SolicitacaoUpdateCommandBehavior()
    {
        //RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
        //RuleFor(v => v.CNPJ).NotEmpty().NotNull().WithMessage("O CNPJ do cliente é obrigatório");
    }
}
