using Rental_Management_System.Server.DTOs.MonthlyCharge;
using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.RentPayment
{
    public class RentPaymentDto
    {
        public Guid PaymentId { get; set; }
        public Guid RentalContractId { get; set; }
        public DateTime PaymentMonth { get; set; }
        public decimal RoomPrice { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }


        // Navigation Data (Flattened for client)
        public string? RoomTitle { get; set; }
        public string? TenantName { get; set; }
        public List<MonthlyChargeDto>? MonthlyCharges { get; set; }
    }
}
            