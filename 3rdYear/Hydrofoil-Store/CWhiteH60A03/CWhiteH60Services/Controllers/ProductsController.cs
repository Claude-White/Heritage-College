using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductRepository _storeRepository;

    public ProductsController(ProductRepository storeRepo)
    {
        _storeRepository = storeRepo;
    }
    
    [HttpGet]
    public ActionResult<List<Product>> AllProducts([FromQuery] string? productName)
    {
        if (!string.IsNullOrEmpty(productName)) {
            return _storeRepository.GetAll().Where(p => p.Description.Contains(productName, StringComparison.InvariantCultureIgnoreCase) || p.ProdCat.ProdCat.Contains(productName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
        return _storeRepository.GetAll().ToList();
    }

    [HttpGet("Manager")]
    public async Task<ActionResult<List<ProductDto>>> GetAllProducts([FromQuery] string? productName) {
        if (!string.IsNullOrEmpty(productName)) {
            return (await _storeRepository.GetAllNoImages())
                .Where(p => p.Description.Contains(productName, StringComparison.InvariantCultureIgnoreCase) || p.ProdCategory.ProdCat.Contains(productName, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
        return await _storeRepository.GetAllNoImages();
    }

    [HttpGet("Image/{id}")]
    public async Task<ActionResult<ImageDto>> GetImageByProductId(int id) {
        return await _storeRepository.GetImageByProductId(id);
    }

    [HttpGet("/api/ProductsByCategory/{id:int}")]
    public ActionResult<List<Product>> FilteredProducts(int id) {
        if (id == 0) return _storeRepository.GetAll();
        return _storeRepository.GetAll().Where(p => p.ProdCat.CategoryId == id).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> Details(int id)
    {
        var product = await _storeRepository.GetByIdInclude(id);
        if (product == null)
        {
            return NotFound();
        }
        
        return product;
    }
    
    [HttpGet("/api/CustomerProducts")]
    public ActionResult<List<CustomerProductDto>> AllProductsCustomer([FromQuery] string? productName)
    {
        if (!string.IsNullOrEmpty(productName)) {
            var test = _storeRepository.GetAll()
                .Where(p => p.Description.Contains(productName, StringComparison.InvariantCultureIgnoreCase) ||
                            p.ProdCat.ProdCat.Contains(productName, StringComparison.InvariantCultureIgnoreCase))
                .Select(p => new CustomerProductDto(p)).ToList();
            return test;
        }
        return _storeRepository.GetAll().Select(p => new CustomerProductDto(p)).ToList();
    }

    [HttpGet("/api/CustomerProductsByCategory/{id:int}")]
    public ActionResult<List<CustomerProductDto>> FilteredProductsCustomer(int id) {
        if (id == 0) return _storeRepository.GetAll().Select(p => new CustomerProductDto(p)).ToList();
        return _storeRepository.GetAll().Where(p => p.ProdCat.CategoryId == id).Select(p => new CustomerProductDto(p)).ToList();
    }

    [HttpGet("/api/CustomerProducts/{id:int}")]
    public async Task<ActionResult<CustomerProductDto>> DetailsCustomer(int id)
    {
        var product = await _storeRepository.GetByIdInclude(id);
        if (product == null)
        {
            return NotFound();
        }
        
        var customerProduct = new CustomerProductDto(product);
        
        return customerProduct;
    }
    
    [HttpPost]
    public async Task<ActionResult<Product>> Create(ProductImageDto productImageDto) {
        var product = new Product() {
            ProductId = productImageDto.ProductId,
            ProdCatId = productImageDto.ProdCatId,
            Description = productImageDto.Description,
            Manufacturer = productImageDto.Manufacturer,
            Stock = productImageDto.Stock,
            BuyPrice = productImageDto.BuyPrice,
            SellPrice = productImageDto.SellPrice,
            Notes = productImageDto.Notes,
            ImageName = productImageDto.ImageName,
            ImageData = productImageDto.ImageData
        };

        if (ModelState.IsValid)
        {
            await _storeRepository.Create(product);
            await _storeRepository.Save();
            return CreatedAtAction(nameof(Create), new { id = product.ProductId }, product);
        }
        
        return BadRequest(ModelState);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Edit(int id, ProductImageDto productImageDto) {
        var product = new Product() {
            ProductId = productImageDto.ProductId,
            ProdCatId = productImageDto.ProdCatId,
            Description = productImageDto.Description,
            Manufacturer = productImageDto.Manufacturer,
            BuyPrice = productImageDto.BuyPrice,
            SellPrice = productImageDto.SellPrice,
            Stock = productImageDto.Stock,
            Notes = productImageDto.Notes,
            ImageName = productImageDto.ImageName,
            ImageData = productImageDto.ImageData
        };
        
        if (id != product.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _storeRepository.Update(product);
                await _storeRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Ok(product);
        }
        
        return BadRequest(ModelState);
    }

    [HttpPatch("Stock/{id:int}")]
    public async Task<ActionResult<Product>> EditStock(int id, ProductStockDto productStockDto)
    {
        var product = await _storeRepository.GetById(id);
        
        if (product.Stock + productStockDto.Stock < 0)
        {
            ModelState.AddModelError("Stock", "Stock must be greater than 0");
        }

        if (ModelState.IsValid)
        {
            product.Stock += productStockDto.Stock;
            try
            {
                await _storeRepository.Update(product);
                await _storeRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Ok(product);
        }
        
        return BadRequest(ModelState);
    }

    [HttpPatch("Price/{id:int}")]
    public async Task<ActionResult<Product>> EditPrice(int id, ProductPriceDto productPriceDto)
    {
        if (id != productPriceDto.ProductId) {
            return NotFound();
        }
        
        var product = await _storeRepository.GetById(id);

        if (productPriceDto.BuyPrice != 0 && productPriceDto.SellPrice != 0) {
            if (productPriceDto.SellPrice <= productPriceDto.BuyPrice) {
                ModelState.AddModelError("Price", "Sell price must be greater than the buy price.");
            }
        }
        else {
            if (productPriceDto.BuyPrice != 0 && productPriceDto.BuyPrice >= product.SellPrice) {
                ModelState.AddModelError("Price", "Buy price must be less than the sell price.");
            }

            if (productPriceDto.SellPrice != 0 && productPriceDto.SellPrice <= product.BuyPrice) {
                ModelState.AddModelError("Price", "Sell price must be greater than the buy price.");
            }
        }
        
        if (ModelState.IsValid)
        {
            if (productPriceDto.BuyPrice != 0) {
                product.BuyPrice = productPriceDto.BuyPrice;
            }
            if (productPriceDto.SellPrice != 0) {
                product.SellPrice = productPriceDto.SellPrice;
            }
            try
            {
                await _storeRepository.Update(product);
                await _storeRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Ok(product);
        }
        return BadRequest(ModelState);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _storeRepository.GetByIdInclude(id);

        if (product != null)
        {
            await _storeRepository.Delete(product);
            await _storeRepository.Save();
            return NoContent();
        }
        
        return NotFound();
    }
    
    [NonAction]
    private bool Exists(int id)
    {
        return _storeRepository.GetAll().Any(e => e.ProductId == id);
    }
}