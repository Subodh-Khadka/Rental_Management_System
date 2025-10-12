using Microsoft.AspNetCore.Identity;

namespace Rental_Management_System.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsDeleted { get; set; } = false;


    }
}
