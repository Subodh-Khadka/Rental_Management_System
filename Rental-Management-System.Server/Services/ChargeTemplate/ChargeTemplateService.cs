using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.ChargeTemplate;
using Rental_Management_System.Server.Repositories.CharegTemplate;
using Rental_Management_System.Server.Repositories.RentPayment;

namespace Rental_Management_System.Server.Services.ChargeTemplate
{
    using Rental_Management_System.Server.Models;
    public class ChargeTemplateService : IChargeTemplateService
    {
        private readonly IMapper _mapper;
        private readonly IChargeTemplateRepository _chargeTemplateRepository;

        public ChargeTemplateService(IChargeTemplateRepository chargeTemplateRepository, IMapper mapper)
        {
            _chargeTemplateRepository = chargeTemplateRepository; 
            _mapper = mapper;
        }

        public async Task<ApiResponse<ChargeTemplateDto>> CreateChargeTemplateAsync(CreateChargeTemplateDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.chargeType))
                return ApiResponse<ChargeTemplateDto>.FailResponse("ChargeType is required");

            var chargeTemplate = _mapper.Map<ChargeTemplate>(createDto);

            await _chargeTemplateRepository.AddAsync(chargeTemplate);
            await _chargeTemplateRepository.SaveChangesAsync();

            var chargeTemplateDtp = _mapper.Map<ChargeTemplateDto>(chargeTemplate);
            return ApiResponse<ChargeTemplateDto>.SuccessResponse(chargeTemplateDtp, "Charge template created successfully");
        }

        public async Task<ApiResponse<bool>> DeleteChargeTemplateAsync(Guid chargeTemplateId)
        {
            var chargeTemplate = await _chargeTemplateRepository.GetByIdAsync(chargeTemplateId);
            if (chargeTemplate == null)
                return ApiResponse<bool>.FailResponse("charge Template Not Found!");

            await _chargeTemplateRepository.DeleteAsync(chargeTemplate);
            await _chargeTemplateRepository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "charge Template  Deleted Successfully");
        }

        public async Task<ApiResponse<IEnumerable<ChargeTemplateDto>>> GetAllChargeTemplateAsync()
        {
           var chargeTemplates = await _chargeTemplateRepository.GetAllAsync();

            if (chargeTemplates == null) return ApiResponse<IEnumerable<ChargeTemplateDto>>.FailResponse("failed to fetch charge templates");
           
            var chargeTemplatesDto = _mapper.Map<IEnumerable<ChargeTemplateDto>>(chargeTemplates);
            return ApiResponse<IEnumerable<ChargeTemplateDto>>.SuccessResponse(chargeTemplatesDto, "charge templates fetched successfully");
        }

        public async Task<ApiResponse<ChargeTemplateDto>> GetChargeTemplateByIdAsync(Guid chargeTemplateId)
        {
            var chargeTemplate = await _chargeTemplateRepository.GetByIdAsync(chargeTemplateId);

            if (chargeTemplate == null) return ApiResponse<ChargeTemplateDto>.FailResponse("Failed to fetch charge template");

            var chargeTemplateDto = _mapper.Map<ChargeTemplateDto>(chargeTemplate);
            return ApiResponse<ChargeTemplateDto>.SuccessResponse(chargeTemplateDto, "charge template fetched successfully");
        }

        public async Task<ApiResponse<ChargeTemplateDto>> UpdateChargeTemplateAsync(Guid chargeTemplateId, UpdateChargeTemplateDto updateDto)
        {
            var chargeTemplate = await _chargeTemplateRepository.GetByIdAsync(chargeTemplateId);
            if (chargeTemplate == null) return ApiResponse<ChargeTemplateDto>.FailResponse("invalid charge Template Id");

            _mapper.Map(updateDto, chargeTemplate);

            await _chargeTemplateRepository.UpdateAsync(chargeTemplate);
            await _chargeTemplateRepository.SaveChangesAsync();

            var chargeTemplateDto = _mapper.Map<ChargeTemplateDto>(chargeTemplate);

            return ApiResponse<ChargeTemplateDto>.SuccessResponse(chargeTemplateDto, "charge Template updated successfully");
        }
    }
}
