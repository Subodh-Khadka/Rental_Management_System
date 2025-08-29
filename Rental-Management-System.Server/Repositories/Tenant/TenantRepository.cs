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

        public async Task<IEnumerable<Models.Tenant>> GetAllAsync()
        {
            return await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync();

        }

        public async Task<Models.Tenant?> GetByIdAsync(Guid tenantId)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.TenantId == tenantId);
        }


        public async Task AddAsync(Tenant tenant)
        {
             await _context.Tenants.AddAsync(tenant); 
        }
        public Task UpdateAsync(Models.Tenant tenant)
        {
            _context.Tenants.Update(tenant);
            return Task.CompletedTask;

        }

        public Task DeleteAsync(Tenant tenant)
        {
            tenant.IsDeleted = true;
            tenant.DeletedDate = DateTime.UtcNow;
            _context.Tenants.Update(tenant);
            return Task.CompletedTask;
        }

        public async Task SavechangesAsync()
        {
            await _context.SaveChangesAsync();
        }

      
    }
}
