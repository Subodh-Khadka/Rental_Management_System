namespace Rental_Management_System.Server.DTOs.PaymentTransaction
{
    public class PaymentTransactionDto
    {
        public string TransactionId { get; set; }
        public string RentPaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
    }
}
