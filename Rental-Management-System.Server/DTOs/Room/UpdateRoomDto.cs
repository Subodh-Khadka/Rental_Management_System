using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.DTOs.Room
{
    public class UpdateRoomDto
    {
        [Required]
        public string RoomTitle { get; set; }
        [Required]
        public decimal RoomPrice { get; set; }
    }
}
