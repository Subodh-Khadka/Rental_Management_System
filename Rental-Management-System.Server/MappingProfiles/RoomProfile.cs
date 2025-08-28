using AutoMapper;
using Rental_Management_System.Server.DTOs.Room;
using Rental_Management_System.Server.DTOs.Room;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.MappingProfiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            // Room -> RoomReadDto
            CreateMap<Room, RoomDto>();

            // RoomCreateDto -> Room
            CreateMap<CreateRoomDto, Room>();

            // UpdateRoomDto -> Room
            CreateMap<UpdateRoomDto, Room>();
        }
    }
}
