using CWhiteH60Customer.Models.DataTransferObjects;

namespace CWhiteH60Customer.DAL;

public interface IProductRepository<T> where T : class {
    Task Create(T model);
    Task Update(T model);
    Task UpdateStock(T model);
    Task UpdatePrice(T model);
    Task Delete(T model);
    Task<List<CustomerProductDto>> GetAll();
    Task<List<CustomerProductDto>> GetByName(string name);
    Task<CustomerProductDto> GetById(int id);
    Task<T> GetByIdInclude(int id);
    Task<bool> NameExists(string prodCat, int categoryId);
}