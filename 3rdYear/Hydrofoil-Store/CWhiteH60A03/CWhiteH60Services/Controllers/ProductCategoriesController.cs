using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
    
namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "manager,clerk")]
public class ProductCategoriesController : ControllerBase {
    private readonly IStoreRepository<ProductCategory> _prodCatRepo;

    public ProductCategoriesController(IStoreRepository<ProductCategory> prodCatRepo) {
        _prodCatRepo = prodCatRepo;
    }
    
    [HttpGet]
    public ActionResult<List<ProductCategory>> AllProductCategories() {
        return _prodCatRepo.GetAll();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductCategory>> Details(int id) {
        var productCategory = await _prodCatRepo.GetById(id);

        if (productCategory == null) {
            return NotFound();
        }

        return productCategory;
    }

    [HttpPost]
    public async Task<ActionResult> Create(ProductCategoryDto productCategoryDto) {
        if (ModelState.IsValid) {
            if (_prodCatRepo.NameExists(productCategoryDto.ProdCat, productCategoryDto.CategoryId))
            {
                ModelState.AddModelError("ProdCat", "This category name already exists");
                return BadRequest(ModelState);
            }
            var productCategory = new ProductCategory() {
                CategoryId = productCategoryDto.CategoryId,
                ProdCat = productCategoryDto.ProdCat
            };
            await _prodCatRepo.Create(productCategory);
            await _prodCatRepo.Save();
            return CreatedAtAction(nameof(Create), new { id = productCategory.CategoryId }, productCategory);
        }
        
        return BadRequest(ModelState);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Edit(int id, ProductCategoryDto productCategoryDto) {
        if (id != productCategoryDto.CategoryId) {
            return NotFound();
        }
        
        if (ModelState.IsValid) {
            try {
                if (_prodCatRepo.NameExists(productCategoryDto.ProdCat, productCategoryDto.CategoryId))
                {
                    ModelState.AddModelError("ProdCat", "This category name already exists");
                    return BadRequest(ModelState);
                }
                var productCategory = new ProductCategory() {
                    CategoryId = productCategoryDto.CategoryId,
                    ProdCat = productCategoryDto.ProdCat
                };
                await _prodCatRepo.Update(productCategory);
                await _prodCatRepo.Save();
            }
            catch (DbUpdateConcurrencyException) {
                if (!Exists(productCategoryDto.CategoryId)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            
            return Ok(productCategoryDto);
        }

        return BadRequest(ModelState);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var productCategory = await _prodCatRepo.GetByIdInclude(id);
        
        if (productCategory == null) {
            return NotFound();
        }

        if (productCategory.Products.Any()) {
            return Conflict("Product category has products");
        }
        
        if (ModelState.IsValid)
        {
            await _prodCatRepo.Delete(productCategory);
            await _prodCatRepo.Save();
            return NoContent();
        }
        return NotFound();
    }

    [HttpGet("NameExists/{id:int}/{name}")]
    public bool NameExists(int id, string name) {
        return _prodCatRepo.NameExists(name, id);
    }

    [NonAction]
    private bool Exists(int id) {
        return _prodCatRepo.GetAll().Any(p => p.CategoryId == id);
    }
}