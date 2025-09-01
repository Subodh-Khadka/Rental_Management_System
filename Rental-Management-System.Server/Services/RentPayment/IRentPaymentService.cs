using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;

namespace Rental_Management_System.Server.Services.RentPayment
{
    using Rental_Management_System.Server.DTOs.RentPayment;
    using Rental_Management_System.Server.Models;
    public interface IRentPaymentService
    {
        Task<ApiResponse<IEnumerable<RentPaymentDto>>> GetAllRentPaymentAsync();
        Task<ApiResponse<RentPaymentDto>> GetRentPaymentByIdAsync(Guid rentPaymentId);

        Task<ApiResponse<RentPaymentDto>> CreateRentPaymentAsync(CreateRentPaymentDto createRentPaymentDto);
        Task<ApiResponse<RentPaymentDto>> UpdateRentPaymentAsync(Guid rentPaymentId, UpdateRentPaymentDto updateRentPaymentDto);

        Task<ApiResponse<bool>> DeleteRentPaymentAsync(Guid rentPaymentId);
    }
}
    