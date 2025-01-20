namespace CWhiteH60Customer.DAL;

public interface IOrderRepository<T> where T : class {
    Task<bool> Create(T model);
    Task<bool> Update(T model);
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<List<T>> GetByDate(DateTime date);
    Task<List<T>> GetByCustomerName(string customerName);
    Task<List<T>> GetByCustomerId(int customerId);
}