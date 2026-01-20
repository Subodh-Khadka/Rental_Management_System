namespace Rental_Management_System.Server.DTOs.MeterReading
{
    public class MeterReadingDto
    {
        public Guid MeterReadingId { get; set; }
        public Guid PaymentId { get; set; }
        public string Month { get; set; } = string.Empty;
        public decimal PreviousReading { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal UnitsUsed { get; set; }
    }
}

