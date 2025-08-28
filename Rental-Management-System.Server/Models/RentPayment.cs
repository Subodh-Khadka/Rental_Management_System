namespace Rental_Management_System.Server.Models
{
    public class RentPayment
    {
        public int PaymentId { get; set; }
        public int TenantId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation Property 
        public Tenant Tenant { get; set; }
    }
}
