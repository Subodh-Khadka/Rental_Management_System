using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.User;

namespace Rental_Management_System.Server.Services.User
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> GetUserByIdAsync(string userId);
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<ApiResponse<UserDto>> UpdateUserAsync(string userId, UpdateUserDto dto);
        Task<ApiResponse<bool>> DeleteUserAsync(string userId);
        Task<ApiResponse<bool>> AssignRoleAsync(string userId, string role);
    }
}

