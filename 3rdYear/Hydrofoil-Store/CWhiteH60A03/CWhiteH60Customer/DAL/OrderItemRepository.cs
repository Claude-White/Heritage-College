using System.Text;
using CWhiteH60Customer.Models;
using Newtonsoft.Json;

namespace CWhiteH60Customer.DAL;

public class OrderItemRepository : IOrderItemRepository<OrderItem> {
    private readonly HttpClient _httpClient;

    public OrderItemRepository(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<bool> Create(OrderItem orderItem) {
        const string createOrderItemEndpoint = "/api/OrderItem";
        
        var jsonOrderItem = JsonConvert.SerializeObject(orderItem);
        
        var content = new StringContent(jsonOrderItem, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(createOrderItemEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(OrderItem orderItem) {
        var createOrderItemEndpoint = $"/api/OrderItem/{orderItem.OrderItemId}";

        var jsonOrderItem = JsonConvert.SerializeObject(orderItem);
        
        var content = new StringContent(jsonOrderItem, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(createOrderItemEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<OrderItem>> GetAll() {
        const string orderItemsEndpoint = "/api/OrderItem";
        
        var orderItemsResponse = await _httpClient.GetAsync(orderItemsEndpoint);

        if (!orderItemsResponse.IsSuccessStatusCode) return [];
        
        var orderItemsData = await orderItemsResponse.Content.ReadAsStringAsync();
        var orderItems = JsonConvert.DeserializeObject<List<OrderItem>>(orderItemsData);
        if (orderItems == null) {
            return [];
        }
        return orderItems.ToList();
    }

    public async Task<OrderItem> GetById(int id) {
        var orderItemEndpoint = $"/api/OrderItem/{id}";
        
        var orderItemResponse = await _httpClient.GetAsync(orderItemEndpoint);
        
        if (!orderItemResponse.IsSuccessStatusCode) return null;
        
        var orderItemData = await orderItemResponse.Content.ReadAsStringAsync();
        var orderItem = JsonConvert.DeserializeObject<OrderItem>(orderItemData);
        if (orderItem == null) {
            return null;
        }
        
        return orderItem;
    }

    public async Task<OrderItem> GetByOrderId(int orderId) {
        var orderItemEndpoint = $"/api/OrderItem/Order/{orderId}";
        
        var orderItemResponse = await _httpClient.GetAsync(orderItemEndpoint);
        
        if (!orderItemResponse.IsSuccessStatusCode) return null;
        
        var orderItemData = await orderItemResponse.Content.ReadAsStringAsync();
        var orderItem = JsonConvert.DeserializeObject<OrderItem>(orderItemData);
        if (orderItem == null) {
            return null;
        }
        
        return orderItem;
    }
}