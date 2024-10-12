using AutoMapper;
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
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src =>  (src.Cliente.CodigoCliente == null ? "": src.Cliente.CodigoCliente.ToString() + " - ")  + src.Cliente.Nome))
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
                .ForMember(dest => dest.ClienteNome, opt =>  opt.MapFrom(src =>  (src.Cliente.CodigoCliente == null ? "": src.Cliente.CodigoCliente.ToString() + " - ")  + src.Cliente.Nome))
                .ForMember(dest => dest.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel.Nome));

            CreateMap<StatusSolicitacao, StatusSolicitacaoResponse>();
            CreateMap<Assunto, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));

            CreateMap<ItemMudanca, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<SolicitacaoComunicado, SolicitacaoComunicadoResponse>()
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome))
                .ForMember(dest => dest.eSocialMatricula, opt => opt.MapFrom(src => src.Funcionario.eSocialMatricula))
                .ReverseMap();

            CreateMap<SolicitacaoDesligamento, SolicitacaoDesligamentoResponse>()
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome))
                .ForMember(dest => dest.eSocialMatricula, opt => opt.MapFrom(src => src.Funcionario.eSocialMatricula))
                .ReverseMap();

            CreateMap<SolicitacaoFerias, SolicitacaoFeriasResponse>()
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome))
                .ForMember(dest => dest.eSocialMatricula, opt => opt.MapFrom(src => src.Funcionario.eSocialMatricula))
                .ForMember(dest => dest.TipoFeriasNome, opt => opt.MapFrom(src => src.TipoFerias.Descricao))
                .ReverseMap();

            CreateMap<SolicitacaoMudancaBase, SolicitacaoMudancaBaseResponse>()
                .ForMember(dest => dest.ClienteDestinoNome, opt => opt.MapFrom(src => src.ClienteDestino.Nome))
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome))
                .ForMember(dest => dest.eSocialMatricula, opt => opt.MapFrom(src => src.Funcionario.eSocialMatricula))
                .ReverseMap();

            CreateMap<SolicitacaoVaga, SolicitacaoVagaResponse>()
                .ForMember(dest => dest.EscalaNome, opt => opt.MapFrom(src => src.Escala.Nome))
                .ForMember(dest => dest.EscolaridadeNome, opt => opt.MapFrom(src => src.Escolaridade.Nome))
                .ForMember(dest => dest.FuncaoNome, opt => opt.MapFrom(src => src.Funcao.Nome))
                .ForMember(dest => dest.GestorNome, opt => opt.MapFrom(src => src.Gestor.Nome))
                .ForMember(dest => dest.HorarioNome, opt => opt.MapFrom(src => src.Horario.Nome))
                .ForMember(dest => dest.MotivoContratacaoNome, opt => opt.MapFrom(src => src.MotivoContratacao.Nome))
                .ForMember(dest => dest.SexoNome, opt => opt.MapFrom(src => src.Sexo.Nome))
                .ForMember(dest => dest.TipoContratoNome, opt => opt.MapFrom(src => src.TipoContrato.Nome))
                .ForMember(dest => dest.TipoFaturamentoNome, opt => opt.MapFrom(src => src.TipoFaturamento.Nome))
                .ReverseMap();

            CreateMap<SolicitacaoComunicado, SolicitacaoComunicadoPaginateResponse>()
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome))
                .ForMember(dest => dest.AssuntoNome, opt => opt.MapFrom(src => src.Assunto.Nome));

            CreateMap<SolicitacaoDesligamento, SolicitacaoFuncionarioCentroCustoPaginateResponse>()
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome));
            
            CreateMap<SolicitacaoFerias, SolicitacaoFuncionarioCentroCustoPaginateResponse>()
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome));

            CreateMap<SolicitacaoMudancaBase, SolicitacaoFuncionarioCentroCustoPaginateResponse>()
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome));
            
            CreateMap<SolicitacaoVaga, SolicitacaoVagaPaginateResponse>()
                .ForMember(dest => dest.FuncaoNome, opt => opt.MapFrom(src => src.Funcao.Nome));

        }
    }
}
