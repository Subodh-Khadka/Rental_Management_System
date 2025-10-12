namespace Rental_Management_System.Server.DTOs.RentalContract
{
    public class RentalContractDto
    {
        public Guid RentalContractId { get; set; }
        public Guid TenantId { get; set; }
        public string? TenantName { get; set; }
        public Guid RoomId { get; set; }
        public string? RoomTitle { get; set; }
        public decimal RoomPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Terms {  get; set; }

        public bool IsDeleted { get; set; }
    }
}
