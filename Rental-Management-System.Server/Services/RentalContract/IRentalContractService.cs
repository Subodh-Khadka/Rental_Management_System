using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.RentalContract;

namespace Rental_Management_System.Server.Services.RentalContract
{
    public interface IRentalContractService
    {
        Task<ApiResponse<IEnumerable<RentalContractDto>>> GetAllRentalContractsAsync();
        Task<ApiResponse<RentalContractDto>> GetRentalContractByIdAsync(Guid rentalContractId);
        Task<ApiResponse<RentalContractDto>> CreateRentalContractAsync(CreateRentalContractDto createRentalContractDto);
        Task<ApiResponse<RentalContractDto>> UpdateRentalContractAsync(Guid rentalContractId, UpdateRentalContractDto updateRentalContractDto);
        Task<ApiResponse<bool>> DeleteRentalContractAsync(Guid rentalContractId);

    }
}
