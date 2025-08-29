using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.Tenant
{
    public class CreateTenantDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string? EmailAdress { get; set; }
        [Required(ErrorMessage = "Room assignment is required!")]
        public Guid RoomId { get; set; }

    }
}
