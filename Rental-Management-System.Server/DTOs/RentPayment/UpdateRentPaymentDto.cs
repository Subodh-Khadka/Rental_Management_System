using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.RentPayment
{
    public class UpdateRentPaymentDto
    {
        [Required(ErrorMessage ="Payment Id is a required field")]
        public Guid PaymentId { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
