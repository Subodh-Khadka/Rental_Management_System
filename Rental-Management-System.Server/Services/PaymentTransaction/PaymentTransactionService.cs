using AutoMapper;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.RentPayment;
using Rental_Management_System.Server.Repositories.MonthlyCharge;
using Rental_Management_System.Server.Repositories.PaymentTransaction;
using Rental_Management_System.Server.Repositories.RentalContract;
using Rental_Management_System.Server.Repositories.RentPayment;
using Rental_Management_System.Server.Repositories.Room;

namespace Rental_Management_System.Server.Services.PaymentTransaction
{
    using Rental_Management_System.Server.Data;
    using Rental_Management_System.Server.DTOs.PaymentTransaction;
    using Rental_Management_System.Server.Models;
    

    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IRentPaymentRepository _paymentRepository;
         
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        private readonly IMapper _mapper;

        public PaymentTransactionService(IPaymentTransactionRepository paymentTransactionRepository, IMapper mapper, IRentPaymentRepository paymentRepository)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _paymentTransactionRepository = paymentTransactionRepository;     
        }

        public async Task<ApiResponse<IEnumerable<PaymentTransactionDto>>> GetAllPaymentTransactionsAsync()
        {
            var transactions = await _paymentTransactionRepository.GetAllAsync();
            if (transactions == null || !transactions.Any())
            {
                return ApiResponse<IEnumerable<PaymentTransactionDto>>.FailResponse("No transaction found");
            }

            var transactionDtos = _mapper.Map<IEnumerable<PaymentTransactionDto>>(transactions);

            foreach (var dto in transactionDtos)
            {
                dto.TransactionId = MaskingHelper.MaskId(dto.TransactionId);
                dto.RentPaymentId = MaskingHelper.MaskId(dto.RentPaymentId);

            }

            return ApiResponse<IEnumerable<PaymentTransactionDto>>.SuccessResponse(transactionDtos, "Data fetched successfully");
        }

        public async  Task<ApiResponse<PaymentTransactionDto>> GetPaymentTransactionByIdAsync(Guid monthlyChargeId)
        {
            var transaction = await _paymentTransactionRepository.GetByIdAsync(monthlyChargeId);
            if (transaction == null) return ApiResponse<PaymentTransactionDto>.FailResponse("Invalid transaction Id");

            var transactionDto = _mapper.Map<PaymentTransactionDto>(transaction);
            return ApiResponse<PaymentTransactionDto>.SuccessResponse(transactionDto, "Transaction fetched Successfullly");
        }

        public async Task<ApiResponse<PaymentTransactionDto>> CreatePaymentTransactionAsync(CreatePaymentTransactionDto createPaymentTransactionDto)
        {
            var existingRentPayment = await _paymentRepository.GetByIdAsync(createPaymentTransactionDto.RentPaymentId);
            if (existingRentPayment == null) return ApiResponse<PaymentTransactionDto>.FailResponse("Invalid rentPayment Id");            

            var paymentTransaction = _mapper.Map<PaymentTransaction>(createPaymentTransactionDto);
            
            await _paymentTransactionRepository.AddAsync(paymentTransaction);
            await _paymentTransactionRepository.SaveChangesAsync();

            var transactionDto = _mapper.Map<PaymentTransactionDto>(paymentTransaction);
            return ApiResponse<PaymentTransactionDto>.SuccessResponse(transactionDto, "Payment Transaction Created successfully");
        }

        public async Task<ApiResponse<PaymentTransactionDto>> UpdatePaymentTransactionAsync(Guid transactionId, UpdatePaymentTransactionDto updatePaymentTransactionDto)
        {
            var existingTransaction = await _paymentTransactionRepository.GetByIdAsync(transactionId);
            if (existingTransaction == null) return ApiResponse<PaymentTransactionDto>.FailResponse($"Payment transaction of Id:{transactionId} was not found.");

            var rentPayment = await _paymentRepository.GetByIdAsync(updatePaymentTransactionDto.RentPaymentId);
            if (rentPayment == null) return ApiResponse<PaymentTransactionDto>.FailResponse("Invalid rent payment ID");

            _mapper.Map(updatePaymentTransactionDto, existingTransaction);
            await _paymentTransactionRepository.UpdateAsync(existingTransaction);
            await _paymentTransactionRepository.SaveChangesAsync();

            var transactionDto = _mapper.Map<PaymentTransactionDto>(existingTransaction);
            return ApiResponse<PaymentTransactionDto>.SuccessResponse(transactionDto, $"Rent payment of id:{transactionId} updated successfully");
        }

        public async Task<ApiResponse<bool>> DeletePaymentTransactionAsync(Guid transactionId)
        {
            var transaction = await _paymentTransactionRepository.GetByIdAsync(transactionId);
            if (transaction == null) return ApiResponse<bool>.FailResponse("Invalid transaction Id");

            await _paymentTransactionRepository.DeleteAsync(transaction);
            await _paymentTransactionRepository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, $"Rent payment of Id:{transactionId} deleted Successfully");

        }
    }
}
