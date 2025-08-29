using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.DTOs.Tenant;
using Rental_Management_System.Server.Models;
using Rental_Management_System.Server.Services.Tenant;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Rental_Management_System.Server.Controllers.Room
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            var response = await _tenantService.GetAllTenantsAsync();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenantById(Guid id)
        {
            var response = await _tenantService.GetTenantByIdAsync(id);

            if (!response.Success) { return NotFound(response); }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto tenantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _tenantService.CreateTenantAsync(tenantDto);

            if (!response.Success) { return BadRequest(response); }

            return Ok(response);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(Guid id, [FromBody] UpdateTenantDto tenantDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var response = await _tenantService.UpdateTenantAsync(id, tenantDto);
            if (!response.Success) { return NotFound(response); }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
          var response = await _tenantService.DeleteTenantAsync(id);
            if (!response.Success)
            {
               return NotFound(response);
            }

            return Ok(response);
        }
    }
}

