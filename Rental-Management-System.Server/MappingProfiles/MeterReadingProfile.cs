using AutoMapper;
using Rental_Management_System.Server.DTOs.MeterReading;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{

    public class MeterReadingProfile : Profile
    {
        public MeterReadingProfile()
        {
            CreateMap<MeterReading, MeterReadingDto>()
                .ForMember(dest => dest.UnitsUsed, opt => opt.MapFrom(src => src.UnitsUsed));

            CreateMap<CreateMeterReadingDto, MeterReading>()
                .ForMember(dest => dest.MeterReadingId, opt => opt.Ignore())
                .ForMember(dest => dest.UnitsUsed, opt => opt.Ignore()); // Add this line

            CreateMap<UpdateMeterReadingDto, MeterReading>()
                .ForMember(dest => dest.UnitsUsed, opt => opt.Ignore()); // Optional
        }
    }
}
