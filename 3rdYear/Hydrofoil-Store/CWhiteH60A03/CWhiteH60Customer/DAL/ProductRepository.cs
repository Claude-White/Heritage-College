using System.Text;
using Microsoft.EntityFrameworkCore;
using CWhiteH60Customer.Models;
using CWhiteH60Customer.Models.DataTransferObjects;
using Newtonsoft.Json;

namespace CWhiteH60Customer.DAL;

public class ProductRepository : IProductRepository<Product> {
    private readonly HttpClient _httpClient;

    public ProductRepository(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }

    public async Task Create(Product product) {
        const string createProductEndpoint = "/api/Products";
        // Prevent self referencing loop
        product.ProdCat = null;
        var jsonProduct = JsonConvert.SerializeObject(product);
        
        var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(createProductEndpoint, content);

        if (response.IsSuccessStatusCode) {
            return;
        }
        
        return;
    }

    public async Task Update(Product product) {
        var editProductEndpoint = $"/api/Products/{product.ProductId}";
        // Prevent self referencing loop
        product.ProdCat = null;
        var jsonProduct = JsonConvert.SerializeObject(product);
        
        var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(editProductEndpoint, content);

        if (response.IsSuccessStatusCode) {
            return;
        }

        return;
    }

    public async Task UpdateStock(Product product) {
        var editProductStockEndpoint = $"/api/Products/Stock/{product.ProductId}";
        // Prevent self referencing loop
        product.ProdCat = null;
        var jsonProduct = JsonConvert.SerializeObject(product);
        
        var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PatchAsync(editProductStockEndpoint, content);

        if (response.IsSuccessStatusCode) {
            return;
        }

        return;
    }

    public async Task UpdatePrice(Product product) {
        var editProductPriceEndpoint = $"/api/Products/Price/{product.ProductId}";
        // Prevent self referencing loop
        product.ProdCat = null;
        var jsonProduct = JsonConvert.SerializeObject(product);
        
        var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PatchAsync(editProductPriceEndpoint, content);

        if (response.IsSuccessStatusCode) {
            return;
        }

        return;
    }

    public async Task Delete(Product product) {
        var deleteProductEndpoint = $"/api/Products/{product.ProductId}";
        var deleteProductResponse = await _httpClient.DeleteAsync(deleteProductEndpoint);
        if (deleteProductResponse.IsSuccessStatusCode) {
            return;
        }

        return;
    }

    public async Task<List<CustomerProductDto>> GetAll() {
        const string productsEndpoint = "/api/CustomerProducts";
        
        var productResponse = await _httpClient.GetAsync(productsEndpoint);

        if (productResponse.IsSuccessStatusCode) {
            var productsData = await productResponse.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<CustomerProductDto>>(productsData);
            if (products == null) {
                return [];
            }
            return products.ToList();
        }

        return null;
    }
    
    public async Task<List<CustomerProductDto>> GetByName(string name) {
        var productEndpoint = $"/api/CustomerProducts?productName={name}";
        
        var productResponse = await _httpClient.GetAsync(productEndpoint);

        if (productResponse.IsSuccessStatusCode) {
            var productData = await productResponse.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<CustomerProductDto>>(productData);
            return products;
        }

        return null;
    }
    
    public async Task<CustomerProductDto> GetById(int id) {
        var productEndpoint = $"/api/CustomerProducts/{id}";
        
        var productResponse = await _httpClient.GetAsync(productEndpoint);

        if (productResponse.IsSuccessStatusCode) {
            var productData = await productResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<CustomerProductDto>(productData);
            if (product == null) {
                return null;
            }
            
            return product;
        }

        return null;
    }

    public async Task<Product> GetByIdInclude(int id) {
        var productEndpoint = $"/api/CustomerProducts/{id}";
        
        var productResponse = await _httpClient.GetAsync(productEndpoint);

        if (productResponse.IsSuccessStatusCode) {
            var productData = await productResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(productData);
            return product;
        }

        return null;
    }
    
    public async Task<bool> NameExists(string prodName, int productId) {
        return true;
    }
}