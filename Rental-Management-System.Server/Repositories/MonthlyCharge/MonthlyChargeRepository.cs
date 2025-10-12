namespace Rental_Management_System.Server.Repositories.MonthlyCharge
{
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Data;
    using Rental_Management_System.Server.DTOs.MonthlyCharge;
    using Rental_Management_System.Server.Models;
    using Rental_Management_System.Server.Repositories;
    public class MonthlyChargeRepository : IMonthlyChargeRepository
    {
        private readonly RentalDbContext _context;

        public MonthlyChargeRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MonthlyCharge>> GetAllAsync()
        {
            return await _context.MonthlyCharges.Where(m => !m.IsDeleted)
            .Include(mc => mc.RentPayment)
                .ThenInclude(rp => rp.Room)
            .Include(mc => mc.RentPayment)
                .ThenInclude(rp => rp.RentalContract)
                    .ThenInclude(rc => rc.Tenant)
            .ToListAsync();
        }

        public async Task<MonthlyCharge?> GetByIdAsync(Guid monthlyChargeId)
        {
            return await _context.MonthlyCharges.FirstOrDefaultAsync(m => m.MonthlyChargeId == monthlyChargeId && !m.IsDeleted);
        }

        public async Task AddAsync(MonthlyCharge monthlyCharge)
        {
           await _context.MonthlyCharges.AddAsync(monthlyCharge); 
        }

        public Task UpdateAsync(MonthlyCharge monthlyCharge)
        {
            _context.MonthlyCharges.Update(monthlyCharge);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MonthlyCharge monthlyCharge)
        {
            monthlyCharge.IsDeleted = true;
            monthlyCharge.DeletedDate = DateTime.UtcNow;
            _context.MonthlyCharges.Update(monthlyCharge);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<MonthlyCharge> charges)
        {
            await _context.MonthlyCharges.AddRangeAsync(charges);
        }

        // ✅ Load relations so we can group in the service
        public async Task<IEnumerable<MonthlyCharge>> GetAllWithRelationsAsync()
        {
            return await _context.MonthlyCharges
                .Include(m => m.RentPayment)
                    .ThenInclude(r => r.Room)
                .Include(m => m.RentPayment)
                    .ThenInclude(r => r.RentalContract)
                        .ThenInclude(c => c.Tenant)
                .ToListAsync();
        }

        public IQueryable<MonthlyCharge> Query()
        {
            return _context.MonthlyCharges
                .Where(m => !m.IsDeleted)
                .Include(m => m.RentPayment)
                    .ThenInclude(r => r.Room)
                .Include(m => m.RentPayment)
                    .ThenInclude(r => r.RentalContract)
                        .ThenInclude(c => c.Tenant);
        }

    }
}
