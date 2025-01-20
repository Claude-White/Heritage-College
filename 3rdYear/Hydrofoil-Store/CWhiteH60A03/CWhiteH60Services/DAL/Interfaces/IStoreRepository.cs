namespace CWhiteH60Services.DAL;

public interface IStoreRepository<T> where T : class {
    Task Create(T model);
    Task Update(T model);
    Task Delete(T model);
    List<T> GetAll();
    Task<T> GetById(int id);
    Task<T> GetByIdInclude(int id);
    Task Save();
    bool NameExists(string name, int id);
}