namespace Rental_Management_System.Server.Models
{
   public class Tenant
    {
        public int TenantId { get; set; }
        public string? Name { get; set; }
        public int RoomId { get; set; }

        // Navigation property
        public Room Room { get; set; }
        public ICollection<RentPayment>? Payments { get; set; }
    }
}
