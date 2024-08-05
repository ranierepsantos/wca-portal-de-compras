using AutoMapper;
using wca.share.application.Features.Vagas.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Vagas.Common
{
    internal class VagaProfile: Profile
    {
        public VagaProfile()
        {
            CreateMap<VagaCreateCommand, Vaga>()
                .ForMember(dest => dest.DocumentoComplementares , opt => opt.Ignore());
            CreateMap<VagaUpdateCommand, Vaga>();
            CreateMap<Vaga, VagaResponse>()
                .ForSourceMember(src => src.StatusSolicitacao, opt => opt.DoNotValidate())
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.EscalaNome, opt => opt.MapFrom(src => src.Escala.Nome))
                .ForMember(dest => dest.EscolaridadeNome, opt => opt.MapFrom(src => src.Escolaridade.Nome))
                .ForMember(dest => dest.FuncaoNome, opt => opt.MapFrom(src => src.Funcao.Nome))
                .ForMember(dest => dest.GestorNome, opt => opt.MapFrom(src => src.Gestor.Nome))
                .ForMember(dest => dest.HorarioNome, opt => opt.MapFrom(src => src.Horario.Nome))
                .ForMember(dest => dest.MotivoContratacaoNome, opt => opt.MapFrom(src => src.MotivoContratacao.Nome))
                .ForMember(dest => dest.SexoNome, opt => opt.MapFrom(src => src.Sexo.Nome))
                .ForMember(dest => dest.StatusSolicitacaoNome, opt => opt.MapFrom(src => src.StatusSolicitacao.Status))
                .ForMember(dest => dest.TipoContratoNome, opt => opt.MapFrom(src => src.TipoContrato.Nome))
                .ForMember(dest => dest.TipoFaturamentoNome, opt => opt.MapFrom(src => src.TipoFaturamento.Nome))
                .ReverseMap();

            CreateMap<StatusSolicitacao, StatusSolicitacaoResponse>();

            CreateMap<Vaga, VagaToPaginateResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FilialId, opt => opt.MapFrom(src => src.Cliente.FilialId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusSolicitacaoId))
                .ForMember(dest => dest.DataSolicitacao, opt => opt.MapFrom(src => src.DataSolicitacao))
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel.Nome))
                .ForMember(dest => dest.FuncaoNome, opt => opt.MapFrom(src => src.Funcao.Nome))
                .ForMember(dest => dest.VagaHistorico, opt => opt.MapFrom(src => src.VagaHistorico));

        }
    }
}
