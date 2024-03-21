using FluentValidation;
using wca.share.application.Features.Funcionarios.Commands;

namespace wca.share.application.Features.Funcionarios.Behaviors
{
    public sealed class FuncionarioCreateCommandBehavior : AbstractValidator<FuncionarioCreateCommand>
    {
        public FuncionarioCreateCommandBehavior()
        {
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome é obrigatório");
            RuleFor(v => v.DataAdmissao).NotEmpty().NotNull().WithMessage("A data de admissão é obrigatório");
        }
    }

    public sealed class FuncionarioUpdateCommandBehavior : AbstractValidator<FuncionarioUpdateCommand>
    {
        public FuncionarioUpdateCommandBehavior()
        {
            RuleFor(v => v.Id).NotEmpty().NotNull().WithMessage("O Id é obrigatório");
            RuleFor(v => v.Nome).NotEmpty().NotNull().WithMessage("O nome é obrigatório");
            RuleFor(v => v.DataAdmissao).NotEmpty().NotNull().WithMessage("A data de admissão é obrigatório");
        }
    }
}
