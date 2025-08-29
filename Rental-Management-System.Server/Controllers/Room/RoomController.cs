namespace Rental_Management_System.Server.Controllers.Room
{
    using Microsoft.AspNetCore.Mvc;
    using Rental_Management_System.Server.DTOs.Room;
    using Rental_Management_System.Server.Services.Room;

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }


        // GET: api/room
        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        // GET: api/room/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            return Ok(room);
        }

        // POST: api/room
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto roomDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdRoom = await _roomService.CreateRoomAsync(roomDto);
            return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.Data.RoomId }, createdRoom);
        }

        // PUT: api/room/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomDto roomDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedRoom = await _roomService.UpdateRoomAsync(id, roomDto);
            if (updatedRoom == null) return NotFound();

            return Ok(updatedRoom);
        }


        // DELETE: api/room/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }
    }
}
