using FluentValidation;
using wca.share.application.Features.Solicitacoes.Commands;

namespace wca.share.application.Features.Solicitacoes.Behaviors
{
    internal sealed class SolicitacaoCreateCommandBehavior : AbstractValidator<SolicitacaoCreateCommand>
    {
        public SolicitacaoCreateCommandBehavior()
        {
            RuleFor(f => f.SolicitacaoTipoId).NotEmpty().NotNull().GreaterThan(0)
                .WithMessage("O tipo de solicitação deve ser informado!");
            RuleFor(f => f.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O funcionário deve ser informado!");
            RuleFor(f => f.ClienteId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O cliente deve ser informado!");
            RuleFor(f => f.Desligamento.DataDemissao).NotEmpty().NotNull().When(f => f.SolicitacaoTipoId == 1).WithMessage("A data da demissão deve ser informada!");
            RuleFor(f => f.Desligamento.MotivoDemissaoId).NotEmpty().NotNull().GreaterThan(0).When(f => f.SolicitacaoTipoId == 1).WithMessage("Motivo da demissão deve ser informado!");

        }
    }

    internal sealed class SolicitacaoUpdateCommandBehavior : AbstractValidator<SolicitacaoUpdateCommand>
    {
        public SolicitacaoUpdateCommandBehavior()
        {
            RuleFor(f => f.Id).NotEmpty().NotNull().GreaterThan(0).WithMessage("O id da solicitação deve ser informado!");
            RuleFor(f => f.SolicitacaoTipoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O tipo de solicitação deve ser informado!");
            RuleFor(f => f.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O funcionário deve ser informado!");
            RuleFor(f => f.ClienteId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O cliente deve ser informado!");
            RuleFor(f => f.Desligamento.DataDemissao).NotEmpty().NotNull().When(f => f.SolicitacaoTipoId == 1).WithMessage("A data da demissão deve ser informada!");
            RuleFor(f => f.Desligamento.MotivoDemissaoId).NotEmpty().NotNull().GreaterThan(0).When(f => f.SolicitacaoTipoId == 1).WithMessage("Motivo da demissão deve ser informado!");

        }
    }
}
