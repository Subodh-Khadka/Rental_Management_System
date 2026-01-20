using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.RentPayment
{
    public class GenerateRentPaymentDto
    {
        [Required(ErrorMessage = "Month is required")]
        public string Month { get; set; } = string.Empty;

    }
}
    