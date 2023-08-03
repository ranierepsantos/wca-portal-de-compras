using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Features.TiposDespesa.Commands;
using wca.reembolso.application6.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.TiposDespesa.Common
{
    public  class TipoDespesaProfile: Profile
    {
        public TipoDespesaProfile()
        {
            CreateMap<TipoDespesaCreateCommand, TipoDespesa>();
            CreateMap<TipoDespesaUpdateCommand, TipoDespesa>();
            CreateMap<TipoDespesa, TipoDespesaResponse>();
            CreateMap<TipoDespesa, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
