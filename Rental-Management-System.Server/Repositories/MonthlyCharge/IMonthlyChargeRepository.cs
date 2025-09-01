namespace Rental_Management_System.Server.Repositories.MonthlyCharge
{
    using Rental_Management_System.Server.Models;
    public interface IMonthlyChargeRepository
    {
        Task<IEnumerable<MonthlyCharge>> GetAllAsync();
        Task<MonthlyCharge?> GetByIdAsync(Guid monthlyChargeId);
        Task AddAsync(MonthlyCharge monthlyCharge);
        Task UpdateAsync(MonthlyCharge monthlyCharge);
        Task DeleteAsync(MonthlyCharge monthlyCharge);
        Task SaveChangesAsync();    
    }
}
