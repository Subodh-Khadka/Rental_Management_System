using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class GenerateMonthlyChargeDto
    {
        [Required(ErrorMessage = "Month is required")]
        public string Month { get; set; } = string.Empty; // format: "2025-10"

        public List<PaymentUnitDto> Payments { get; set; } = new();
    }

    public class PaymentUnitDto
    {
        public Guid PaymentId { get; set; }
        public List<TemplateUnitDto> Templates { get; set; } = new();
    }

    public class TemplateUnitDto
    {
        public Guid TemplateId { get; set; }
        public decimal Units { get; set; }
    }
}

    //public class GenerateMonthlyChargeDto
    //{
    //    [Required(ErrorMessage = "Month is required")]
    //    public string Month { get; set; } // format: "2025-10"

    //    // Units input: { paymentId: { templateId: units } }
    //    public Dictionary<Guid, Dictionary<Guid, decimal>> UnitsData { get; set; } = new();
    //}
