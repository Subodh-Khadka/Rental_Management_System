namespace Rental_Management_System.Server.Repositories.RentPayment
{
    using Rental_Management_System.Server.Models;
    public interface IRentPaymentRepository
    {
        Task<IEnumerable<RentPayment>> GetAllAsync();
        Task<RentPayment?> GetByIdAsync(Guid rentPaymentId);
        Task AddAsync(RentPayment rentPayment);
        Task UpdateAsync(RentPayment rentPayment);
        Task DeleteAsync(RentPayment rentPayment);
        Task SaveChangesAsync();

        IQueryable<RentPayment> Query();
        Task AddRangeAsync(IEnumerable<RentPayment> rentPaymentList);
        Task<IEnumerable<RentPayment>> GetRentPaymentsByMonthAsync(DateTime month);
    }
}
