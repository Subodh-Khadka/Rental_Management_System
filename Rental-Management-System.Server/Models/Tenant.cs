using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
   public class Tenant
    {
        [Key]
        public Guid TenantId { get; set; }
        public string? Name { get; set; }
        public Guid RoomId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }

        // Navigation property
        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
        public ICollection<RentPayment> Payments { get; set; } = new List<RentPayment>();
        public Room Room { get; set; }

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
