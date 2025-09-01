namespace Rental_Management_System.Server.MappingProfiles
{
    using Rental_Management_System.Server.DTOs;
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.MappingProfiles;
using AutoMapper;
    using Rental_Management_System.Server.DTOs.Tenant;
    using Rental_Management_System.Server.DTOs.RentalContract;

    public class RentalContractProfile : Profile
    {
        public RentalContractProfile() 
        {
            CreateMap<RentalContract, RentalContractDto>();
            CreateMap<CreateRentalContractDto,RentalContract >();
            CreateMap<UpdateRentalContractDto,RentalContract >();
        }
    }
}
