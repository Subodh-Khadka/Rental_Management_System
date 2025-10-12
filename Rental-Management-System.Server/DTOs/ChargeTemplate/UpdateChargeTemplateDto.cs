using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.ChargeTemplate
{
    public class UpdateChargeTemplateDto
    {
        //[Required(ErrorMessage = "Charge template Id is a required field")]
        //public Guid ChargeTemplateId { get; set; }
        [Required(ErrorMessage = "Charge type is required.")]
        [StringLength(50, ErrorMessage = "Charge type can't exceed 50 characters.")]
        public string chargeType { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Default amount must be a positive value.")]
        public decimal defaultAmount { get; set; }
        public bool IsVariable { get; set; }
        [StringLength(50, ErrorMessage = "Calculation method can't exceed 50 characters.")]
        public string CalculationMethod { get; set; }
        public bool IsActive { get; set; }
    }
}
