using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;

namespace wca.compras.crosscutting.Mapping
{
    public class RecorrenciaProfile: Profile
    {
        public RecorrenciaProfile()
        {
            CreateMap<Recorrencia, RecorrenciaDto>().ReverseMap();

            CreateMap<CreateRecorrenciaDto, Recorrencia>()
                .ForMember(src => src.RecorrenciaProdutos, dest => dest.Ignore());

            CreateMap<UpdateRecorrenciaDto, Recorrencia>()
                .ForMember(src => src.RecorrenciaProdutos, dest => dest.MapFrom(opt => opt.RecorrenciaProdutos));

            CreateMap<RecorrenciaProduto, RecorrenciaProdutoDto>()
                .ReverseMap();

            CreateMap<RecorrenciaLog, RecorrenciaLogDto>()
                .ReverseMap();

        }
    }
}
