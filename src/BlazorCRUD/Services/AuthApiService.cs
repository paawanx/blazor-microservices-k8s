using BlazorCRUD.Interfaces;
using System.Net.Http.Json;
using BlazorCRUD.Models;

namespace BlazorCRUD.Services;

public class AuthApiService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Gateway");
    }

    public async Task<string?> Login(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "auth/api/auth/login",
                new { username, password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content
                .ReadFromJsonAsync<LoginResponse>();

            return result!.Token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed: {ex.ToString()}");
            throw;
        }
    }

    public async Task Register(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", new { email, password });
        response.EnsureSuccessStatusCode();
    }
}