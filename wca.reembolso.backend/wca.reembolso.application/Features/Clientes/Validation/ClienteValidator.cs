using FluentValidation;
using wca.reembolso.application.Features.Clientes.Commands;

namespace wca.reembolso.application.Features.Clientes.Validation
{
    public sealed class CreateClienteCommandValidator : AbstractValidator<ClienteCreateCommand>
    {
        public CreateClienteCommandValidator()
        {
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
            RuleFor(v => v.CNPJ).NotEmpty().NotNull().WithMessage("O CNPJ do cliente é obrigatório");
        }
    }

    public sealed class UpdateClienteCommandValidator : AbstractValidator<ClienteUpdateCommand>
    {
        public UpdateClienteCommandValidator()
        {
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome do cliente é obrigatório");
            RuleFor(v => v.CNPJ).NotEmpty().NotNull().WithMessage("O CNPJ do cliente é obrigatório");
        }
    }
}
