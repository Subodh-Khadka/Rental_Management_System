using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.DTOs.RentalContract;
using Rental_Management_System.Server.Repositories.MonthlyCharge;


namespace Rental_Management_System.Server.Services.MonthlyCharge
{
using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.RentPayment;

    public class MonthlyChargeService : IMonthlyChargeService
    {
        private readonly IMonthlyChargeRepository _monthlyChargeRepository;
        private readonly IRentPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public MonthlyChargeService(IMonthlyChargeRepository monthlyChargeRepository, IMapper mapper, IRentPaymentRepository paymentRepository) 
        {
            _mapper = mapper;
            _monthlyChargeRepository = monthlyChargeRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<ApiResponse<IEnumerable<MonthlyChargeDto>>> GetAllMonthlyChargeAsync()
        {
            var monthlyCharges =  await _monthlyChargeRepository.GetAllAsync();
            if(monthlyCharges == null)
            {
                return ApiResponse<IEnumerable<MonthlyChargeDto>>.FailResponse("no data found");
            }

            var charges = _mapper.Map<IEnumerable<MonthlyChargeDto>>(monthlyCharges);
            return ApiResponse<IEnumerable<MonthlyChargeDto>>.SuccessResponse(charges, "data fetched successfully");
        }

        public async Task<ApiResponse<MonthlyChargeDto>> GetMonthlyChargeByIdAsync(Guid monthlychargeId)
        {
            var monthlyCharge = await _monthlyChargeRepository.GetByIdAsync(monthlychargeId);
            if (monthlyCharge == null) return ApiResponse<MonthlyChargeDto>.FailResponse("Invalid monthly charge Id");

            var charge = _mapper.Map<MonthlyChargeDto>(monthlyCharge);
            return ApiResponse<MonthlyChargeDto>.SuccessResponse(charge, "Monthly charge fetched Successfullly");
        }

        public async Task<ApiResponse<MonthlyChargeDto>> CreateMonthlyChargeAsync(CreateMonthlyChargeDto createMonthlyChargeDto)
        {
            var rentPayment = await _paymentRepository.GetByIdAsync(createMonthlyChargeDto.RentPaymentId);
            if (rentPayment == null) return ApiResponse<MonthlyChargeDto>.FailResponse("Invalid payment Id");
         
            var monthlyCharge = _mapper.Map<MonthlyCharge>(createMonthlyChargeDto);
            await _monthlyChargeRepository.AddAsync(monthlyCharge);
            await _monthlyChargeRepository.SaveChangesAsync();

            var monthlychargeDto = _mapper.Map<MonthlyChargeDto>(monthlyCharge);
            return ApiResponse<MonthlyChargeDto>.SuccessResponse(monthlychargeDto, "Data fetched successfully");
        }

        public async Task<ApiResponse<MonthlyChargeDto>> UpdateMonthlyChargeAsync(Guid monthlyChargeId, UpdateMonthlyChargeDto updateMonthlyChargeDto)
        {
            var monthlyCharge = await _monthlyChargeRepository.GetByIdAsync(monthlyChargeId);
            if (monthlyCharge == null) return ApiResponse<MonthlyChargeDto>.FailResponse($"monthly charge of Id:{monthlyChargeId} was not found.");

            var rentPayment = _paymentRepository.GetByIdAsync(updateMonthlyChargeDto.RentPaymentId);
            if (rentPayment == null) return ApiResponse<MonthlyChargeDto>.FailResponse("Invalid payment Id");

            _mapper.Map(updateMonthlyChargeDto, monthlyCharge);
            await _monthlyChargeRepository.UpdateAsync(monthlyCharge);
            await _monthlyChargeRepository.SaveChangesAsync();

            var monthlyChargeDto = _mapper.Map<MonthlyChargeDto>(monthlyCharge);
            return ApiResponse<MonthlyChargeDto>.SuccessResponse(monthlyChargeDto, $"Monthly charge of id:{monthlyChargeId} update successfully");
        }

        public async Task<ApiResponse<bool>> DeleteMonthlyChargeAsync(Guid monthlyChargeId)
        {
            var monthlyCharge = await _monthlyChargeRepository.GetByIdAsync(monthlyChargeId);
            if (monthlyCharge == null) return ApiResponse<bool>.FailResponse("Invalid monthly charge Id");

            await _monthlyChargeRepository.DeleteAsync(monthlyCharge);
            await _monthlyChargeRepository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, $"Monthly charge of Id:{monthlyChargeId} deleted Successfully");

        }
    }
}
