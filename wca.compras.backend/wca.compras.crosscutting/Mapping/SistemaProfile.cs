using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;

namespace wca.compras.crosscutting.Mapping
{
    public sealed class SistemaProfile : Profile
    {
        public SistemaProfile()
        {
            CreateMap<Sistema, SistemaDto>();
                
        }
    }
}
