
using Microsoft.AspNetCore.Mvc;
using ReminderApp.Application.UseCases;

namespace ReminderApp.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterUser _register;
    private readonly LoginUser _login;

    public AuthController(RegisterUser register, LoginUser login) => (_register, _login) = (register, login);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
    {
        var userId = await _register.ExecuteAsync(req.Name, req.Email, req.Password, ct);
        return Ok(new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
    {
        try
        {
            var token = await _login.ExecuteAsync(req.Email, req.Password, ct);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}

public record RegisterRequest(string Name, string Email, string Password);
public record LoginRequest(string Email, string Password);
