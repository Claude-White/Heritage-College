using System.Text;
using CWhiteH60Customer.Models;
using Newtonsoft.Json;

namespace CWhiteH60Customer.DAL;

public class CartItemRepository : ICartItemRepository<CartItem> {
    private readonly HttpClient _httpClient;

    public CartItemRepository(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<bool> Create(CartItem cartItem) {
        const string createCartItemEndpoint = "/api/CartItem";
        
        var jsonCartItem = JsonConvert.SerializeObject(cartItem);
        
        var content = new StringContent(jsonCartItem, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(createCartItemEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(CartItem cartItem) {
        var createCartItemEndpoint = $"/api/CartItem/{cartItem.CartItemId}";

        var jsonCartItem = JsonConvert.SerializeObject(cartItem);
        
        var content = new StringContent(jsonCartItem, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(createCartItemEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(CartItem cartItem) {
        var deleteCartItemEndpoint = $"/api/CartItem/{cartItem.CartItemId}";
        var deleteCartItemResponse = await _httpClient.DeleteAsync(deleteCartItemEndpoint);
        return deleteCartItemResponse.IsSuccessStatusCode;
    }

    public async Task<List<CartItem>> GetAll() {
        const string cartItemsEndpoint = "/api/CartItem";
        
        var cartItemsResponse = await _httpClient.GetAsync(cartItemsEndpoint);

        if (!cartItemsResponse.IsSuccessStatusCode) return [];
        
        var cartItemsData = await cartItemsResponse.Content.ReadAsStringAsync();
        var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartItemsData);
        if (cartItems == null) {
            return [];
        }
        return cartItems.ToList();
    }

    public async Task<CartItem> GetById(int id) {
        var cartItemEndpoint = $"/api/CartItem/{id}";
        
        var cartItemResponse = await _httpClient.GetAsync(cartItemEndpoint);
        
        if (!cartItemResponse.IsSuccessStatusCode) return null;
        
        var cartItemData = await cartItemResponse.Content.ReadAsStringAsync();
        var cartItem = JsonConvert.DeserializeObject<CartItem>(cartItemData);
        if (cartItem == null) {
            return null;
        }
        
        return cartItem;
    }
}