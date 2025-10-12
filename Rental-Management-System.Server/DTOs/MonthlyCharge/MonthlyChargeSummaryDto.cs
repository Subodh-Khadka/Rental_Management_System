namespace Rental_Management_System.Server.DTOs.MonthlyCharge
{
    public class MonthlyChargeSummaryDto
    {
        public string RoomName { get; set; }
        public string TenantName { get; set; }
        public string Month {  get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

    }
}
