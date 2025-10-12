using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class MonthlyCharge
    {
        [Key]
        public Guid MonthlyChargeId { get; set; }

        public Guid RentPaymentId { get; set; }
        public Guid ChargeTemplateId { get; set; } // <-- Link to template

        public string? ChargeType { get; set; } // snapshot for history (redundant but useful)

        public decimal Amount { get; set; } // Final amount for this month
        public decimal? Units { get; set; } // e.g., kWh consumed, devices used

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        // Navigation
        public RentPayment? RentPayment { get; set; }
        public ChargeTemplate? ChargeTemplate { get; set; }
    }
}
