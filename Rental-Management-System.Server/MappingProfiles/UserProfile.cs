using AutoMapper;
using Rental_Management_System.Server.DTOs.User;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // ApplicationUser -> UserDto (for reading)
            CreateMap<ApplicationUser, UserDto>();

            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<UpdateUserDto, ApplicationUser>();
        }
    }
}
