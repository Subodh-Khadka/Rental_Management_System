namespace Rental_Management_System.Server.MappingProfiles
{
    using Rental_Management_System.Server.DTOs;
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.MappingProfiles;
using AutoMapper;
    using Rental_Management_System.Server.DTOs.Tenant;

    public class TenantProfile : Profile
    {
        public TenantProfile() 
        {
            CreateMap<Tenant, TenantDto>();
            CreateMap<CreateTenantDto,Tenant >();
            CreateMap<UpdateTenantDto,Tenant >();
        }
    }
}
