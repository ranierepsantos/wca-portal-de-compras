using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Features.Filiais.Commands;
using wca.reembolso.application6.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Filiais.Common
{
    internal class FilialProfile: Profile
    {
        public FilialProfile() { 
            CreateMap<Filial, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ReverseMap();

            CreateMap<Filial, FilialResponse>().ReverseMap();

            CreateMap<FilialCreateCommand, Filial>();
            CreateMap<FilialUpdateCommand, Filial>();
        }
    }
}
