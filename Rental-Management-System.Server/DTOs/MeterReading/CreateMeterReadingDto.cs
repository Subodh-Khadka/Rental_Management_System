namespace Rental_Management_System.Server.DTOs.MeterReading
{
    public class CreateMeterReadingDto
    {
        public Guid PaymentId { get; set; }
        public string Month { get; set; } = string.Empty;
        public decimal PreviousReading { get; set; }
        public decimal CurrentReading { get; set; }
    }
}
