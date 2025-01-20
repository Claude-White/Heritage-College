using System.Text;
using Microsoft.EntityFrameworkCore;
using CWhiteH60Store.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace CWhiteH60Store.DAL;

public class ProdCatRepository : IStoreRepository<ProductCategory> {
    private readonly HttpClient _httpClient;

    public ProdCatRepository(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }

    public async Task Create(ProductCategory prodCat) {
        const string createProdCatEndpoint = "/api/ProductCategories";
        // Prevent self referencing loop
        prodCat.Products = null;
        var jsonProdCat = JsonConvert.SerializeObject(prodCat);
        
        var content = new StringContent(jsonProdCat, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(createProdCatEndpoint, content);

        if (response.IsSuccessStatusCode) {
            return;
        }

        return;
    }

    public async Task Update(ProductCategory prodCat) {
        var editProdCatEndpoint = $"/api/ProductCategories/{prodCat.CategoryID}";
        // Prevent self referencing loop
        prodCat.Products = null;
        var jsonProdCat = JsonConvert.SerializeObject(prodCat);
        
        var content = new StringContent(jsonProdCat, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(editProdCatEndpoint, content);

        if (response.IsSuccessStatusCode) {
            return;
        }
        
        return;
    }

    public async Task Delete(ProductCategory prodCat) {
        var deleteProdCatEndpoint = $"/api/ProductCategories/{prodCat.CategoryID}";
        var deleteProdCatResponse = await _httpClient.DeleteAsync(deleteProdCatEndpoint);
        if (deleteProdCatResponse.IsSuccessStatusCode) {
            return;
        }
        
        return;
    }
    
    public async Task<List<ProductCategory>> GetAll() {
        const string prodCatsEndpoint = "/api/ProductCategories";
        
        var prodCatResponse = await _httpClient.GetAsync(prodCatsEndpoint);

        if (prodCatResponse.IsSuccessStatusCode) {
            var prodCatsData = await prodCatResponse.Content.ReadAsStringAsync();
            var prodCats = JsonConvert.DeserializeObject<List<ProductCategory>>(prodCatsData);
            if (prodCats == null) {
                return [];
            }
            return prodCats.ToList();
        }
        
        return null;
    }
    

    public async Task<ProductCategory> GetById(int id) {
        var prodCatEndpoint = $"/api/ProductCategories/{id}";
        var prodCatResponse = await _httpClient.GetAsync(prodCatEndpoint);
        if (prodCatResponse.IsSuccessStatusCode) {
            var prodCatData = await prodCatResponse.Content.ReadAsStringAsync();
            var prodCat = JsonConvert.DeserializeObject<ProductCategory>(prodCatData);
            if (prodCat == null) {
                return null;
            }
            prodCat.Products = null;
            return prodCat;
        }

        return null;
    }
    
    public async Task<ProductCategory> GetByIdInclude(int id) {
        var prodCatEndpoint = $"/api/ProductCategories/{id}";
        var prodCatResponse = await _httpClient.GetAsync(prodCatEndpoint);
        if (prodCatResponse.IsSuccessStatusCode) {
            var prodCatData = await prodCatResponse.Content.ReadAsStringAsync();
            var prodCat = JsonConvert.DeserializeObject<ProductCategory>(prodCatData);
            return prodCat;
        }

        return null;
    }
    
    public async Task<bool> NameExists(string prodCat, int categoryId) {
        var prodCatExistsEndpoint = $"/api/ProductCategories/NameExists/{categoryId}/{prodCat}";
        var prodCatExistsResponse = await _httpClient.GetAsync(prodCatExistsEndpoint);
        if (prodCatExistsResponse.IsSuccessStatusCode) {
            var prodCatExistsData = await prodCatExistsResponse.Content.ReadAsStringAsync();
            var nameExists = bool.Parse(prodCatExistsData);
            return nameExists;
        }

        return true;
    }
}