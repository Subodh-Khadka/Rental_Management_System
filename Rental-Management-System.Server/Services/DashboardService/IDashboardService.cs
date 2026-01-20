using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.DashboardDataDto;

namespace Rental_Management_System.Server.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<ApiResponse<DashboardDataDto>> GetDashboardDataAsync();
    }
}
