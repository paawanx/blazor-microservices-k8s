using BlazorCRUD.AuthApi.Interfaces;
using BlazorCRUD.AuthApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCRUD.AuthApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        await _authService.Register(user.Email, user.PasswordHash);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
        var token = await _authService.Login(user.Email, user.PasswordHash);
        if (token != null)
        {
            return Ok(new { token });
        }

        return Unauthorized();
    }
}
