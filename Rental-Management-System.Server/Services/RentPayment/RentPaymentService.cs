using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.Repositories.MonthlyCharge;
using Rental_Management_System.Server.Repositories.RentPayment;

namespace Rental_Management_System.Server.Services.RentPayment
{
    using Rental_Management_System.Server.DTOs.RentPayment;
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories.RentalContract;
    using Rental_Management_System.Server.Repositories.Room;

    public class RentPaymentService : IRentPaymentService
    {
        private readonly IRentPaymentRepository _paymentRepository;
        private readonly IMonthlyChargeRepository _monthlyChargeRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRentalContractRepository _rentalContractRepository;
        private readonly IMapper _mapper;

        public RentPaymentService(IMonthlyChargeRepository monthlyChargeRepository, IMapper mapper, IRentPaymentRepository paymentRepository, IRoomRepository roomRepository, IRentalContractRepository rentalContractRepository)
        {
            _mapper = mapper;
            _monthlyChargeRepository = monthlyChargeRepository;
            _paymentRepository = paymentRepository;
            _roomRepository = roomRepository;
            _rentalContractRepository = rentalContractRepository;
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
            var rentalContractId = await _rentalContractRepository.GetByIdAsync(createRentPaymentDto.RentalContractId);
            if (rentalContractId == null) return ApiResponse<RentPaymentDto>.FailResponse("Invalid rental Contract Id");

            var roomId = await _roomRepository.GetByIdAsync(createRentPaymentDto.RoomId);
            if (roomId == null) return ApiResponse<RentPaymentDto>.FailResponse("Invalid room Id");

            var rentPayment = _mapper.Map<RentPayment>(createRentPaymentDto);
            //rentPayment.MonthlyCharges = null;
            await _paymentRepository.AddAsync(rentPayment);
            await _paymentRepository.SaveChangesAsync();

            //if (createRentPaymentDto.MonthlyCharges != null && createRentPaymentDto.MonthlyCharges.Any())
            //{
            //    foreach (var chargeDto in createRentPaymentDto.MonthlyCharges)
            //    {
            //        var charge = _mapper.Map<MonthlyCharge>(chargeDto);
            //        charge.RentPaymentId = rentPayment.PaymentId;
            //        await _monthlyChargeRepository.AddAsync(charge);
            //    }
            //    await _monthlyChargeRepository.SaveChangesAsync();
            //}

            if(rentPayment.MonthlyCharges != null && rentPayment.MonthlyCharges.Any())
            {
                foreach(var charge in rentPayment.MonthlyCharges)
                {
                    charge.RentPaymentId = rentPayment.PaymentId;
                }
            } 

            var rentPaymentDto = _mapper.Map<RentPaymentDto>(rentPayment);
            return ApiResponse<RentPaymentDto>.SuccessResponse(rentPaymentDto, "Data fetched successfully");
        }

        public async Task<ApiResponse<RentPaymentDto>> UpdateRentPaymentAsync(Guid rentPaymentId, UpdateRentPaymentDto updateRentPaymentDto)
        {
            var rentPayment = await _paymentRepository.GetByIdAsync(rentPaymentId);
            if (rentPayment == null) return ApiResponse<RentPaymentDto>.FailResponse($"Rent payment of Id:{rentPaymentId} was not found.");

           

            _mapper.Map(updateRentPaymentDto, rentPayment);
            await _paymentRepository.UpdateAsync(rentPayment);
            await _paymentRepository.SaveChangesAsync();

            var rentPaymentDto = _mapper.Map<RentPaymentDto>(rentPayment);
            return ApiResponse<RentPaymentDto>.SuccessResponse(rentPaymentDto, $"Rent payment of id:{rentPaymentId} update successfully");
        }

        public async Task<ApiResponse<bool>> DeleteRentPaymentAsync(Guid rentPaymentId)
        {
            var rentPayment = await _paymentRepository.GetByIdAsync(rentPaymentId);
            if (rentPayment == null) return ApiResponse<bool>.FailResponse("Invalid rent payment Id");

            await _paymentRepository.DeleteAsync(rentPayment);
            await _paymentRepository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, $"Rent payment of Id:{rentPaymentId} deleted Successfully");

        }
    }
}
