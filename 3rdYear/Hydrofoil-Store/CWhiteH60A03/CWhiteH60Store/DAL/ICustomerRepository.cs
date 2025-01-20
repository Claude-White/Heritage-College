namespace CWhiteH60Store.DAL; 

public interface ICustomerRepository<T> where T : class {
    Task<bool> Create(T model);
    Task Update(T model);
    Task Delete(T model);
    Task<List<T>> GetAll();
    Task<T> GetByUserId(string userId);
    Task<T> GetById(int id);
    Task<T> GetByIdInclude(int id);
    Task<bool> NameExists(string prodCat, int categoryId);
}