using AutoMapper;

namespace Rental_Management_System.Server.MappingProfiles
{
    using Rental_Management_System.Server.DTOs.PaymentTransaction;
    using Rental_Management_System.Server.Models;
    public class PaymentTransactionProfile : Profile
    {
        public PaymentTransactionProfile()
        {
            CreateMap<PaymentTransaction, PaymentTransactionDto>();
            CreateMap<CreatePaymentTransactionDto, PaymentTransaction>();
            CreateMap<UpdatePaymentTransactionDto, PaymentTransaction>();
        }
    }
}
