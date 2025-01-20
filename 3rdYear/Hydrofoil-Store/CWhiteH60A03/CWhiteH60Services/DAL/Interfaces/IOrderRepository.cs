namespace CWhiteH60Services.DAL;

public interface IOrderRepository<T> where T: class {
    public Task<bool> Create(T model);
    public Task<List<T>> Read();
    public Task<List<T>> ReadByDate(DateTime date);
    public Task<List<T>> ReadByCustomer(string customerName);
    public Task<List<T>> ReadByCustomer(int customerId);
    public Task<bool> Update(T model);
    public Task<T> Find(int id);
}