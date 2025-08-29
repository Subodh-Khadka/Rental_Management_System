namespace Rental_Management_System.Server.Repositories.Room
{
    using Rental_Management_System.Server.Models;

    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> GetByIdAsync(Guid roomId);
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(Room room);
        Task SaveChangesAsync();
    }
}
