using Microsoft.AspNetCore.Mvc;
using PartnerControlAPI.Models.DTOs;
using PartnerControlAPI.Services;

namespace PartnerControlAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("submittrxmessage")]
        public async Task<ActionResult<TransactionResponse>> SubmitTransaction([FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TransactionResponse.Failure("Invalid request format"));
            }

            var response = await _transactionService.ProcessTransactionAsync(request);
            return Ok(response);
        }
    }
} 