using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class MonthlyCharge
    {
        [Key]
        public Guid MonthlyChargeId { get; set; }
        public Guid RentPaymentId { get; set; }
        public string ChargeType { get; set; }
        public decimal Amount { get; set; }
        public decimal? Units { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        // Navigation
        public RentPayment RentPayment { get; set; }
    }
}
