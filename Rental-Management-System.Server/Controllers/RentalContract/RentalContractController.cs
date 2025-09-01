using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.DTOs.RentalContract;
using Rental_Management_System.Server.Services.RentalContract;
using Rental_Management_System.Server.Services.Room;

namespace Rental_Management_System.Server.Controllers.RentalContract
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalContractController : ControllerBase
    {
        private IRentalContractService _rentalContractService;

        public RentalContractController(IRentalContractService rentalContractService)
        {
            _rentalContractService = rentalContractService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRentalContracts()
        {
            var response = await _rentalContractService.GetAllRentalContractsAsync();
            return response.Success ? Ok(response) : BadRequest(response);  
        }

        [HttpGet("{rentalContractId}")]
        public async Task<IActionResult> GetRentalContractById(Guid rentalContractId)
        {
            if (rentalContractId == Guid.Empty)
            {
                return BadRequest(new { Success = false, Message = "Invalid rental contract ID." });
            }

            var response = await _rentalContractService.GetRentalContractByIdAsync(rentalContractId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRentalContract([FromBody] CreateRentalContractDto rentalContractDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _rentalContractService.CreateRentalContractAsync(rentalContractDto);

            if (!response.Success)
                return BadRequest(response);

            // Return 201 Created with location of the new resource
            return CreatedAtAction(nameof(GetRentalContractById),
                                   new { rentalContractId = response.Data.RentalContractId },
                                   response);
        }

        [HttpPut("{rentalContractId}")]
        public async Task<IActionResult> UpdateRentalContract(Guid rentalContractId, [FromBody] UpdateRentalContractDto updateRentalContractDto)
        {
            if(rentalContractId  == Guid.Empty)
            {
                return BadRequest(new {success = false, message="Invalid rental Contract Id"});
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _rentalContractService.UpdateRentalContractAsync(rentalContractId, updateRentalContractDto);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{rentalContractId}")]
        public async Task<IActionResult> DeleteRentalContract(Guid rentalContractId)
        {
            if(rentalContractId == Guid.Empty)
            {
                return BadRequest(new { success = false, message = "Invalid rental Id" });
            }

            var response = await _rentalContractService.DeleteRentalContractAsync(rentalContractId);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
