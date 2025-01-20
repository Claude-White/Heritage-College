namespace CWhiteH60Customer.DAL;

public interface IOrderItemRepository<T> where T : class {
    Task<bool> Create(T model);
    Task<bool> Update(T model);
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> GetByOrderId(int orderId);
}