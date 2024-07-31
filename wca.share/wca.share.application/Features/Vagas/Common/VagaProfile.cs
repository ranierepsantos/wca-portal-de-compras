using AutoMapper;
using wca.share.application.Features.Vagas.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Vagas.Common
{
    internal class VagaProfile: Profile
    {
        public VagaProfile()
        {
            CreateMap<VagaCreateCommand, Vaga>();
            CreateMap<Vaga, VagaResponse>()
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.EscalaNome, opt => opt.MapFrom(src => src.Escala.Nome))
                .ForMember(dest => dest.EscolaridadeNome, opt => opt.MapFrom(src => src.Escolaridade.Nome))
                .ForMember(dest => dest.FuncaoNome, opt => opt.MapFrom(src => src.Funcao.Nome))
                .ForMember(dest => dest.GestorNome, opt => opt.MapFrom(src => src.Gestor.Nome))
                .ForMember(dest => dest.HorarioNome, opt => opt.MapFrom(src => src.Horario.Nome))
                .ForMember(dest => dest.MotivoContratacaoNome, opt => opt.MapFrom(src => src.MotivoContratacao.Nome))
                .ForMember(dest => dest.SexoNome, opt => opt.MapFrom(src => src.Sexo.Nome))
                .ForMember(dest => dest.StatusVagaNome, opt => opt.MapFrom(src => src.StatusVaga.Status))
                .ForMember(dest => dest.TipoContratoNome, opt => opt.MapFrom(src => src.TipoContrato.Nome))
                .ForMember(dest => dest.TipoFaturamentoNome, opt => opt.MapFrom(src => src.TipoFaturamento.Nome));

        }
    }
}
