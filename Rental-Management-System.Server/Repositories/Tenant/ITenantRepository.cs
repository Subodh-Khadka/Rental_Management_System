
namespace Rental_Management_System.Server.Repositories.Tenant
{
    using Rental_Management_System.Server.Models;
    public interface ITenantRepository
    {
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<Tenant?> GetByIdAsync(Guid tenantId);
        Task AddAsync(Tenant tenant);
        Task UpdateAsync(Tenant tenant);
        Task DeleteAsync(Tenant tenant);
        Task SavechangesAsync();

    }
}
