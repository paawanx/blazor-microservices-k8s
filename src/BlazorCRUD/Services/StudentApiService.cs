using BlazorCRUD.Models;
using BlazorCRUD.Interfaces;
using System.Net.Http.Json;

namespace BlazorCRUD.Services;

public class StudentApiService : IStudentService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthTokenStore _tokenStore;
    private const string baseUrl = "api/students";

    public StudentApiService(IHttpClientFactory httpClientFactory, IAuthTokenStore tokenStore)
    {
        _httpClient = httpClientFactory.CreateClient("Gateway");
        _tokenStore = tokenStore;
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        await _tokenStore.SetHttpClientToken(_httpClient);
        return await _httpClient.GetFromJsonAsync<List<Student>>(baseUrl) ?? new List<Student>();
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        await _tokenStore.SetHttpClientToken(_httpClient);
        return await _httpClient.GetFromJsonAsync<Student>($"{baseUrl}/{id}");
    }

    public async Task<Student> AddStudentAsync(Student student)
    {
        await _tokenStore.SetHttpClientToken(_httpClient);
        var response = await _httpClient.PostAsJsonAsync(baseUrl, student);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Student>() ?? throw new Exception("Failed to parse student");
    }

    public async Task<bool> UpdateStudentAsync(Student student)
    {
        await _tokenStore.SetHttpClientToken(_httpClient);
        var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/{student.Id}", student);
        if (!response.IsSuccessStatusCode)
            return false;
        return true;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        await _tokenStore.SetHttpClientToken(_httpClient);
        var response = await _httpClient.DeleteAsync($"{baseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return false;
        return true;
    }
}