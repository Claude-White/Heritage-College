namespace CWhiteH60Customer.DAL;

public interface ICartItemRepository<T> where T : class {
    Task<bool> Create(T model);
    Task<bool> Update(T model);
    Task<bool> Delete(T model);
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
}