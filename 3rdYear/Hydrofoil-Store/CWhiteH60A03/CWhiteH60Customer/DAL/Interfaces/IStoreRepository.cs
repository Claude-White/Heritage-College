namespace CWhiteH60Customer.DAL; 

public interface IStoreRepository<T> where T : class {
    Task Create(T model);
    Task Update(T model);
    Task Delete(T model);
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> GetByIdInclude(int id);
    Task<bool> NameExists(string prodCat, int categoryId);
}