using AutoMapper;
using Rental_Management_System.Server.DTOs.RentPayment;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{
    public class RentPaymentProfile : Profile
    {
        public RentPaymentProfile()
        {
            // Entity → DTO
            CreateMap<RentPayment, RentPaymentDto>()
                .ForMember(dest => dest.RoomTitle, opt => opt.MapFrom(src => src.RentalContract.Room.RoomTitle))
                .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.RentalContract.Tenant.Name))
                  .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.MonthlyCharges, opt => opt.MapFrom(src => src.MonthlyCharges));

            // DTO → Entity
            CreateMap<CreateRentPaymentDto, RentPayment>()
                .ForMember(dest => dest.MonthlyCharges, opt => opt.Ignore()); // handled manually in service

            CreateMap<UpdateRentPaymentDto, RentPayment>()
                .ForMember(dest => dest.MonthlyCharges, opt => opt.Ignore());
        }
    }
}
