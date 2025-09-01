using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class CreateMonthlyChargeDto
    {
        [Required(ErrorMessage = "Rent payment Id is a required field")]
        public Guid RentPaymentId { get; set; }
        public string? ChargeType { get; set; }
        public decimal Amount { get; set; }
        public decimal? Units { get; set; }

    }
}
