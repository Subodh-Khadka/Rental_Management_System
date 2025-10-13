using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.User;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var dto = _mapper.Map<UserDto>(user);
                dto.Roles = roles.ToArray();
                userDtos.Add(dto);
            }

            return ApiResponse<IEnumerable<UserDto>>.SuccessResponse(userDtos);
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ApiResponse<UserDto>.FailResponse("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = roles.ToArray();
            return ApiResponse<UserDto>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return ApiResponse<UserDto>.FailResponse("Email already exists");

            var user = _mapper.Map<ApplicationUser>(dto);
            user.UserName = dto.Email;
            user.CreatedAt = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return ApiResponse<UserDto>.FailResponse(result.Errors.First().Description);

            var role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role;

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = new[] { role };
            return ApiResponse<UserDto>.SuccessResponse(userDto);
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(string userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ApiResponse<UserDto>.FailResponse("User not found");

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Address = dto.Address ?? user.Address;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return ApiResponse<UserDto>.FailResponse(updateResult.Errors.First().Description);

            if (!string.IsNullOrEmpty(dto.Role))
                await AssignRoleAsync(userId, dto.Role);

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToArray();

            return ApiResponse<UserDto>.SuccessResponse(userDto);
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ApiResponse<bool>.FailResponse("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return ApiResponse<bool>.FailResponse(result.Errors.First().Description);

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<bool>> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ApiResponse<bool>.FailResponse("User not found");

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}

