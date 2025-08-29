namespace Rental_Management_System.Server.Services.Room
{
    using Rental_Management_System.Server.DTOs;
    using Rental_Management_System.Server.DTOs.Room;
    using Rental_Management_System.Server.Models;

    public interface IRoomService
    {
        Task<ApiResponse<IEnumerable<RoomDto>>> GetAllRoomsAsync();
        Task<ApiResponse<RoomDto>> GetRoomByIdAsync(Guid roomId);
        Task<ApiResponse<RoomDto>> CreateRoomAsync(CreateRoomDto room);
        Task<ApiResponse<RoomDto>> UpdateRoomAsync(Guid roomId, UpdateRoomDto room);
        Task<ApiResponse<bool>> DeleteRoomAsync(Guid roomId);
    }
}
