using api.IServices;
using api.Models;
using api.Models.DTOs.CustomerDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BankingDbContext _context;
        private readonly IMapper _mapper;

        public CustomerService(BankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> CreateAsync(CustomerCreateRequest createRequest)
        {
            var customer = _mapper.Map<Customer>(createRequest);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var customerDTO = new CustomerDTO();
            _mapper.Map(customer, customerDTO);

            return customerDTO;
        }

        public async Task<IEnumerable<CustomerDTO>?> GetAllAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            
            var customerDTO = new CustomerDTO();
            _mapper.Map(customer, customerDTO);
            return customerDTO;
        }

        public async Task<bool> UpdateAsync(int id, CustomerUpdateRequest updateRequest)
        {
            var existingCustomer = _context.Customers.Find(id);
            if (existingCustomer == null) return false;

            _mapper.Map(updateRequest, existingCustomer);
            existingCustomer.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
