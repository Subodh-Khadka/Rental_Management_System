using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class RentPayment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public Guid RentalContractId { get; set; }
        public DateTime PaymentMonth { get; set; }
        public decimal RoomPrice { get; set; }
        public Guid RoomId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        // Track Payment
        public decimal TotalAmount => RoomPrice + MonthlyCharges.Sum(x => x.Amount);
        public decimal PaidAmount { get; set; }
        public decimal DueAmount => TotalAmount - PaidAmount;


        // Navigation Property 
        public Room Room { get; set; }
        public RentalContract RentalContract { get; set; }
        public ICollection<MonthlyCharge> MonthlyCharges { get; set; } = new List<MonthlyCharge>();


    }
}
