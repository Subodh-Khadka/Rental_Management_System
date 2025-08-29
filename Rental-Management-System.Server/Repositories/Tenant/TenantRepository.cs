


namespace Rental_Management_System.Server.Repositories.Tenant
{
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Data;
    using Rental_Management_System.Server.Models;
    public class TenantRepository : ITenantRepository
    {
        private readonly RentalDbContext _context;

        public TenantRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Tenant> AddAsync(Tenant tenant)
        {
             var entityEntry = await _context.Tenants.AddAsync(tenant);
            _context.SaveChanges(); 
            return entityEntry.Entity;
        }

        public async Task<bool> DeleteAsync(Guid tenantId)
        {
            var existingTenant = _context.Tenants.FirstOrDefault(t => t.TenantId == tenantId);
            if (existingTenant == null)
            {
                return false;
            }
            existingTenant.IsDeleted = true;
            existingTenant.DeletedDate = DateTime.UtcNow;
            _context.Tenants.Update(existingTenant);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Models.Tenant>> GetAllAsync()
        {
            var tenants = await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync();
            return tenants;
        }

        public async Task<Models.Tenant> GetByIdAsync(Guid tenantId)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.TenantId == tenantId);
            return tenant;
        }

        public async Task SavechangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public  async Task<Models.Tenant> UpdateAsync(Models.Tenant tenant)
        {
            var updatedRoom = _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();
            return updatedRoom.Entity;
        }
    }
}
