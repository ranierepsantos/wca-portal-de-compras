using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.DocumentoComplementares.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.DocumentoComplementares.Common
{
    internal class DocumentoComplementarProfile : Profile
    {
        public DocumentoComplementarProfile()
        {
            CreateMap<DocumentoComplementar, DocumentoComplementarResponse>();
            CreateMap<DocumentoComplementarCreateCommand, DocumentoComplementar>();
            CreateMap<DocumentoComplementarUpdateCommand, DocumentoComplementar>();
            CreateMap<DocumentoComplementar, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
