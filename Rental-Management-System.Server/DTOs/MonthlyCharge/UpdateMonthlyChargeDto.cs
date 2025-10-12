using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class UpdateMonthlyChargeDto
    {
        [Required]
        public Guid MonthlyChargeId { get; set; }
        [Required(ErrorMessage = "Rent payment Id is a required field")]

        public Guid RentPaymentId { get; set; }
        public Guid ChargeTemplateId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Units must be positive.")]
        public decimal? Units { get; set; }
    }
}
