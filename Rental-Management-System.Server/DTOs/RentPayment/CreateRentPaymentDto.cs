using Rental_Management_System.Server.DTOs.MonthlyCharge;
using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.RentPayment
{
    public class CreateRentPaymentDto
    {
        [Required(ErrorMessage = "Rental Contrac tId Id is a required field")]
        public Guid RentalContractId { get; set; }
        [Required(ErrorMessage = "Room Id is a required field")]

        public DateTime PaymentMonth { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal PaidAmount { get; set; }
        public string Status { get; set; }

        // Optional: to add monthly charges while creating
        public List<CreateMonthlyChargeDto>? MonthlyCharges { get; set; }
    }
}
