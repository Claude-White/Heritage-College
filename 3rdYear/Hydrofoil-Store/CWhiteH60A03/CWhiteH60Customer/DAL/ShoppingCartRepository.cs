using System.Text;
using CWhiteH60Customer.Models;
using Newtonsoft.Json;

namespace CWhiteH60Customer.DAL;

public class ShoppingCartRepository : IShoppingCartRepository<ShoppingCart> {
    private readonly HttpClient _httpClient;

    public ShoppingCartRepository(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }

    public async Task<bool> Create(ShoppingCart shoppingCart) {
        const string createShoppingCartEndpoint = "/api/ShoppingCart";
        shoppingCart.CartItems = new List<CartItem>();
        var jsonShoppingCart = JsonConvert.SerializeObject(shoppingCart);
        
        var content = new StringContent(jsonShoppingCart, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(createShoppingCartEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(ShoppingCart shoppingCart) {
        var editShoppingCartEndpoint = $"/api/ShoppingCart/{shoppingCart.CartId}";
        shoppingCart.CartItems = new List<CartItem>();
        var jsonShoppingCart = JsonConvert.SerializeObject(shoppingCart);
        
        var content = new StringContent(jsonShoppingCart, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(editShoppingCartEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(ShoppingCart shoppingCart) {
        var deleteShoppingCartEndpoint = $"/api/ShoppingCart/{shoppingCart.CartId}";
        var deleteShoppingCartResponse = await _httpClient.DeleteAsync(deleteShoppingCartEndpoint);
        return deleteShoppingCartResponse.IsSuccessStatusCode;
    }

    public async Task<List<ShoppingCart>> GetAll() {
        const string shoppingCartsEndpoint = "/api/ShoppingCart";
        
        var shoppingCartsResponse = await _httpClient.GetAsync(shoppingCartsEndpoint);

        if (!shoppingCartsResponse.IsSuccessStatusCode) return [];
        
        var shoppingCartsData = await shoppingCartsResponse.Content.ReadAsStringAsync();
        var shoppingCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(shoppingCartsData);
        if (shoppingCarts == null) {
            return [];
        }
        return shoppingCarts.ToList();
    }

    public async Task<ShoppingCart> GetById(int id) {
        var shoppingCartEndpoint = $"/api/ShoppingCart/{id}";
        
        var shoppingCartResponse = await _httpClient.GetAsync(shoppingCartEndpoint);
        
        if (!shoppingCartResponse.IsSuccessStatusCode) return null;
        
        var shoppingCartData = await shoppingCartResponse.Content.ReadAsStringAsync();
        var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(shoppingCartData);
        if (shoppingCart == null) {
            return null;
        }

        shoppingCart.CartItems = null;
        return shoppingCart;
    }
    
    public async Task<ShoppingCart> GetByCustomerId(int id) {
        var shoppingCartEndpoint = $"/api/ShoppingCart/CustomerId/{id}";
        
        var shoppingCartResponse = await _httpClient.GetAsync(shoppingCartEndpoint);
        
        if (!shoppingCartResponse.IsSuccessStatusCode) return null;
        
        var shoppingCartData = await shoppingCartResponse.Content.ReadAsStringAsync();
        var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(shoppingCartData);
        if (shoppingCart == null) {
            return null;
        }

        shoppingCart.CartItems = null;
        return shoppingCart;
    }

    public async Task<ShoppingCart> GetByIdInclude(int id) {
        var shoppingCartEndpoint = $"/api/ShoppingCart/{id}";
        
        var shoppingCartResponse = await _httpClient.GetAsync(shoppingCartEndpoint);
        
        if (!shoppingCartResponse.IsSuccessStatusCode) return null;
        
        var shoppingCartData = await shoppingCartResponse.Content.ReadAsStringAsync();
        var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(shoppingCartData);
        return shoppingCart;
    }
    
    public async Task<ShoppingCart> GetByCustomerIdInclude(int id) {
        var shoppingCartEndpoint = $"/api/ShoppingCart/CustomerId/{id}";
        
        var shoppingCartResponse = await _httpClient.GetAsync(shoppingCartEndpoint);
        
        if (!shoppingCartResponse.IsSuccessStatusCode) return null;
        
        var shoppingCartData = await shoppingCartResponse.Content.ReadAsStringAsync();
        var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(shoppingCartData);
        return shoppingCart;
    }
}