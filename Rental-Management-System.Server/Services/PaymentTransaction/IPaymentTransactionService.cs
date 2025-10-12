using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.DTOs.PaymentTransaction;

namespace Rental_Management_System.Server.Services.PaymentTransaction
{
    public interface IPaymentTransactionService
    {
        Task<ApiResponse<IEnumerable<PaymentTransactionDto>>> GetAllPaymentTransactionsAsync();
        Task<ApiResponse<PaymentTransactionDto>> GetPaymentTransactionByIdAsync(Guid monthlyChargeId);

        Task<ApiResponse<PaymentTransactionDto>> CreatePaymentTransactionAsync(CreatePaymentTransactionDto createPaymentTransactionDto);
        Task<ApiResponse<PaymentTransactionDto>> UpdatePaymentTransactionAsync(Guid transactionId, UpdatePaymentTransactionDto updatePaymentTransactionDto);

        Task<ApiResponse<bool>> DeletePaymentTransactionAsync(Guid transactionId);
    }
}
