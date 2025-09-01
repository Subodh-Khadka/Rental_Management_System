using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;

namespace Rental_Management_System.Server.Services.MonthlyCharge
{
    public interface IMonthlyChargeService
    {
        Task<ApiResponse<IEnumerable<MonthlyChargeDto>>> GetAllMonthlyChargeAsync();
        Task<ApiResponse<MonthlyChargeDto>> GetMonthlyChargeByIdAsync(Guid monthlyChargeId);
        
        Task<ApiResponse<MonthlyChargeDto>> CreateMonthlyChargeAsync(CreateMonthlyChargeDto createMonthlyChargeDto);
        Task<ApiResponse<MonthlyChargeDto>> UpdateMonthlyChargeAsync(Guid monthlyChargeId, UpdateMonthlyChargeDto updateMonthlyChargeDto);

        Task<ApiResponse<bool>> DeleteMonthlyChargeAsync(Guid monthlyChargeId);
    }
}
