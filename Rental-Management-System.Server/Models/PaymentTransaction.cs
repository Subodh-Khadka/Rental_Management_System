using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class PaymentTransaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        public Guid RentPaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        //Navigation
        public RentPayment RentPayment { get; set; }
    }
}
