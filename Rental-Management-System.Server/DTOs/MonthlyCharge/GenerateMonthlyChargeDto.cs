using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class GenerateMonthlyChargeDto
    {
        [Required(ErrorMessage = "Month is required")]
        public string Month { get; set; } = string.Empty; // "2026-01"
        public List<PaymentUnitDto> Payments { get; set; } = new();
    }

    public class PaymentUnitDto
    {
        [Required]
        public Guid PaymentId { get; set; }

        [Required]
        public List<TemplateUnitDto> Templates { get; set; } = new();
    }

    public class TemplateUnitDto
    {
        [Required]
        public Guid TemplateId { get; set; }

        [Required]
        public decimal Units { get; set; }
    }

}