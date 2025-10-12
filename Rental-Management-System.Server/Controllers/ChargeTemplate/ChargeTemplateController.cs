using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.DTOs.ChargeTemplate;
using Rental_Management_System.Server.Models;
using Rental_Management_System.Server.Services.ChargeTemplate;
using System.Text.Json.Serialization;

namespace Rental_Management_System.Server.Controllers.ChargeTemplate
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargeTemplateController : ControllerBase
    {
        private readonly IChargeTemplateService _chargeService;

        public ChargeTemplateController(IChargeTemplateService chargeService)
        {
            _chargeService = chargeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChargeTemplates()
        {
            var response = await _chargeService.GetAllChargeTemplateAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{chargeTemplateId}")]
        public async Task<IActionResult> GetChargeTemplateById(Guid chargeTemplateId)
        {
            var response = await _chargeService.GetChargeTemplateByIdAsync(chargeTemplateId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChargeTemplate([FromBody] CreateChargeTemplateDto dto)
        {
            var response = await _chargeService.CreateChargeTemplateAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{chargeTemplateId}")]
        public async Task<IActionResult> UpdateChargeTemplate(Guid chargeTemplateId,  [FromBody] UpdateChargeTemplateDto dto)
        {
            var response = await _chargeService.UpdateChargeTemplateAsync(chargeTemplateId, dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{chargeTemplateId}")]
        public async Task<IActionResult> DeleteChargeTemplate(Guid chargeTemplateId)
        {
            var resonse = await _chargeService.DeleteChargeTemplateAsync(chargeTemplateId);
            return resonse.Success ? Ok(resonse) : BadRequest(resonse);
        }
    }
}
