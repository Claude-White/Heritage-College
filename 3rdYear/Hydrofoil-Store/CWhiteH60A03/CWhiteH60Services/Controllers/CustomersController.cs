using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase {
    private readonly ICustomerRepository<Customer> _customerRepository;

    public CustomersController(ICustomerRepository<Customer> customerRepository) {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public ActionResult<List<Customer>> AllCustomers([FromQuery] string? param) {
        if (string.IsNullOrEmpty(param)) {
            return _customerRepository.GetAll();
        }
        
        var customers = _customerRepository.GetAll()
            .Where(c =>
                (c.FirstName?.Contains(param, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.LastName?.Contains(param, StringComparison.OrdinalIgnoreCase) ?? false) ||
                c.Email.Contains(param, StringComparison.OrdinalIgnoreCase) ||
                (c.PhoneNumber?.Contains(param, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Province?.Contains(param, StringComparison.OrdinalIgnoreCase) ?? false));

        if (customers == null) {
            return new List<Customer>();
        }

        return customers.ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> Details(int id) {
        var customer = await _customerRepository.GetByIdInclude(id);
        if (customer == null) {
            return NotFound();
        }

        return customer;
    }

    [HttpGet("UserId/{userId}")]
    public async Task<ActionResult<Customer>> GetByUserId(string userId) {
        var customer = await _customerRepository.GetByUserId(userId);
        if (customer == null) {
            return NotFound();
        }

        return customer;
    }

    [HttpPost]
    public async Task<ActionResult> Create(CustomerDto customerDto) {
        var validProvinces = new List<string>() {
            "QC",
            "ON",
            "NB",
            "MB"
        };

        if (customerDto.Province != null && !validProvinces.Contains(customerDto.Province)) {
            ModelState.AddModelError("Province", "Province is invalid");
        }

        if (ModelState.IsValid) {
            var customer = new Customer() {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber,
                Province = customerDto.Province,
                CreditCard = customerDto.CreditCard,
                Password = customerDto.Password
            };
            await _customerRepository.Create(customer);
            await _customerRepository.Save();
            return CreatedAtAction(nameof(Create), new { id = customer.CustomerId }, customer);
        }

        return BadRequest(ModelState);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Edit(int id, CustomerDto customerDto) {
        if (id != customerDto.CustomerId) {
            return NotFound();
        }

        var validProvinces = new List<string>() {
            "QC",
            "ON",
            "NB",
            "MB"
        };

        if (customerDto.Province != null && !validProvinces.Contains(customerDto.Province)) {
            ModelState.AddModelError("Province", "Province is invalid");
        }

        if (ModelState.IsValid) {
            try {
                var customer = new Customer() {
                    CustomerId = customerDto.CustomerId,
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber,
                    Province = customerDto.Province,
                    CreditCard = customerDto.CreditCard,
                    UserId = customerDto.UserId
                };

                await _customerRepository.Update(customer);
                await _customerRepository.Save();
            }
            catch (DbUpdateConcurrencyException) {
                if (!Exists(customerDto.CustomerId)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            // Don't show customer password as string
            customerDto.Password = null;
            return Ok(customerDto);
        }

        return BadRequest(ModelState);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id) {
        var customer = await _customerRepository.GetById(id);

        if (customer == null) {
            return NotFound();
        }

        if (customer.ShoppingCart != null || customer.Orders.Any()) {
            return Conflict("Customer has shopping cart or orders");
        }

        if (ModelState.IsValid) {
            await _customerRepository.Delete(customer);
            return NoContent();
        }

        return NotFound();
    }

    [NonAction]
    private bool Exists(int id) {
        return _customerRepository.GetAll().Any(e => e.CustomerId == id);
    }
}