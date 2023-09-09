using FluentValidation;
using wca.reembolso.application.Features.Filiais.Commands;

namespace wca.reembolso.application.Features.Filiais.Behaviors
{
    public class FilialCreateCommandBehavior : AbstractValidator<FilialCreateCommand>
    {
        public FilialCreateCommandBehavior() 
        {
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome da é obrigatório");
        }
    }

    public class FilialUpdateCommandBehavior : AbstractValidator<FilialUpdateCommand>
    {
        public FilialUpdateCommandBehavior()
        {
            RuleFor(v => v.Id).NotNull().GreaterThan(0).WithMessage("O id é obrigatório, para atualização de dados");
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome da Filial é obrigatório");
        }
    }
}
