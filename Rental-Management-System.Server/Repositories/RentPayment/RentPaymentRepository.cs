namespace Rental_Management_System.Server.Repositories.RentPayment
{
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Data;
    using Microsoft.EntityFrameworkCore;

    public class RentPaymentRepository : IRentPaymentRepository
    {
        private readonly RentalDbContext _context;

        public RentPaymentRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentPayment>> GetAllAsync()
        {
            return await _context.RentPayments.Where(r => !r.IsDeleted).ToListAsync();
        }

        public async Task<RentPayment?> GetByIdAsync(Guid rentPaymentId)
        {
            return await _context.RentPayments.FirstOrDefaultAsync(r => r.PaymentId == rentPaymentId && !r.IsDeleted);
        }

        public async Task AddAsync(RentPayment rentPayment)
        {
            await _context.RentPayments.AddAsync(rentPayment);
        }

        public Task UpdateAsync(RentPayment rentPayment)
        {
            _context.RentPayments.Update(rentPayment);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(RentPayment rentPayment)
        {
            rentPayment.IsDeleted = true;
            rentPayment.DeletedDate = DateTime.UtcNow;
            _context.RentPayments.Update(rentPayment);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
