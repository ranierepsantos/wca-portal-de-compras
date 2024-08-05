using FluentValidation;
using wca.share.application.Features.Vagas.Commands;

namespace wca.share.application.Features.Vagas.Behaviors
{
    internal class VagaCreateCommandBehavior: AbstractValidator<VagaCreateCommand>
    {
        public VagaCreateCommandBehavior()
        {
            RuleFor(p => p.ClienteId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O cliente deve ser informado!");
            RuleFor(p => p.EscalaId).NotEmpty().NotNull().GreaterThan(0).WithMessage("A escala deve ser informada!");
            RuleFor(p => p.HorarioId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O horário deve ser informado!");
            RuleFor(p => p.QuantidadeVagas).NotEmpty().NotNull().GreaterThan(0).WithMessage("A quantidade de vagas deve ser informada!");
            RuleFor(p => p.TipoContratoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O tipo de contrato deve ser informado!");
            RuleFor(p => p.TipoFaturamentoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O tipo de faturamento deve ser informado!");
            RuleFor(p => p.FuncaoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("A função deve ser informada!");
        }
    }

    internal class VagaUpdateCommandBehavior : AbstractValidator<VagaUpdateCommand>
    {
        public VagaUpdateCommandBehavior()
        {
            RuleFor(p => p.Id).NotEmpty().NotNull().GreaterThan(0).WithMessage("O id deve ser informado!");
            RuleFor(p => p.ClienteId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O cliente deve ser informado!");
            RuleFor(p => p.EscalaId).NotEmpty().NotNull().GreaterThan(0).WithMessage("A escala deve ser informada!");
            RuleFor(p => p.HorarioId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O horário deve ser informado!");
            RuleFor(p => p.QuantidadeVagas).NotEmpty().NotNull().GreaterThan(0).WithMessage("A quantidade de vagas deve ser informada!");
            RuleFor(p => p.TipoContratoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O tipo de contrato deve ser informado!");
            RuleFor(p => p.TipoFaturamentoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O tipo de faturamento deve ser informado!");
            RuleFor(p => p.FuncaoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("A função deve ser informada!");
        }
    }
}
