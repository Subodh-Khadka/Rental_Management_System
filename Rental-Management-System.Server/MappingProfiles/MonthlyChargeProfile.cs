//using AutoMapper;
//using Rental_Management_System.Server.DTOs.MonthlyCharge;
//using Rental_Management_System.Server.Models;

//namespace Rental_Management_System.Server.MappingProfiles
//{
//    public class MonthlyChargeProfile : Profile
//    {
//        public MonthlyChargeProfile()
//        {
//            CreateMap<MonthlyCharge, MonthlyChargeDto>();
//            CreateMap<CreateMonthlyChargeDto, MonthlyCharge>();
//            CreateMap<UpdateMonthlyChargeDto, MonthlyCharge>();
//        }
//    }
//}

using AutoMapper;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{
    public class MonthlyChargeProfile : Profile
    {
        public MonthlyChargeProfile()
        {
            // Entity → DTO mapping
            CreateMap<MonthlyCharge, MonthlyChargeDto>()
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.RentPayment.Room.RoomTitle))
                .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.RentPayment.RentalContract.Tenant.Name))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.RentPayment.PaymentMonth.ToString("yyyy-MM")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RentPayment.DueAmount > 0 ? "Pending" : "Paid"));

            // DTO → Entity mapping
            CreateMap<MonthlyChargeDto, MonthlyCharge>();
            CreateMap<UpdateMonthlyChargeDto, MonthlyCharge>();
        }
    }
}

