using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.Room
{
    public class UpdateRoomDto
    {
        [Required(ErrorMessage = "Room title is required")]
        [StringLength(20, ErrorMessage = "Room title can't be longer than 20 characters")]
        public string RoomTitle { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Room price must be greater than zero")]
        public decimal RoomPrice { get; set; }
    }
}
