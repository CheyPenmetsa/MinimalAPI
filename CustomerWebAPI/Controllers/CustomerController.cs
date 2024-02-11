using Customer.BusinessLogic;
using Customer.BusinessLogic.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDb _context;
        
        private readonly IMapper _mapper;

        private IValidator<UpsertCustomerDto> _validator;

        public CustomerController(CustomerDb context, IMapper mapper, IValidator<UpsertCustomerDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        // GET: api/customer
        [HttpGet]
        [ApiKey]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            return await _context.Customers
                .Select(x => _mapper.Map<CustomerDto>(x))
                .ToListAsync();
        }

        // GET: api/customer/1
        [HttpGet("{id}", Name = "GetCustomerById")]
        [Authorize(Policy ="ApiKeyPolicy")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById([FromRoute] int id)
        {
            var customerEntity = await _context.Customers.FindAsync(id);

            if (customerEntity == null)
            {
                return NotFound();
            }

            return _mapper.Map<CustomerDto>(customerEntity);
        }

        // PUT: api/customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody]UpsertCustomerDto upsertCustomerDto)
        {
            var customerEntity = await _context.Customers.FindAsync(id);
            if (customerEntity == null)
            {
                return NotFound();
            }

            customerEntity.Adapt(upsertCustomerDto);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CustomerExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/customer
        [HttpPost]
        public async Task<ActionResult> PostCustomer([FromBody]UpsertCustomerDto upsertCustomerDto)
        {
            ValidationResult result = await _validator.ValidateAsync(upsertCustomerDto);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var customerEntity = _mapper.Map<CustomerEntity>(upsertCustomerDto);
            var currentCount = await _context.Customers.CountAsync();
            customerEntity.Id = currentCount + 1;

            _context.Customers.Add(customerEntity);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(
            //    nameof(GetCustomerById),
            //    new { id = customerEntity.Id },
            //    _mapper.Map<CustomerDto>(customerEntity));
            return Created(Url.Link("GetCustomerById", new { id = customerEntity.Id })!, customerEntity);
        }

        // DELETE: api/customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute]int id)
        {
            var customerEntity = await _context.Customers.FindAsync(id);
            if (customerEntity == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customerEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
