using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.DTOs.RentalContract;
using Rental_Management_System.Server.Repositories.MonthlyCharge;

using Microsoft.EntityFrameworkCore;



namespace Rental_Management_System.Server.Services.MonthlyCharge
{
using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.CharegTemplate;
    using Rental_Management_System.Server.Repositories.RentPayment;

    public class MonthlyChargeService : IMonthlyChargeService
    {
        private readonly IMonthlyChargeRepository _monthlyChargeRepository;
        private readonly IChargeTemplateRepository _chargeTemplateRepository;
        private readonly IRentPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public MonthlyChargeService(IMonthlyChargeRepository monthlyChargeRepository, IMapper mapper, IRentPaymentRepository paymentRepository, IChargeTemplateRepository chargeTemplateRepository) 
        {
            _mapper = mapper;
            _monthlyChargeRepository = monthlyChargeRepository;
            _paymentRepository = paymentRepository;
            _chargeTemplateRepository = chargeTemplateRepository;
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
            // 1️⃣ Parse month
            if (!DateTime.TryParse(dto.Month + "-01", out var monthDate))
                return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("Invalid month format");

            // 2️⃣ Check if monthly charges already exist for that month
            var existingCharges = await _monthlyChargeRepository.Query()
                .AnyAsync(mc => mc.RentPayment.PaymentMonth.Year == monthDate.Year &&
                                mc.RentPayment.PaymentMonth.Month == monthDate.Month);

            if (existingCharges)
                return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("Monthly charges already generated for this month");

            var payments = await _paymentRepository.Query()
                         .Where(p => p.PaymentMonth.Year == monthDate.Year &&
                                     p.PaymentMonth.Month == monthDate.Month &&
                                     p.Room.IsActive == true) // 👈 safe check
                          .ToListAsync();


            if (payments == null || !payments.Any())
                return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("No active rent payments found for the selected month");

            // 4️⃣ Get active (non-deleted) charge templates
            var templates = await _chargeTemplateRepository.Query()
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            if (templates == null || !templates.Any())
                return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("No active charge templates found");

            var chargesToCreate = new List<MonthlyCharge>();

            // 5️⃣ Generate monthly charges
            foreach (var payment in payments)
            {
                foreach (var template in templates)
                {
                    decimal units = 0;

                    if (template.IsVariable)
                    {
                        var paymentDto = dto.Payments.FirstOrDefault(p => p.PaymentId == payment.PaymentId);
                        if (paymentDto != null)
                        {
                            var templateDto = paymentDto.Templates.FirstOrDefault(t => t.TemplateId == template.ChargeTemplateId);
                            if (templateDto != null)
                                units = templateDto.Units;
                        }
                    }

                    decimal amount = template.IsVariable ? units * template.DefaultAmount : template.DefaultAmount;

                    var monthlyCharge = new MonthlyCharge
                    {
                        MonthlyChargeId = Guid.NewGuid(),
                        RentPaymentId = payment.PaymentId,
                        ChargeTemplateId = template.ChargeTemplateId,
                        ChargeType = template.ChargeType,
                        Units = template.IsVariable ? units : null,
                        Amount = amount
                    };

                    chargesToCreate.Add(monthlyCharge);
                }
            }

            // 6️⃣ Save all charges
            await _monthlyChargeRepository.AddRangeAsync(chargesToCreate);
            await _monthlyChargeRepository.SaveChangesAsync();

            // 7️⃣ Prepare result
            var result = new GenerateMonthlyChargeResultDto
            {
                Month = dto.Month,
                TotalChargesCreated = chargesToCreate.Count
            };

            return ApiResponse<GenerateMonthlyChargeResultDto>.SuccessResponse(result, "Monthly charges generated successfully");
        }


        //public async Task<ApiResponse<GenerateMonthlyChargeResultDto>> GenerateMonthlyChargesAsync(GenerateMonthlyChargeDto dto)
        //{

        //    // Parse month
        //    if (!DateTime.TryParse(dto.Month + "-01", out var monthDate))
        //        return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("Invalid month format");

        //    // 2️⃣ Check if charges already exist for this month
        //    var existingCharges = await _monthlyChargeRepository.Query()
        //        .AnyAsync(mc => mc.RentPayment.PaymentMonth.Year == monthDate.Year &&
        //                        mc.RentPayment.PaymentMonth.Month == monthDate.Month);

        //    if (existingCharges)
        //        return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("Monthly charges already generated for this month");

        //    // Get active rent payments
        //    var payments = await _paymentRepository.GetAllAsync();
        //    if (payments == null || !payments.Any())
        //        return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("No active payments found");

        //    // Get charge templates
        //    var templates = await _chargeTemplateRepository.GetAllAsync();
        //    if (templates == null || !templates.Any())
        //        return ApiResponse<GenerateMonthlyChargeResultDto>.FailResponse("No charge templates found");

        //    var chargesToCreate = new List<MonthlyCharge>();

        //    //Generate charges
        //    foreach (var payment in payments)
        //    {
        //        foreach (var template in templates)
        //        {
        //            decimal units = 0;

        //            if (template.IsVariable)
        //            {
        //                var paymentDto = dto.Payments.FirstOrDefault(p => p.PaymentId == payment.PaymentId);
        //                if (paymentDto != null)
        //                {
        //                    var templateDto = paymentDto.Templates.FirstOrDefault(t => t.TemplateId == template.ChargeTemplateId);
        //                    if (templateDto != null)
        //                    {
        //                        units = templateDto.Units;
        //                    }
        //                }
        //            }

        //            decimal amount = template.IsVariable ? units * template.DefaultAmount : template.DefaultAmount;

        //            var monthlyCharge = new MonthlyCharge
        //            {
        //                MonthlyChargeId = Guid.NewGuid(),
        //                RentPaymentId = payment.PaymentId,
        //                ChargeTemplateId = template.ChargeTemplateId,
        //                ChargeType = template.ChargeType,
        //                Units = template.IsVariable ? units : null,
        //                Amount = amount
        //            };

        //            chargesToCreate.Add(monthlyCharge);
        //        }
        //    }

        //    // 6️⃣ Save all charges
        //    await _monthlyChargeRepository.AddRangeAsync(chargesToCreate);
        //    await _monthlyChargeRepository.SaveChangesAsync();

        //    var result = new GenerateMonthlyChargeResultDto
        //    {
        //        Month = dto.Month,
        //        TotalChargesCreated = chargesToCreate.Count
        //    };

        //    return ApiResponse<GenerateMonthlyChargeResultDto>.SuccessResponse(result, "Monthly charges generated successfully");
        //}


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
