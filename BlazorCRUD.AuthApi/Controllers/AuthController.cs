using BlazorCRUD.AuthApi.Data;
using BlazorCRUD.AuthApi.Models;
using BlazorCRUD.AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUD.AuthApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _db;

    public AuthController(AuthDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        user.PasswordHash = PasswordHasher.Hash(user.PasswordHash);
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
        var hash = PasswordHasher.Hash(user.PasswordHash);

        var existing = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == user.Email &&
                                      x.PasswordHash == hash);

        if (existing == null)
            return Unauthorized();

        return Ok("LOGGED_IN");
    }
}
