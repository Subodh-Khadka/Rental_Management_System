using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.DTOs.PaymentTransaction;
using Rental_Management_System.Server.DTOs.RentPayment;
using Rental_Management_System.Server.Services.PaymentTransaction;
using Rental_Management_System.Server.Services.RentPayment;

namespace Rental_Management_System.Server.Controllers.PaymentTransaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentTransactionService _transactionService;

        public PaymentTransactionController(IPaymentTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentTransactions()
        {
            var response = await _transactionService.GetAllPaymentTransactionsAsync();
            if (response == null) return NotFound(response);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetPaymentTransactionById(Guid transactionId)
        {
            if (transactionId == Guid.Empty) return NotFound(nameof(transactionId));

            var response = await _transactionService.GetPaymentTransactionByIdAsync(transactionId);

            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentTransaction([FromBody] CreatePaymentTransactionDto createPaymentTransactionDto)
        {
            if (createPaymentTransactionDto == null) return NotFound(nameof(createPaymentTransactionDto));

            var response = await _transactionService.CreatePaymentTransactionAsync(createPaymentTransactionDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{transactionId}")]
        public async Task<IActionResult> updatePaymentTransaction(Guid transactionId, [FromBody] UpdatePaymentTransactionDto updatePaymentTransactionDto)
        {
            if (transactionId == Guid.Empty) return NotFound(nameof(transactionId));

            var response = await _transactionService.UpdatePaymentTransactionAsync(transactionId, updatePaymentTransactionDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> DeletePaymentTransaction(Guid transactionId)
        {
            if (transactionId == Guid.Empty) return NotFound(nameof(transactionId));

            var response = await _transactionService.DeletePaymentTransactionAsync(transactionId);
            return response.Success ? Ok(response) : BadRequest(nameof(response));
        }
    }
}
