using System.Text;
using CWhiteH60Customer.Models;
using Newtonsoft.Json;

namespace CWhiteH60Customer.DAL;

public class OrderRepository : IOrderRepository<Order> {
    private readonly HttpClient _httpClient;

    public OrderRepository(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<bool> Create(Order order) {
        const string createOrderEndpoint = "/api/Order";
        
        var jsonOrder = JsonConvert.SerializeObject(order);
        
        var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(createOrderEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(Order order) {
        var createOrderEndpoint = $"/api/Order/{order.OrderId}";

        var jsonOrder = JsonConvert.SerializeObject(order);
        
        var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(createOrderEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<Order>> GetAll() {
        const string ordersEndpoint = "/api/Order";
        
        var ordersResponse = await _httpClient.GetAsync(ordersEndpoint);

        if (!ordersResponse.IsSuccessStatusCode) return [];
        
        var ordersData = await ordersResponse.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<Order>>(ordersData);
        if (orders == null) {
            return [];
        }
        return orders.ToList();
    }

    public async Task<Order> GetById(int id) {
        var orderEndpoint = $"/api/Order/{id}";
        
        var orderResponse = await _httpClient.GetAsync(orderEndpoint);
        
        if (!orderResponse.IsSuccessStatusCode) return null;
        
        var orderData = await orderResponse.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<Order>(orderData);
        if (order == null) {
            return null;
        }
        
        return order;
    }

    public async Task<List<Order>> GetByDate(DateTime date) {
        var ordersEndpoint = $"/api/Order/Date/{date}";
        
        var ordersResponse = await _httpClient.GetAsync(ordersEndpoint);

        if (!ordersResponse.IsSuccessStatusCode) return [];
        
        var ordersData = await ordersResponse.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<Order>>(ordersData);
        if (orders == null) {
            return [];
        }
        return orders.ToList();
    }

    public async Task<List<Order>> GetByCustomerName(string customerName) {
        var ordersEndpoint = $"/api/Order/Name/{customerName}";
        
        var ordersResponse = await _httpClient.GetAsync(ordersEndpoint);

        if (!ordersResponse.IsSuccessStatusCode) return [];
        
        var ordersData = await ordersResponse.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<Order>>(ordersData);
        if (orders == null) {
            return [];
        }
        return orders.ToList();
    }

    public async Task<List<Order>> GetByCustomerId(int customerId) {
        var ordersEndpoint = $"/api/Order/Id/{customerId}";
        
        var ordersResponse = await _httpClient.GetAsync(ordersEndpoint);

        if (!ordersResponse.IsSuccessStatusCode) return [];
        
        var ordersData = await ordersResponse.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<Order>>(ordersData);
        if (orders == null) {
            return [];
        }
        return orders.ToList();
    }
}