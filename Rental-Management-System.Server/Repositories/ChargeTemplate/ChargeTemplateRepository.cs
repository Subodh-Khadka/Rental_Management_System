using Microsoft.EntityFrameworkCore;
using Rental_Management_System.Server.Data;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.Repositories.CharegTemplate
{
    public class ChargeTemplateRepository : IChargeTemplateRepository
    {
        private readonly RentalDbContext _context;

        public ChargeTemplateRepository(RentalDbContext context) {
            _context = context;
        }

        public async Task AddAsync(ChargeTemplate chargeTemplate)
        {
            await _context.ChargeTemplates.AddAsync(chargeTemplate);
        }

        public Task DeleteAsync(ChargeTemplate chargeTemplate)
        {
            chargeTemplate.IsDeleted = true;
            chargeTemplate.DeletedDate = DateTime.UtcNow;
            _context.ChargeTemplates.Update(chargeTemplate);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ChargeTemplate>> GetAllAsync()
        {
            return await _context.ChargeTemplates.Where(ct => !ct.IsDeleted).ToListAsync(); 
        }

        public async Task<ChargeTemplate?> GetByIdAsync(Guid chargeTemplateId)
        {
            return await _context.ChargeTemplates
                .FirstOrDefaultAsync(ct => ct.ChargeTemplateId == chargeTemplateId && !ct.IsDeleted);
        }

        public Task UpdateAsync(ChargeTemplate chargeTemplate)
        {
            _context.ChargeTemplates.Update(chargeTemplate);
            return Task.CompletedTask;
        }
        
        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }

        public IQueryable<ChargeTemplate> Query()
        {
            return _context.ChargeTemplates
                .Where(m => !m.IsDeleted);
               
        }
    }
}
