namespace CWhiteH60Services.DAL;

public interface ICustomerRepository<T> where T : class {
    Task Create(T model);
    Task Update(T model);
    Task Delete(T model);
    List<T> GetAll();
    Task<T> GetByUserId(string userId);
    Task<T> GetById(int id);
    Task<T> GetByIdInclude(int id);
    Task Save();
    bool NameExists(string name, int id);
}