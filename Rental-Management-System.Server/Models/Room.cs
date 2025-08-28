namespace Rental_Management_System.Server.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public decimal RentAmount { get; set; }

        // Navigation property
        public ICollection<Tenant> Tenants { get; set; }
    }
}
