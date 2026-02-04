using BlazorCRUD.AuthApi.Data;
using BlazorCRUD.AuthApi.Models;
using BlazorCRUD.AuthApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUD.AuthApi.Services;

public class AuthService : IAuthService
{
    private readonly AuthDbContext _db;
    private readonly IJwtService _jwtService;

    public AuthService(AuthDbContext db, IJwtService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    public async Task Register(string email, string password)
    {
        var user = new User
        {
            Email = email,
            PasswordHash = PasswordHasher.Hash(password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task<string?> Login(string email, string password)
    {
        var hash = PasswordHasher.Hash(password);
        var existing = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == email &&
                                      x.PasswordHash == hash);

        if (existing != null)
        {
            var token = _jwtService.GenerateToken(existing.Id.ToString(), existing.Email);
            return token;
        }

        return null;
    }
}
