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
            CreateMap<Tenant, TenantDto>()
                .ForMember(dest => dest.RoomTitle,
                            opt => opt.MapFrom(src => src.Room !=null ? src.Room.RoomTitle : null));
            CreateMap<CreateTenantDto,Tenant >();
            CreateMap<UpdateTenantDto,Tenant >();
        }
    }
}
