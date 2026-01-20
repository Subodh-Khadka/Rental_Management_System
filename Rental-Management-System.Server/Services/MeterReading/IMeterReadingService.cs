using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MeterReading;

namespace Rental_Management_System.Server.Services.MeterReading
{
    public interface IMeterReadingService
    {
        Task<ApiResponse<IEnumerable<MeterReadingDto>>> GetAllAsync();
        Task<ApiResponse<MeterReadingDto>> GetByIdAsync(Guid meterReadingId);
        Task<ApiResponse<MeterReadingDto>> GetByPaymentAndMonthAsync(Guid paymentId, string month);
        Task<ApiResponse<MeterReadingDto>> CreateAsync(CreateMeterReadingDto createDto);
        Task<ApiResponse<MeterReadingDto>> UpdateAsync(Guid meterReadingId, UpdateMeterReadingDto updateDto);
        Task<ApiResponse<bool>> DeleteAsync(Guid meterReadingId);
    }
}
