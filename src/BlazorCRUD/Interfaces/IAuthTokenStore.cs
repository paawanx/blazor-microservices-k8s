namespace BlazorCRUD.Interfaces;

public interface IAuthTokenStore
{
    Task SetToken(string token);
    Task<string?> GetToken();
    Task ClearToken();
    Task SetHttpClientToken(HttpClient httpClient);
}