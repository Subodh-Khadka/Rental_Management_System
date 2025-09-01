using AutoMapper;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{
    public class MonthlyChargeProfile : Profile
    {
        public MonthlyChargeProfile()
        {
            CreateMap<MonthlyCharge, MonthlyChargeDto>();
            CreateMap<CreateMonthlyChargeDto, MonthlyCharge>();
            CreateMap<UpdateMonthlyChargeDto, MonthlyCharge>();
        }
    }
}
