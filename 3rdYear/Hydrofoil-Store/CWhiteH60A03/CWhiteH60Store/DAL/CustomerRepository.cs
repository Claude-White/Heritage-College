using System.Text;
using CWhiteH60Store.DAL;
using CWhiteH60Store.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CWhiteH60Services.DAL;

public class CustomerRepository : ICustomerRepository<Customer> {
    private readonly HttpClient _httpClient;

    public CustomerRepository(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<bool> Create(Customer customer) {
        const string createCustomerEndpoint = "/api/Customers";
        // Prevent self referencing loop
        customer.Orders = null;
        customer.Cart = null;
        var jsonCustomer = JsonConvert.SerializeObject(customer);
        
        var content = new StringContent(jsonCustomer, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(createCustomerEndpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task Update(Customer customer) {
        var editCustomerEndpoint = $"/api/Customers/{customer.CustomerId}";
        // Prevent self referencing loop
        customer.Orders = null;
        customer.Cart = null;
        var jsonCustomer = JsonConvert.SerializeObject(customer);
        
        var content = new StringContent(jsonCustomer, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(editCustomerEndpoint, content);

        if (response.IsSuccessStatusCode) {
            return;
        }
        
        return;
    }

    public async Task Delete(Customer customer) {
        var deleteCustomerEndpoint = $"/api/Customers/{customer.CustomerId}";
        var deleteCustomerResponse = await _httpClient.DeleteAsync(deleteCustomerEndpoint);
        if (deleteCustomerResponse.IsSuccessStatusCode) {
            return;
        }
        
        return;
    }

    public async Task<List<Customer>> GetAll() {
        const string customersEndpoint = "/api/Customers";
        
        var customersResponse = await _httpClient.GetAsync(customersEndpoint);

        if (customersResponse.IsSuccessStatusCode) {
            var customersData = await customersResponse.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<Customer>>(customersData);
            if (customers == null) {
                return [];
            }
            return customers.ToList();
        }
        
        return null;
    }
    
    public async Task<Customer> GetByUserId(string userId) {
        var customerEndpoint = $"/api/Customers/UserId/{userId}";
        var customerResponse = await _httpClient.GetAsync(customerEndpoint);
        if (customerResponse.IsSuccessStatusCode) {
            var customerData = await customerResponse.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(customerData);
            if (customer == null) {
                return null;
            }
            customer.Orders = null;
            customer.Cart = null;
            return customer;
        }

        return null;
    }

    public async Task<Customer> GetById(int id) {
        var customerEndpoint = $"/api/Customers/{id}";
        var customerResponse = await _httpClient.GetAsync(customerEndpoint);
        if (customerResponse.IsSuccessStatusCode) {
            var customerData = await customerResponse.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(customerData);
            if (customer == null) {
                return null;
            }
            customer.Orders = null;
            customer.Cart = null;
            return customer;
        }

        return null;
    }

    public async Task<Customer> GetByIdInclude(int id) {
        var customerEndpoint = $"/api/Customers/{id}";
        var customerResponse = await _httpClient.GetAsync(customerEndpoint);
        if (customerResponse.IsSuccessStatusCode) {
            var customerData = await customerResponse.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(customerData);
            return customer;
        }

        return null;
    }

    public async Task<bool> NameExists(string name, int id) {
        // Many customers can have same name
        return true;
    }
}