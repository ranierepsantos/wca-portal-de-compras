using AutoMapper;
using wca.share.application.Features.Solicitacoes.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Common
{
    internal class SolicitacaoProfile : Profile
    {
        public SolicitacaoProfile()
        {
            CreateMap<Solicitacao, SolicitacaoResponse>()
                .ForSourceMember(src =>src.StatusSolicitacao, opt => opt.DoNotValidate()) ;
            CreateMap<SolicitacaoCreateCommand, Solicitacao>();
            CreateMap<SolicitacaoUpdateCommand, Solicitacao>()
                .ForMember(dest => dest.Anexos, src => src.Ignore());

            CreateMap<Solicitacao, SolicitacaoToPaginateResponse>()
                .ForMember(dest => dest.Id, opt =>  opt.MapFrom(src =>  src.Id))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.SolicitacaoTipo.Tipo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusSolicitacaoId))
                .ForMember(dest => dest.DataSolicitacao, opt =>  opt.MapFrom(src =>  src.DataSolicitacao))
                .ForMember(dest => dest.ClienteNome, opt =>  opt.MapFrom(src =>  src.Cliente.Nome))
                .ForMember(dest => dest.FuncionarioNome, opt =>  opt.MapFrom(src =>  src.Funcionario.Nome))
                .ForMember(dest => dest.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel.Nome))
                .ForMember(dest => dest.GestorNome, opt =>  opt.MapFrom(src =>  src.Gestor.Nome))
                ;
        }
        /*
         int? Id,
        string? Tipo,
        int? Status,
        DateTime? DataSolicitacao,
        string? ClienteNome,
        string? FuncionarioNome,
        string? ResponsavelNome,
        string? GestorNome
         */

    }
}
