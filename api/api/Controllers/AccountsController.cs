using api.IServices;
using api.Models.DTOs.AccountDTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST api/<AccountsController>
        [HttpPost]
        public async Task<ActionResult<AccountDTO>> Create(AccountCreateRequest createRequest)
        {
            var account = await _accountService.CreateAsync(createRequest);
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }

        // GET: api/<AccountsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        // GET: api/<AccountsController>
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccountsByCustomerId(int customerId)
        {
            var accounts = await _accountService.GetAccountsByCustomerIdAsync(customerId);
            return Ok(accounts);
        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        // GET api/<AccountsController>/accountnumber/{accountNumber}
        [HttpGet("accountnumber/{accountNumber}")]
        public async Task<ActionResult<AccountDTO>> GetAccountByAccountNumber(string accountNumber)
        {
            var account = await _accountService.GetAccountByAccountNumber(accountNumber);
            if (account == null) return NotFound();
            return Ok(account);
        }
    }
}
