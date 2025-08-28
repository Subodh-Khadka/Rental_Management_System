namespace Rental_Management_System.Server.Services.Room
{
    using Rental_Management_System.Server.DTOs.Room;
    using Rental_Management_System.Server.Models;

    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
        Task<RoomDto> GetRoomByIdAsync(Guid roomId);
        Task<RoomDto> CreateRoomAsync(CreateRoomDto room);
        Task<RoomDto> UpdateRoomAsync(Guid roomId, UpdateRoomDto room);
        Task DeleteRoomAsync(Guid roomId);
    }
}
