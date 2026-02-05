using System.Text.Json.Serialization;

namespace BlazorCRUD.AuthApi.Models;

public class User
{
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string PasswordHash { get; set; } = string.Empty;
}