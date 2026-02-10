using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.Repositories.Room;
using System.Numerics;
using System.Text.Json;

namespace Rental_Management_System.Server.Controllers.Practice
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIController : ControllerBase
    {
        private readonly IRoomRepository _roomRepo;

        public APIController(IRoomRepository roomRep)
        {
            _roomRepo = roomRep;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResult()
        {
            var rooms = new List<string> { "room1","room2","room3"};
            return Ok(rooms);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            //var rooms = new Dictionary<int, string>();
            //rooms.Add(1, "room1");
            //rooms.Add(2, "room2");

            var rooms = new[]
            {
                new{Id = 1, Name = "Room1"},
                new{Id = 2, Name = "Room2"}

            };
            var result = rooms.FirstOrDefault(r => r.Id == Id);
            if (result != null) {  return Unauthorized(result); } else { return BadRequest("Result not found"); }
        }

        [HttpGet("test/{TestId}")]
        public async Task<IActionResult> Test(int TestId)
        {
            var footballer = new[]
            {
                new {Id = 1, Name = "Messi", age = 38},
                new {Id = 2, Name = "Ronaldo", age = 40}
            };

            //using var client = new HttpClient();
            //var response = await client.GetAsync("https://api.example.com/players");
            //var jsonString = await response.Content.ReadAsStringAsync();
            //var players = JsonSerializer.Deserialize<List<Player>>(jsonString);

            var result =  footballer.Where(f => f.age > 30);

            int[] arr = [1, 2,3, 4, 5];
            var arrResult = arr[0];

            int[] arr1 = { 1, 2, 3, 4, 5 };
            int[] arr2 = new[] { 1, 2, 3, 4, 5 };
            int[] arr3 = new int[] { 12, 4 };

            Dictionary<int, string> test = new Dictionary<int, string>();
            test.Add(1, "subodh");
            test.Add(2, "Khadka");

            List<string> list = test.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToList();

            IEnumerable<int> list2 = new List<int> { 1, 23, 4, 5 }.Where(n => n > 1);
            //List<int> list2 = new List<int> { 1, 23, 4, 5 }.Where(n => n > 1);

            return Ok(arrResult);
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Database failed badly");
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var room = await _roomRepo.GetByIdAsync(id); // actual DB call

            if (room == null)
            {
                // Instead of returning null, throw an exception
                throw new Exception($"Room with ID {id} was not found");
            }

            return Ok(room);
        }


    }


}

