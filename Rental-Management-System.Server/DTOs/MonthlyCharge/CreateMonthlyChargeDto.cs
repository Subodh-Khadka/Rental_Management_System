using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class CreateMonthlyChargeDto
    {
        [Required(ErrorMessage = "PaymentId is a required field")]
        public  Guid PaymentId {  get; set; }

        [Required(ErrorMessage = "Template id is a required field")]
        public Guid ChargeTemplateId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Units must be positive.")]
        public decimal? Units { get; set; }

    }
}
