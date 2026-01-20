using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MeterReading;
using Rental_Management_System.Server.Repositories.MeterReading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental_Management_System.Server.Services.MeterReading
{
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.RentPayment;

    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _repo;
        private readonly IRentPaymentRepository _rentPaymentRepository;
        private readonly IMapper _mapper;

        public MeterReadingService(IMeterReadingRepository repo, IMapper mapper, IRentPaymentRepository rentPaymentRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _rentPaymentRepository = rentPaymentRepository;
        }
        public async Task<ApiResponse<MeterReadingDto>> CreateAsync(CreateMeterReadingDto createDto)
        {
            if (createDto.PaymentId == Guid.Empty)
                return ApiResponse<MeterReadingDto>.FailResponse("PaymentId is required");

            // Fetch RentPayment
            var payment = await _rentPaymentRepository.GetByIdAsync(createDto.PaymentId);   
                                

            if (payment == null)
                return ApiResponse<MeterReadingDto>.FailResponse("Payment not found");

            // Map DTO to MeterReading
            var meterReading = _mapper.Map<MeterReading>(createDto);
            meterReading.MeterReadingId = Guid.NewGuid();
            meterReading.RentPayment = payment; // assign navigation property

            await _repo.AddAsync(meterReading);
            await _repo.SaveChangesAsync();

            var dto = _mapper.Map<MeterReadingDto>(meterReading);
            return ApiResponse<MeterReadingDto>.SuccessResponse(dto, "Meter reading created successfully");
        }


        public async Task<ApiResponse<bool>> DeleteAsync(Guid meterReadingId)
        {
            var meterReading = await _repo.GetByIdAsync(meterReadingId);
            if (meterReading == null)
                return ApiResponse<bool>.FailResponse("Meter reading not found");

            await _repo.DeleteAsync(meterReading);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Meter reading deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<MeterReadingDto>>> GetAllAsync()
        {
            var readings = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<MeterReadingDto>>(readings);
            return ApiResponse<IEnumerable<MeterReadingDto>>.SuccessResponse(dto, "Meter readings fetched successfully");
        }

        public async Task<ApiResponse<MeterReadingDto>> GetByIdAsync(Guid meterReadingId)
        {
            var reading = await _repo.GetByIdAsync(meterReadingId);
            if (reading == null)
                return ApiResponse<MeterReadingDto>.FailResponse("Meter reading not found");

            var dto = _mapper.Map<MeterReadingDto>(reading);
            return ApiResponse<MeterReadingDto>.SuccessResponse(dto, "Meter reading fetched successfully");
        }

        public async Task<ApiResponse<MeterReadingDto>> GetByPaymentAndMonthAsync(Guid paymentId,DateTime month)
        {
            // Normalize month → first day of the month
            month = new DateTime(month.Year, month.Month, 1);

            // Fetch reading for the same payment & same month
            var reading = await _repo.Query()
                .FirstOrDefaultAsync(r =>
                    r.PaymentId == paymentId &&
                    r.Month.Year == month.Year &&
                    r.Month.Month == month.Month
                );

            MeterReadingDto dto;

            if (reading == null)
            {
                // Return default DTO if no reading exists
                dto = new MeterReadingDto
                {
                    MeterReadingId = Guid.Empty,
                    PaymentId = paymentId,
                    Month = month,
                    PreviousReading = 0,
                    CurrentReading = 0,
                    UnitsUsed = 0
                };

                return ApiResponse<MeterReadingDto>.SuccessResponse(
                    dto,
                    "No meter reading found, returning default"
                );
            }

            dto = _mapper.Map<MeterReadingDto>(reading);
            return ApiResponse<MeterReadingDto>.SuccessResponse(
                dto,
                "Meter reading fetched successfully"
            );
        }



        public async Task<ApiResponse<MeterReadingDto>> UpdateAsync(Guid meterReadingId, UpdateMeterReadingDto updateDto)
        {
            var reading = await _repo.GetByIdAsync(meterReadingId);
            if (reading == null)
                return ApiResponse<MeterReadingDto>.FailResponse("Meter reading not found");

            _mapper.Map(updateDto, reading);

            await _repo.UpdateAsync(reading);
            await _repo.SaveChangesAsync();

            var dto = _mapper.Map<MeterReadingDto>(reading);
            return ApiResponse<MeterReadingDto>.SuccessResponse(dto, "Meter reading updated successfully");
        }
    }
}
