namespace Rental_Management_System.Server.DTOs.PaymentTransaction
{
    public class PaymentTransactionDto
    {
        public Guid TransactionId { get; set; }
        public Guid RentPaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
    }
}
