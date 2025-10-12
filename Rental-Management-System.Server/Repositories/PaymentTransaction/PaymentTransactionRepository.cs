using Rental_Management_System.Server.Data;

namespace Rental_Management_System.Server.Repositories.PaymentTransaction
{
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Models;
    public class PaymentTransactionRepository : IPaymentTransactionRepository
    {
        private readonly RentalDbContext _context;

        public PaymentTransactionRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentTransaction>> GetAllAsync()
        {
            return await _context.PaymentTransactions.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<PaymentTransaction?> GetByIdAsync(Guid rentPaymentId)
        {
            return await _context.PaymentTransactions.FirstOrDefaultAsync(p => p.TransactionId == rentPaymentId && !p.IsDeleted);
        }

        public async Task AddAsync(PaymentTransaction transaction)
        {
            await _context.PaymentTransactions.AddAsync(transaction);
        }

        public Task UpdateAsync(PaymentTransaction transaction)
        {
            _context.PaymentTransactions.Update(transaction);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(PaymentTransaction transaction)
        {
            transaction.IsDeleted = true;
            transaction.DeletedDate = DateTime.UtcNow;
            _context.PaymentTransactions.Update(transaction);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
