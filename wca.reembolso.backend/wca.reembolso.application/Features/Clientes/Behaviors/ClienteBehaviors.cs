using FluentValidation;
using wca.reembolso.application.Features.Clientes.Commands;

namespace wca.reembolso.application.Features.Clientes.Behaviors
{
    public sealed class CreateClienteCommandBehavior : AbstractValidator<ClienteCreateCommand>
    {
        public CreateClienteCommandBehavior()
        {
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
            RuleFor(v => v.CNPJ).NotEmpty().NotNull().WithMessage("O CNPJ do cliente é obrigatório");
        }
    }

    public sealed class UpdateClienteCommandBehavior : AbstractValidator<ClienteUpdateCommand>
    {
        public UpdateClienteCommandBehavior()
        {
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
            RuleFor(v => v.CNPJ).NotEmpty().NotNull().WithMessage("O CNPJ do cliente é obrigatório");
        }
    }
}
