using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class MeterReading
    {
        [Key]
        public Guid MeterReadingId { get; set; }

        public Guid RoomId { get; set; } 
        public Room? Room { get; set; }

        public Guid PaymentId { get; set; }
        public RentPayment? RentPayment { get; set; }

        public DateTime Month { get; set; }

        public decimal PreviousReading { get; set; }
        public decimal CurrentReading { get; set; }

        public decimal UnitsUsed => CurrentReading - PreviousReading;
    }
}
