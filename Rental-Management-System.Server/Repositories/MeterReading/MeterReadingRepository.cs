using Rental_Management_System.Server.Data;

namespace Rental_Management_System.Server.Repositories.MeterReading
{
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Models;
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly RentalDbContext _context;

        public MeterReadingRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MeterReading meterReading)
        {
            await _context.MeterReadings.AddAsync(meterReading);
        }

        public async Task DeleteAsync(MeterReading meterReading)
        {
            _context.MeterReadings.Remove(meterReading);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<MeterReading>> GetAllAsync()
        {
            return await _context.MeterReadings.ToListAsync();
        }

        public async Task<MeterReading?> GetByIdAsync(Guid meterReadingId)
        {
            return await _context.MeterReadings.FirstOrDefaultAsync(m => m.MeterReadingId == meterReadingId);
        }

        public async Task<MeterReading?> GetByPaymentAndMonthAsync(Guid paymentId, string month)
        {
            return await _context.MeterReadings
                .FirstOrDefaultAsync(m => m.PaymentId == paymentId && m.Month == month);
        }

        public async Task UpdateAsync(MeterReading meterReading)
        {
            _context.MeterReadings.Update(meterReading);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<MeterReading> Query()
        {
            return _context.MeterReadings.AsQueryable();
        }

        public async Task AddRangeAsync(IEnumerable<MeterReading> meterReadingsToUpdate)
        {
            await _context.MeterReadings.AddRangeAsync(meterReadingsToUpdate);
        }
    }
}
