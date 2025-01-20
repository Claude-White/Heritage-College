using CWhiteH60Services.Models;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Services.DAL;

public class ProdCatRepository : IStoreRepository<ProductCategory> {
    private readonly H60AssignmentDbCWContext _context;

    public ProdCatRepository(H60AssignmentDbCWContext context) {
        _context = context;
    }

    public async Task Create(ProductCategory prodCat) {
        _context.ProductCategories.Add(prodCat);
    }

    public async Task Update(ProductCategory prodCat) {
        _context.ProductCategories.Update(prodCat);
    }

    public async Task Delete(ProductCategory prodCat) {
        _context.ProductCategories.Remove(prodCat);
    }
    
    public List<ProductCategory> GetAll() {
        return _context.ProductCategories
            .Include(pc => pc.Products)
            .OrderBy(pc => pc.ProdCat)
            .ToList();
    }
    

    public async Task<ProductCategory> GetById(int id) {
        return await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.CategoryId == id);
    }
    
    public async Task<ProductCategory> GetByIdInclude(int id) {
        return await _context.ProductCategories.Include(pc => pc.Products).FirstOrDefaultAsync(pc => pc.CategoryId == id);
    }

    public async Task Save() {
        await _context.SaveChangesAsync();
    }
    
    public bool NameExists(string prodCat, int categoryId) {
        return _context.ProductCategories
            .Any(pc => pc.ProdCat == prodCat && pc.CategoryId != categoryId);
    }
}