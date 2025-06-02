using api.IServices;
using api.Models;
using api.Models.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerSerice;

        public CustomersController(ICustomerService customerService)
        {
            _customerSerice = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAll()
        {
            var customers = await _customerSerice.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetById(int id)
        {
            var customer = await _customerSerice.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CustomerCreateRequest customer)
        {
            var created = await _customerSerice.CreateAsync(customer);
            return Created();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<bool>> Update(int id, CustomerUpdateRequest updateRequest)
        {
            if(updateRequest.Id == 0) return BadRequest("El id del Cliente no puede ser 0");
            if (id != updateRequest.Id) return BadRequest("El id del Cliente no coincide con el parámetro de URL Id");

            var updated = await _customerSerice.UpdateAsync(id, updateRequest);
            if (!updated) return NotFound();

            return Ok(updated);
        }
    }
}
