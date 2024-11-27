using FluentValidation;
using wca.share.application.Common;
using wca.share.application.Features.Solicitacoes.Commands;

namespace wca.share.application.Features.Solicitacoes.Behaviors
{
    internal sealed class SolicitacaoCreateCommandBehavior : AbstractValidator<SolicitacaoCreateCommand>
    {
        public SolicitacaoCreateCommandBehavior()
        {
            RuleFor(p => p.SolicitacaoTipoId).NotEmpty().NotNull().GreaterThan(0)
                .WithMessage("O tipo de solicitação deve ser informado!");
            RuleFor(p => p.ClienteId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O cliente deve ser informado!");
            RuleFor(p => p.Desligamento.DataDemissao).NotEmpty().NotNull().When(p => p.SolicitacaoTipoId == 1).WithMessage("A data da demissão deve ser informada!");
            RuleFor(p => p.Desligamento.MotivoDemissaoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.Desligamento).WithMessage("Motivo da demissão deve ser informado!");
            RuleFor(p => p.Desligamento.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.Desligamento).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.Comunicado.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.Comunicado).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.Ferias.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.Ferias).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.MudancaBase.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.MudancaBase).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.Vaga.EscalaId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("A escala deve ser informada!");
            RuleFor(p => p.Vaga.HorarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("O horário deve ser informado!");
            RuleFor(p => p.Vaga.QuantidadeVagas).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("A quantidade de vagas deve ser informada!");
            RuleFor(p => p.Vaga.TipoContratoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("O tipo de contrato deve ser informado!");
            RuleFor(p => p.Vaga.TipoFaturamentoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("O tipo de faturamento deve ser informado!");
            RuleFor(p => p.Vaga.FuncaoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("A função deve ser informada!");

        }
    }

    internal sealed class SolicitacaoUpdateCommandBehavior : AbstractValidator<SolicitacaoUpdateCommand>
    {
        public SolicitacaoUpdateCommandBehavior()
        {
            RuleFor(p => p.Id).NotEmpty().NotNull().GreaterThan(0).WithMessage("O id da solicitação deve ser informado!");
            RuleFor(p => p.SolicitacaoTipoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O tipo de solicitação deve ser informado!");
            RuleFor(p => p.ClienteId).NotEmpty().NotNull().GreaterThan(0).WithMessage("O cliente deve ser informado!");
            RuleFor(p => p.Desligamento.DataDemissao).NotEmpty().NotNull().When(p => p.SolicitacaoTipoId == 1).WithMessage("A data da demissão deve ser informada!");
            RuleFor(p => p.Desligamento.MotivoDemissaoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == 1).WithMessage("Motivo da demissão deve ser informado!");
            RuleFor(p => p.Desligamento.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.Desligamento).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.Comunicado.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.Comunicado).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.Ferias.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.Ferias).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.MudancaBase.FuncionarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int) EnumTipoSolicitacao.MudancaBase).WithMessage("O funcionário deve ser informado!");
            RuleFor(p => p.Vaga.EscalaId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("A escala deve ser informada!");
            RuleFor(p => p.Vaga.HorarioId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("O horário deve ser informado!");
            RuleFor(p => p.Vaga.QuantidadeVagas).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("A quantidade de vagas deve ser informada!");
            RuleFor(p => p.Vaga.TipoContratoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("O tipo de contrato deve ser informado!");
            RuleFor(p => p.Vaga.TipoFaturamentoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("O tipo de faturamento deve ser informado!");
            RuleFor(p => p.Vaga.FuncaoId).NotEmpty().NotNull().GreaterThan(0).When(p => p.SolicitacaoTipoId == (int)EnumTipoSolicitacao.Vaga).WithMessage("A função deve ser informada!");
        }
    }
}
