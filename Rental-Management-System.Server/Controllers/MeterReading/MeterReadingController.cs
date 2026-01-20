using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.DTOs.MeterReading;
using Rental_Management_System.Server.Services.MeterReading;

namespace Rental_Management_System.Server.Controllers.MeterReading
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _service;

        public MeterReadingController(IMeterReadingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMeterReadingDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("payment/{paymentId}/month/{month}")]
        public async Task<IActionResult> GetByPaymentAndMonth(Guid paymentId, DateTime month)
        {
            var result = await _service.GetByPaymentAndMonthAsync(paymentId, month);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateMeterReadingDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
