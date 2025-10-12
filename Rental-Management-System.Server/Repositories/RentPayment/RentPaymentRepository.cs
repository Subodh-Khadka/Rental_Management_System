namespace Rental_Management_System.Server.Repositories.RentPayment
{
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Data;
    using Rental_Management_System.Server.Models;
    using System.Runtime.InteropServices;

    public class RentPaymentRepository : IRentPaymentRepository
    {
        private readonly RentalDbContext _context;

        public RentPaymentRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentPayment>> GetAllAsync()
        {
            return await _context.RentPayments.Where(r => !r.IsDeleted)
                .Include(rp => rp.RentalContract).ThenInclude(rc => rc.Room)
                .Include(rp => rp.RentalContract).ThenInclude(rc => rc.Tenant)
                .Include(rp => rp.MonthlyCharges)         
                .ToListAsync();
        }

        public async Task<RentPayment?> GetByIdAsync(Guid transactionId)
        {
            return await _context.RentPayments
        .Include(rp => rp.RentalContract)
            .ThenInclude(rc => rc.Room)
        .Include(rp => rp.RentalContract)
            .ThenInclude(rc => rc.Tenant)
        .Include(rp => rp.MonthlyCharges)
        .FirstOrDefaultAsync(rp => rp.PaymentId == transactionId);
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

        public IQueryable<RentPayment> Query()
        {
            return _context.RentPayments.Where(m => !m.IsDeleted).Include(p => p.Room)
                .Include(r => r.RentalContract).ThenInclude(r => r.Room);


        }

        public async Task AddRangeAsync(IEnumerable<RentPayment> rentPaymentList)
        {
            await _context.RentPayments.AddRangeAsync(rentPaymentList);
        }
    public async Task<IEnumerable<RentPayment>> GetRentPaymentsByMonthAsync(DateTime month)
        {
            return await _context.RentPayments.Where(rp => !rp.IsDeleted)
               .Include(rp => rp.Room)
               .Include(rp => rp.RentalContract)
                   .ThenInclude(rc => rc.Tenant)
               .Where(rp => rp.PaymentMonth.Year == month.Year &&
                            rp.PaymentMonth.Month == month.Month)
               .ToListAsync();
        }
    }
}
