using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class RentalContract
    {
        [Key]
        public Guid RentalContractId { get; set; }
        public Guid TenantId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Terms { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        // Navigation
        public Tenant Tenant { get; set; }
        public Room Room { get; set; }
        public ICollection<RentPayment> Payments { get; set; } = new List<RentPayment>();
       
    }
}
