namespace Rental_Management_System.Server.Repositories.MeterReading
{
    using Rental_Management_System.Server.Models;
    public interface IMeterReadingRepository
    {
        Task<IEnumerable<MeterReading>> GetAllAsync();
        Task<MeterReading?> GetByIdAsync(Guid meterReadingId);
        Task<MeterReading?> GetByPaymentAndMonthAsync(Guid paymentId, DateTime month);
        Task AddAsync(MeterReading meterReading);
        Task UpdateAsync(MeterReading meterReading);
        Task DeleteAsync(MeterReading meterReading);
        Task SaveChangesAsync();
        IQueryable<MeterReading> Query();
        Task AddRangeAsync(IEnumerable<MeterReading> meterReadingsToUpdate);
    }
}
