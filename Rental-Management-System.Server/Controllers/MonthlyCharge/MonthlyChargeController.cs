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

        [HttpPut("{monthlyChargeId}")]
        public async Task<IActionResult> UpdateMonthlyCharge(Guid monthlyChargeId, [FromBody] UpdateMonthlyChargeDto updateMonthlyChargeDto)
        {
            if (monthlyChargeId == Guid.Empty) return NotFound(nameof(monthlyChargeId));

           var response = await _monthlyChargeService.UpdateMonthlyChargeAsync(monthlyChargeId, updateMonthlyChargeDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{monthlyChargeId}")]
        public async Task<IActionResult> DeleteMonthlyCharge(Guid monthlyChargeId)
        {
            if(monthlyChargeId == Guid.Empty) return NotFound(nameof(monthlyChargeId));

            var response = await _monthlyChargeService.DeleteMonthlyChargeAsync(monthlyChargeId);
            return response.Success ? Ok(response) : BadRequest(nameof(response));
        }
    }
}
