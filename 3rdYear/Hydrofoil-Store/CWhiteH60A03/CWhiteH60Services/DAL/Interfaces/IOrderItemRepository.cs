namespace CWhiteH60Services.DAL;

public interface IOrderItemRepository<T> where T: class {
    public Task<bool> Create(T model);
    public Task<List<T>> Read();
    public Task<bool> Update(T model);
    public Task<T> Find(int id);
    public Task<List<T>> FindByOrderId(int id);
}