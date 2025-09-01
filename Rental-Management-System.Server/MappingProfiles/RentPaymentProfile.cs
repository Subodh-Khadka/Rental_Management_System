using AutoMapper;
using Rental_Management_System.Server.DTOs.RentPayment;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{
    public class RentPaymentProfile : Profile
    {
        public RentPaymentProfile() 
        {



            CreateMap<RentPayment,  RentPaymentDto>();
            CreateMap<CreateRentPaymentDto, RentPayment>();
            CreateMap<UpdateRentPaymentDto, RentPayment>();
        }
    }
}
