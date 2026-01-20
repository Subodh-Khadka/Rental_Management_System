using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.Repositories.MonthlyCharge;
using Rental_Management_System.Server.Repositories.RentPayment;

namespace Rental_Management_System.Server.Services.RentPayment
{
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Data;
    using Rental_Management_System.Server.DTOs.RentPayment;
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.RentalContract;
    using Rental_Management_System.Server.Repositories.Room;
    using Rental_Management_System.Server.Repositories.Tenant;

    public class RentPaymentService : IRentPaymentService
    {
        private readonly IRentPaymentRepository _paymentRepository;
        private readonly IMonthlyChargeRepository _monthlyChargeRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRentalContractRepository _rentalContractRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;

        public RentPaymentService(IMonthlyChargeRepository monthlyChargeRepository, ITenantRepository tenantRepository,
            IMapper mapper, IRentPaymentRepository paymentRepository, IRoomRepository roomRepository, IRentalContractRepository rentalContractRepository)
        {
            _mapper = mapper;
            _monthlyChargeRepository = monthlyChargeRepository;
            _paymentRepository = paymentRepository;
            _roomRepository = roomRepository;
            _rentalContractRepository = rentalContractRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<ApiResponse<IEnumerable<RentPaymentDto>>> GetAllRentPaymentAsync()
        {
            var rentPayments = await _paymentRepository.GetAllAsync();

            if (rentPayments == null)
            {
                return ApiResponse<IEnumerable<RentPaymentDto>>.FailResponse("no data found");
            }

            var payments = _mapper.Map<IEnumerable<RentPaymentDto>>(rentPayments);
            return ApiResponse<IEnumerable<RentPaymentDto>>.SuccessResponse(payments, "data fetched successfully");
        }

        public async Task<ApiResponse<RentPaymentDto>> GetRentPaymentByIdAsync(Guid rentPaymentId)
        {
            var rentPayment = await _paymentRepository.GetByIdAsync(rentPaymentId);
            if (rentPayment == null) return ApiResponse<RentPaymentDto>.FailResponse("Invalid rent payment Id");

            var payment = _mapper.Map<RentPaymentDto>(rentPayment);
            return ApiResponse<RentPaymentDto>.SuccessResponse(payment, "Rent Payment fetched Successfullly");
        }

        public async Task<ApiResponse<RentPaymentDto>> CreateRentPaymentAsync(CreateRentPaymentDto createRentPaymentDto)
        {
            // 1. Fetch rental contract including Room and Tenant
            //var rentalContract = await _rentalContractRepository
            //    .GetQueryable()
            //    .Include(rc => rc.Room)
            //    .Include(rc => rc.Tenant)
            //    .FirstOrDefaultAsync(rc => rc.RentalContractId == createRentPaymentDto.RentalContractId);

            //if (rentalContract == null)
            //    return ApiResponse<RentPaymentDto>.FailResponse("Invalid rental contract Id");

            var rentalContract = await _rentalContractRepository.GetByIdAsync(createRentPaymentDto.RentalContractId);

            if (rentalContract == null)
                return ApiResponse<RentPaymentDto>.FailResponse("Invalid rental contract Id");

            // Manually load related entities
            rentalContract.Room = await _roomRepository.GetByIdAsync(rentalContract.RoomId);
            rentalContract.Tenant = await _tenantRepository.GetByIdAsync(rentalContract.TenantId);


            // 2. Map DTO to RentPayment entity
            var rentPayment = _mapper.Map<RentPayment>(createRentPaymentDto);

            // 3. Set navigation properties
            rentPayment.RentalContract = rentalContract;
            rentPayment.RoomId = rentalContract.RoomId;

            // 4. Handle monthly charges manually to avoid duplicate mapping
            if (createRentPaymentDto.MonthlyCharges != null && createRentPaymentDto.MonthlyCharges.Any())
            {
                foreach (var chargeDto in createRentPaymentDto.MonthlyCharges)
                {
                    var charge = _mapper.Map<MonthlyCharge>(chargeDto);
                    charge.RentPayment = rentPayment; // link charge to payment
                    rentPayment.MonthlyCharges.Add(charge);
                }
            }

            // 5. Save RentPayment (and charges through navigation property)
            await _paymentRepository.AddAsync(rentPayment);
            await _paymentRepository.SaveChangesAsync();

            // 6. Map entity back to DTO for response
            var rentPaymentDto = _mapper.Map<RentPaymentDto>(rentPayment);
            return ApiResponse<RentPaymentDto>.SuccessResponse(rentPaymentDto, "Rent payment created successfully");
        }


        public async Task<ApiResponse<RentPaymentDto>> UpdateRentPaymentAsync(Guid rentPaymentId, UpdateRentPaymentDto updateRentPaymentDto)
        {
            var rentPayment = await _paymentRepository.GetByIdAsync(rentPaymentId);
            if (rentPayment == null) return ApiResponse<RentPaymentDto>.FailResponse($"Rent payment of Id:{rentPaymentId} was not found.");

           
            _mapper.Map(updateRentPaymentDto, rentPayment);
            await _paymentRepository.UpdateAsync(rentPayment);
            await _paymentRepository.SaveChangesAsync();

            var rentPaymentDto = _mapper.Map<RentPaymentDto>(rentPayment);
            return ApiResponse<RentPaymentDto>.SuccessResponse(rentPaymentDto, $"Rent payment of id:{rentPaymentId} updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteRentPaymentAsync(Guid rentPaymentId)
        {
            var rentPayment = await _paymentRepository.GetByIdAsync(rentPaymentId);
            if (rentPayment == null) return ApiResponse<bool>.FailResponse("Invalid rent payment Id");

            await _paymentRepository.DeleteAsync(rentPayment);
            await _paymentRepository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, $"Rent payment of Id:{rentPaymentId} deleted Successfully");
        }

        public async Task<ApiResponse<GenerateRentPaymentResultDto>> GenerateRentPaymentAsync(GenerateRentPaymentDto dto)
        {
            // Parse Month
            if (!DateTime.TryParse(dto.Month + "-01", out var monthDate))
            {
                return ApiResponse<GenerateRentPaymentResultDto>.FailResponse("Invalid date");
            }

            // Check if rent payment already exists for that month
            var existingRentPayments = await _paymentRepository.Query()
                .Where(rp => rp.PaymentMonth.Year == monthDate.Year &&
                                rp.PaymentMonth.Month == monthDate.Month).ToListAsync();
            
            if (existingRentPayments.Any())
            {
                return ApiResponse<GenerateRentPaymentResultDto>.FailResponse("Rent payments already exist for the selected month");
            }

            var rooms = await _roomRepository.GetAllAsync();

            if (!rooms.Any())
            {
                return ApiResponse<GenerateRentPaymentResultDto>.FailResponse("No active rooms found.");
            }

            var paymentsToCreate = new List<RentPayment>();

            foreach(var room in rooms)
            {
                var activeContract = room.RentalContracts.FirstOrDefault();

                if (activeContract == null)
                    continue; // skip rooms with no active contract

                var rentPayment = new RentPayment
                {
                    PaymentId = Guid.NewGuid(),
                    RentalContractId = activeContract.RentalContractId,
                    RoomId = room.RoomId,
                    PaymentMonth = monthDate,
                    RoomPrice = room.RoomPrice,
                    PaidAmount = 0,
                    status = StaticDetail.RentPaymentStatusPending
                };

                paymentsToCreate.Add(rentPayment);
            }

            // 5️⃣ Save to database
            if (paymentsToCreate.Count == 0)
                return ApiResponse<GenerateRentPaymentResultDto>.FailResponse("No payments were generated — no rooms with active contracts.");

            await _paymentRepository.AddRangeAsync(paymentsToCreate);
            await _paymentRepository.SaveChangesAsync();

            var result = new GenerateRentPaymentResultDto
            {
                Month = dto.Month,
                TotalRentPaymentCreated = paymentsToCreate.Count,
            };

            return ApiResponse<GenerateRentPaymentResultDto>.SuccessResponse(result, "Rent payments generated successfully.");
        }

        public async Task<ApiResponse<IEnumerable<RentPaymentDto>>> GetRentPaymentsByMonthAsync(string month)
        {
            if (!DateTime.TryParse(month + "-01", out var monthDate))
                return ApiResponse<IEnumerable<RentPaymentDto>>.FailResponse("Invalid month format");

            var payments = await _paymentRepository.GetRentPaymentsByMonthAsync(monthDate);

            // Return empty list instead of failing
            var mapped = _mapper.Map<IEnumerable<RentPaymentDto>>(payments ?? new List<RentPayment>());
            return ApiResponse<IEnumerable<RentPaymentDto>>.SuccessResponse(mapped);
        }


        //public async Task<ApiResponse<IEnumerable<RentPaymentDto>>> GetRentPaymentsByMonthAsync(string month)
        //{
        //    if (!DateTime.TryParse(month + "-01", out var monthDate))
        //        return ApiResponse<IEnumerable<RentPaymentDto>>.FailResponse("Invalid month format");

        //    var payments = await _paymentRepository.GetRentPaymentsByMonthAsync(monthDate);

        //    if (payments == null || !payments.Any())
        //        return ApiResponse<IEnumerable<RentPaymentDto>>.FailResponse("No rent payments found for this month");

        //    var mapped = _mapper.Map<IEnumerable<RentPaymentDto>>(payments);
        //    return ApiResponse<IEnumerable<RentPaymentDto>>.SuccessResponse(mapped);
        //}

    }
}
