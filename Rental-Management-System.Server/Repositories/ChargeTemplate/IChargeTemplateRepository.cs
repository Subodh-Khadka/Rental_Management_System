using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.Repositories.CharegTemplate
{
    public interface IChargeTemplateRepository
    {
        Task<IEnumerable<ChargeTemplate>> GetAllAsync();
        Task<ChargeTemplate?> GetByIdAsync(Guid chargeTemplateId);
        Task AddAsync(ChargeTemplate chargeTemplate);
        Task UpdateAsync (ChargeTemplate chargeTemplate);
        Task DeleteAsync(ChargeTemplate chargeTemplate);
        Task SaveChangesAsync();
        IQueryable<ChargeTemplate> Query();
    }
}
