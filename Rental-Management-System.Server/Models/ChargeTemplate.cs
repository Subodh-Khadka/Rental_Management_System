using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class ChargeTemplate
    {

        [Key]
        public Guid ChargeTemplateId { get; set; }

        [Required]
        public string ChargeType { get; set; } = string.Empty; // "Electricity", "Water", etc.

        public decimal DefaultAmount { get; set; } // e.g., 12 per unit, or 100 flat
        public bool IsVariable { get; set; } // true for electricity/wifi, false for water/waste

        // Optional
        public string? CalculationMethod { get; set; } // "PerUnit", "PerDevice", "Flat"
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
