using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using FCOrderManagement.Models;
using FCOrderManagement.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _configuration["ApiSettings:ApiKey"]);
    }

    public async Task<bool> AuthenticateUser(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth", new { username, password });
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Product>> GetProducts()
    {
        var response = await _httpClient.GetAsync("api/products");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Product>>();
        }
        return new List<Product>();
    }

    public async Task<bool> PlaceOrder(Order order)
    {
        var response = await _httpClient.PostAsJsonAsync("api/orders", order);
        return response.IsSuccessStatusCode;
    }
}