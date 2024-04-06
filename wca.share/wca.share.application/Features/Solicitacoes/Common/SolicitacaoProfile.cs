using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using wca.share.application.Common;
using wca.share.application.Features.Solicitacoes.Commands;
using wca.share.domain.Entities;
using ListItem = wca.share.application.Common.ListItem;

namespace wca.share.application.Features.Solicitacoes.Common
{
    internal class SolicitacaoProfile : Profile
    {
        public SolicitacaoProfile()
        {
            CreateMap<Solicitacao, SolicitacaoResponse>()
                .ForSourceMember(src =>src.StatusSolicitacao, opt => opt.DoNotValidate())
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.FuncionarioDataAdmissao, opt => opt.MapFrom(src => src.Funcionario.DataAdmissao))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome))
                .ForMember(dest => dest.NumeroPis, opt => opt.MapFrom(src => src.Funcionario.NumeroPis))
                .ReverseMap();
            CreateMap<SolicitacaoCreateCommand, Solicitacao>();
            CreateMap<SolicitacaoUpdateCommand, Solicitacao>()
                .ForMember(dest => dest.Anexos, src => src.Ignore());

            CreateMap<Solicitacao, SolicitacaoToPaginateResponse>()
                .ForMember(dest => dest.Id, opt =>  opt.MapFrom(src =>  src.Id))
                .ForMember(dest => dest.FilialId, opt =>  opt.MapFrom(src =>  src.Cliente.FilialId))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.SolicitacaoTipo.Tipo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusSolicitacaoId))
                .ForMember(dest => dest.DataSolicitacao, opt =>  opt.MapFrom(src =>  src.DataSolicitacao))
                .ForMember(dest => dest.ClienteNome, opt =>  opt.MapFrom(src =>  src.Cliente.Nome))
                .ForMember(dest => dest.FuncionarioNome, opt =>  opt.MapFrom(src =>  src.Funcionario.Nome))
                .ForMember(dest => dest.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt =>  opt.MapFrom(src =>  src.CentroCusto.Nome));

            CreateMap<StatusSolicitacao, StatusSolicitacaoResponse>();
            CreateMap<Assunto, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));

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
