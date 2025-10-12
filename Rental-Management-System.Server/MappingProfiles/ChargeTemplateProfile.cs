using AutoMapper;
using Rental_Management_System.Server.DTOs.ChargeTemplate;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{
    public class ChargeTemplateProfile : Profile
    {
        public ChargeTemplateProfile() {
            CreateMap<ChargeTemplate, ChargeTemplateDto>();
            CreateMap<CreateChargeTemplateDto, ChargeTemplate>()
                 .ForMember(dest => dest.ChargeTemplateId, opt => opt.Ignore());
            CreateMap<UpdateChargeTemplateDto, ChargeTemplate>();
        }
    }
}
