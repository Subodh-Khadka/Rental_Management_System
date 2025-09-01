namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class MonthlyChargeDto
    {
        public Guid MonthlyChargeId { get; set; }
        public Guid RentPaymentId { get; set; }
        public string?  ChargeType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Units { get; set; }
    }
}
