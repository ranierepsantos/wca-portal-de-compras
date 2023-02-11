using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;

namespace wca.compras.crosscutting.Mapping
{
    public class RequisicaoProfile: Profile
    {
        public RequisicaoProfile() {
            CreateMap<Requisicao, RequisicaoDto>().ReverseMap();

            CreateMap<CreateRequisicaoDto, Requisicao>()
                .ForMember(src => src.RequisicaoItens, dest => dest.Ignore());

            CreateMap<UpdateRequisicaoDto, Requisicao>()
                .ForMember(src => src.RequisicaoItens, dest => dest.MapFrom(opt => opt.RequisicaoItens))
                .ForMember(src => src.RequisicaoHistorico, dest => dest.MapFrom(opt => opt.RequisicaoHistorico));

            CreateMap<RequisicaoItem, RequisicaoItemDto>()
                .ReverseMap();

            CreateMap<RequisicaoHistorico, RequisicaoHistoricoDto>()
                .ReverseMap();

            CreateMap<Requisicao, RequisicaoAprovacaoDto>();

        }
    }
}
