using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorCRUD.Services;

public class AuthTokenStore
{
    private readonly ProtectedLocalStorage _localStorage;
    private const string key = "jwt";

    public AuthTokenStore(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SetToken(string token)
    {
        await _localStorage.SetAsync(key, token);
    }

    public async Task<string?> GetToken()
    {
        try
        {
            var result = await _localStorage.GetAsync<string>(key);
            return result.Success ? result.Value : null;
        }
        catch (InvalidOperationException)
        {
            // JS interop not available during prerendering
            return null;
        }
    }

    public async Task ClearToken()
    {
        await _localStorage.DeleteAsync(key);
    }

    public async Task SetHttpClientToken(HttpClient httpClient)
    {
        var token = await GetToken();

        httpClient.DefaultRequestHeaders.Remove("Authorization");

        if (!string.IsNullOrWhiteSpace(token))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}