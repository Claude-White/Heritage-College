namespace CWhiteH60Customer.DAL;

public interface IShoppingCartRepository<T> where T : class {
    Task<bool> Create(T model);
    Task<bool> Update(T model);
    Task<bool> Delete(T model);
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> GetByCustomerId(int id);
    Task<T> GetByIdInclude(int id);
    Task<T> GetByCustomerIdInclude(int id);
}