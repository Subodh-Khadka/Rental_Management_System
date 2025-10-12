namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class MonthlyChargeDto
    {
        public Guid MonthlyChargeId { get; set; }
        public Guid RentPaymentId { get; set; }
        public Guid ChargeTemplateId { get; set; }
        public string?  ChargeType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Units { get; set; }

        // Add these
        public string RoomName { get; set; }
        public string TenantName { get; set; }
        public string Month { get; set; }
        public string Status { get; set; }

     
    }
}