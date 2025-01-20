using Microsoft.EntityFrameworkCore;
using CWhiteH60Store.DAL;
using CWhiteH60Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Store.Controllers;

[Authorize(Roles = "manager,clerk")]
public class ProductCategoryController : Controller {
    private readonly IStoreRepository<ProductCategory> _prodCatRepo;

    public ProductCategoryController(IStoreRepository<ProductCategory> prodCatRepo) {
        _prodCatRepo = prodCatRepo;
    }

    public async Task<IActionResult> Index() {
        return View(await _prodCatRepo.GetAll());
    }

    public async Task<IActionResult> Details(int? id) {
        if (id == null) {
            return NotFound();
        }

        var productCategory = await _prodCatRepo.GetById(id.Value);

        if (productCategory == null) {
            return NotFound();
        }

        return View(productCategory);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCategory productCategory) {
        if (ModelState.IsValid) {
            if (await _prodCatRepo.NameExists(productCategory.ProdCat, productCategory.CategoryID))
            {
                ModelState.AddModelError("ProdCat", "This category name already exists");
                return View(productCategory);
            }
            await _prodCatRepo.Create(productCategory);
            return RedirectToAction(nameof(Index));
        }

        return View(productCategory);
    }

    public async Task<IActionResult> Edit(int? id) {
        if (id == null) {
            return NotFound();
        }

        var productCategory = await _prodCatRepo.GetById(id.Value);
        if (productCategory == null) {
            return NotFound();
        }

        return View(productCategory);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductCategory productCategory) {
        if (id != productCategory.CategoryID) {
            return NotFound();
        }
        
        if (ModelState.IsValid) {
            try {
                if (await _prodCatRepo.NameExists(productCategory.ProdCat, productCategory.CategoryID))
                {
                    ModelState.AddModelError("ProdCat", "This category name already exists");
                    return View(productCategory);
                }
                await _prodCatRepo.Update(productCategory);
            }
            catch (DbUpdateConcurrencyException) {
                if (!await Exists(productCategory.CategoryID)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(productCategory);
    }

    public async Task<IActionResult> Delete(int? id) {
        if (id == null) {
            return NotFound();
        }

        var productCategory = await _prodCatRepo.GetById(id.Value);
        if (productCategory == null) {
            return NotFound();
        }

        return View(productCategory);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var productCategory = await _prodCatRepo.GetByIdInclude(id);
        
        if (productCategory.Products.Any()) {
            ModelState.AddModelError("Products", "There are products with this category");
        }
        
        if (productCategory != null && ModelState.IsValid)
        {
            await _prodCatRepo.Delete(productCategory);
            return RedirectToAction(nameof(Index));
        }
        
        return View(productCategory);
    }

    public async Task<bool> Exists(int id) {
        var productCategory = await _prodCatRepo.GetAll();
        return productCategory.Any(p => p.CategoryID == id);
    }
}