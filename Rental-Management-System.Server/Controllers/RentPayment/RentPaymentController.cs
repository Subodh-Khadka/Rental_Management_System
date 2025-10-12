using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.DTOs.MonthlyCharge;
using Rental_Management_System.Server.DTOs.RentalContract;
using Rental_Management_System.Server.DTOs.RentPayment;
using Rental_Management_System.Server.Services.MonthlyCharge;
using Rental_Management_System.Server.Services.RentPayment;

namespace Rental_Management_System.Server.Controllers.RentPayment
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentPaymentController : ControllerBase
    {
        private readonly IRentPaymentService _rentPaymentService;

        public RentPaymentController(IRentPaymentService rentPaymentService)
        {
            _rentPaymentService = rentPaymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRentPayments()
        {
            var response = await _rentPaymentService.GetAllRentPaymentAsync();
            if (response == null) return NotFound(response);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{rentPaymentId}")]
        public async Task<IActionResult> GetRentPaymentByID(Guid rentPaymentId)
        {
            if (rentPaymentId == Guid.Empty) return NotFound(nameof(rentPaymentId));

            var response = await _rentPaymentService.GetRentPaymentByIdAsync(rentPaymentId);

            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRentPayment([FromBody] CreateRentPaymentDto createRentPaymentDto)
        {
            if (createRentPaymentDto == null) return NotFound(nameof(createRentPaymentDto));

            var response = await _rentPaymentService.CreateRentPaymentAsync(createRentPaymentDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{rentPaymentId}")]
        public async Task<IActionResult> UpdateRentPayment(Guid rentPaymentId, [FromBody] UpdateRentPaymentDto updateRentPaymentDto)
        {
            if (rentPaymentId == Guid.Empty) return NotFound(nameof(rentPaymentId));

            var response = await _rentPaymentService.UpdateRentPaymentAsync(rentPaymentId, updateRentPaymentDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{rentPaymentId}")]
        public async Task<IActionResult> DeleteRentPayment(Guid rentPaymentId)
        {
            if (rentPaymentId == Guid.Empty) return NotFound(nameof(rentPaymentId));

            var response = await _rentPaymentService.DeleteRentPaymentAsync(rentPaymentId);
            return response.Success ? Ok(response) : BadRequest(nameof(response));
        }

        [HttpGet("by-month/{month}")]
        public async Task<IActionResult> GetByMonth(string month)
        {
            var response = await _rentPaymentService.GetRentPaymentsByMonthAsync(month);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateRentPayment([FromBody] GenerateRentPaymentDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request body.");

            var response = await _rentPaymentService.GenerateRentPaymentAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
