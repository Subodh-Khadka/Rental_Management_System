using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.Tenant
{
    public class UpdateTenantDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [Phone]
        public string Phonenumber { get; set; }

    }
}
