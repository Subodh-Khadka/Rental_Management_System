using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.PaymentTransaction
{
    public class CreatePaymentTransactionDto
    {
        [Required(ErrorMessage = "Rent payment Id is a required field")]
        public Guid RentPaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
    }
}
