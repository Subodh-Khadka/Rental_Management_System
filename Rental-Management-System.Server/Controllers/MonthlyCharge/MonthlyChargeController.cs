using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.Services.MonthlyCharge;

namespace Rental_Management_System.Server.Controllers.MonthlyCharge
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyChargeController : ControllerBase
    {
        private readonly IMonthlyChargeService _monthlyChargeService;

        public MonthlyChargeController(IMonthlyChargeService monthlyChargeService) 
        {
            _monthlyChargeService = monthlyChargeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMonthlyCharges()
        {
            var response = await _monthlyChargeService.GetAllMonthlyChargeAsync();
            if (response == null) return NotFound(response);

            return response.Success ? Ok(response) : BadRequest(response); 
        }

        [HttpGet("{monthlyChargeId}")]
        public async Task<IActionResult> GetMonthlyChargeById(Guid monthlyChargeId)
        {
            if (monthlyChargeId == Guid.Empty) return NotFound(nameof(monthlyChargeId));

            var response = await _monthlyChargeService.GetMonthlyChargeByIdAsync(monthlyChargeId);

            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMonthlyCharge(CreateMonthlyChargeDto createMonthlyChargeDto)
        {
            var response = await _monthlyChargeService.CreateMonthlyChargeAsync(createMonthlyChargeDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        //[HttpPut("{monthlyChargeId}")]
        //public async Task<IActionResult> UpdateMonthlyCharge(Guid monthlyChargeId, [FromBody] UpdateMonthlyChargeDto updateMonthlyChargeDto)
        //{
        //    if (monthlyChargeId == Guid.Empty) return NotFound(nameof(monthlyChargeId));

        //   var response = await _monthlyChargeService.UpdateMonthlyChargeAsync(monthlyChargeId, updateMonthlyChargeDto);
        //    return response.Success ? Ok(response) : BadRequest(response);
        //}

        [HttpPut("{monthlyChargeId}")]
        // [Authorize(Roles = "Admin")]  // enable when auth is ready
        public async Task<IActionResult> UpdateMonthlyCharge(Guid monthlyChargeId,[FromBody] UpdateMonthlyChargeDto dto)
        {
            if (monthlyChargeId == Guid.Empty)
                return BadRequest("Invalid monthly charge id");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _monthlyChargeService
                .UpdateMonthlyChargeAsync(monthlyChargeId, dto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpDelete("{monthlyChargeId}")]
        public async Task<IActionResult> DeleteMonthlyCharge(Guid monthlyChargeId)
        {
            if(monthlyChargeId == Guid.Empty) return NotFound(nameof(monthlyChargeId));

            var response = await _monthlyChargeService.DeleteMonthlyChargeAsync(monthlyChargeId);
            return response.Success ? Ok(response) : BadRequest(nameof(response));
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMonthlyCharges([FromBody] GenerateMonthlyChargeDto dto)
        {
            var response = await _monthlyChargeService.GenerateMonthlyChargesAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _monthlyChargeService.GetMonthlyChargeSummaryAsync();
            return Ok(result);
        }

        [HttpGet("by-room-month/{roomId}/{month}")]
        public async Task<IActionResult> GetByRoomAndMonth(Guid roomId, string month)
        {
            var response = await _monthlyChargeService.GetMonthlyChargesByRoomAndMonthAsync(roomId, month);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }


    }
}
