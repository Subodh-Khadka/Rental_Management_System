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
        public string? EmailAdress { get; set; }

        // Navigation property
        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
        public ICollection<RentPayment> Payments { get; set; } = new List<RentPayment>();

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
