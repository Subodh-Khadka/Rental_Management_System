using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.RentalContract
{
    public class UpdateRentalContractDto
    {
        [Required(ErrorMessage = "TenantId is a required field")]
        public Guid TenantId { get; set; }
        [Required(ErrorMessage = "RoomId is a required field")]
        public Guid RoomId { get; set; }
        [Required(ErrorMessage = "Start Date is a required field")]

        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is a required field")]
        public DateTime? EndDate { get; set; }
        [MaxLength(1000, ErrorMessage = "Terms cannot exceed 1000 characters.")]
        public string? Terms { get; set; }
    }
}
