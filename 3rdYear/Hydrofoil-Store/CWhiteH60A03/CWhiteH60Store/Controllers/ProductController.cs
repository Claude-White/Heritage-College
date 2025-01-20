using Microsoft.EntityFrameworkCore;
using CWhiteH60Store.DAL;
using CWhiteH60Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace CWhiteH60Store.Controllers;

[Authorize(Roles = "manager,clerk")]
public class ProductController : Controller
{
    private readonly IProductRepository<Product> _productRepository;
    private readonly IStoreRepository<ProductCategory> _prodCatRepo;

    public ProductController(IProductRepository<Product> productRepo, IStoreRepository<ProductCategory> prodCatRepo)
    {
        _productRepository = productRepo;
        _prodCatRepo = prodCatRepo;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.CatId = 0;
        ViewBag.ProdCats = new SelectList(await _prodCatRepo.GetAll(), "CategoryID", "ProdCat");
        return View(await _productRepository.GetAll());
    }

    public async Task<IActionResult> FilterProducts(int id) {
        ViewBag.CatId = id;
        ViewBag.ProdCats = new SelectList(await _prodCatRepo.GetAll(), "CategoryID", "ProdCat");
        if (id == 0) return View("Index", await _productRepository.GetAll());
        var prodCats = await _productRepository.GetAll();
        return View("Index", prodCats.Where(p => p.ProdCat.CategoryID == id));

    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdInclude(id.Value);
        if (product == null)
        {
            return NotFound();
        }
        
        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }
    
    public async Task<IActionResult> GetImage(int id)
    {
        var product = await _productRepository.GetById(id);
        if (product.ImageData != null)
        {
            return File(product.ImageData, "image/png");
        }
        return NotFound();
    }

    public async Task<IActionResult> Create()
    {
        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (imageFile is { Length: > 0 })
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                product.ImageData = memoryStream.ToArray();
                product.ImageName = imageFile.FileName;
            }
        }

        if (product.ProdCatId == 0) {
            ModelState.AddModelError("ProdCatId", "Category is required");
        }
        else {
            product.ProdCat = await _prodCatRepo.GetByIdInclude(product.ProdCatId);
            ModelState.Remove("ProdCat");
        }

        if (ModelState.IsValid)
        {
            await _productRepository.Create(product);
            return RedirectToAction(nameof(Index));
        }

        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdInclude(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }

    public async Task<IActionResult> EditStock(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdInclude(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }

    [Authorize(Roles = "manager")]
    public async Task<IActionResult> EditPrice(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdInclude(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile, string? removeImage)
    {
        if (id != product.ProductID)
        {
            return NotFound();
        }
        
        bool shouldRemoveImage = !string.IsNullOrEmpty(removeImage);
        

        product.ProdCat = await _prodCatRepo.GetById(product.ProdCatId);
        ModelState.Remove("ProdCat");
        
        if (shouldRemoveImage) {
            product.ImageData = null;
            product.ImageName = null;
        }
        if (imageFile is { Length: > 0 }) {
            using (var memoryStream = new MemoryStream()) {
                await imageFile.CopyToAsync(memoryStream);
                product.ImageData = memoryStream.ToArray();
                product.ImageName = imageFile.FileName;
            }
        }
        

        if (ModelState.IsValid)
        {
            try
            {
                await _productRepository.Update(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await Exists(product.ProductID))
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

        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStock(int id, int stock)
    {
        var product = await _productRepository.GetByIdInclude(id);

        if (product.Stock + stock < 0)
        {
            ModelState.AddModelError("Stock", "Stock must be greater than 0");
        }
        
        product.Stock = stock;

        if (ModelState.IsValid)
        {
            try
            {
                await _productRepository.UpdateStock(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await Exists(product.ProductID))
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

        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "manager")]
    public async Task<IActionResult> EditPrice(int id, Product product)
    {
        if (id != product.ProductID)
        {
            return NotFound();
        }

        product.ProdCat = await _prodCatRepo.GetById(product.ProdCatId);
        ModelState.Remove("ProdCat");

        if (ModelState.IsValid)
        {
            try
            {
                await _productRepository.UpdatePrice(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await Exists(product.ProductID))
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

        ViewData["ProdCat"] = await _prodCatRepo.GetAll();
        return View(product);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdInclude(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productRepository.GetByIdInclude(id);

        if (product != null)
        {
            await _productRepository.Delete(product);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<bool> Exists(int id) {
        var products = await _productRepository.GetAll();
        return products.Any(e => e.ProductID == id);
    }
}