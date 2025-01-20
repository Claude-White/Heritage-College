using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase {
    private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;

    public ShoppingCartController(IShoppingCartRepository<ShoppingCart> shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ShoppingCart>>> AllCarts() {
        return await _shoppingCartRepository.Read();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingCart>> FindCart(int id) {
        var shoppingCart = await _shoppingCartRepository.Find(id);
        if (shoppingCart == null) {
            return NotFound();
        }
        return shoppingCart;
    }
    
    [HttpGet("CustomerId/{id}")]
    public async Task<ActionResult<ShoppingCart>> FindCartByCustomerId(int id) {
        var shoppingCart = await _shoppingCartRepository.FindByCustomerId(id);
        if (shoppingCart == null) {
            return NotFound();
        }
        return shoppingCart;
    }

    [HttpPost]
    public async Task<ActionResult> CreateCart(ShoppingCartDto shoppingCartDto) {
        var shoppingCart = new ShoppingCart(shoppingCartDto);
        var success = await _shoppingCartRepository.Create(shoppingCart);
        if (!success) {
            return BadRequest(ModelState);
        }
        return CreatedAtAction(nameof(CreateCart), new { id = shoppingCart.CartId }, shoppingCart);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCart(ShoppingCartDto shoppingCartDto) {
        var shoppingCart = new ShoppingCart(shoppingCartDto);
        var success = await _shoppingCartRepository.Update(shoppingCart);
        if (!success) {
            return BadRequest(ModelState);
        }
        return Ok(shoppingCart);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCart(int id) {
        var shoppingCart = await _shoppingCartRepository.Find(id);
        if (shoppingCart == null) {
            return NotFound();
        }
        
        var success = await _shoppingCartRepository.Delete(shoppingCart);
        if (!success) {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}