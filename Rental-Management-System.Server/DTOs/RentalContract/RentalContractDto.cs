namespace Rental_Management_System.Server.DTOs.RentalContract
{
    public class RentalContractDto
    {
        public Guid RentalContractId { get; set; }
        public Guid TenantId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
