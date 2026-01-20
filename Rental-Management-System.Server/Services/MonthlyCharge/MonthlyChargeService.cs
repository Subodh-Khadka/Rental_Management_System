using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.DTOs.RentalContract;
using Rental_Management_System.Server.Repositories.MonthlyCharge;



namespace Rental_Management_System.Server.Services.MonthlyCharge
{
using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.CharegTemplate;
    using Rental_Management_System.Server.Repositories.MeterReading;
    using Rental_Management_System.Server.Repositories.RentPayment;

    public class MonthlyChargeService : IMonthlyChargeService
    {
        private readonly IMonthlyChargeRepository _monthlyChargeRepository;
        private readonly IChargeTemplateRepository _chargeTemplateRepository;
        private readonly IRentPaymentRepository _paymentRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;

        private readonly IMapper _mapper;

        public MonthlyChargeService(IMonthlyChargeRepository monthlyChargeRepository, IMapper mapper,
            IRentPaymentRepository paymentRepository, IChargeTemplateRepository chargeTemplateRepository,
            IMeterReadingRepository meterReadingRepository) 
        {
            _mapper = mapper;
            _monthlyChargeRepository = monthlyChargeRepository;
            _paymentRepository = paymentRepository;
            _chargeTemplateRepository = chargeTemplateRepository;
            _meterReadingRepository = meterReadingRepository;
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
            var rentPayment = await _paymentRepository.GetByIdAsync(createMonthlyChargeDto.PaymentId);
            if (rentPayment == null) return ApiResponse<MonthlyChargeDto>.FailResponse("Invalid payment Id");
         
            var monthlyCharge = _mapper.Map<MonthlyCharge>(createMonthlyChargeDto);

            monthlyCharge.RentPayment = rentPayment;

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

        public async Task<ApiResponse<GenerateMonthlyChargeResultDto>> GenerateMonthlyChargesAsync(GenerateMonthlyChargeDto dto)
        {         
            try
            {
                // Parse month "yyyy-MM" -> DateTime
                if (!DateTime.TryParseExact(dto.Month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDate))
                    return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("Invalid month format");

                // Check if charges already exist
                var existingCharges = await _monthlyChargeRepository.Query()
                    .AnyAsync(mc => mc.RentPayment.PaymentMonth.Year == monthDate.Year &&
                                    mc.RentPayment.PaymentMonth.Month == monthDate.Month);
                if (existingCharges)
                    return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("Monthly charges already generated for this month");

                // Get active payments
                var payments = await _paymentRepository.Query()
                    .Where(p => p.PaymentMonth.Year == monthDate.Year &&
                                p.PaymentMonth.Month == monthDate.Month &&
                                p.Room.IsActive == true)
                    .ToListAsync();

                if (!payments.Any())
                    return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("No active rent payments found");

                // Get active templates
                var templates = await _chargeTemplateRepository.Query()
                    .Where(t => !t.IsDeleted)
                    .ToListAsync();

                if (!templates.Any())
                    return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("No active charge templates found");

                var chargesToCreate = new List<MonthlyCharge>();
                var meterReadingsToUpdate = new List<MeterReading>();

                foreach (var payment in payments)
                {
                    var paymentDto = dto.Payments.FirstOrDefault(p => p.PaymentId == payment.PaymentId);

                    foreach (var template in templates)
                    {
                        decimal units = 0;

                        if (template.ChargeType == "Electricity")
                        {
                            var previousReading = await _meterReadingRepository.Query()
                                .Where(r => r.PaymentId == payment.PaymentId)
                                .OrderByDescending(r => r.Month)
                                .Select(r => r.CurrentReading)
                                .FirstOrDefaultAsync();

                            var currentReading = paymentDto?.Templates
                                ?.FirstOrDefault(t => t.TemplateId == template.ChargeTemplateId)?.Units ?? 0;

                            units = Math.Max(currentReading - previousReading, 0);

                            meterReadingsToUpdate.Add(new MeterReading
                            {
                                MeterReadingId = Guid.NewGuid(),
                                PaymentId = payment.PaymentId,
                                RentPayment = payment,
                                Month = dto.Month,
                                PreviousReading = previousReading,
                                CurrentReading = currentReading
                            });
                        }
                        else if (template.IsVariable && paymentDto != null)
                        {
                            units = paymentDto.Templates
                                ?.FirstOrDefault(t => t.TemplateId == template.ChargeTemplateId)?.Units ?? 0;
                        }

                        decimal amount = template.IsVariable || template.ChargeType == "Electricity"
                            ? units * template.DefaultAmount
                            : template.DefaultAmount;

                        chargesToCreate.Add(new MonthlyCharge
                        {
                            MonthlyChargeId = Guid.NewGuid(),
                            RentPaymentId = payment.PaymentId,
                            ChargeTemplateId = template.ChargeTemplateId,
                            ChargeType = template.ChargeType,
                            Units = template.IsVariable || template.ChargeType == "Electricity" ? units : null,
                            Amount = amount
                        });
                    }
                }

                if (meterReadingsToUpdate.Any())
                    await _meterReadingRepository.AddRangeAsync(meterReadingsToUpdate);                 

                if (chargesToCreate.Any())
                {
                    await _meterReadingRepository.SaveChangesAsync();
                    await _monthlyChargeRepository.AddRangeAsync(chargesToCreate);
                    await _monthlyChargeRepository.SaveChangesAsync();
                }

                return ApiResponse<GenerateMonthlyChargeResultDto>.SuccessResponse(
                    new GenerateMonthlyChargeResultDto
                    {
                        Month = dto.Month,
                        TotalChargesCreated = chargesToCreate.Count
                    },
                    "Monthly charges generated successfully"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating monthly charges: " + ex);
                return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("An error occurred: " + ex.Message);
            }
        }











        public async Task<ApiResponse<IEnumerable<MonthlyChargeSummaryDto>>> GetMonthlyChargeSummaryAsync()
        {
            var result = await _monthlyChargeRepository.GetAllWithRelationsAsync();

            if (result == null || !result.Any())
                return ApiResponse<IEnumerable<MonthlyChargeSummaryDto>>.SuccessResponse(
                    Enumerable.Empty<MonthlyChargeSummaryDto>(),
                    "No data found"
                );

            var summary = result
                .GroupBy(mc => new
                {
                    RoomName = mc.RentPayment?.Room?.RoomTitle ?? "Unknown Room",
                    TenantName = mc.RentPayment?.RentalContract?.Tenant?.Name ?? "Unknown Tenant",
                    Month = mc.RentPayment?.PaymentMonth.ToString("yyyy-MM") ?? "Unknown Month"
                })
                .Select(g => new MonthlyChargeSummaryDto
                {
                    RoomName = g.Key.RoomName,
                    TenantName = g.Key.TenantName,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(x => x.Amount),
                    Status = g.Any(x => x.RentPayment != null && x.RentPayment.DueAmount > 0) ? "Pending" : "Paid"
                })
                .ToList();

            return ApiResponse<IEnumerable<MonthlyChargeSummaryDto>>.SuccessResponse(summary, "Data fetched successfully");
        }

    }
}
