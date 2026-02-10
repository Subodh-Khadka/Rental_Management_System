namespace Rental_Management_System.Server.Repositories.Room
{
    using Microsoft.EntityFrameworkCore;
    using Rental_Management_System.Server.Data;
    using Rental_Management_System.Server.Models;
    public class RoomRepository : IRoomRepository
    {
        private readonly RentalDbContext _context;
        public RoomRepository(RentalDbContext context) 
        {
            _context = context;    
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
           var allRooms =  await _context.Rooms.Where(r => r.IsActive == true && r.IsDeleted != true)
                .Include(r => r.RentalContracts).OrderBy(r => r.RoomTitle)
                .ToListAsync();
            return allRooms;
        }

        public async Task<Room?> GetByIdAsync(Guid roomId)
        {
           var selectedRoom = await _context.Rooms
                                 .FirstOrDefaultAsync(r => r.RoomId == roomId && !r.IsDeleted);
            return selectedRoom;
        }

        public async Task AddAsync(Room room)
        {
             await _context.Rooms.AddAsync(room);
        }

        public Task UpdateAsync(Room room)
        {
             _context.Rooms.Update(room);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Room room)
        {
            room.IsDeleted = true;
            room.DeletedDate = DateTime.UtcNow;
            _context.Rooms.Update(room);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}   
