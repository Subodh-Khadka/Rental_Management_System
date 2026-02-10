using Rental_Management_System.Server.Data;

namespace Rental_Management_System.Server.Repositories.RentalContract
{
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Data;
    using Rental_Management_System.Server.Models;
    public class RentalContractRepository : IRentalContractRepository
    {
        public readonly RentalDbContext _context;

        public RentalContractRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentalContract>> GetAllAsync()
        {
            return await _context.RentalContracts.Where(r => !r.IsDeleted)
                .Include(cr => cr.Room)
                .Include(cr => cr.Tenant)
                .OrderBy(r => r.Room.RoomTitle)
                .ToListAsync();
        }

        public async Task<RentalContract?> GetByIdAsync(Guid rentalContractId)
        {
            return await _context.RentalContracts.FirstOrDefaultAsync(r => r.RentalContractId == rentalContractId && !r.IsDeleted);
        }

        public async Task AddAsync(RentalContract rentalContract)
        {
            await _context.RentalContracts.AddAsync(rentalContract);
        }

        public Task UpdateAsync(RentalContract rentalContract)
        {
            _context.RentalContracts.Update(rentalContract);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(RentalContract rentalContract)
        {
            rentalContract.IsDeleted = true;
            rentalContract.DeletedDate = DateTime.UtcNow;
            _context.RentalContracts.Update(rentalContract);
             return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
