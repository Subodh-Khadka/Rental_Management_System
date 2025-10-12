namespace Rental_Management_System.Server.DTOs.ChargeTemplate
{
    public class ChargeTemplateDto
    {
        public Guid ChargeTemplateId { get; set; }
        public string ChargeType { get; set; }
        public decimal DefaultAmount { get; set; }
        public bool IsVariable { get; set; }
        public string? CalculationMethod { get; set; }
        public bool IsActive { get; set; }
    }

}
