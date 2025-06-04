using api.IServices;
using api.Models.DTOs.TransactionDTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/<TransactionsController>/account/{accountId}
        [HttpGet("/account/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>?>> GetAllByAccountIdAsync(int accountId)
        {
            var transactions = await _transactionService.GetAllByAccountIdAsync(accountId);
            return NotFound(transactions);
        }

        // GET api/<TransactionsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetById(int id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        // POST api/<TransactionsController>
        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> Create(TransactionCreateRequest createRequest)
        {
            // TODO: Validate with try-catch
            var transaction = await _transactionService.CreateAsync(createRequest);
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
    }
}
