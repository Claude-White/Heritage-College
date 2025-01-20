using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Services.DAL;

public class ProductRepository : IStoreRepository<Product> {
    private readonly H60AssignmentDbCWContext _context;

    public ProductRepository(H60AssignmentDbCWContext context) {
        _context = context;
    }

    public async Task Create(Product product) {
        _context.Products.Add(product);
    }

    public async Task Update(Product product) {
        _context.Products.Update(product);
    }

    public async Task Delete(Product product) {
        _context.Products.Remove(product);
    }

    public List<Product> GetAll() {
        return _context.Products
            .Include(p => p.ProdCat)
            .OrderBy(p => p.ProdCat.ProdCat)
            .ThenBy(p => p.Description)
            .ToList();
    }

    public async Task<List<ProductDto>> GetAllNoImages() {
        return await _context.Products
            .Include(p => p.ProdCat)
            .OrderBy(p => p.ProdCat.ProdCat)
            .ThenBy(p => p.Description)
            .Select(p => new ProductDto(p))
            .ToListAsync();
    }

    public async Task<ImageDto> GetImageByProductId(int id) {
        return await _context.Products
            .Where(product => product.ProductId == id)
            .Select(product => new ImageDto(product))
            .FirstOrDefaultAsync();

    }
    
    public async Task<Product> GetById(int id) {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<Product> GetByIdInclude(int id) {
        return await _context.Products
            .Include(p => p.ProdCat)
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task Save() {
        await _context.SaveChangesAsync();
    }
    
    public bool NameExists(string prodName, int productId) {
        return _context.Products
            .Any(p => p.Description == prodName && p.ProductId != productId);
    }
}