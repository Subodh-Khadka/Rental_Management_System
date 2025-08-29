
namespace Rental_Management_System.Server.Repositories.Tenant
{
    using Rental_Management_System.Server.Models;
    public interface ITenantRepository
    {
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<Tenant> GetByIdAsync(Guid tenantId);
        Task<Tenant> AddAsync(Tenant tenant);
        Task<Tenant> UpdateAsync(Tenant tenant);
        Task<bool> DeleteAsync(Guid tenantId);
        Task SavechangesAsync();

    }
}
