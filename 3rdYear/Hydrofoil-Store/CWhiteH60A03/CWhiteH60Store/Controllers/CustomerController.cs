using CWhiteH60Store.DAL;
using CWhiteH60Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Store.Controllers;

[Authorize(Roles = "manager")]
public class CustomerController : Controller {

    private readonly ICustomerRepository<Customer> _customerRepository;
    
    public CustomerController(ICustomerRepository<Customer> customerRepository) {
        _customerRepository = customerRepository;
    }

    public async Task<IActionResult> Index() {
        return View(await _customerRepository.GetAll());
    }

    public async Task<IActionResult> Details(int? id) {
        if (id == null) {
            return NotFound();
        }
        
        var customer = await _customerRepository.GetById(id.Value);
        if (customer == null)
        {
            return NotFound();
        }
        
        return View(customer);
    }
    
    public async Task<IActionResult> Edit(int id) {
        var customer = await _customerRepository.GetById(id);
        if (customer == null)
        {
            return NotFound();
        }

        ViewData["Provinces"] = new Dictionary<string, string>() {
            { "QC", "Quebec" },
            { "ON", "Ontario" },
            { "NB", "New Brunswick" },
            { "MB", "Manitoba" },
        };
        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Customer customer) {
        if (id != customer.CustomerId)
        {
            return NotFound();
        }
        
        ModelState.Remove("Password");
        ModelState.Remove("UserId");
        
        if (ModelState.IsValid)
        {
            try
            {
                await _customerRepository.Update(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await Exists(customer.CustomerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Provinces"] = new Dictionary<string, string>() {
            { "QC", "Quebec" },
            { "ON", "Ontario" },
            { "NB", "New Brunswick" },
            { "MB", "Manitoba" },
        };
        return View(customer);
    }

    public async Task<IActionResult> Create() {
        ViewData["Provinces"] = new Dictionary<string, string>() {
            { "QC", "Quebec" },
            { "ON", "Ontario" },
            { "NB", "New Brunswick" },
            { "MB", "Manitoba" },
        };
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer) {

        if (customer.Password == null) {
            ModelState.AddModelError("Password", "The Password field is required.");
        }
        
        ModelState.Remove("UserId");

        if (ModelState.IsValid)
        {
            await _customerRepository.Create(customer);
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Provinces"] = new Dictionary<string, string>() {
            { "QC", "Quebec" },
            { "ON", "Ontario" },
            { "NB", "New Brunswick" },
            { "MB", "Manitoba" },
        };
        return View(customer);
    }
    
    
    public async Task<bool> Exists(int id) {
        var customers = await _customerRepository.GetAll();
        return customers.Any(e => e.CustomerId == id);
    }
}