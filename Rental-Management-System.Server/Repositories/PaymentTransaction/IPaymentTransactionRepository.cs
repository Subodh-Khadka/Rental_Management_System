using System.Threading.Tasks;

namespace Rental_Management_System.Server.Repositories.PaymentTransaction
{
    using Rental_Management_System.Server.Models;
    public interface IPaymentTransactionRepository
    {
        Task<IEnumerable<PaymentTransaction>> GetAllAsync();
        Task<PaymentTransaction?> GetByIdAsync(Guid transactionId); 
        Task AddAsync(PaymentTransaction transaction);
        Task UpdateAsync(PaymentTransaction transaction);
        Task DeleteAsync(PaymentTransaction transaction);
        Task SaveChangesAsync();
    }
}
