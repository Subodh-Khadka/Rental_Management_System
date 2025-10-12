using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.ChargeTemplate;

namespace Rental_Management_System.Server.Services.ChargeTemplate
{
    public interface IChargeTemplateService
    {
        Task<ApiResponse<IEnumerable<ChargeTemplateDto>>> GetAllChargeTemplateAsync();
        Task<ApiResponse<ChargeTemplateDto>> GetChargeTemplateByIdAsync(Guid chargeTemplateId);

        Task<ApiResponse<ChargeTemplateDto>> CreateChargeTemplateAsync(CreateChargeTemplateDto createDto);
        Task<ApiResponse<ChargeTemplateDto>> UpdateChargeTemplateAsync(Guid chargeTemplateId, UpdateChargeTemplateDto updateDto);
        Task<ApiResponse<bool>> DeleteChargeTemplateAsync(Guid chargeTemplateId);

    }
}
